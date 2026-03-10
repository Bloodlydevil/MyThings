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
            SetCursorLockMode(Default_Lock);
        }

        /// <summary>
        /// Sets the cursor lock mode to the specified value, with optional tracking of the previous or new lock state.
        /// </summary>
        /// <remarks>Use this method to change the cursor lock mode while optionally preserving the
        /// previous or new state for future reference. This can be useful for toggling between different cursor states
        /// and restoring them as needed.</remarks>
        /// <param name="mode">The cursor lock mode to apply. This value determines how the cursor is locked or confined, as defined by the
        /// CursorLockMode enumeration.</param>
        /// <param name="SaveOlder">true to save the current cursor lock state before changing it; otherwise, false. When true, the previous
        /// state is stored for later retrieval.</param>
        /// <param name="SaveNewer">true to save the new cursor lock state after it has been set; otherwise, false. When true, the updated state
        /// is stored for later retrieval.</param>
        public static void SetCursorLockMode(CursorLockMode mode,bool SaveOlder=false,bool SaveNewer=false)
        {
            if (SaveOlder)
                LastMode = Cursor.lockState;

            Cursor.lockState = mode;

            if (SaveNewer)
                LastMode = Cursor.lockState;
        }
        /// <summary>
        /// Sets the cursor lock mode to either locked or unlocked based on the specified value.
        /// </summary>
        /// <remarks>Use this method to control whether the cursor is locked or unlocked, which is
        /// commonly required in full-screen applications or games to manage user input. Saving the previous or new
        /// state can be useful for restoring cursor behavior later.</remarks>
        /// <param name="Locked">true to lock the cursor; false to unlock the cursor.</param>
        /// <param name="SaveOlder">true to save the previous cursor lock state before changing it; otherwise, false. The default is false.</param>
        /// <param name="SaveNewer">true to save the new cursor lock state after changing it; otherwise, false. The default is false.</param>
        public static void SetCursorLockMode(bool Locked, bool SaveOlder = false, bool SaveNewer = false)
        {
            if (Locked)
                SetCursorLockMode(CursorLockMode.Locked, SaveOlder, SaveNewer);
            else
                SetCursorLockMode(CursorLockMode.None, SaveOlder, SaveNewer);
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
