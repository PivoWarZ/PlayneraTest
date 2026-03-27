using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class HandView: MonoBehaviour, IHandView
    {
        public event Action OnStartPosition;
        public event Action OnMoveStarted;
        public event Action OnMovingComplete;
        public event Action OnYoYoStarted;
        public event Action OnYoYoEnded;
        public float MoveTime { get; set; }
        public Vector3 Offset { get; set; }
        
        [SerializeField] private List<GameObject> _hands;
        private RectTransform _startPosition;
        private bool _isMakeupReady;
        Sequence _moveSequence;
        private const float MOVE_TIME = 1f;

        private void Awake()
        {
            Clear();
            _startPosition = transform.GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
            _moveSequence?.Kill();
        }

        public async UniTask MoveAsync(RectTransform target, CancellationToken token)
        {
            transform.SetParent(target.parent);
            target.SetAsLastSibling();
            
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            _moveSequence = DOTween.Sequence();
            
            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                _moveSequence.Kill();
            });

            if (!_isMakeupReady)
            {
                _moveSequence
                    .Append(Move(target.position))
                    .InsertCallback(MoveTime/_hands.Count, () =>
                    {
                        HideWrist(_hands[0].gameObject);
                        ShowWrist(_hands[1].gameObject);
                    })
                    .OnComplete(() =>
                    {
                        HideWrist(_hands[1].gameObject);
                        ShowWrist(_hands[2].gameObject);
                        task.TrySetResult();
                        _isMakeupReady = true;
                    })
                    .SetEase(Ease.InSine);
            }
            else
            {
                _moveSequence
                    .Append(Move(target.position))
                    .OnComplete(() => task.TrySetResult());
            }
            
            await task.Task;
            
            _moveSequence.Kill();
        }
        
        public async UniTask PlayYoyoAnimationAsync(List<Vector3> yoyoPoints, int yoyoCount, CancellationToken token)
        {
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            _moveSequence = DOTween.Sequence().Pause();

            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                _moveSequence.Kill();
            });
            
            _moveSequence.AppendCallback(() => OnYoYoStarted?.Invoke());

            yoyoPoints.ForEach(x => _moveSequence.Append(Move(x)));
            
            _moveSequence.SetLoops(yoyoCount, LoopType.Yoyo);
            
            _moveSequence.OnComplete(() =>
            {
                OnYoYoEnded?.Invoke();
                task.TrySetResult();
            });

            _moveSequence.Play();
            
            await task.Task;
            
            _moveSequence.Kill();
        }

        public void ReturnToStartPosition()
        {
            Clear();
            _moveSequence = Move(_startPosition.position)
                .OnComplete(MovingStartPositionComplete);
        }

        private Sequence Move(Vector3 target)
        {
            _moveSequence = DOTween.Sequence();

            _moveSequence
                .AppendCallback(MoveStarted)
                .Append(transform.DOMove(target + Offset, MoveTime))
                .OnComplete(MovingCompleted);

            return _moveSequence;
        }

        public void Clear()
        {
            _isMakeupReady = false;
            Offset = Vector3.zero;
            MoveTime = MOVE_TIME;
        }

        private void ShowWrist(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void HideWrist(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void MoveStarted()
        {
            OnMoveStarted?.Invoke();
        }

        private void MovingCompleted()
        {
            OnMovingComplete?.Invoke();
        }

        private void MovingStartPositionComplete()
        {
            OnStartPosition?.Invoke();
        }
    }
}