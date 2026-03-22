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
        [SerializeField] private Image Brush;
        [SerializeField] private RoundedImage Border;
        [SerializeField] private Button MakeupButton;
        private Sprite _faceBrushLeft;
        private Sprite _faceBrushRight;

        public Sprite FaceBrushLeft => _faceBrushLeft;

        public Sprite FaceBrushRight => _faceBrushRight;

        private void Awake()
        {
            MakeupButton.onClick.AddListener(MakeupRequest);
        }

        private void OnDestroy()
        {
            MakeupButton.onClick.RemoveAllListeners();
        }

        private void MakeupRequest()
        {
            OnMakeupRequest?.Invoke(gameObject);
        }

        public void SetBlush(Blush blush)
        {
            Brush.color = blush.BlushColor;
            Border.color = blush.BorderColor;
            _faceBrushLeft = blush.FaceBrushLeft;
            _faceBrushRight = blush.FaceBrushRight;
        }
    }
}
