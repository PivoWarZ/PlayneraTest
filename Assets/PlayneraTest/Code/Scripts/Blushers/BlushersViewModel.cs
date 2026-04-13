using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Base;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: MakeupViewModelBase, IBlushersViewModel, IDisposable
    {
        public event Action OnMakeup;
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        private BlushMakeupTargets _makeup;
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
            _isReturn = false;
            _isMakeupProcessing = true;
            var token = _cancell.Token;
            
            ((IMakeupViewModel)this).RunMakeupAsync(token).Forget();
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

        async UniTask IMakeupViewModel.RunMakeupAsync(CancellationToken token)
        {
            IHandView hand = _model.Hand;
            var brushHandle = _makeup.BrushHandle;
            var brush = _makeup.Brush;
            var blush = _makeup.Blush;
            var rotateParameters = _model.GetRotateParameters();
            bool isReturn = false;
            
            try
            {
                await GrabMakeupAsync(brushHandle, rotateParameters, token);

                SetHandOffset(brush);

                await MoveAsync(blush, token);
                await PlayYoyoAnimationAsync(blush, token);
                await MoveToMakeUpPositionAsync(token);
                await WaitingMakeUpPositionAsync(brush, token);
                await MakeUpAsync(token);
                
                OnMakeup?.Invoke();
                
                await ReturnAsync(token);
            }
            catch (OperationCanceledException)
            {
                using (var timeoutSource = new CancellationTokenSource(TimeSpan.FromSeconds(3f)))
                {
                    await ReturnAsync(timeoutSource.Token);
                }
            }
            finally
            {
                if (token.IsCancellationRequested)
                {
                    OnMakeupCancelled?.Invoke();
                }
                
                _isMakeupProcessing = false;
                hand.IsBack.Value = false;
            }
        }

        protected override async UniTask RotateAsync(CancellationToken token)
        {
            var rotateParameters = _model.GetRotateParameters();
            rotateParameters.RotateDirection = Vector3.zero;
            await _model.Hand.Rotate(_makeup.BrushHandle, rotateParameters, token);
        }
    }
}