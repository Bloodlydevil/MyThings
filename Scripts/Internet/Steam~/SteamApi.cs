namespace MyThings.Internet.Steam
{
    /// <summary>
    /// A Class To Deal With Steam Api Stuff
    /// </summary>
    public static class SteamApi
    {
        /// <summary>
        /// Convert SteamId 64 To Steam Id
        /// </summary>
        /// <param name="steamID64">Steam Id64</param>
        /// <returns>SteamId</returns>
        public static string ConvertSteamID64ToSteamID(string steamID64)
        {
            // Convert the SteamID64 string to a long integer
            long id64 = long.Parse(steamID64);

            // Calculate the "X" value in STEAM_X:Y:Z
            int y = (int)(id64 % 2);

            // Calculate the "Z" value in STEAM_X:Y:Z
            long z = (id64 - 76561197960265728) / 2;

            // Combine parts to form the classic Steam ID format
            return $"STEAM_0:{y}:{z}";
        }
        /// <summary>
        /// Get The URL To Make Connection With Steam To GEt Steam Detail With Username
        /// </summary>
        /// <param name="apiKey">The Api Key Used</param>
        /// <param name="userName">The UserName Of The Account</param>
        /// <returns>URL</returns>
        public static string GetResolveVanityURL(string apiKey, string userName)
        {
            return $"https://api.steampowered.com/ISteamUser/ResolveVanityURL/v1/?key={apiKey}&vanityurl={userName}";
        }
        /// <summary>
        /// Get The URL To MAke Connection With Steam To Get Owned Games 
        /// </summary>
        /// <param name="apiKey">Steam Api Key</param>
        /// <param name="Steam64ID">Steam 64 ID</param>
        /// <param name="AppInfo">Extra App Info Request</param>
        /// <returns>URL</returns>
        public static string GetOwnedGamesURL(string apiKey, string Steam64ID,bool AppInfo=true)
        {
            return $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={apiKey}&steamid={Steam64ID}&include_appinfo={AppInfo}&format=json";
        }
        /// <summary>
        /// Get The URL to Make Connection With Steam To Get Player Details
        /// </summary>
        /// <param name="apiKey">Steam Api Key</param>
        /// <param name="SteamId">Steam ID</param>
        /// <returns>URL</returns>
        public static string GetPlayerDataURL(string apiKey, string SteamId)
        {
            return $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={SteamId}";
        }
        /// <summary>
        /// Get The Game Icon URL
        /// </summary>
        /// <param name="AppId">Game ID</param>
        /// <returns>URL</returns>
        public static string GetGamesIconURL(uint AppId)
        {
            return $"https://steamcdn-a.akamaihd.net/steam/apps/{AppId}/header.jpg";
        }
        /// <summary>
        /// Get The App Details
        /// </summary>
        /// <param name="AppId">The App ID</param>
        /// <returns>URL</returns>
        public static string GetAppDetails(uint AppId)
        {
            return $"https://store.steampowered.com/api/appdetails?appids={AppId}";
        }
    }
}