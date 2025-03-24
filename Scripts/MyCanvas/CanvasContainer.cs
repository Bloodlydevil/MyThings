using MyThings.Extension;
using UnityEngine;

/// <summary>
/// A Class To Limit All The Movement Of The Work Area (Confined In the Fixed Area)
/// </summary>
public class CanvasContainer : MonoBehaviour
{

    [SerializeField] private RectTransform m_Confiner;

    [field: SerializeField] public float CanvasScaleFactor { get; set; }

    private float GetExtra(float centerDis,float Containedsize,float ContainerSize)
    {
        float extra = Mathf.Abs(centerDis) + Containedsize / 2 - ContainerSize / 2;
        extra = Mathf.Max(extra, 0);
        return extra;
    }
    private Vector2 limitConfined(RectTransform Confined, bool ConfinedIsSmall)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.lossyScale / CanvasScaleFactor;

        Vector2 WindowSize = m_Confiner.rect.size * m_Confiner.lossyScale / CanvasScaleFactor;

        Vector2 ObjectDir = (Confined.position - m_Confiner.position) / CanvasScaleFactor;

        Vector2 Extra;

        if (ConfinedIsSmall)
            Extra = new(GetExtra(ObjectDir.x, ObjectSize.x, WindowSize.x), GetExtra(ObjectDir.y, ObjectSize.y, WindowSize.y));
        else
            Extra= new(GetExtra(ObjectDir.x, WindowSize.x, ObjectSize.x), GetExtra(ObjectDir.y, WindowSize.y, ObjectSize.y));

        float Scale= (Confined.OnlyGlobalScale().x / CanvasScaleFactor);

        Vector2 Change = ObjectDir.GetSign() * Extra / Scale;

        Confined.localPosition -= Change.ToVector3(0);

        return Change;
    }
    public Vector2 LimitConfined(RectTransform Confined)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.localScale;

        Vector2 WindowSize = m_Confiner.rect.size * m_Confiner.localScale;

        return limitConfined(Confined, ObjectSize.GetSize() < WindowSize.GetSize());
    }
    public Vector2 LimitBigConfined(RectTransform Confined) => limitConfined(Confined, false);
    public Vector2 LimitSmallConfined(RectTransform Confined) => limitConfined(Confined, true);

    ///// <summary>
    ///// Adjust The Node To Be Inside The Limit Area
    ///// </summary>
    ///// <param name="Node">The Node To Adjust</param>
    //public void AdjustNode(RectTransform Node)
    //{
    //    Vector2 NodeSize = Node.rect.size/2;
    //    Vector2 min = Node.offsetMin.Abs() + NodeSize;
    //    Vector2 max = Node.offsetMax.Abs() + NodeSize;

    //    Vector2 size = m_WorkArea.rect.size / 2;
    //    Vector2 extraMin = min - size;
    //    Vector2 extraMax = max - size;
    //    Vector2 extra;
    //    extra.x = extraMin.x > 0 ? extraMin.x : extraMax.x > 0 ? extraMax.x : 0;
    //    extra.y = extraMin.y > 0 ? extraMin.y : extraMax.y > 0 ? extraMax.y : 0;
    //    extra /= 2 * Node.anchoredPosition.GetSign();
    //    Node.offsetMin -= extra * CanvasScaleFactor;
    //    Node.offsetMax -= extra * CanvasScaleFactor;
    //}
}
