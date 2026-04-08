using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using UnityEngine;

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
        private bool _isYoyoAnimation;

        private void Awake()
        {
            Clear();
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.position;
            _animator = new HandAnimator(transform);
            
            _dragAndDropHandler.OnDropped += OnDrop;
        }

        private void OnDrop()
        {
            OnDropped?.Invoke();
        }
        

        public async UniTask MoveAsync(Vector3 target, CancellationToken token, bool isBack = false)
        {
            token.ThrowIfCancellationRequested();
            
            target -= _offset;

            _animator.NewAnimation.AddMoving(target);

            if (isBack)
            {
                _animator.IsBackAnimation.Run();
            }
            else
            {
                _animator.Run();
            }

            await AwaitingAnimationAsync(token);
        }

        public async UniTask PlayYoyoAnimationAsync(List<Vector3> yoyoPoints, int yoyoCount, CancellationToken token)
        {
            
            token.ThrowIfCancellationRequested();
            
            List<Vector3> yoyoPointsWithOffset = new List<Vector3>();
            
            yoyoPoints.ForEach(y => yoyoPointsWithOffset.Add(y-_offset));
            
            _animator.NewAnimation.AddYoyo(yoyoPointsWithOffset, yoyoCount).Run();
            
            await AwaitingAnimationAsync(token);
        }

        private async UniTask AwaitingAnimationAsync(CancellationToken token)
        {
            try
            {
                await _animator.Sequence.ToUniTask(cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                _animator.Clear();
                throw;
            }
        }

        public async UniTask MoveToBottomMakeupPosition(CancellationToken token)
        {
            await MoveAsync(GirlFaceMakeupPositions.BottomMakeupPosition.position, token);
        }

        public async UniTask ReturnToStartPosition(CancellationToken token)
        {
            Clear();
            await MoveAsync(_startPosition, token, true);
            MovingStartPositionComplete();
        }

        public async UniTask Grab(Vector3 target, CancellationToken token)
        {
            _animator.NewAnimation.AddMoving(target).AddGrab(_hands).Run();
            
            await AwaitingAnimationAsync(token);
        }

        public async UniTask GrabAndRotate(RectTransform target, RotateParameters parameters, CancellationToken token)
        {
            await Grab(target.position, token);
            await Rotate(target, parameters, token);
        }

        public async UniTask Rotate(RectTransform target, RotateParameters parameters, CancellationToken token)
        {
            _animator.NewAnimation.AddRotate(target, parameters).Run();
            
            await AwaitingAnimationAsync(token);
        }

        public void Clear()
        {
            _isMakeupReady = false;
            _offset = Vector3.zero;
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