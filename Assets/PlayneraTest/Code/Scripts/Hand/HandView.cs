using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class HandView: MonoBehaviour
    {
        public event Action OnMakeupPosition;

        public float MoveTime;
        [SerializeField] private Image _handImage;
        [SerializeField] private List<GameObject> _hands;
        
        public async UniTask MoveAsync(Transform target)
        {
            transform.SetParent(target.parent);
            target.SetAsLastSibling();
            
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            Sequence seq = DOTween.Sequence();
            
            seq
                .Append(transform.DOMove(target.position, MoveTime))
                .InsertCallback(MoveTime/3, () =>
                {
                    _hands[0].gameObject.SetActive(false);
                    _hands[1].gameObject.SetActive(true);
                })
                .OnComplete(() =>
                {
                    _hands[1].gameObject.SetActive(false);
                    _hands[2].gameObject.SetActive(true);
                    tcs.TrySetResult();
                }).SetEase(Ease.InSine);
            
            await tcs.Task;
        }

    }
}