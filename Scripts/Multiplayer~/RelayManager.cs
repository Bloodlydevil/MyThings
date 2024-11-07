using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using System.Threading.Tasks;

namespace MyThings.Multiplayer
{
    /// <summary>
    /// A Relay Manager To Connect Using Relay
    /// </summary>
    public class RelayManager
    {
        /// <summary>
        /// Create A Relay And Join At the Same Time
        /// </summary>
        /// <param name="player">The Player Count</param>
        /// <param name="Server">If We Have To Start Sever Or Host</param>
        /// <returns>The Relay Code</returns>
        public static async Task<string> CreateRelay(int player,bool Server=false)
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(player);

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData data = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
                if (Server)
                    NetworkManager.Singleton.StartServer();
                else
                    NetworkManager.Singleton.StartHost();
                return joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
            return null;
        }
        /// <summary>
        /// Join The Relay With The code
        /// </summary>
        /// <param name="relayCode">The relay Code</param>
        /// <returns>Just wait For The End Of The Execution</returns>
        public static async Task JoinRelay(string relayCode)
        {
            try
            {
                JoinAllocation asc = await RelayService.Instance.JoinAllocationAsync(relayCode);
                RelayServerData data = new RelayServerData(asc, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}