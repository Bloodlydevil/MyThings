using MyThings.Window.Utills;
using MyThings.Window.WindowHelper;
using UnityEngine;
using UnityEngine.Events;
namespace MyThings.Window.Windows
{
    public class WindowPrompt : MonoBehaviour
    {
        private const string WindowWorldSpace = "PreFabs/Windows/PromptWindow";
        private const string WindowOverLayScreen = "PreFabs/Windows/PromptWindowOverLay";
        /// <summary>
        /// Use To Create Basic Prompt Window
        /// </summary>
        /// <param name="text">The Text To Display</param>
        /// <param name="OnAccept">The Function To Call When User Clicks Tick</param>
        /// <param name="OnReject">the Function To Call When User Clicks Cross</param>
        /// <param name="WindowName">The Full Window Name In Resource Folder</param>
        /// <returns>The GameObject</returns>
        public static GameObject Create(string text, UnityAction OnAccept, UnityAction OnReject,string WindowName, WindowMode mode)
        {
            WindowBasic obj;
            obj = WindowCreate.Create(WindowName);
            obj.SetUp(mode);
            var helper = ((PromptWindowHelper)obj.Helper);
            helper.TextArea.text = text;
            helper.Close.onClick.AddListener(OnReject);
            helper.Accept.onClick.AddListener(OnAccept); 
            helper.Close.onClick.AddListener(()=>Destroy(obj.gameObject));
            helper.Accept.onClick.AddListener(() => Destroy(obj.gameObject));
            obj.gameObject.SetActive(true);
            return obj.gameObject;
        }
        /// <summary>
        /// Create A Basic WorldSpace Prompt
        /// </summary>
        /// <param name="text">The Text To Display</param>
        /// <param name="OnAccept">The Function To Call When User Clicks Tick</param>
        /// <param name="OnReject">the Function To Call When User Clicks Cross</param>
        /// <returns>The GameObject</returns>
        public static GameObject CreateWorldSpace(string text, UnityAction OnAccept, UnityAction OnReject)
        {
            return Create(text, OnAccept, OnReject, WindowWorldSpace, WindowMode.WorldSpace);
        }
        /// <summary>
        /// Create A Basic OverLay Prompt
        /// </summary>
        /// <param name="text">The Text To Display</param>
        /// <param name="OnAccept">The Function To Call When User Clicks Tick</param>
        /// <param name="OnReject">the Function To Call When User Clicks Cross</param>
        /// <returns>The GameObject</returns>
        public static GameObject CreateOverLayScreen(string text, UnityAction OnAccept, UnityAction OnReject)
        {
            return Create(text, OnAccept, OnReject, WindowOverLayScreen, WindowMode.ScreenSpace_OverLay);
        }
    }
}