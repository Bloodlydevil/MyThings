using MyThings.Extension;
using System;
using UnityEngine;

/// <summary>
/// A Class To Make Any Canvas Confined Inside A Bigger Canvas (Does Not Allow a canvas to go out of bound)
/// if the canvas confined is small then it will remain inside the boundary area
/// if the canvas is bigger then the bigger canvas which is confined will cover up the smaller boundary area
/// </summary>
public class CanvasContainer : MonoBehaviour
{
    [Tooltip("The Area In Which All Should Be Confined")]
    [SerializeField] private RectTransform m_Boundary;

    [Tooltip("The Canvas Scale Factor (Needs To Be Set Up For Proper Working)")]
    [field: SerializeField] public float CanvasScaleFactor { get; set; } = 1;

    public event Action OnOverLimit;


    #region Private

    /// <summary>
    /// Get The Distance The Contained object has gone out of the boundary
    /// </summary>
    /// <param name="centerDis">the distance between the center of the boundary and the confined</param>
    /// <param name="smallSize">The Small Object's Size</param>
    /// <param name="biggSize">The Big Object's Size</param>
    /// <returns>The Extra Distance Traveled</returns>
    private float GetExtra(float centerDis,float smallSize,float biggSize)
    {
        float extra = Mathf.Abs(centerDis) + smallSize / 2 - biggSize / 2;
        extra = Mathf.Max(extra, 0);
        return extra;
    }

    /// <summary>
    /// limit the confined object to be inside the boundaray
    /// </summary>
    /// <param name="Confined">The Object To Confine</param>
    /// <param name="ConfinedIsSmall">Is The Confined Object Small?</param>
    /// <returns>The Delta Position The Object Is Shifted</returns>
    private Vector2 limitConfined(RectTransform Confined, bool ConfinedIsSmall)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.lossyScale / CanvasScaleFactor;

        Vector2 WindowSize = m_Boundary.rect.size * m_Boundary.lossyScale / CanvasScaleFactor;

        Vector2 ObjectDir = (Confined.position - m_Boundary.position) / CanvasScaleFactor;

        Vector2 Extra;

        if (ConfinedIsSmall)
            Extra = new(GetExtra(ObjectDir.x, ObjectSize.x, WindowSize.x), GetExtra(ObjectDir.y, ObjectSize.y, WindowSize.y));
        else
            Extra= new(GetExtra(ObjectDir.x, WindowSize.x, ObjectSize.x), GetExtra(ObjectDir.y, WindowSize.y, ObjectSize.y));

        float Scale= (Confined.OnlyGlobalScale().x / CanvasScaleFactor);

        Vector2 Change = ObjectDir.GetSign() * Extra / Scale;

        Confined.localPosition -= Change.ToVector3(0);

        if (Change != Vector2.zero)
            OnOverLimit?.Invoke();

        return Change;
    }

    #endregion

    #region Public

    /// <summary>
    /// limit the confined object to be inside the boundaray
    /// </summary>
    /// <param name="Confined">The Object To Be Confined</param>
    /// <returns>The Delta Position The Object Is Shifted</returns>
    public Vector2 LimitConfined(RectTransform Confined)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.localScale;

        Vector2 WindowSize = m_Boundary.rect.size * m_Boundary.localScale;

        return limitConfined(Confined, ObjectSize.GetSize() < WindowSize.GetSize());
    }
    /// <summary>
    /// limit the confined object to be inside the boundaray . The Object To Confine Is Bigger Than The Boundary
    /// </summary>
    /// <param name="Confined">The Object To Be Confined</param>
    /// <returns>The Delta Position The Object Is Shifted</returns>
    public Vector2 LimitBigConfined(RectTransform Confined) => limitConfined(Confined, false);
    /// <summary>
    /// limit the confined object to be inside the boundaray . The Object To Confine Is Smaller Than the Boundary
    /// </summary>
    /// <param name="Confined">The Object To Be Confined</param>
    /// <returns>The Delta Position The Object Is Shifted</returns>
    public Vector2 LimitSmallConfined(RectTransform Confined) => limitConfined(Confined, true);

    #endregion
}
