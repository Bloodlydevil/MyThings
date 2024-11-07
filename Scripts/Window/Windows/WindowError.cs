using MyThings.Window.Utills;
using MyThings.Window.WindowHelper;
using UnityEngine;

namespace MyThings.Window.Windows
{
    /// <summary>
    /// A Basic Error Window
    /// </summary>
    public class WindowError:MonoBehaviour
    {
        private const string WindowWorldSpace = "PreFabs/Windows/ErrorWindow";
        private const string WindowOverLay = "PreFabs/Windows/ErrorWindowOverLay";
        private WindowBasic Window;
        private void Start()
        {
            
            Application.logMessageReceived += Application_logMessageReceived;
        }
        private void OnDestroy()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }
        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error||type==LogType.Exception||type==LogType.Warning)
            {
                if (Window == null)
                    Window = WindowCreate.Create(WindowWorldSpace);
                ((ErrorWindowHelper)Window.Helper).TextMeshProUGUI.text = stackTrace;
                Window.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// Use To Make Special Error Window With Default Things
        /// </summary>
        /// <param name="text">The Text To Show</param>
        /// <param name="WindowName">The WindowName(Full Location In Resourse Folder)</param>
        /// <returns>The Main GameObject</returns>
        public static GameObject Create(string text,string WindowName,ProperDrag.Mode mode)
        {
            WindowBasic Window=WindowCreate.Create(WindowName);
            Window.SetUp(mode);
            ((ErrorWindowHelper)Window.Helper).TextMeshProUGUI.text = text;
            Window.gameObject.SetActive(true);
            return Window.gameObject;
        }
        /// <summary>
        /// Create A WorldSpace Error
        /// </summary>
        /// <param name="text">the Text To Display</param>
        /// <returns>The Object</returns>
        public static GameObject CreateWorldSpace(string text)
        {
            return Create(text, WindowWorldSpace,ProperDrag.Mode.WorldView);
        }
        /// <summary>
        /// Create Overlay Error
        /// </summary>
        /// <param name="text">The Text To Display</param>
        /// <returns>The GameObject</returns>
        public static GameObject CreateOverLay(string text)
        {
            return Create(text, WindowOverLay, ProperDrag.Mode.ScreenOverlay);
        }
    }
}
