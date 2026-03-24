using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IGirlView
    {
        RectTransform Head { get; }
        RectTransform Lips { get; }
        RectTransform Сheeks { get; }
        RectTransform Face { get; }
        RectTransform FaceBrushRight { get; }
        RectTransform FaceBrushLeft { get; }
        RectTransform Ashes { get; }

        RectTransform BottomMakeupPosition { get; }
        RectTransform TopMakeupPosition { get; }
    }
}