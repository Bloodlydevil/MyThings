using UnityEngine;

namespace MyThings
{
    /// <summary>
    ///  A class to give more functionality to random
    /// </summary>
    public static class Random
    {
        public const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Number = "0123456789";

        /// <summary>
        /// The String Accepted By The NetworkPlayer
        /// </summary>
        public const string PlayerNameAccepted = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

        /// <summary>
        /// get a random color
        /// </summary>
        /// <returns>The Color(r,b,g,1)</returns>
        public static Color GetColor() => new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        /// <summary>
        /// Choose A char From The Given String
        /// </summary>
        /// <param name="ToChooseFrom">The String To Choose From</param>
        /// <returns>The Chosen Charecter</returns>
        public static char GetChar(string ToChooseFrom)=> ToChooseFrom[UnityEngine.Random.Range(0,ToChooseFrom.Length)];
    }
}