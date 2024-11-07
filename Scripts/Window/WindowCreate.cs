using UnityEngine;

namespace MyThings.Window
{
    /// <summary>
    /// Window Creator To Creator
    /// </summary>
    public class WindowCreate : MonoBehaviour
    {
        /// <summary>
        /// Create A Window Which Is Available In The ReSource Folder
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static WindowBasic Create(string Name)
        {
            var Window = Instantiate(Resources.Load<GameObject>( Name).GetComponent<WindowBasic>());
            Window.gameObject.SetActive(false);
            return Window;
        }
    }
}