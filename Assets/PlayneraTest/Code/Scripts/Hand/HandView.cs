using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class HandView: MonoBehaviour
    {
        public event Action OnMakeupPosition;

        public float MoveTime;
        [SerializeField] private List<GameObject> _hands;
        [SerializeField] private Transform _targetPoint;
        
        
        public async UniTaskVoid MoveAsync(Vector3 position)
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            Sequence seq = DOTween.Sequence();
            
            seq
                .Append(transform.DOMove(position, MoveTime))
                .OnComplete(() => tcs.TrySetResult());
            
            await tcs.Task;
        }

    }
}