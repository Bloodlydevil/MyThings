using MyThings.Window.Utills;
using MyThings.Window.WindowHelper;
using UnityEngine;
namespace MyThings.Window.Windows
{
    public class WindowObjectViewer : MonoBehaviour
    {
        private const string WindowOverLap = "PreFabs/Windows/ObjectViewer/ObjectViewerWindowOverLay";
        public static WindowBasic Create(object obj, string Name = null)
        {
            var window = WindowCreate.Create(WindowOverLap);
            window.SetUp(WindowMode.ScreenSpace_OverLay);
            window.Title = Name ?? obj.ToString();
            ((OVWindowHelper)window.Helper).SetUp(obj);
            window.gameObject.SetActive(true);
            return window;
        }
    }
}