using UnityEngine;
using MyThings.Interface;
// has high cost for finding radius
namespace MyThings.Data
{
    /// <summary>
    /// A struct to represent sphere
    /// </summary>
    [System.Serializable]
    public struct Sphere : IShapes3D , IAreaShapes
    {
        /// <summary>
        /// The Position
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// The squar of Radius ( stored For Quick Calculation )
        /// </summary>
        public float RadiusSquar;
        /// <summary>
        /// The Radius ( Uses Sqrt)
        /// </summary>
        public float Radius { get => Mathf.Sqrt(RadiusSquar); }


        /// <summary>
        /// Constructor to create a sphere
        /// </summary>
        /// <param name="position">The Position</param>
        /// <param name="radiu">The radius</param>
        public Sphere(Vector3 position, float radiu)
        {
            Position = position;
            RadiusSquar = radiu*radiu;
        }

        /// <summary>
        /// Function to get the squar Dis From Origin
        /// </summary>
        /// <param name="x">The X Cordinate</param>
        /// <param name="y">The Y Cordinate</param>
        /// <param name="z">The Z Cordinate</param>
        /// <returns>The Squar Dis</returns>
        private float SquarDisFromOrigin(float x, float y, float z) => x * x + y * y + z * z;


        #region Interface


        public bool IsInside(Vector3 point)
        {
            return SquarDisFromOrigin(point.x - Position.x, point.y - Position.y, point.z - Position.z) <= RadiusSquar;
        }

        public bool IsInside(float x, float y, float z)
        {
            return SquarDisFromOrigin(x - Position.x, y - Position.y, z - Position.z) <= RadiusSquar;
        }

        public string GetAddress() => "MyThings.Data.Sphere";

        public void DrawGiz(Color color, Vector3 Positio, bool WireFrame)
        {
            Gizmos.color = color;
            if (WireFrame)
                Gizmos.DrawWireSphere(Positio + Position, Radius);
            else
                Gizmos.DrawSphere(Positio + Position, Radius);
        }

        public IAreaShapes Create() => new Sphere();


        #endregion
    }
}