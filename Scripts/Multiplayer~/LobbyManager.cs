using MyThings.ExtendableClass;
using MyThings.Timer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using MyThings.Scene;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace MyThings.Multiplayer
{

    /// <summary>
    /// The Lobby Manager
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class LobbyManager : Singleton_C<LobbyManager>
    {
        private Lobby n_joinedLobby;
        private ITimer n_Heartbeat;
        private ITimer n_LobbyPolling;
        private ITimer n_LobbyListPolling;
        /// <summary>
        /// Is The Player Connected
        /// </summary>
        private bool connected = false;
        private Func<Player> GetPlayer;

        private Scheduler SJoinLobbyQuick;
        private Scheduler SJoinLobbyLobby;
        private Scheduler SLeaveLobby;
        private Scheduler SKickPlayer;
        private Scheduler SUpdateLobby;
        private Scheduler SCreateLobby;


        public event Action<Lobby> OnJoinedLobbyUpdate;
        public event Action<Lobby> OnKickedFromLobby;
        public event Action<Lobby> OnLobbyJoined;
        public event Action OnLobbyLeft;
        public event Action OnNetworkConnected;
        public event Action OnNetworkConnectionFailed;
        public event Action OnLobbyUpdateFailed;
        public event Action OnLobbyJoinFail;
        public event Action OnPlayerKickFailed;
        public event Action<List<Lobby>> OnLobbyListChange;
        public event Action<int> OnLobbyPlayerMaxCountChange;
        public event Action<bool> OnLobbyPrivateChange;
        public event Action<string> OnLobbyNameChange;
        public event Action<bool> OnLobbyLockedChange;

        protected override bool AddToDontDestroyOnLoad => false;

        public const float LobbyJoin_Rate = 3f;
        public const float LobbyLeave_Rate = 0.2f;
        public const float LobbyUpdate_Rate = 1f;
        public const float LobbyCreate_Rate = 3f;

        public const string Key_Relay_Code = "RelayCode";
        public const int Max_PlayerCount = 10;
        public const int Min_PlayerCount = 1;
        public const int MAX_GameStartWait = 10;
        public const float HeartBeat_Rate = 15;
        public const float LobbyPolling_Rate = 1.5f;
        public const float LobbyListPolling_Rate = 1.5f;


        private static QueryLobbiesOptions BasicOption;


        static LobbyManager()
        {
            QueryLobbiesOptions queryOptions = new QueryLobbiesOptions();
            queryOptions.Count = 25;
            queryOptions.Filters = new List<QueryFilter>
                {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                };
            queryOptions.Order = new List<QueryOrder>
                {
                new QueryOrder(false,QueryOrder.FieldOptions.Created)
                };
            BasicOption = queryOptions;
        }


        #region Unity

        protected override void OnDestroy()
        {
            base.OnDestroy();
            LeaveLobby();
        }
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            LeaveLobby();
        }

        #endregion

        #region Private

        private async void HeartBeat()
        {
            if (!IsLobbyHost()) return;
            Debug.Log("Heart Beat");
            await LobbyService.Instance.SendHeartbeatPingAsync(n_joinedLobby.Id);
        }
        private async void LobbyPooling()
        {
            if (n_joinedLobby == null) return;
            try
            {
                Lobby temp = await LobbyService.Instance.GetLobbyAsync(n_joinedLobby.Id);
                if (!IsPlayerInLobby(temp))
                {
                    Debug.Log("Kicked from lobby");
                    OnKickedFromLobby?.Invoke(n_joinedLobby);
                    n_LobbyPolling.Stop();
                    n_joinedLobby = null;
                    return;
                }
                if (temp.IsPrivate != n_joinedLobby.IsPrivate)
                    OnLobbyPrivateChange?.Invoke(temp.IsPrivate);
                if (temp.MaxPlayers != n_joinedLobby.MaxPlayers)
                    OnLobbyPlayerMaxCountChange?.Invoke(temp.MaxPlayers);
                if (temp.Name != n_joinedLobby.Name)
                    OnLobbyNameChange?.Invoke(temp.Name);
                if (temp.HostId != n_joinedLobby.HostId)
                {
                    if (IsHost(AuthenticationService.Instance.PlayerId))
                    {
                        n_Heartbeat.Start();
                    }
                }
                if (temp.Data[Key_Relay_Code].Value != null)
                {
                    if (!IsLobbyHost())
                    {
                        if (!connected)
                        {
                            ConnectForClient(temp);
                            OnNetworkConnected?.Invoke();
                        }
                    }
                }
                n_joinedLobby = temp;
                OnJoinedLobbyUpdate?.Invoke(n_joinedLobby);
            }
            catch (Exception ex)
            {
                n_joinedLobby = null;
                OnLobbyLeft?.Invoke();
                Debug.Log(ex);
            }
        }
        private bool IsPlayerInLobby(Lobby a)
        {
            if (a == null || a.Players == null) return false;
            for (int i = 0; i < a.Players.Count; ++i)
                if (a.Players[i].Id == AuthenticationService.Instance.PlayerId) return true;
            return false;
        }
        private async void ConnectForClient(Lobby temp)
        {
            try
            {
                await RelayManager.JoinRelay(temp.Data[Key_Relay_Code].Value);
                connected = true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                OnNetworkConnectionFailed?.Invoke();
            }
        }
        private async void RefreshLobbyList(QueryLobbiesOptions options)
        {
            try
            {
                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(options);
                OnLobbyListChange?.Invoke(queryResponse.Results);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        private async void internalJoinLobby(Lobby lobby)
        {
            if (n_joinedLobby != null) return;
            if (lobby.IsPrivate) return;
            n_joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions { Player = GetPlayer() });
            OnLobbyJoined?.Invoke(lobby);
            OnJoinedLobbyUpdate?.Invoke(n_joinedLobby);
            n_LobbyPolling.Start();
            n_LobbyListPolling.Stop();
        }
        private async void internalJoinLobby()
        {
            if (n_joinedLobby == null) return;
            try
            {
                Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                n_joinedLobby = lobby;
                OnLobbyJoined?.Invoke(n_joinedLobby);
                OnJoinedLobbyUpdate?.Invoke(n_joinedLobby);
                n_LobbyPolling.Start();
                n_LobbyListPolling.Stop();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                OnLobbyJoinFail?.Invoke();
            }
        }
        private async void internalLeaveLobby()
        {
            if (n_joinedLobby == null) return;
            try
            {
                if (IsLobbyHost())
                    n_Heartbeat.Stop();
                n_LobbyPolling.Stop();
                await LobbyService.Instance.RemovePlayerAsync(n_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                if (this == null)
                    return;
                n_joinedLobby = null;
                OnLobbyLeft?.Invoke();
                n_LobbyListPolling.Start();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);// when the thread calls the await after the scene change it is fine as the thread just leaves the lobby
                                      // and the rest dose not mater as we will have switched the scene
                                      // It is more than likely a OK ERRor
            }
        }
        private async void internalKickPlayer(string PlayerID)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(n_joinedLobby.Id, PlayerID);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                OnPlayerKickFailed?.Invoke();
            }
        }
        private async void internalCreateLobby(string LobbyName, bool isPrivate, int maxPlayer)
        {

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Player = GetPlayer(),
                IsPrivate = isPrivate,
                Data = new Dictionary<string, DataObject> { { Key_Relay_Code, new DataObject(DataObject.VisibilityOptions.Member, null) } }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(LobbyName, maxPlayer, options);
            n_joinedLobby = lobby;
            n_Heartbeat.Start();
            n_LobbyPolling.Start();
            OnLobbyJoined?.Invoke(lobby);
            n_LobbyListPolling.Stop();
        }
        private async void internalUpdateLobby(UpdateLobbyOptions options)
        {
            try
            {
                Lobby old = n_joinedLobby;
                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(n_joinedLobby.Id, options);
                if (lobby.Name != old.Name)
                    OnLobbyNameChange?.Invoke(lobby.Name);
                if (lobby.MaxPlayers != old.MaxPlayers)
                    OnLobbyPlayerMaxCountChange?.Invoke(lobby.MaxPlayers);
                if (lobby.IsPrivate != old.IsPrivate)
                    OnLobbyPrivateChange?.Invoke(lobby.IsPrivate);
                if (lobby.IsLocked != old.IsLocked)
                    OnLobbyLockedChange?.Invoke(lobby.IsLocked);
                n_joinedLobby = lobby;
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
                OnLobbyUpdateFailed?.Invoke();
            }
        }
        #endregion

        #region Public

        /// <summary>
        /// Initialize The Online Connection start unity services before statering lobby
        /// </summary>
        /// <param name="Name">The Player Name</param>
        public void InitializeOnlineConnection(Func<Player> PlayerGiver)
        {
            GetPlayer = PlayerGiver;
            n_Heartbeat = TimerManager.Create(HeartBeat_Rate, HeartBeat, true);
            n_LobbyPolling = TimerManager.Create(LobbyPolling_Rate, LobbyPooling, true);
            n_LobbyListPolling = TimerManager.Create(LobbyListPolling_Rate,()=>RefreshLobbyList(BasicOption), true);
            SJoinLobbyQuick = new Scheduler(LobbyJoin_Rate, true);
            SJoinLobbyLobby = new Scheduler(LobbyJoin_Rate, true);
            SLeaveLobby = new Scheduler(LobbyLeave_Rate, true);
            SKickPlayer = new Scheduler(LobbyLeave_Rate, true);
            SUpdateLobby = new Scheduler(LobbyUpdate_Rate, true);
            SCreateLobby = new Scheduler(LobbyCreate_Rate, true);
        n_LobbyListPolling.Start();
        }
        /// <summary>
        /// Join Lobby
        /// </summary>
        /// <param name="lobby">The Lobby To Join</param>
        public void JoinLobby(Lobby lobby)
        {
            SJoinLobbyQuick.Cancel();
            SJoinLobbyLobby.Schedule(() => internalJoinLobby(lobby));
        }
        /// <summary>
        /// Join Lobby
        /// </summary>
        /// <param name="ID">The Lobby Id</param>
        /// <returns>Is The Lobby Joined</returns>
        public async Task<bool> JoinLobby(string ID)
        {
            if (n_joinedLobby != null) return false;
            Lobby temp;
            try
            {
                temp = await LobbyService.Instance.JoinLobbyByCodeAsync(ID, new JoinLobbyByCodeOptions { Player = GetPlayer() });
            }
            catch (Exception ex) 
            {
                Debug.Log(ex.ToString());
                OnLobbyJoinFail?.Invoke();
                return false; 
            }
            n_joinedLobby = temp;
            OnLobbyJoined?.Invoke(n_joinedLobby);
            OnJoinedLobbyUpdate?.Invoke(n_joinedLobby);
            n_LobbyPolling.Start();
            n_LobbyListPolling.Stop();
            return true;
        }
        /// <summary>
        /// Join Lobby Quick Join
        /// </summary>
        public void JoinLobby()
        {
            if (n_joinedLobby != null) return;
            SJoinLobbyLobby.Cancel();
            SJoinLobbyQuick.Schedule(internalJoinLobby);
        }
        public void SetLobbySearchMethod(QueryLobbiesOptions queryLobbiesOptions)
        {
            BasicOption = queryLobbiesOptions;
        }
        /// <summary>
        /// Leave The Lobby
        /// </summary>
        public void LeaveLobby()
        {
            if (n_joinedLobby == null) return;
            SLeaveLobby.Schedule(internalLeaveLobby);
        }
        /// <summary>
        /// Kick The Player
        /// </summary>
        /// <param name="PlayerID">The Player Id</param>
        public void KickPlayer(string PlayerID)
        {
            if (!IsLobbyHost()) return;
            SKickPlayer.Schedule(()=>internalKickPlayer(PlayerID));
        }
        /// <summary>
        /// Create Lobby
        /// </summary>
        /// <param name="LobbyName">The Lobby Name</param>
        /// <param name="isPrivate">Is It Private</param>
        /// <param name="maxPlayer">Max Player</param>
        public void CreateLobby(string LobbyName, bool isPrivate, int maxPlayer)
        {
            SCreateLobby.Schedule(()=>internalCreateLobby(LobbyName, isPrivate, maxPlayer));
        }
        /// <summary>
        /// Is Lobby Host
        /// </summary>
        /// <returns>Is It?</returns>
        public bool IsLobbyHost()
        {
            return n_joinedLobby != null && n_joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }
        /// <summary>
        /// Is Host
        /// </summary>
        /// <param name="PlayerID">The ID To Check</param>
        /// <returns>Is it?</returns>
        public bool IsHost(string PlayerID) => n_joinedLobby != null && n_joinedLobby.HostId == PlayerID;
        /// <summary>
        /// Try To Get The Lobby
        /// </summary>
        /// <param name="b">The Lobby</param>
        /// <returns>If Lobby Is Present</returns>
        public bool TryGetLobby(out Lobby b)
        {
            b = n_joinedLobby;
            return n_joinedLobby != null;
        }
        /// <summary>
        /// Update The Lobby 
        /// </summary>
        /// <param name="options">The Options To Update</param>
        public void UpdateLobby(UpdateLobbyOptions options)
        {
            SUpdateLobby.Schedule(() => internalUpdateLobby(options));
        }
        /// <summary>
        /// Get The Lobby Name
        /// </summary>
        /// <returns>The Lobby Name</returns>
        public string GetLobbyName() => n_joinedLobby?.Name;
        /// <summary>
        /// Get THe Max Player
        /// </summary>
        /// <returns>The Max Player</returns>
        public int GetMaxPlayers() => n_joinedLobby == null ? 0 : n_joinedLobby.MaxPlayers;
        /// <summary>
        /// Get The Player Connected
        /// </summary>
        /// <returns>Player Connected</returns>
        public int GetTotalPlayers() => n_joinedLobby == null ? 0 : n_joinedLobby.Players.Count;
        /// <summary>
        /// Set up Things For Starting Game
        /// </summary>
        public async Task<bool> SetUpStartGame()
        {
            try
            {
                string relayCode = await RelayManager.CreateRelay(n_joinedLobby.Players.Count);
                connected = true;
                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(n_joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        {Key_Relay_Code,new DataObject(DataObject.VisibilityOptions.Member,relayCode)},
                    }
                });
                OnNetworkConnected?.Invoke();
                n_joinedLobby = lobby;
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }
        }
        /// <summary>
        /// Start The Game
        /// </summary>
        public void StartGame(string scene)
        {
            if (NetworkManager.Singleton.IsHost)
                NetworkSceneLoader.Instance.Load(scene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        #endregion

    }
}