using UnityEngine;
using MyThings.Extension;
using System.Runtime.CompilerServices;
using System;

namespace MyThings.Directions
{

    /// <summary>
    /// Class Which deals with Directions
    /// </summary>
    public static class DirectionsHandler
    {
        /// <summary>
        /// Ristricted Directions ( Basic )
        /// </summary>
        public enum Direction
        {
            None,
            Forward,
            Backward,
            Up,
            Down,
            Right,
            Left
        }

        #region Private

        /// <summary>
        /// Get The Closest Direction from Given Direction
        /// </summary>
        /// <param name="direction">The Direction</param>
        /// <param name="nearestDegree">The Degree Of THe Nearest</param>
        /// <returns>The Nearest Direction</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Direction Internal_ClosestDirection(Vector2 direction,out float nearestDegree)
        {
            var up = Vector2.Dot(Vector2.up, direction);
            var right = Vector2.Dot(Vector2.right, direction);
            if (Mathf.Abs(up) > Mathf.Abs(right))
            {
                nearestDegree = up;
                return up switch
                {
                    >= 0 => Direction.Up,
                    _ => Direction.Down,
                };
            }
                
            nearestDegree = right;
            return right switch
            {
                >= 0 => Direction.Right,
                _ => Direction.Left,
            };
        }


        #endregion


        #region Public

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Get A Random Direction in 2D (Ristricted To 4 Directions)
        /// </summary>
        /// <returns>The Direction</returns>
        public static Direction GetRandom2D()
        {
            return UnityEngine.Random.Range(0, 4) switch
            {
                0 => Direction.Left,
                1 => Direction.Right,
                2 => Direction.Up,
                3 => Direction.Down,
                _ => Direction.None,
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Get A Vector In The Direction
        /// </summary>
        /// <param name="direction">The Direction</param>
        /// <returns>a Vector</returns>
        public static Vector3 GetVector(this Direction direction)
        {
            return direction switch
            {
                Direction.None => Vector3.zero,
                Direction.Forward => Vector3.forward,
                Direction.Backward => Vector3.back,
                Direction.Up => Vector3.up,
                Direction.Down => Vector3.down,
                Direction.Right => Vector3.right,
                Direction.Left => Vector3.left,
                _ => Vector3.zero,
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Get A Random Direction in 3D (Ristricted To 6 Directions)
        /// </summary>
        /// <returns>The Direction</returns>
        public static Direction GetRandom3D()
        {
            return UnityEngine.Random.Range(0, 6) switch
            {
                0 => Direction.Left,
                1 => Direction.Right,
                2 => Direction.Up,
                3 => Direction.Down,
                4 => Direction.Forward,
                5 => Direction.Backward,
                _ => Direction.None,
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Get The Reverse Of The Direction
        /// </summary>
        /// <param name="direction">The Direction To Reverse</param>
        /// <returns>The Reversed Direction</returns>
        /// <exception cref="System.NotImplementedException">When Some Random Direction is Given</exception>
        public static Direction Reverse(this Direction direction)
        {

            return direction switch
            {
                Direction.Backward => Direction.Forward,
                Direction.None => Direction.None,
                Direction.Forward => Direction.Backward,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Right => Direction.Left,
                Direction.Left => Direction.Right,
                _ => throw new System.NotImplementedException(),
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Create A Ray In The Direction With the transfrom
        /// </summary>
        /// <param name="transform">The Transform From Which We Get The Position</param>
        /// <param name="direction">The Direction Of The Ray</param>
        /// <returns>The Ray</returns>
        public static Ray GetDirectionR(this Transform transform, Direction direction)
        {
            return direction switch
            {
                Direction.Forward => new Ray(transform.position, transform.forward),
                Direction.Backward => new Ray(transform.position, -transform.forward),
                Direction.Up => new Ray(transform.position, transform.up),
                Direction.Down => new Ray(transform.position, -transform.up),
                Direction.Right => new Ray(transform.position, transform.right),
                Direction.Left => new Ray(transform.position, -transform.right),
                _ => new Ray(transform.position, transform.position),
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Allocate A Ray In The Direction With the transfrom
        /// </summary>
        /// <param name="transform">The Transform From Which We Get The Position</param>
        /// <param name="direction">The Direction Of The Ray</param>
        /// <param name="ray">The Ray To Allocate Into</param>
        /// <returns>The Ray</returns>
        public static Ray GetDirectionR(this Transform transform, Direction direction, Ray ray)
        {

            return direction switch
            {
                Direction.Forward => ray.ChangeRay(transform.position, transform.forward),
                Direction.Backward => ray.ChangeRay(transform.position, -transform.forward),
                Direction.Up => ray.ChangeRay(transform.position, transform.up),
                Direction.Down => ray.ChangeRay(transform.position, -transform.up),
                Direction.Right => ray.ChangeRay(transform.position, transform.right),
                Direction.Left => ray.ChangeRay(transform.position, -transform.right),
                _ => ray.ChangeRay(transform.position, transform.position),
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Get The Direction In The Form Of Vector3
        /// </summary>
        /// <param name="transform">The Transform From Which We Get The Position</param>
        /// <param name="direction">The Direction Of The Ray</param>
        /// <returns>The Vector3 Direction</returns>
        public static Vector3 GetDirectionV(this Transform transform, Direction direction)
        {
            return direction switch
            {
                Direction.Forward => transform.forward,
                Direction.Backward => -transform.forward,
                Direction.Up => transform.up,
                Direction.Down => -transform.up,
                Direction.Right => transform.right,
                Direction.Left => -transform.right,
                _ => transform.position,
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// It Is An Extention Function Which Is Used To Get Direction Vector From The Object To The Point
        /// </summary>
        /// <param name="transform">The Objects Transform</param>
        /// <param name="point">The Point Who'es Direction Is To Be Found</param>
        /// <returns>The Direction</returns>
        public static Vector3 GetDirection(this Transform transform, Vector3 point)
        {
            return (point - transform.position).normalized;
        }


        /// <summary>
        /// Get The Closest Direction from Given Direction
        /// </summary>
        /// <param name="direction">The Direction</param>
        /// <returns>The Nearest Direction</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Direction ClosestDirection(Vector2 direction)
        {
            direction.Normalize();
            return Internal_ClosestDirection(direction,out float _);
        }


        /// <summary>
        /// Get The Closest Direction from Given Direction
        /// </summary>
        /// <param name="direction">The Direction</param>
        /// <returns>The Nearest Direction</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Direction ClosestDirection(Vector3 direction)
        {
            direction.Normalize();
            var dir=Internal_ClosestDirection(direction, out float nearest);
            var forward = Vector3.Dot(Vector3.forward, direction);
            if (Mathf.Abs(nearest) > Mathf.Abs(forward))
                return dir;
            return forward switch
            {
                >= 0 => Direction.Forward,
                _ => Direction.Backward
            };
        }


        #endregion

    }
}