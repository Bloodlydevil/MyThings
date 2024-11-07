
using UnityEngine;
using UnityEngine.UI;

namespace MyThings.Extension
{

    /// <summary>
    /// Class For Things Related To Images
    /// </summary>
    public static class ExtensionImages
    {
        /// <summary>
        /// An Extention Method To Reduce The Alpha To The Value
        /// </summary>
        /// <param name="image">The Image</param>
        /// <param name="a">The Value Of Alpha</param>
        /// <returns>The Current Alpha</returns>
        public static void AlphaChangeTo(this Image image, float a)
        {
            Color color = image.color;
            color.a = a;
            image.color = color;
        }
        /// <summary>
        /// An Extention Method To Reduce The Alpha by The Value
        /// </summary>
        /// <param name="image">The Image</param>
        /// <param name="a">The Value TO Reduce</param>
        /// <returns>The Current Alpha</returns>
        public static void AlphaReduceBy(this Image image, float a)
        {
            Color color = image.color;
            color.a -= a;
            image.color = color;
        }
        /// <summary>
        /// An Extention Method To Reduce The Alpha by The Value
        /// </summary>
        /// <param name="image">The Image</param>
        /// <param name="a">The Value TO Reduce</param>
        /// <returns>The Current Alpha</returns>
        public static float AlphaIncreaseBy(this Image image, float a)
        {
            Color color = image.color;
            color.a += a;
            image.color = color;
            return color.a;
        }
    }
}