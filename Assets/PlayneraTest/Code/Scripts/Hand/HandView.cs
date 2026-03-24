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
        public Vector3 Offset;
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
                    HideWrist(_hands[0].gameObject);
                    ShowWrist(_hands[1].gameObject);
                })
                .OnComplete(() =>
                {
                    HideWrist(_hands[1].gameObject);
                    ShowWrist(_hands[2].gameObject);
                    tcs.TrySetResult();
                }).SetEase(Ease.InSine);
            
            await tcs.Task;
        }

        private void ShowWrist(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void HideWrist(GameObject obj)
        {
            obj.SetActive(false);
        }

    }
}