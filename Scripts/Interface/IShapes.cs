using UnityEngine;

namespace MyThings.Interface
{
    /// <summary>
    /// An interface which all the shapes mut impliment
    /// </summary>
    public interface IShapes
    {
        /// <summary>
        /// Function To Check If The Point Is Inside The Shape Or Not
        /// </summary>
        /// <param name="point">The Point</param>
        /// <returns>If The Point Is Inside The Shape</returns>
        public bool IsInside(Vector3 point);
    }
}