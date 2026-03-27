using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: IBlushersViewModel, IDisposable
    {
        private BlushMakeupTargets _makeup = new BlushMakeupTargets();
        private HandView _hand;
        private bool _isMakeupProcessing;
        private CancellationTokenSource _cancell = new CancellationTokenSource();
        
        void IBlushersViewModel.SetMakeupTarget(BlushMakeupTargets targets)
        {
            _makeup = targets;
        }
        
        void IMakeUpViewModel.StartMakeUp()
        {
            _isMakeupProcessing = true;
            
            var handPrefab = Resources.Load<HandView>("Hand");
            _hand = GameObject.Instantiate(handPrefab, _makeup.BrushHandle.transform);
            RunMakeupRequest(_cancell.Token).Forget();
        }

        private async UniTask RunMakeupRequest(CancellationToken token)
        {
            var brushHandle = _makeup.BrushHandle;
            var brush = _makeup.Brush;
            
            await _hand.MoveAsync(brushHandle, token);
            
            brushHandle.SetParent(_hand.transform);
            
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(brushHandle.transform.DORotate(new Vector3(0, 0, -90), 0.3f))
                .OnComplete(() => task.TrySetResult());

            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                sequence.Kill();
            });
            
            await task.Task;
            
            /*
            _hand.Offset = brush.position - _hand.transform.position;
            
            await _hand.MoveAsync(_makeup.BlushTransform.gameObject.GetComponent<RectTransform>());

            var rect = _makeup.BlushTransform.GetComponent<RectTransform>().rect;
            
            Vector3 localLeft = new Vector3(rect.xMin, rect.yMin + rect.height / 2, 0);
            Vector3 localRight = new Vector3(rect.xMax, rect.yMin + rect.height / 2, 0);
            
            
            
            
            await _hand.MoveAsync(Girl.BottomMakeupPosition);*/
        }
        
        void IMakeUpViewModel.BreakMakeUp()
        {
            throw new System.NotImplementedException();
        }
        
        void IDisposable.Dispose()
        {
            _cancell.Cancel();
        }
    }
}