using System;
using DTT.UI.ProceduralUI;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushView : MonoBehaviour, IMakeupRequester
    {
        public event Action<GameObject> OnMakeupRequest;
        [SerializeField] private Image _brush;
        [SerializeField] private RoundedImage _border;
        [SerializeField] private Button _makeupButton;
        private Sprite _faceBrushLeft;
        private Sprite _faceBrushRight;

        public Sprite FaceBrushLeft => _faceBrushLeft;

        public Sprite FaceBrushRight => _faceBrushRight;

        private void Awake()
        {
            _makeupButton.onClick.AddListener(MakeupRequest);
        }

        private void OnDestroy()
        {
            _makeupButton.onClick.RemoveAllListeners();
        }

        private void MakeupRequest()
        {
            OnMakeupRequest?.Invoke(gameObject);
        }

        public void SetBlush(Blush blush)
        {
            _brush.color = blush.BlushColor;
            _border.color = blush.BorderColor;
            _faceBrushLeft = blush.FaceBrushLeft;
            _faceBrushRight = blush.FaceBrushRight;
        }
    }
}
