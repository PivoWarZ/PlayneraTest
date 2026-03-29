using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
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
		public event Action OnDropped;
        public float MoveTime { get; set; }
        public Vector3 Offset { get; set; }
        public RectTransform RectTransform => _rectTransform;
        
        [SerializeField] private List<GameObject> _hands;
        [SerializeField] private DragAndDropHandler _dragAndDropHandler;
        private RectTransform _rectTransform;
        private RectTransform _startPosition;
        private bool _isMakeupReady;
        Sequence _moveSequence;
        private const float MOVE_TIME = 1f;

        private void Awake()
        {
            Clear();
            _startPosition = transform.GetComponent<RectTransform>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
            _moveSequence?.Kill();
        }

        public async UniTask MoveAsync(RectTransform target, CancellationToken token)
        {
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            _moveSequence = DOTween.Sequence();
            
            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                _moveSequence.Kill();
            });
            
            _moveSequence
                    .Append(Move(target.position))
                    .OnComplete(() => task.TrySetResult());
            
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
                .Append(transform.DOMove(target - Offset, MoveTime))
                .OnComplete(MovingCompleted);

            return _moveSequence;
        }

        private Sequence GrabSequence(Vector3 target)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(Move(target))
                .InsertCallback(MoveTime/1.15f, () =>
                {
                    HideWrist(_hands[0].gameObject);
                    ShowWrist(_hands[1].gameObject);
                })
                .OnComplete(() =>
                {
                    HideWrist(_hands[1].gameObject);
                    ShowWrist(_hands[2].gameObject);
                    _isMakeupReady = true;
                })
                .SetEase(Ease.InSine);
            
            return sequence;
        }

        public async UniTask Grab(RectTransform target, CancellationToken token, bool isRotateNeeded = false,
            Vector3 rotateDirection = default)
        {
            Debug.Log("Grab");
            float rotateTime = 0.2f;
            float scalefactor = 1.2f;
            float scaleTime = 0.2f;
            
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            
            _moveSequence = DOTween.Sequence();
            _moveSequence
                .Append(Move(target.position))
                .Join(GrabSequence(target.position))
                .OnComplete(() =>
                {
                    task.TrySetResult();
                    Debug.Log("OnComplete");
                });
            
            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                _moveSequence.Kill();
                Debug.Log($"<color=yellow>{GetType()} : Cancelled</color>");
            });
            
            await task.Task;
            
            _moveSequence.Kill();
            
            Sequence sequence = DOTween.Sequence();
            
            if (isRotateNeeded)
            {
                sequence
                    .Append(transform.DOScale(scalefactor, scaleTime))
                    .Join(target.DORotate(rotateDirection, rotateTime))
                    .OnComplete(() =>
                    {
                        sequence.Kill();
                    });
            }
            
            Debug.Log($"DragEnded");
            
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