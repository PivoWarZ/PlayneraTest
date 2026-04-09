using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: IBlushersViewModel, IDisposable
    {
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        private IBlushersModel _model;
        private BlushMakeupTargets _makeup;
        private bool _isMakeupProcessing;
        private CancellationTokenSource _cancell;

        public BlushersViewModel(IBlushersModel model)
        {
            _model = model;
        }

        void IBlushersViewModel.SetMakeupTarget(BlushMakeupTargets targets)
        {
            _makeup = targets;
        }
        
        void IMakeupViewModel.StartMakeUp()
        {
            if (_isMakeupProcessing)
            {
                Cancel();
                return;
            }

            _cancell = new CancellationTokenSource();
            
            _isMakeupProcessing = true;
            var token = _cancell.Token;
            
            RunMakeupRequest(token).Forget();
        }
        
        void IDisposable.Dispose()
        {
            Cancel();
        }

        private void Cancel()
        {
      		_cancell?.Cancel();
        	_cancell?.Dispose();
        	_cancell = null;
        }

        private async UniTask RunMakeupRequest(CancellationToken token)
        {
            IHandView hand = _model.Hand;
            var brushHandle = _makeup.BrushHandle;
            var brush = _makeup.Brush;
            var blush = _makeup.Blush;
            var brushHandleStartPosition = _makeup.BrushHandle.position;
            var rotateParameters = _model.GetRotateParameters();
            bool isReturn = false;
            
            try
            {
                await hand.GrabAndRotate(brushHandle, rotateParameters, token);

                brushHandle.SetParent(hand.RectTransform);
                brushHandle.SetAsLastSibling();

                var offset = brush.position - hand.RectTransform.position;
                hand.SetOffset(offset);

                await hand.MoveAsync(blush.position, token);
                await hand.PlayYoyoAnimationAsync(_model.GetYoyoPoints(blush), _model.YoyoCount, token);
                await hand.MoveToBottomMakeupPosition(token);
                await WaitingMakeUpPosition(token);
                await MakeUp(token);
                
                OnMakeupCompleted?.Invoke();
                
                await Return(token);
            }
            catch (OperationCanceledException)
            {
                using (var timeoutSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    await Return(timeoutSource.Token);
                }
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    OnMakeupCompleted?.Invoke();
                }
                
                hand.IsBack.Value = false;
            }
            async UniTask Return(CancellationToken tcn)
            {
                if(isReturn)
                    return;
                
                hand.IsBack.Value = true;
                isReturn = true;
                hand.SetOffset(Vector3.zero);
                await hand.MoveAsync(brushHandleStartPosition, tcn);

                rotateParameters.RotateDirection = Vector3.zero;
                await hand.Rotate(brushHandle, rotateParameters, tcn);

                brushHandle.SetParent(hand.RectTransform.root);
                await hand.ReturnToStartPosition(tcn);
                _isMakeupProcessing = false;
                hand.IsBack.Value = false;
            }
        }

        private async UniTask WaitingMakeUpPosition(CancellationToken token)
        {
            bool isMakeUpPosition = false;
            IHandView hand = _model.Hand;
            float returnAnimationSpeedModifier = 0.2f;
            UniTaskCompletionSource<bool> makeupTargetTask;

            while (!isMakeUpPosition)
            {
                token.ThrowIfCancellationRequested();
                
                 makeupTargetTask = new UniTaskCompletionSource<bool>();
                
                void CompleteTask()
                {
                    var point = GirlFaceMakeupPositions.Cheeks.InverseTransformPoint(_makeup.Brush.position);
                    makeupTargetTask.TrySetResult(GirlFaceMakeupPositions.Cheeks.rect.Contains(point));
                }
                
                hand.OnDropped += CompleteTask;
                
                isMakeUpPosition = await makeupTargetTask.Task.AttachExternalCancellation(token);

                try
                {
                    if (!isMakeUpPosition)
                    {
                        await hand.MoveToBottomMakeupPosition(token);
                    }
                }
                finally
                {
                    hand.OnDropped -= CompleteTask;
                }
            }
        }

        private async UniTask MakeUp(CancellationToken token)
        {
            Debug.Log($"<color=green> MAKE UP!!!! </color>");

            var yoyoPoints = _model.GetYoyoPoints(GirlFaceMakeupPositions.Cheeks);
            
            await _model.Hand.PlayYoyoAnimationAsync(yoyoPoints, 3, token);
        }
    }
}