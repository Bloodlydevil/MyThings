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
        public enum asd
        {
            X,
            Y,
            Z
        }
        /// <summary>
        /// Make A Vector2 To Vector3
        /// </summary>
        /// <param name="vector">The Vector 2</param>
        /// <param name="z">The Z Value</param>
        /// <returns>Vector 3</returns>
        public static Vector3 ToVector3(this Vector2 vector,float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
        /// <summary>
        /// Multiply x To x And y To y and z To z
        /// </summary>
        /// <param name="first">First Vector</param>
        /// <param name="second">Second Vector</param>
        /// <returns>The Multiplied Vector</returns>
        public static Vector3 Multiply(this Vector3 first, Vector3 second)
        {
            return new(first.x * second.x, first.y * second.y, first.z * second.z);
        }
        /// <summary>
        /// Add Value To Each Parameter
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Vector3 Add(this Vector3 vector, float Value)
        {
            return new(vector.x + Value, vector.y + Value, vector.z + Value);
        }
        /// <summary>
        /// Is The Relative Position Inside Absolute Size
        /// </summary>
        /// <param name="point">the Relative Position</param>
        /// <param name="size">The Absolute Size</param>
        /// <returns>If It Is Inside</returns>
        public static bool IsInsideBox(this Vector2 point,Vector2 size)
        {
            return !(Mathf.Abs(point.x) > size.x || Mathf.Abs(point.y) > size.y);
        }
        /// <summary>
        /// Is The Vector Grater Than The Other Grater (does A Basic Check For The Value)
        /// </summary>
        /// <param name="v1">The Vector </param>
        /// <param name="v2">The Vector To Check Aganst</param>
        /// <returns></returns>
        public static bool IsGreater(this Vector2 v1, Vector2 v2)
        {
            return !(v1.x < v2.x || v1.y < v2.y);
        }
        /// <summary>
        /// Is The Vector Grater Than The Other Grater (does A Basic Check For The Value)
        /// </summary>
        /// <param name="v1">The Vector </param>
        /// <param name="v2">The Vector To Check Aganst</param>
        /// <param name="value">The Diffrence in The Value</param>
        /// <returns></returns>
        public static bool IsGreater(this Vector2 v1, Vector2 v2, out Vector2 value)
        {
            value=new Vector2(v1.x-v2.x, v1.y-v2.y);
            return !(value.x < 0 || value.y < 0);
        }
        public static bool IsAnyGreater(this Vector2 v1, Vector2 v2, out Vector2 value)
        {
            value = new Vector2(v1.x - v2.x, v1.y - v2.y);
            return value.x > 0 || value.y > 0;
        }
        public static bool IsAnyGreater(this Vector2 v1, Vector2 v2)
        {
            return v1.x > v2.x || v1.y > v2.y;
        }
        public static Vector2 GetSign(this Vector2 v)
        {
            return new Vector2(Mathf.Sign(v.x), Mathf.Sign(v.y));
        }
    }
}
