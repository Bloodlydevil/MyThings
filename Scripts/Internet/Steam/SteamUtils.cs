using MyThings.Internet.Steam;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class SteamUtils
{
    /// <summary>
    /// Get The Steam Games Owned 
    /// </summary>
    /// <param name="req">The Web Request Result</param>
    /// <returns>The Games</returns>
    public static SteamGame[] GetGames(UnityWebRequest req) => GetGames(req.downloadHandler.text);
    /// <summary>
    /// Get The Player Data 
    /// </summary>
    /// <param name="req">The Web Request Result</param>
    /// <returns>The PlayerData</returns>
    public static SteamPlayer GetPlayer(UnityWebRequest req) => GetPlayer(req.downloadHandler.text);
    public static SteamApp GetApp(UnityWebRequest req, uint appID) => GetApp(req.downloadHandler.text, appID);

    /// <summary>
    /// Get The Steam Games Owned 
    /// </summary>
    /// <param name="Data">The Data Given By Web Call</param>
    /// <returns>The Games</returns>
    public static SteamGame[] GetGames(string Data)
    {
        JObject jsonResponse = JObject.Parse(Data);
        var gamesArray = jsonResponse["response"]["games"];

        if (gamesArray != null && gamesArray.Type == JTokenType.Array)
        {
            JArray gamesJArray = (JArray)gamesArray;
            int gameCount = gamesJArray.Count;
            SteamGame[] ownedGameNames = new SteamGame[gameCount];

            for (int i = 0; i < gameCount; i++)
            {
                ownedGameNames[i] = new SteamGame(gamesJArray[i]);
            }
            return ownedGameNames;
        }
        Debug.LogWarning("No Games Found");
        return null;
    }
    /// <summary>
    /// Get The Player Data 
    /// </summary>
    /// <param name="Data">The Data Given By Web Call</param>
    /// <returns>The PlayerData</returns>
    public static SteamPlayer GetPlayer(string Data)
    {
        try
        {
            return new SteamPlayer(JObject.Parse(Data)["response"]["players"][0]);
        }
        catch
        {
            return new SteamPlayer();
        }
    }
    public static SteamApp GetApp(string Data,uint appID)
    {
        try
        {
            return new SteamApp(JObject.Parse(Data)[appID.ToString()]["data"]);
        }
        catch
        {
            return new SteamApp();
        }
    }
}
