using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MyThings.Scripts.Extension
{
    public static class ExtensionTime
    {
        /// <summary>
        /// Convert The Given MilliSecond To Seconds
        /// </summary>
        /// <param name="Milli">The MilliSeconds</param>
        /// <returns>Seconds</returns>
        public static float MilliToSeconds(this long Milli)
        {
            if(Milli < 0) 
                return 0;
            return Milli / 1000.0f;
        }
    }
}
