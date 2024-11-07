using MyThings.Interface;
using MyThings.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.PointsArea
{
    /// <summary>
    /// A Single Area
    /// </summary>
    [System.Serializable]
    public class AreaValue_Single
    {
        [Tooltip("The Shape")]
        [SerializeReference] public IAreaShapes Shape;
        [Tooltip("The Value To REturn If A Point Is Inside the Shape")]
        public float Value;
        [Tooltip("The Color With Which to Show in Gizmos")]
        public Color color;
        [Tooltip("If Should show In Editior")]
        public bool ShowInEditor=true;
        [Tooltip("If It Is Wireframed")]
        public bool WireFrame = false;
        /// <summary>
        /// Create A Area
        /// </summary>
        /// <param name="temp">The Type Of The Shape</param>
        public AreaValue_Single(IAreaShapes temp) { Shape = temp.Create(); color = UnityEngine.Random.ColorHSV(); }
    }
    /// <summary>
    /// The Area Dealer
    /// </summary>
    public class AreaValue : MonoBehaviour
    {
        [Tooltip("The Areas")]
        [SerializeField] private List<AreaValue_Single> Areas;
        [Tooltip("The Object Center")]
        [SerializeField] private Transform ObjectCenter;
        [Header("Shape to add")]
        [Tooltip("The Shapes avilable")]
        [SerializeField] private ShapesContainerSO ShapesAvaialble;
        [Tooltip("The Shape Id To Add")]
        [SerializeField] private int ShapeNo;
        [Tooltip("Click To Add A New Shape")]
        [SerializeField] private bool Add_Shape;

        #region Unity

        private void OnValidate()
        {
            if (ShapesAvaialble == null||ShapesAvaialble.Shapes==null)
                return;
            Functions.ValueCut(ref ShapeNo, 0, ShapesAvaialble.Shapes.Length-1);
            if(Add_Shape)
            {
                Add_Shape = false;
                
                Areas.Add(new AreaValue_Single(ShapesAvaialble.Shapes[ShapeNo]));
            }
        }
        private void OnDrawGizmos()
        {
            for(int i=0;i<Areas.Count;i++)
            {
                if (Areas[i].ShowInEditor)
                {
                    Areas[i].Shape.DrawGiz(Areas[i].color, ObjectCenter.position, Areas[i].WireFrame);
                }
            }
        }

        #endregion

        /// <summary>
        /// Check the Value for the Position
        /// </summary>
        /// <param name="pos">The psotion</param>
        /// <param name="def">The Default value to return if not found</param>
        /// <returns>The Value associated with position</returns>
        public float CheckValueFor(Vector3 pos,int def=1)
        {
            pos -= ObjectCenter.position;
            for(int i=0;i<Areas.Count;i++)
            {
                if (Areas[i].Shape.IsInside(pos)) 
                {
                    return Areas[i].Value;
                }
            }
            return def;
        }
    }
}