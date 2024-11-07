
namespace MyThings.Extension
{
    /// <summary>
    /// Extention Methods To Deal With The floats
    /// </summary>
    public static class ExtensionFloat
    {
        /// <summary>
        /// Extention to Get Int Part Of A float And change The float To Contain Only Fractional Part
        /// </summary>
        /// <param name="a">The Float value </param>
        /// <returns>The Int Value</returns>
        public static int GetIntR(this ref float a)
        {
            int temp = (int)a;
            a -= temp;
            return temp;
        }
    }
}