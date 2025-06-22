using UnityEngine;
namespace MyThings.Extension
{
    public static class ExtensionInt
    {
        public static int LoopIn(this int x, int low, int high)
        {
            if (x >= low && x <= high)
                return x;

            int dis = high - low + 1;

            if (x < low)
            {
                x = dis * (1 - (int)Mathf.Ceil((float)(low - x + 1) / dis)) - x;
            }
            else
                x -= (int)Mathf.Ceil((float)(x - high + 1) / dis) * dis;

            return x;
        }
    }
}