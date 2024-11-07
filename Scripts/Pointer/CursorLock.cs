using Unity.VisualScripting;
using UnityEngine;

namespace MyThings.Pointer
{
    public class CursorLock : MonoBehaviour
    {
        private static CursorLockMode LastMode;
        private void Start()
        {
            Lock(true);
        }

        public static void Lock(bool saveMode=false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (saveMode)
                LastMode = Cursor.lockState;
        }
        public static void Unlock(bool saveMode=false)
        {
            
            Cursor.lockState = CursorLockMode.None;
            if (saveMode)
                LastMode = Cursor.lockState;
        }
        public static void RevertBack()
        {
            Cursor.lockState = LastMode;
        }
    }
}
