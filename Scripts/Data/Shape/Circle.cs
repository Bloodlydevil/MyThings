using MyThings.Interface;
using UnityEngine;

namespace MyThings.Data
{
    /// <summary>
    /// A Data Holder For Circle
    /// </summary>
    [System.Serializable]
    public struct Circle : IShapes2D , IAreaShapes
    {
        /// <summary>
        /// The Position
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// The Squar Of Radius
        /// </summary>
        public float RadiusSquar;
        /// <summary>
        /// The Radius ( Uses Sqrt)
        /// </summary>
        public float Radius { get => Mathf.Sqrt(RadiusSquar); }

        /// <summary>
        /// A Constructor To Create A Circle
        /// </summary>
        /// <param name="position">The Position Of The Circle </param>
        /// <param name="radiu">The Radius Of The Circle</param>
        public Circle(Vector2 position, float radiu)
        {
            Position = position;
            RadiusSquar = radiu * radiu;
        }

        /// <summary>
        /// Function to get the squar Dis From Origin
        /// </summary>
        /// <param name="x">The X Cordinate</param>
        /// <param name="y">The Y Cordinate</param>
        /// <returns>The Squar Dis</returns>
        private float SquarDisFromOrigin(float x, float y) => x * x + y * y;


        #region Interface


        public bool IsInside(Vector3 point)
        {
            return SquarDisFromOrigin(point.x - Position.x, point.y - Position.y) <= RadiusSquar;
        }

        public bool IsInside(float x, float y)
        {
            return SquarDisFromOrigin(x - Position.x, y - Position.y) <= RadiusSquar;
        }

        public string GetAddress() => "MyThings.Data.Circle";

        public void DrawGiz(Color color, Vector3 Positio, bool WireFrame)
        {
            Gizmos.color = color;
            if (WireFrame)
                Gizmos.DrawWireSphere(Positio + (Vector3)Position, Radius);
            else
                Gizmos.DrawSphere(Positio + (Vector3)Position, Radius);
        }

        public IAreaShapes Create() => new Circle();

        #endregion
    }
}