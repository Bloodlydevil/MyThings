using System;
using UnityEngine;

namespace MyThings.Data
{
    [Serializable]
    public struct Vector3_Draw
    {
        public Vector3 Vector;
        public float size;
        public Color color;
        public bool Wired;
        public readonly void Draw(Vector3 CorrectionPos=new())
        {
            Gizmos.color = color;
            if (Wired)
                Gizmos.DrawWireSphere(Vector + CorrectionPos, size);
            else
                Gizmos.DrawSphere(Vector + CorrectionPos, size);
        }
        public static implicit operator Vector3(Vector3_Draw vector3_D)
        {
            return vector3_D.Vector;
        }
    }
}
