using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.MakeupGirl
{
    public static class Girl
    {
        private static IGirlView _girlView;
        public static RectTransform Head => _girlView.Head;
        public static RectTransform Lips => _girlView.Lips;
        public static RectTransform Cheeks => _girlView.Сheeks;
        public static RectTransform Face => _girlView.Face;
        public static RectTransform FaceBrushRight => _girlView.FaceBrushRight;
        public static RectTransform FaceBrushLeft => _girlView.FaceBrushLeft;
        public static RectTransform Ashes => _girlView.Ashes;
        public static RectTransform BottomMakeupPosition => _girlView.BottomMakeupPosition;
        public static RectTransform TopMakeupPosition => _girlView.TopMakeupPosition;

        public static void Initialize(IGirlView girlView)
        {
            _girlView = girlView;
        }
    }
}