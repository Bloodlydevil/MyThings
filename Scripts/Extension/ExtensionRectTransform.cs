using MyThings.Window.Windows;
using UnityEngine;


namespace MyThings.Extension
{

    /// <summary>
    /// A Class To Deal With The Classes Of The Rect Treansform
    /// </summary>
    public static class ExtensionRectTransform
    {
        /// <summary>
        /// Get The Anchor Min Based On The Anchor
        /// </summary>
        /// <param name="anchor">The Anchor</param>
        /// <returns>Vector2 Representing Anchor</returns>
        public static Vector2 GetAnchorMin(Anchors anchor)
        {
            return anchor switch
            {
                Anchors.Full_Stretch => new(0,0),
                Anchors.Top_Left => new(0, 1),
                Anchors.Top_Center => new(0.5f, 1),
                Anchors.Top_Right => new(1, 1),
                Anchors.Top_Stretch => new(0, 1),
                Anchors.Left_Stretch => new(0, 0),
                Anchors.Middle_Left => new(0, 0.5f),
                Anchors.Middle_Center => new(0.5f, 0.5f),
                Anchors.Middle_Right => new(1, 0.5f),
                Anchors.Middle_Stretch => new(0,0.5f),
                Anchors.Center_Stretch => new(0.5f,0),
                Anchors.Bottom_Left => new(0,0),
                Anchors.Bottom_Center => new(0.5f,0),
                Anchors.Bottom_Right => new(1,0),
                Anchors.Bottom_Stretch => new(0,0),
                Anchors.Right_Stretch => new(1,0),
                _ => new(0,0),
            };
        }
        /// <summary>
        /// Get The Anchor Max Based On The Anchor
        /// </summary>
        /// <param name="anchor">The Anchor</param>
        /// <returns>Vector2 Representing Anchor</returns>
        public static Vector2 GetAnchorMax(Anchors anchor)
        {
            return anchor switch
            {
                Anchors.Full_Stretch => new(1,1),
                Anchors.Top_Left => new(0,1),
                Anchors.Top_Center => new(0.5f,1),
                Anchors.Top_Right => new(1,1),
                Anchors.Top_Stretch => new(1,1),
                Anchors.Left_Stretch => new(0,1),
                Anchors.Middle_Left => new(0,0.5f),
                Anchors.Middle_Center => new(0.5f,0.5f),
                Anchors.Middle_Right => new(1, 0.5f),
                Anchors.Middle_Stretch => new(1,0.5f),
                Anchors.Center_Stretch => new(0.5f,1),
                Anchors.Bottom_Left => new(0,0),
                Anchors.Bottom_Center => new(0.5f,0),
                Anchors.Bottom_Right => new(1,0),
                Anchors.Bottom_Stretch => new(1,0),
                Anchors.Right_Stretch => new(1,1),
                _ => new(0,0),
            };
        }
        /// <summary>
        /// A Extention Function To Set The X Position Of The RectTransfrom
        /// </summary>
        /// <param name="rectTransform">The Rect Transform</param>
        /// <param name="x">The Position X</param>
        public static void SetPosX(this RectTransform rectTransform, float x)
        {
            Vector2 rect = rectTransform.anchoredPosition;
            rect.x = x;
            rectTransform.anchoredPosition = rect;
        }
        /// <summary>
        /// A Extention Function To Set The Y Position Of The RectTransfrom
        /// </summary>
        /// <param name="rectTransform">The Rect Transform</param>
        /// <param name="y">The Position X</param>
        public static void SetPosY(this RectTransform rectTransform, float y)
        {
            Vector2 rect = rectTransform.anchoredPosition;
            rect.y = y;
            rectTransform.anchoredPosition = rect;
        }
        /// <summary>
        /// A Extention Function To Set The Position Of The RectTransfrom
        /// </summary>
        /// <param name="rectTransform">The Rect Transform</param>
        /// <param name="pos">The Position</param>
        public static void SetPos(this RectTransform rectTransform, Vector2 pos)
        {
            rectTransform.anchoredPosition = pos;
        }
        /// <summary>
        /// Set The Anchor Of The RectTransform
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="anchor"></param>
        public static void SetAnchor(this RectTransform rectTransform,Anchors anchor)
        {
            rectTransform.anchorMax=GetAnchorMax(anchor);
            rectTransform.anchorMin=GetAnchorMin(anchor);
        }
        public static bool Contains(this RectTransform rectTransform,Vector2 point)
        {
            WindowObjectViewer.Create(rectTransform.rect);
            return rectTransform.rect.Contains( point.Print());
        }
        public static Vector2 OnlyGlobalScale(this RectTransform rectTransform)
        {
            return rectTransform.lossyScale/(Vector2)rectTransform.localScale;
        }
    }
    public enum Anchors
    {
        Full_Stretch,
        Top_Left,
        Top_Center,
        Top_Right,
        Top_Stretch,
        Left_Stretch,
        Middle_Left,
        Middle_Center,
        Middle_Right,
        Middle_Stretch,
        Center_Stretch,
        Bottom_Left,
        Bottom_Center,
        Bottom_Right,
        Bottom_Stretch,
        Right_Stretch,
    }
}