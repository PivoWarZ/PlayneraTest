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
        
        [SerializeField] private GameObject[] _hands;
        [SerializeField] private DragAndDropHandler _dragAndDropHandler;
        private Vector3 _offset;
        private RectTransform _rectTransform;
        private Vector3 _startPosition;
        private HandAnimator _animator;
        private bool _isMakeupReady;
        private UniTaskCompletionSource _completeTask;
        private const float MOVE_TIME = 1f;

        private void Awake()
        {
            Clear();
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.position;

            MoveParameters parameters = new MoveParameters
            {
                MoveTime = MOVE_TIME
            };
            
            _animator = new HandAnimator(transform, parameters);
            _animator.OnAnimationCompleted += CompleteTask;
            
            _dragAndDropHandler.OnDropped += OnDrop;
        }

        private void OnDrop()
        {
            OnDropped?.Invoke();
        }

        private void OnDestroy()
        {
            _animator.OnAnimationCompleted -= CompleteTask;
        }

        private void CompleteTask()
        {
            _completeTask.TrySetResult();
        }

        public async UniTask MoveAsync(Vector3 target, CancellationToken token)
        {
            target -= _offset;
            
            _completeTask = new UniTaskCompletionSource();
            
            using var registration = token.Register(() =>
            {
                _completeTask.TrySetCanceled();
                _animator.Clear();
            });

            _animator.NewAnimation.AddMoving(target).Run();
            
            await _completeTask.Task;
        }

        public async UniTask PlayYoyoAnimationAsync(List<Vector3> yoyoPoints, int yoyoCount, CancellationToken token)
        {
            _completeTask = new UniTaskCompletionSource();

            using var registration = token.Register(() =>
            {
                _completeTask.TrySetCanceled();
                _animator.Clear();
            });
            
            List<Vector3> yoyoPointsWithOffset = new List<Vector3>();
            
            yoyoPoints.ForEach(y => yoyoPointsWithOffset.Add(y-_offset));
            
            _animator.NewAnimation.AddYoyo(yoyoPointsWithOffset, yoyoCount).Run();
            
            await _completeTask.Task;
        }

        public async UniTask MoveToBottomMakeupPosition(CancellationToken token)
        {
            await MoveAsync(Girl.BottomMakeupPosition.position, token);
        }

        public async UniTask ReturnToStartPosition(CancellationToken token)
        {
            Clear();
            await MoveAsync(_startPosition, token);
            MovingStartPositionComplete();
        }

        public async UniTask Grab(Vector3 target, CancellationToken token)
        {
            _completeTask = new UniTaskCompletionSource();
            
            using var registration = token.Register(() =>
            {
                _completeTask.TrySetCanceled();
                _animator.Clear();
            });
            
            _animator.NewAnimation.AddMoving(target).AddGrab(_hands).Run();
            
            await _completeTask.Task;
        }

        public async UniTask GrabAndRotate(RectTransform target, RotationParameters parameters, CancellationToken token)
        {
            await Grab(target.position, token);
            await Rotate(target, parameters, token);
        }

        public async UniTask Rotate(RectTransform target, RotationParameters parameters, CancellationToken token)
        {
            _completeTask = new UniTaskCompletionSource();
            
            using var registration = token.Register(() =>
            {
                _completeTask.TrySetCanceled();
                _animator.Clear();
            });
            
            _animator.NewAnimation.AddRotate(target, parameters).Run();
            
            await _completeTask.Task;
        }

        public void Clear()
        {
            _isMakeupReady = false;
            _offset = Vector3.zero;
            MoveTime = MOVE_TIME;
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