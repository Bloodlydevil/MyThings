using MyThings.Interface;
using System;
using UnityEngine;

namespace MyThings.ScriptableObjects
{

    /// <summary>
    /// A Container to store different shapes
    /// </summary>
    [CreateAssetMenu(menuName = "MyThings/ShapesContainer")]
    public class ShapesContainerSO : ScriptableObject
    {
        [Tooltip("The Location Of The Shapes")]
        public string[] ShapesAddress;
        [Tooltip("Check If All The Names Are Ok Or Not")]
        [HideInInspector] public IAreaShapes[] Shapes;
        /// <summary>
        /// The type Of The Shape Used
        /// </summary>
        private Type Interface;



        private void OnValidate()
        {
            Interface = typeof(IAreaShapes);
            Shapes=new IAreaShapes[ShapesAddress.Length];
            for(int i = 0;i<ShapesAddress.Length;i++)
            {
                Type temp = Type.GetType(ShapesAddress[i]);
                if (temp != null)
                {
                    if (temp.GetInterface(Interface.FullName) != Interface)
                    {
                        Debug.LogError($"There Is A class Which Is Not A Type Of 3d Shape(IShapes3D) At ----> {i}    ");
                    }
                    else
                    {
                        Shapes[i] = (IAreaShapes)Activator.CreateInstance(temp);
                    }
                }
            }
        }
    }
}