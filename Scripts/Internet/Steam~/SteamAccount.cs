using MyThings.Data;
using MyThings.SaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace MyThings.Internet.Steam
{

    [Serializable]
    public class SteamAccount
    {
        private readonly string m_TextureLoc = "Texture/";
        public string Password { get; set; }
        public string AccountName { get; set; }
        public Texture2DLocSavable ProfileTexture { get;    set; }
        public string AccountID64 { get; set; }
        public SteamPlayer Player { get; set; }
        public List<SteamGame> Games { get; set; }
        
        public SteamAccount( string password, string accountName, string accountID64, List<SteamGame> games = null)
        {
            Password = password;
            AccountName = accountName;
            AccountID64 = accountID64;
            if (games != null)
                Games = games;
            else
                Games = new List<SteamGame>();
            m_TextureLoc += AccountID64;
        }
        private void GamesVerifyInAccount(UnityWebRequest request, Action<SteamAccount> CallBack)
        {
            if (request != null)
            {
                var NewGames = SteamUtils.GetGames(request);
                if (NewGames != null)
                {
                    Games = new List<SteamGame>(Games.Union(NewGames));
                }
            }
            CallBack(this);
        }
        private void AccountVerify(UnityWebRequest request, Action<SteamAccount> CallBack)
        {

            if (request != null)
            {
                Player = SteamUtils.GetPlayer(request);
            }
            CallBack(this);
        }

        #region Public
        public void CheckForGames(string SteamApiKey, Action<SteamAccount> CallBack)
        {
            if (AccountID64 == null)
            {
                Debug.Log("No AccountsId64 found");
                return;
            }
            InternetUtility.Instance.MakeBasicRequest(SteamApi.GetOwnedGamesURL(SteamApiKey, AccountID64), i => { GamesVerifyInAccount(i, CallBack); });
        }
        public void CheckForProfile(string SteamApiKey, Action<SteamAccount> CallBack)
        {
            if (AccountID64 == null)
            {
                Debug.Log("No AccountsId64 found");
                return;
            }
            InternetUtility.Instance.MakeBasicRequest(
                SteamApi.GetPlayerDataURL(SteamApiKey, AccountID64),
                i =>
                {
                    AccountVerify(i, CallBack);
                });
        }
        public void CompleteProfileCheck(string SteamApiKey, Action<SteamAccount> GamesCallBack, Action<SteamAccount> ProfileCallBack)
        {
            CheckForGames(SteamApiKey, GamesCallBack);
            CheckForProfile(SteamApiKey, ProfileCallBack);
        }
        public void RequestProfilePic(Action ProfileCallBack)
        {
            if (AccountID64 == null || Player.avatar == null) return;
            InternetUtility.Instance.MakeTextureRequest(
                Player.avatarfull,
                i =>
                {
                    ProfileTexture =new Texture2DLocSavable(InternetUtility.GetTexture(i),m_TextureLoc);
                    ProfileCallBack();
                }
                );
        }
        public void AddGame(SteamGame game)
        {
            if (Games.TrueForAll(i => i.Name != game.Name))
            {
                Games.Add(game);
            }
            else
            {
                Debug.Log("Game Already Exist");
            }
        }
        public void RemoveGame(uint Id)
        {
            for (int i = 0; i < Games.Count; i++)
            {
                if (Games[i].AppId == Id)
                {
                    Games.RemoveAt(i);
                    return;
                }
            }
        }
        public void SetUp()
        {
            if (AccountID64 == null)
                return;
            try
            {
                ProfileTexture = new Texture2DLocSavable(m_TextureLoc);
            }
            catch
            {
                SaverAndLoader.DeleteData(m_TextureLoc);
                ProfileTexture = new Texture2DLocSavable(m_TextureLoc);
            }
        }
        public override string ToString()
        {
            return AccountName;
        }
        #endregion
    }
}