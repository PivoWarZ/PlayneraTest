using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Hand;
using R3;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IHandView
    {
        public event Action OnStartPosition;
        public event Action OnMoveStarted;
        public event Action OnMovingComplete;
        public event Action OnYoYoStarted;
        public event Action OnYoYoEnded;
		public event Action OnDropped;
        public RectTransform RectTransform { get; }
        public void Clear();
        ReactiveProperty<bool> IsBack { get; set; }
        public void SetOffset(Vector3 offset);
        public UniTask ReturnToStartPosition(CancellationToken token);
        UniTask MoveAsync(Vector3 target, CancellationToken token);
        UniTask PlayYoyoAnimationAsync(List<Vector3> yoyoPoints, int yoyoCount, CancellationToken token);
        UniTask MoveToBottomMakeupPosition(CancellationToken token);
        UniTask Grab(Vector3 target, CancellationToken token);
        UniTask GrabAndRotate(RectTransform target, RotateParameters parameters, CancellationToken token);
        UniTask Rotate(RectTransform target, RotateParameters parameters, CancellationToken token);
    }
}