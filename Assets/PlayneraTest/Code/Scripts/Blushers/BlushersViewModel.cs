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
        private Transform _brush;
        private HandView _hand;
        private CancellationTokenSource _cancell = new CancellationTokenSource();

        public void Init(Transform brush)
        {
            _brush = brush;
        }

        void IMakeUpViewModel.StartMakeUp(GameObject obj)
        {
            var handPrefab = Resources.Load<HandView>("Hand");
            _hand = GameObject.Instantiate(handPrefab, _brush.transform);
            RunMakeupRequest(_cancell.Token).Forget();
        }

        private async UniTask RunMakeupRequest(CancellationToken token)
        {
            await _hand.MoveAsync(_brush);
            
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(_brush.transform.DORotate(new Vector3(0, 0, -90), 0.3f))
                .OnComplete(() => task.TrySetResult());

            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                sequence.Kill();
                Debug.Log("ОТМЕНА");
            });
            
            await task.Task;
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