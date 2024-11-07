using TMPro;
using UnityEngine;


namespace MyThings.Extension
{
    /// <summary>
    /// A Class To Deal With the Text Function s
    /// </summary>
    public static class ExtensionText
    {
        /// <summary>
        /// An Extention Function To Set alpha Of The Text
        /// </summary>
        /// <param name="text">The Text</param>
        /// <param name="Alpha">The Alpha</param>
        public static void SetColorA(this TextMeshProUGUI text, float Alpha)
        {
            Color Temp = text.color;
            Temp.a = Alpha;
            text.color = Temp;
        }
    }
}