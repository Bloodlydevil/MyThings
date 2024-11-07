using UnityEngine;
using MyThings.Pooling;
using MyThings.ExtendableClass;

namespace MyThings.TextPopUpSystem
{

    /// <summary>
    /// text Pop Up System
    /// </summary>
    public class Text_PopUpSystem : Singleton<Text_PopUpSystem>
    {

        [Tooltip("The Text Pop ")]
        [SerializeField] private GameObject Text_PopPreFab;
        [Tooltip("transfrom of The camera ( So The Text Face The Camera )")]
        [SerializeField] private Transform _MainCameraT;

        /// <summary>
        /// The Name Of The String To Spawn Objects
        /// </summary>
        private static string NameOfPreFab;

        #region Unity

        override protected void Awake()
        {
            base.Awake();
            NameOfPreFab = Text_PopPreFab.name;
            PoolingUnitySystem.Instance.TryCreateNewPool<Text_Pop>(NameOfPreFab, 50, Text_PopPreFab, transform);
        }
        private void OnValidate()
        {
            if (Text_PopPreFab != null && !Text_PopPreFab.TryGetComponent(out IText_Pop _))
            {
                Debug.LogError("Text Pop Up Does Not Have Text_Pop Script Attached");
                Text_PopPreFab = null;
            }
        }

        #endregion


        #region Public

        //Make MorePop Up For More Customized Things
        /// <summary>
        /// A Function To Create Text Pop Up 
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="pos">Teh Position</param>
        /// <param name="MaxTime">The Max Time Of The Text To Stay</param>
        /// <param name="TimeScale">The TimeScale</param>
        public static void PopUp(string text, Vector3 pos, float MaxTime = -1, bool TimeScale = true)
        {
            if (Instance == null)
                return;
            PoolingUnitySystem.Instance.SpawnGameObject<Text_Pop>(NameOfPreFab, pos, Instance._MainCameraT.rotation).SetText(text, MaxTime, TimeScale);
        }
        /// <summary>
        /// Function To Store The Pop Text To Pool
        /// </summary>
        /// <param name="gameObjectt"></param>
        public static void UnPopUp(Text_Pop gameObjectt)
        {
            PoolingUnitySystem.Instance.ReturnObjectToPool(NameOfPreFab, gameObjectt);
        }

        #endregion


    }
}