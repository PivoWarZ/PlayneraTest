using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class DragAndDropProxy: MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        private DragAndDropHandler _handler;

        private void Start()
        {
            _handler = GetComponentInParent<DragAndDropHandler>();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            _handler.OnDrag(eventData);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _handler.OnBeginDrag(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _handler.OnEndDrag(eventData);
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            _handler.OnDrop(eventData);
        }
    }
}