using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        public float MoveTime { get; set; }
        public RectTransform RectTransform { get; }
        public void ReturnToStartPosition();
        public void Clear();
        public void SetOffset(RectTransform targetOffsetPosition);
        UniTask MoveAsync(RectTransform target, CancellationToken token);
        UniTask PlayYoyoAnimationAsync(List<Vector3> yoyoPoints, int yoyoCount, CancellationToken token);
        UniTask MoveToBottomMakeupPosition(CancellationToken token);
        UniTask Grab(RectTransform target, CancellationToken token, bool isRotateNeeded = false,
            Vector3 rotateDirection = default);
    }
}