using System;
using System.Collections.Generic;
using DTT.UI.ProceduralUI;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushView : MonoBehaviour, IMakeupRequester, IYoyoMakeup
    {
        public event Action<GameObject> OnMakeupRequest;
        [SerializeField] private Image _brush;
        [SerializeField] private RoundedImage _border;
        [SerializeField] private Button _makeupButton;
        [SerializeField] private List<RectTransform> _yoyoPoints;
        private Sprite _faceBrushLeft;
        private Sprite _faceBrushRight;

        public Sprite FaceBrushLeft => _faceBrushLeft;

        public Sprite FaceBrushRight => _faceBrushRight;

        public List<Vector3> YoyoPoints
        {
            get
            {
                List<Vector3> yoyoPoints = new List<Vector3>();
                _yoyoPoints.ForEach(p => yoyoPoints.Add(p.transform.position));
                return yoyoPoints;
            }
        }

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
