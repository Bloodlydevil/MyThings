using UnityEngine;

namespace MyThings.Extension
{
    /// <summary>
    /// A Class To Use Some Util For Color
    /// </summary>
    public static class ExtensionColor
    {
        /// <summary>
        /// When We Use Rich Text To Color The Text , Use This To Start The Color
        /// </summary>
        /// <param name="color">The Color To Make In Text</param>
        /// <returns>The Rich Text</returns>
        public static string GetRichTextStart(this Color color)
        {
            return "<color=#" +ColorUtility.ToHtmlStringRGBA(color)  + ">";
        }
        /// <summary>
        /// When We Use Rich Text To Color The Text , Use This To End The Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetRichTextEnd(this Color _)
        {
            return "</color>";
        }
    }
}