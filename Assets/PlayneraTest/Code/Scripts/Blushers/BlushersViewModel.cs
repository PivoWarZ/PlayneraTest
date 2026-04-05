using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using TMPro;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: IBlushersViewModel, IDisposable, INeedHandService
    {
        public event Action OnMakeUpAnomationCompleted;
        private BlushMakeupTargets _makeup = new BlushMakeupTargets();
        private IHandView _hand;
        private bool _isMakeupProcessing;
        private CancellationTokenSource _cancell;
        private IHandService _handService;

        void IBlushersViewModel.SetMakeupTarget(BlushMakeupTargets targets)
        {
            _makeup = targets;
        }
        
        void IMakeUpViewModel.StartMakeUp()
        {
            if (_isMakeupProcessing)
            {
                Cancel();
                return;
            }

            _cancell = new CancellationTokenSource();
            
            _isMakeupProcessing = true;
            _hand = _handService.GetHand();
            var token = _cancell.Token;
            
            RunMakeupRequest(token).Forget();
        }
        
        void IDisposable.Dispose()
        {
            _cancell?.Cancel();
            _cancell?.Dispose();

        }

        private void Cancel()
        {
            _cancell?.Cancel();
            _cancell?.Dispose();
            Debug.Log($"<color=green>Cancellation MakeUp!</color>");
        }

        private async UniTask RunMakeupRequest(CancellationToken token)
        {
            var brushHandle = _makeup.BrushHandle;
            var brush = _makeup.Brush;
            var blush = _makeup.Blush;
            var yoyoPoints = _makeup.Blush.GetComponent<IYoyoMakeup>().YoyoPoints;
            int yoyoCount = 3;
            float yoyoSpeed = _hand.MoveTime / 12;
            var brushHandleStartPosition = _makeup.BrushHandle.position;
            float backAnimationSpeedModifier = 0.3f;
            var rotateParameters = GetRotateParameters();
            
            using var registration = token.Register(() =>
            {
                Cancel();
                _cancell = new CancellationTokenSource();
                var token = _cancell.Token;
                Return(token).Forget();
            });
            
            async UniTask Return(CancellationToken token)
            {
                _hand.SetOffset(Vector3.zero);
                Settings.AnimationSpeedModifier = backAnimationSpeedModifier;
                await _hand.MoveAsync(brushHandleStartPosition, token).AttachExternalCancellation(token);

                rotateParameters.RotateDirection = Vector3.zero;
                await _hand.Rotate(brushHandle, rotateParameters, token).AttachExternalCancellation(token);

                brushHandle.SetParent(_hand.RectTransform.root);
                await _hand.ReturnToStartPosition(token).AttachExternalCancellation(token);

                Settings.AnimationSpeedModifier = 1;
                OnMakeUpAnomationCompleted?.Invoke();
                _isMakeupProcessing = false;
            }

            try
            {
                await _hand.GrabAndRotate(brushHandle, rotateParameters, token);

                brushHandle.SetParent(_hand.RectTransform);
                brushHandle.SetAsLastSibling();

                var offset = brush.position - _hand.RectTransform.position;
                _hand.SetOffset(offset);

                await _hand.MoveAsync(blush.position, token);
                await _hand.PlayYoyoAnimationAsync(yoyoPoints, yoyoCount, token);
                await _hand.MoveToBottomMakeupPosition(token);
                await WaitingMakeUpPosition(token);
                await MakeUp(token);
                await Return(token);
                
            }
            finally
            {
                if (_isMakeupProcessing)
                {
                    Cancel();
                    _cancell = new CancellationTokenSource();
                    var finallyToken = _cancell.Token;
                    Return(finallyToken).Forget();
                }
                else
                {
                    Cancel();
                }
            }
            
        }

        private async UniTask WaitingMakeUpPosition(CancellationToken token)
        {
            bool isMakeUpPosition = false;
            float returnAnimationSpeedModifier = 0.2f;
            UniTaskCompletionSource<bool> makeupTargetTask;

            while (!isMakeUpPosition)
            {
                token.ThrowIfCancellationRequested();
                
                 makeupTargetTask = new UniTaskCompletionSource<bool>();
                
                void CompleteTask()
                {
                    var point = Girl.Cheeks.InverseTransformPoint(_makeup.Brush.position);
                    makeupTargetTask.TrySetResult(Girl.Cheeks.rect.Contains(point));
                }
                
                _hand.OnDropped += CompleteTask;
                
                isMakeUpPosition = await makeupTargetTask.Task.AttachExternalCancellation(token);
                Debug.Log($"<color=yellow> {isMakeUpPosition} </color>");

                try
                {
                    if (!isMakeUpPosition)
                    {
                        Settings.AnimationSpeedModifier = returnAnimationSpeedModifier;
                        await _hand.MoveToBottomMakeupPosition(token);
                        Settings.AnimationSpeedModifier = 1f;
                    }
                }
                finally
                {
                    _hand.OnDropped -= CompleteTask;
                }
            }
        }

        private async UniTask MakeUp(CancellationToken token)
        {
            Debug.Log($"<color=green> MAKE UP!!!! </color>");
            
            List<Vector3> yoyoPoints = new List<Vector3>();
            yoyoPoints.Add(Girl.FaceBrushLeft.position);
            yoyoPoints.Add(Girl.FaceBrushRight.position);
            
            await _hand.PlayYoyoAnimationAsync(yoyoPoints, 3, token);
        }

        private RotationParameters GetRotateParameters()
        {
            RotationParameters parameters = new RotationParameters
            {
                RotateDirection = new Vector3(0, 0, -90),
                RotateTime = 0.2f,
                ScaleTime = 0.2f,
                ScaleFactor = 1.15f,
            };
            
            return parameters;
        }

        void IMakeUpViewModel.BreakMakeUp()
        {
            Cancel();
        }

        void INeedHandService.Initialize(IHandService handService)
        {
            _handService = handService;
        }
    }
}