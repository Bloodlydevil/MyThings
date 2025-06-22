using Newtonsoft.Json.Linq;
using System;

namespace MyThings.Internet.Steam
{
    /// <summary>
    /// A Steam Game Representative
    /// </summary>
    [Serializable]
    public struct SteamGame : IEquatable<SteamGame>
    {
        public uint AppId;
        public string Name;
        public int PlaytimeForever;
        public string ImageIconUrl;
        public string ImageLogoUrl;
        public bool HasCommunityVisibleStats;
        public int PlaytimeForeverWindows;
        public int PlaytimeForeverMac;
        public int PlaytimeForeverLinux;
        public SteamGame(JToken jToken)
        {
            AppId = jToken.Value<uint>("appid");
            Name = jToken.Value<string>("name");
            PlaytimeForever = jToken.Value<int>("playtime_forever");
            ImageIconUrl = jToken.Value<string>("img_icon_url");
            ImageLogoUrl = jToken.Value<string>("img_logo_url");
            HasCommunityVisibleStats = jToken.Value<bool>("has_community_visible_stats");
            PlaytimeForeverWindows = jToken.Value<int>("playtime_windows_forever");
            PlaytimeForeverMac = jToken.Value<int>("playtime_mac_forever");
            PlaytimeForeverLinux = jToken.Value<int>("playtime_linux_forever");
        }
        public override bool Equals(object obj)
        {
            if (obj is SteamGame otherGame)
            {
                return Equals(otherGame);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return AppId.GetHashCode();
        }
        public bool Equals(SteamGame other)
        {
            return other.AppId == AppId;
        }
        public override string ToString()
        {
            return Name;
        }
        public static bool operator ==(SteamGame x, SteamGame y)
        {
            return x.AppId==y.AppId;
        }
        public static bool operator !=(SteamGame x, SteamGame y)
        {
            return x.AppId != y.AppId;
        }
    }
}