using MyThings.ExtendableClass;
using UnityEngine.SceneManagement;

namespace MyThings.Multiplayer.Scene
{
    /// <summary>
    /// A Class To Load Scene With Some Extra Functionality
    /// </summary>
    public class NetworkSceneLoader : NetworkSingleton_C<NetworkSceneLoader>
    {
        /// <summary>
        /// Load the Scene
        /// </summary>
        /// <param name="Scene">Scene Name</param>
        /// <param name="mode">Loading mode</param>
        public void Load(string Scene, LoadSceneMode mode)
        {
            NetworkManager.SceneManager.LoadScene(Scene, mode);
        }
    }
}