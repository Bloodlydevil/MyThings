using MyThings.Extension;
using UnityEngine;

/// <summary>
/// A Class To Limit All The Movement Of The Work Area (Confined In the Fixed Area)
/// </summary>
public class CanvasContainer : MonoBehaviour
{

    [SerializeField] private RectTransform m_Confiner;
    //[SerializeField] private RectTransform m_WorkView;
    //[SerializeField] private RectTransform m_WorkArea;

    [field: SerializeField] public float CanvasScaleFactor { get; set; }

    private float GetExtra(float centerDis,float Containedsize,float ContainerSize)
    {
        float extra = Mathf.Abs(centerDis) + Containedsize / 2 - ContainerSize / 2;
        extra = Mathf.Max(extra.Print(), 0);
        return extra;
    }
    public Vector2 LimitConfined(RectTransform Confined)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.localScale;

        Vector2 WindowSize = m_Confiner.rect.size * m_Confiner.localScale;

        if (ObjectSize.GetSize() < WindowSize.GetSize())
        {
            return LimitSmallConfined(Confined);
        }
        else
        {
            return LimitBigConfined(Confined);
        }
    }
    public Vector2 LimitBigConfined(RectTransform Confined)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.localScale;

        Vector2 WindowSize = m_Confiner.rect.size * m_Confiner.localScale;

        Vector2 ObjectDir = Confined.position - m_Confiner.position;

        Vector2 Extra = new(GetExtra(ObjectDir.x, WindowSize.x, ObjectSize.x), GetExtra(ObjectDir.y, WindowSize.y, ObjectSize.y));

        Vector2 Change = ObjectDir.GetSign() * Extra;

        Confined.anchoredPosition -= Change;

        return Change;
    }
    public Vector2 LimitSmallConfined(RectTransform Confined)
    {
        Vector2 ObjectSize = Confined.rect.size * Confined.localScale;

        Vector2 WindowSize = m_Confiner.rect.size * m_Confiner.localScale;

        Vector2 ObjectDir = Confined.position - m_Confiner.position;

        Vector2 Extra = new(GetExtra(ObjectDir.x, ObjectSize.x, WindowSize.x), GetExtra(ObjectDir.y, ObjectSize.y, WindowSize.y));

        Vector2 Change = ObjectDir.GetSign() * Extra;

        Confined.anchoredPosition -= Change;
        return Change;
    }
    ///// <summary>
    ///// Adjust The Work Area If It Has Gone Outside The Limit
    ///// </summary>
    ///// <returns>The Delta Change The Work Area Has Done</returns>
    //public Vector2 AdjustDrawArea()
    //{
    //    Vector2 Maxdis = m_WorkArea.rect.size * m_WorkArea.localScale / 2;
    //    Vector2 CurrentDis = m_WorkArea.anchoredPosition.Abs() + m_WorkView.rect.size / 2;
    //    if(CurrentDis.IsAnyGreater(Maxdis,out Vector2 Value))
    //    {
    //        Vector2 Sign = m_WorkArea.anchoredPosition.GetSign();
    //        m_WorkArea.anchoredPosition -= new Vector2(Value.x > 0 ? Value.x * Sign.x : 0, Value.y > 0 ? Value.y * Sign.y : 0);
    //        return -Value;
    //    }
    //    return Vector2.zero;
    //}

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
