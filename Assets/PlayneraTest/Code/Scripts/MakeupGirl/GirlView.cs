using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.MakeupGirl
{
    public class GirlView: MonoBehaviour, IGirlView
    {
        [SerializeField] private RectTransform _head;
        [SerializeField] private RectTransform _lips;
        [SerializeField] private RectTransform _cheeks;
        [SerializeField] private RectTransform _face;
        [SerializeField] private RectTransform _faceBrushRight;
        [SerializeField] private RectTransform _faceBrushLeft;
        [SerializeField] private RectTransform _ashes;
        [SerializeField] private RectTransform _bottomMakeupPosition;
        [SerializeField] private RectTransform _topMakeupPosition;

        public RectTransform Head => _head;

        public RectTransform Lips => _lips;

        public RectTransform Face => _face;

        public RectTransform FaceBrushRight => _faceBrushRight;

        public RectTransform FaceBrushLeft => _faceBrushLeft;

        public RectTransform Ashes => _ashes;

        public RectTransform BottomMakeupPosition => _bottomMakeupPosition;

        public RectTransform TopMakeupPosition => _topMakeupPosition;

        public RectTransform Сheeks => _cheeks;

        private void Awake()
        {
            Girl.Initialize(this);
        }
    }
}