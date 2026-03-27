using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        public float MoveTime { get; set; }
        public Vector3 Offset { get; set; }
        public void ReturnToStartPosition();
        public void Clear();
        UniTask MoveAsync(RectTransform target, CancellationToken token);
        UniTask PlayYoyoAnimationAsync(List<Vector3> yoyoPoints, int yoyoCount, CancellationToken token);
    }
}