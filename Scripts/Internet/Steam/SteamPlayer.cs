
using Newtonsoft.Json.Linq;
using System;

namespace MyThings.Internet.Steam
{
    [Serializable]
    public struct SteamPlayer
    {
        public string steamid;                  // 64-bit Steam ID
        public string personaname;               // Display name
        public string profileurl;                // Profile URL
        public string avatar;                    // 32x32 avatar URL
        public string avatarmedium;              // 64x64 avatar URL
        public string avatarfull;                // 184x184 avatar URL
        public int personastate;                 // Online status (0-6)
        public int communityvisibilitystate;     // Profile visibility (1=private, 3=public)
        public int profilestate;                 // Profile state (1 if customized)
        public long lastlogoff;                  // Unix timestamp of last logoff
        public string realname;                  // Real name (if provided)
        public string primaryclanid;             // Primary group SteamID64 (if any)
        public long timecreated;                 // Unix timestamp for account creation
        public string loccountrycode;            // ISO country code
        public string locstatecode;              // State or province code
        public int loccityid;                    // City ID (if provided)
        public SteamPlayer(JToken playerToken)
        {
            steamid = playerToken["steamid"]?.ToString();
            personaname = playerToken["personaname"]?.ToString();
            profileurl = playerToken["profileurl"]?.ToString();
            avatar = playerToken["avatar"]?.ToString();
            avatarmedium = playerToken["avatarmedium"]?.ToString();
            avatarfull = playerToken["avatarfull"]?.ToString();
            personastate = (int)(playerToken["personastate"] ?? 0);
            communityvisibilitystate = (int)(playerToken["communityvisibilitystate"] ?? 0);
            profilestate = (int)(playerToken["profilestate"] ?? 0);
            lastlogoff = (long)(playerToken["lastlogoff"] ?? 0);
            realname = playerToken["realname"]?.ToString();
            primaryclanid = playerToken["primaryclanid"]?.ToString();
            timecreated = (long)(playerToken["timecreated"] ?? 0);
            loccountrycode = playerToken["loccountrycode"]?.ToString();
            locstatecode = playerToken["locstatecode"]?.ToString();
            loccityid = (int)(playerToken["loccityid"] ?? 0);
        }
    }
    public enum CommunityVisibility
    {
        Visible = 3,
        Private = 1
    }
}