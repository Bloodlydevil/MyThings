using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyThings.Extension
{
    public static class ExtensionVector
    {
        public static Vector3 ToVector3(this Vector2 vector,float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    }
}
