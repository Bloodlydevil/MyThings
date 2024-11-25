using System;
using UnityEngine;

namespace MyThings.Pointer
{
    [Serializable]
    public struct CursorPointer
    {
        public Texture2D Texture;
        public Vector2 Hotspot;
        public CursorMode CursorMode;
        public void Show()
        {
            CursorManager.ShowCursor(this);
        }
    }
}