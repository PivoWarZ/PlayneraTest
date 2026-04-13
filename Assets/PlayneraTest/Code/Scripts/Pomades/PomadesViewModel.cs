using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Base;
using PlayneraTest.Code.Scripts.Interfaces;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadesViewModel: MakeupViewModelBase, IPomadesViewModel, IDisposable
    {
        public event Action OnMakeup;
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        private PomadeView _pomade;
        private CancellationTokenSource _cancell;

        public PomadesViewModel(IPomadesModel model)
        {
            _model = model;
        }

        void IPomadesViewModel.Initialize(PomadeView pomadeView)
        {
            _pomade = pomadeView;
        }
        public void StartMakeUp()
        {
            if (_isMakeupProcessing)
            {
                Cancel();
                return;
            }
            
            _cancell = new CancellationTokenSource();
            _isMakeupProcessing = true;
            _isReturn = false;
            
            ((IMakeupViewModel)this).RunMakeupAsync(_cancell.Token).Forget();
            
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
            try
            {
                await GrabMakeupAsync(_pomade.Rect, _model.GetRotateParameters, token);
                
                SetHandOffset(_pomade.Rect);

                await MoveAsync(_model.DragPosition, token);
                await WaitingMakeUpPositionAsync(_model.MakeupZone, token);
                await MakeUpAsync(token);

                OnMakeupCompleted?.Invoke();

                await ReturnAsync(token);
            }
            catch (OperationCanceledException)
            {
                await ReturnToTimeoutTokenAsync();
            }
            finally
            {
                if (token.IsCancellationRequested)
                {
                    OnMakeupCancelled?.Invoke();
                }
                
                _isMakeupProcessing = false;
                _model.Hand.IsBack.Value = false;
            }
        }
    }
}