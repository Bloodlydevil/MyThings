using UnityEngine;
/// <summary>
/// A Struct To Act As A Holder For Static Lerp
/// </summary>
public struct Vector3Move
{
    public Vector3 Position;
    public Vector3 Target;
    /// <summary>
    /// Lerp From The Current Pos To The Target
    /// </summary>
    /// <param name="t">The Lerp T</param>
    /// <returns>A Lerped Value</returns>
    public readonly Vector3 Lerp(float t) => Vector3.Lerp(Position, Target, t);
}
