using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Base
{
    public abstract class MakeupViewModelBase
    {
        protected IMakeupModel _model;
        protected bool _isMakeupProcessing;
        protected bool _isReturn;
        private TakenItem _item;
        
        protected async UniTask MoveToMakeUpPositionAsync(CancellationToken token)
        {
            await _model.Hand.MoveAsync(_model.DragPosition, token);
        }

        protected async UniTask PlayYoyoAnimationAsync(RectTransform rect, CancellationToken token)
        {
            await _model.Hand.PlayYoyoAnimationAsync(GetYoyoPoints(rect), _model.YoyoCount, token);
        }

        protected async UniTask MoveAsync(RectTransform rect, CancellationToken token)
        {
            await _model.Hand.MoveAsync(rect.position, token);
        }

        protected void SetHandOffset(RectTransform rect)
        {
            var offset = rect.position - _model.Hand.RectTransform.position;
            _model.Hand.SetOffset(offset);
        }

        protected async UniTask GrabMakeupAsync(RectTransform rect,
            RotateParameters rotateParameters, CancellationToken token)
        {
            _item = GetTakenItem(rect);

            await _model.Hand.GrabAndRotate(rect, rotateParameters, token);
            
            rect.SetParent(_model.Hand.RectTransform);
            rect.SetAsLastSibling();
        }

        private TakenItem GetTakenItem(RectTransform rect)
        {
            var takenItem = new TakenItem();
            takenItem.Item = rect;
            takenItem.Parent = rect.parent;
            takenItem.StartPosition = rect.position;
            
            return takenItem;
        }

        protected async UniTask WaitingMakeUpPositionAsync(RectTransform makeupPoint, CancellationToken token)
        {
            bool isMakeUpPosition = false;
            UniTaskCompletionSource<bool> makeupTargetTask;

            while (!isMakeUpPosition)
            {
                token.ThrowIfCancellationRequested();
                
                 makeupTargetTask = new UniTaskCompletionSource<bool>();
                
                void CompleteTask()
                {
                    var point = _model.MakeupZone.InverseTransformPoint(makeupPoint.position);
                    makeupTargetTask.TrySetResult(_model.MakeupZone.rect.Contains(point));
                }
                
                _model.Hand.OnDragEnded += CompleteTask;
                
                isMakeUpPosition = await makeupTargetTask.Task.AttachExternalCancellation(token);

                _model.Hand.OnDragEnded -= CompleteTask;
                
                try
                {
                    if (!isMakeUpPosition)
                    {
                        await _model.Hand.MoveAsync(_model.DragPosition, token);
                    }
                }
                finally
                {
                    makeupTargetTask.TrySetCanceled();
                }
            }
        }
        
        protected List<Vector3> GetYoyoPoints(RectTransform rect)
        {
            var canYoyo = rect.TryGetComponent<IYoyoMakeup>(out var yoyo);

            if (canYoyo)
            {
                return yoyo.YoyoPoints;
            }

            throw new ArgumentNullException();
        }

        protected async UniTask MakeUpAsync(CancellationToken token)
        {
            Debug.Log($"<color=green> MAKE UP!!!! </color>");

            var yoyoPoints = GetYoyoPoints(_model.MakeupZone);
            
            await _model.Hand.PlayYoyoAnimationAsync(yoyoPoints, _model.YoyoCount, token);
        }
        
        protected async UniTask ReturnAsync(CancellationToken token)
        {
            Debug.Log($"Is return {_isReturn}");
            if(_isReturn)
                return;
                
            _model.Hand.IsBack.Value = true;
            _isReturn = true;
            _model.Hand.SetOffset(Vector3.zero);
            await _model.Hand.MoveAsync(_item.StartPosition, token);
            
            await RotateAsync(token);

            _item.Item.SetParent(_item.Parent);
            await _model.Hand.ReturnToStartPosition(token);
            _model.Hand.IsBack.Value = false;
        }
        
        protected virtual async UniTask RotateAsync(CancellationToken token) {  }
    }
}