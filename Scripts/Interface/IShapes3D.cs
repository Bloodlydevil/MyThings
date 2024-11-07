namespace MyThings.Interface
{
    /// <summary>
    /// An Interface Which All The self Created 3d Shapes Must Have In Order To Work Well With Other Things
    /// </summary>
    public interface IShapes3D : IShapes
    {
        /// <summary>
        /// Function To Check If The Point Is Inside The Shap Or Not
        /// </summary>
        /// <param name="x">The X Position</param>
        /// <param name="y">The Y Position</param>
        /// <param name="z">The Z Position</param>
        /// <returns>If The Point Is Inside The Shape</returns>
        public bool IsInside(float x, float y, float z);
    }
}
