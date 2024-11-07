using UnityEngine;

namespace MyThings.Interface
{
    /// <summary>
    /// Interface used by AreaPoints Shapes
    /// </summary>
    public interface IAreaShapes : ITypeAddress, IShapes
    {
        /// <summary>
        /// Function Used To Draw Gizmoz 
        /// </summary>
        /// <param name="color">The Color Of The Area</param>
        /// <param name="Positio">Extra Position To be Added</param>
        /// <param name="WireFrame">If the Draw Should be Wireframed</param>
        public void DrawGiz(Color color,Vector3 Positio,bool WireFrame);
        /// <summary>
        /// Create a New Copy Of The Shape Type
        /// </summary>
        /// <returns>The New Copy</returns>
        public IAreaShapes Create();
    }
}
