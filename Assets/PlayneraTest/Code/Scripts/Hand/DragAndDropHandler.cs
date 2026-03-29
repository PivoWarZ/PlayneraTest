using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using UnityEngine;
using UnityEngine.EventSystems;


namespace PlayneraTest.Code.Scripts.Hand
{
    public class DragAndDropHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        public event Action OnDropped;
        public event Action OnDragBegin;
        public event Action OnDragEnded;
        
        private RectTransform _rectTransform;
        private Vector2 _pointerOffset;
        private Vector2 _anchorsMin;
        private Vector2 _anchorsMax;
        private Vector2 _anchoredPosition;
        private CancellationTokenSource _cancell;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDragBegin?.Invoke();
            
            Vector3 rootPosition = _rectTransform.position;
            
            // _anchorsMin = _rectTransform.anchorMin;
            // _anchorsMax = _rectTransform.anchorMax;
            // _anchoredPosition = _rectTransform.anchoredPosition;
            
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            
            _rectTransform.position = rootPosition;

            
           var isLocalPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position,
                eventData.pressEventCamera, out Vector2 localPoint);
            
            if(isLocalPoint)
                _pointerOffset = localPoint;
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            var isLocalPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, eventData.position, 
                eventData.pressEventCamera, out Vector2 localPoint);
            
            if(isLocalPoint)
                _rectTransform.anchoredPosition = localPoint - _pointerOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnded?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropped?.Invoke();
        }
        

        private void OnDestroy()
        {
            _cancell?.Cancel();
        }
    }
}