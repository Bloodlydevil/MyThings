using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
namespace MyThings.Internet.Steam
{

    [Serializable]
    public struct SteamApp
    {
        public string type;
        public string name;
        public uint appId;
        public string requiredAge;
        public bool isFree;
        public List<int> dlc;
        public string detailedDescription;
        public string aboutTheGame;
        public string shortDescription;
        public string[] supportedLanguages;
        public string headerImage;
        public string website;
        public List<string> developers;
        public List<string> publishers;
        public PriceOverview priceOverview;
        public Platforms platforms;
        public List<Category> categories;
        public List<Genre> genres;
        public ReleaseDate releaseDate;
        public Metacritic metacritic;

        [Serializable]
        public class PriceOverview
        {
            public string currency;
            public int initial;
            public int final;
            public int discountPercent;
        }

        [Serializable]
        public class Platforms
        {
            public bool windows;
            public bool mac;
            public bool linux;
        }

        [Serializable]
        public class Category
        {
            public int id;
            public string description;
        }

        [Serializable]
        public class Genre
        {
            public string id;
            public string description;
        }

        [Serializable]
        public class ReleaseDate
        {
            public bool comingSoon;
            public string date;
        }

        [Serializable]
        public class Metacritic
        {
            public int score;
            public string url;
        }
        public SteamApp(JToken data)
        {
            type = data["type"]?.ToString();
            name = data["name"]?.ToString();
            appId = data["steam_appid"]?.ToObject<uint>() ?? 0;
            requiredAge = data["required_age"]?.ToString();
            isFree = data["is_free"]?.ToObject<bool>() ?? false;
            dlc = data["dlc"]?.ToObject<List<int>>() ?? new List<int>();
            detailedDescription = data["detailed_description"]?.ToString();
            aboutTheGame = data["about_the_game"]?.ToString();
            shortDescription = data["short_description"]?.ToString();
            supportedLanguages = data["supported_languages"]?.ToString().Split(',');
            headerImage = data["header_image"]?.ToString();
            website = data["website"]?.ToString();
            developers = data["developers"]?.ToObject<List<string>>() ?? new List<string>();
            publishers = data["publishers"]?.ToObject<List<string>>() ?? new List<string>();
            priceOverview = data["price_overview"]?.ToObject<PriceOverview>();
            platforms = data["platforms"]?.ToObject<Platforms>();
            categories = data["categories"]?.ToObject<List<Category>>() ?? new List<Category>();
            genres = data["genres"]?.ToObject<List<Genre>>() ?? new List<Genre>();
            releaseDate = data["release_date"]?.ToObject<ReleaseDate>();
            metacritic = data["metacritic"]?.ToObject<Metacritic>();
        }
    }
}