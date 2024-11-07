using System;
using UnityEngine;

namespace MyThings.Data
{// think of a way to make sure the check does not happen in the change
    /// <summary>
    /// A Class to Use As Target System For Colors
    /// </summary>
    [Serializable]
    public class Target// falged for improper
    {
        [Range(-1, 1)] public float r;
        [Range(-1, 1)] public float g;
        [Range(-1, 1)] public float b;
        [Range(-1, 1)] public float a;
        public Target(float r = -1, float g = -1, float b = -1, float a = -1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public void Change(ref Color colorToChange, Color min, float change)
        {
            if (!(r < 0))
                colorToChange.r = Mathf.LerpUnclamped(min.r, r, change);
            if (!(g < 0))
                colorToChange.g = Mathf.LerpUnclamped(min.g, g, change);
            if (!(b < 0))
                colorToChange.b = Mathf.LerpUnclamped(min.b, b, change);
            if (!(a < 0))
                colorToChange.a = Mathf.LerpUnclamped(min.a, a, change);
        }
    }
}