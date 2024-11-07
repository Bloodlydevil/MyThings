using UnityEngine;


namespace MyThings.Extension
{

    /// <summary>
    /// A Class Which Deals With The Rays
    /// </summary>
    public static class ExtensionRay
    {
        /// <summary>
        /// An Extention Function To change The Ray Origin and Direction (NO Allocation)
        /// </summary>
        /// <param name="ray">The Ray</param>
        /// <param name="origin">New Origin</param>
        /// <param name="direction">New Direction</param>
        /// <returns>The new Ray With Changed origin and Direction</returns>
        public static Ray ChangeRay(this Ray ray, Vector3 origin, Vector3 direction)
        {
            ray.direction = direction;
            ray.origin = origin;
            return ray;
        }


        /// <summary>
        /// An Extention Function To change The Ray Origin (NO Allocation)
        /// </summary>
        /// <param name="ray">The Ray</param>
        /// <param name="origin">New Origin</param>
        /// <returns>The new Ray With Changed origin</returns>
        public static Ray ChangeRayOrigin(this Ray ray, Vector3 origin)
        {
            ray.origin = origin;
            return ray;
        }


        /// <summary>
        /// An Extention Function To change The Ray  Direction (NO Allocation)
        /// </summary>
        /// <param name="ray">The Ray</param>
        /// <param name="Direction">New Direction</param>
        /// <returns>The new Ray With ChangedDirection</returns>
        public static Ray ChangeRayDirection(this Ray ray, Vector3 Direction)
        {
            ray.direction = Direction;
            return ray;
        }
    }
}