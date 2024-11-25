using MyThings.ExtendableClass;
using MyThings.Timer;
using UnityEngine;

namespace MyThings.Pointer
{

    public class CursorManager : Singleton_C<CursorManager>
    {
        private CursorAnimation cursorAnimation;
        private ITimer timer;
        private int m_Frame;
        protected override void Awake()
        {
            base.Awake();
            timer = TimerManager.Create(0, UpdateCursorAnimation, true, false);
        }
        private void UpdateCursorAnimation()
        {
            m_Frame = (m_Frame + 1) % cursorAnimation.Textures.Length;
            Cursor.SetCursor(cursorAnimation.Textures[m_Frame],cursorAnimation.Hotspot,cursorAnimation.CursorMode);
        }
        public static void ShowCursor(CursorPointer pointer)
        {
            Instance.timer.Stop();
            Cursor.SetCursor(pointer.Texture, pointer.Hotspot, pointer.CursorMode);
        }
        public void ShowCursor(CursorAnimation pointer)
        {
            cursorAnimation= pointer;
            timer.MaxTime = 1/pointer.AnimationFPS;
            timer.Start();
        }
        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        public static void UnlockCursor()
        {
            Cursor.lockState=CursorLockMode.None;
        }
    }
}