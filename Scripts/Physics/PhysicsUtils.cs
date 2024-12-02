using MyThings.Extension;
using UnityEngine;

namespace MyThings.Physics
{

    public static class PhysicsUtils
    {
        public static bool IsInsideRectangle(Vector2 leftPos, Vector2 rightPos, Vector2 point)
        {
            return point.x.IsInside(leftPos.x, rightPos.x) && point.y.IsInside(leftPos.y, rightPos.y);
        }
    }
}