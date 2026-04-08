using System;
using System.Threading;
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
        private CancellationTokenSource _cancell;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDragBegin?.Invoke();
            
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