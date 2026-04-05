using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
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
        public RectTransform RectTransform => _rectTransform;
        
        [SerializeField] private List<GameObject> _hands;
        [SerializeField] private DragAndDropHandler _dragAndDropHandler;
        private Vector3 _offset;
        private RectTransform _rectTransform;
        private Vector3 _startPosition;
        private bool _isMakeupReady;
        Sequence _moveSequence;
        private const float MOVE_TIME = 1f;

        private void Awake()
        {
            Clear();
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.position;
        }

        private void OnDestroy()
        {
            _moveSequence?.Kill();
        }

        public async UniTask MoveAsync(Vector3 target, CancellationToken token)
        {
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            _moveSequence = DOTween.Sequence();
            
            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                _moveSequence.Kill();
            });
            
            AddMovingTweens(target, _moveSequence);
                _moveSequence
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

            yoyoPoints.ForEach(x => AddMovingTweens(x, _moveSequence));
            
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

        public async UniTask MoveToBottomMakeupPosition(CancellationToken token)
        {
            await MoveAsync(Girl.BottomMakeupPosition.position, token);
        }

        public void ReturnToStartPosition()
        {
            Clear();
            AddMovingTweens(_startPosition, _moveSequence);
                _moveSequence
                .OnComplete(MovingStartPositionComplete);
        }

        private void AddMovingTweens(Vector3 target, Sequence sequence)
        {
			var targetPosition = target + _offset;
            Debug.Log($"Target position: {targetPosition}");
            Debug.Log($"Move Offset {_offset}");
            
            sequence
                .AppendCallback(MoveStarted)
                .Append(transform.DOMove(targetPosition, MoveTime))
                .OnComplete(MovingCompleted);
        }

        private void AddGrabTweens(Sequence sequence)
        {
            sequence
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
        }

        public async UniTask Grab(Vector3 target, CancellationToken token)
        {
            
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            
            _moveSequence = DOTween.Sequence();
            
            AddMovingTweens(target, _moveSequence);
            AddGrabTweens(_moveSequence);
            _moveSequence
                .OnComplete(() =>
                {
                    task.TrySetResult();
                });
            
            using var registration = token.Register(() =>
            {
                task.TrySetCanceled();
                _moveSequence.Kill();
                Debug.Log($"<color=yellow>{GetType()} : Cancelled</color>");
            });
            
            await task.Task;
            
            _moveSequence.Kill();
        }

        public async UniTask GrabAndRotate(RectTransform target, RotationParameters parameters, CancellationToken token)
        {
            await Grab(target.position, token);
            await Rotate(target, parameters, token);
        }

        public async UniTask Rotate(RectTransform target, RotationParameters parameters, CancellationToken token)
        {
            Vector3 rotateDirection = parameters.RotateDirection;
            float rotateTime = parameters.RotateTime;
            float scalefactor = parameters.ScaleFactor;
            float scaleTime = parameters.ScaleTime;
            
            UniTaskCompletionSource task = new UniTaskCompletionSource();
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(target.transform.DOScale(scalefactor, scaleTime))
                .Join(target.DORotate(rotateDirection, rotateTime))
                .OnComplete(() =>
                {
                    sequence.Kill();
                    task.TrySetResult();
                });
            
            await task.Task;
        }

        public void Clear()
        {
            _isMakeupReady = false;
            _offset = Vector3.zero;
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

        public void SetOffset(Vector3 offset)
        {
           _offset = offset;
        }
    }
}