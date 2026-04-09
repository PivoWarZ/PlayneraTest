using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadesViewModel: IPomadesViewModel, IDisposable
    {
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        private IPomadesModel _model;
        private PomadeView _pomade;
        private bool _isMakeupProcessing;
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
            IHandView hand = _model.Hand;
           var targetPoint = _pomade.transform.position;
           
           await hand.Grab(targetPoint, token);
           _pomade.transform.SetParent(hand.RectTransform);
           _pomade.transform.SetAsLastSibling();
        }
    }
}