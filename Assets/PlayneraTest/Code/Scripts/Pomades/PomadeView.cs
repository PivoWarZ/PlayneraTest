using System;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadeView: MonoBehaviour, IMakeupRequester
    {
        public event Action<GameObject> OnMakeupRequest;
        public RectTransform PomadePoint;
        public RectTransform Rect;
        public Image Image;
        public Sprite LipkColor;
        [SerializeField] Button _button;

        private void Start()
        {
            _button.onClick.AddListener(MakeupRequest);
        }

        private void MakeupRequest()
        {
            OnMakeupRequest?.Invoke(gameObject);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}