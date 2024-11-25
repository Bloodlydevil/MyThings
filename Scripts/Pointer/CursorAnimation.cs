using System;
using UnityEngine;
namespace MyThings.Pointer
{
    [Serializable]
    public struct CursorAnimation
    {
        public Texture2D[] Textures;
        public Vector2 Hotspot;
        public CursorMode CursorMode;
        public float AnimationFPS;

        public void Show()
        {
            CursorManager.Instance.ShowCursor(this);
        }
    }
}