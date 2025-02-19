using Unity.VisualScripting;
using UnityEngine;

namespace MyThings.Pointer
{
    /// <summary>
    /// A Cursor Lock Manager
    /// </summary>
    public class CursorLock : MonoBehaviour
    {
        [Tooltip("If The Cursor Should Be Locked By Default")]
        [SerializeField] private bool Default_Lock;

        private static CursorLockMode LastMode;

        private void Start()
        {
            if (Default_Lock)
                Lock(true);
        }

        /// <summary>
        /// Lock The Cursor And Save The Mode (To Revert Back To This Mode)
        /// </summary>
        /// <param name="SaveNewer">Save The Newer Lock State (Overrides The SaveOlder)</param>
        /// <param name="SaveOlder">Save The Older Lock State</param>
        public static void Lock(bool SaveOlder=false,bool SaveNewer=false)
        {
            if (SaveOlder)
                LastMode = Cursor.lockState;

            Cursor.lockState = CursorLockMode.Locked;

            if (SaveNewer)
                LastMode = Cursor.lockState;
        }
        /// <summary>
        /// Unlock The Cursor And Save The Mode (To Revert Back To This Mode)
        /// </summary>
        /// <param name="SaveNewer">Save The Newer Lock State (Overrides The SaveOlder)</param>
        /// <param name="SaveOlder">Save The Older Lock State</param>
        public static void Unlock(bool SaveOlder = false, bool SaveNewer = false)
        {

            if (SaveOlder)
                LastMode = Cursor.lockState;

            Cursor.lockState = CursorLockMode.None;

            if (SaveNewer)
                LastMode = Cursor.lockState;
        }

        /// <summary>
        /// Revert Back To The OlderSaved Mode
        /// </summary>
        public static void RevertBack()
        {
            Cursor.lockState = LastMode;
        }
    }
}
