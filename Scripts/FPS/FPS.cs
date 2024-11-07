
using UnityEngine;

namespace MyThings.FPS
{
    /// <summary>
    /// A Class To Tell FPS Of The Game
    /// </summary>
    public class FPS
    {
        private static int lastFrameCount;
        private static float lastFrameTime;
        public static int FPS_int
        {
            get
            {
                int tmep= Mathf.RoundToInt((Time.frameCount - lastFrameCount) / (Time.realtimeSinceStartup - lastFrameTime));
                lastFrameTime = Time.realtimeSinceStartup;
                lastFrameCount = Time.frameCount;
                return tmep;
            }
        }
        public static string FPS_string { get => string.Format("FPS: {0}", FPS_int); }
    }
}