using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: IBlushersViewModel, IDisposable, INeedHandService
    {
        private BlushMakeupTargets _makeup = new BlushMakeupTargets();
        private HandView _hand;
        private bool _isMakeupProcessing;
        private CancellationTokenSource _cancell = new CancellationTokenSource();
        private IHandService _handService;

        void IBlushersViewModel.SetMakeupTarget(BlushMakeupTargets targets)
        {
            _makeup = targets;
        }
        
        void IMakeUpViewModel.StartMakeUp()
        {
            _isMakeupProcessing = true;
            
            var handPrefab = Resources.Load<HandView>("Hand");
            _hand = _handService.GetHand();
            RunMakeupRequest(_cancell.Token).Forget();
        }

        private async UniTask RunMakeupRequest(CancellationToken token)
        {
            var brushHandle = _makeup.BrushHandle;
            var brush = _makeup.Brush;
            var blush = _makeup.Blush;
            
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
            
            _hand.Offset = brush.transform.position - _hand.transform.position;
            _hand.transform.SetAsLastSibling();
            
            await _hand.MoveAsync(blush, token);
        }
        
        void IMakeUpViewModel.BreakMakeUp()
        {
            throw new System.NotImplementedException();
        }
        
        void IDisposable.Dispose()
        {
            _cancell.Cancel();
        }

        void INeedHandService.Initialize(IHandService handService)
        {
            _handService = handService;
        }
    }
}