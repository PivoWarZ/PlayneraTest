using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class HandAnimator
    {
        public event Action OnMoveStarted;
        public event Action OnMoveEnded;
        public event Action OnAnimationCompleted;
        public event Action OnAnimationStarted;
        public event Action OnYoyoStarted;
        public event Action OnYoyoEnded;
        private Sequence _sequence;
        private Transform _transform;
        private MoveParameters _parameters;

        public HandAnimator(Transform movingTransform, MoveParameters parameters)
        {
            _transform = movingTransform;
            _parameters = parameters;
        }

        public HandAnimator NewAnimation => CreateAnimation();

        private HandAnimator CreateAnimation()
        {
            _sequence = DOTween.Sequence();
            _sequence
                .AppendCallback(() =>
                {
                    OnAnimationStarted?.Invoke();
                });
            return this;
        }

        public HandAnimator AddMoving(Vector3 target)
        {
            _transform.SetAsLastSibling();
            var animationTime = _parameters.MoveTime * Settings.AnimationSpeedModifier;
            
            _sequence
                .AppendCallback(MoveStarted)
                .Append(_transform.DOMove(target, animationTime))
                .OnComplete(MovingCompleted);
            
            return this;
        }
        
        private void AddMoving(Vector3 target, float duration)
        {
            var animationTime = duration * Settings.AnimationSpeedModifier;
            
            _transform.SetAsLastSibling();
            
            _sequence
                .AppendCallback(MoveStarted)
                .Append(_transform.DOMove(target, animationTime))
                .OnComplete(MovingCompleted);
        }
        
        public HandAnimator AddRotate(RectTransform target, RotationParameters parameters)
        {
            Vector3 rotateDirection = parameters.RotateDirection;
            float rotateTime = parameters.RotateTime * Settings.AnimationSpeedModifier;
            float scalefactor = parameters.ScaleFactor;
            float scaleTime = parameters.ScaleTime;

            _sequence
                .Append(target.transform.DOScale(scalefactor, scaleTime))
                .Join(target.DORotate(rotateDirection, rotateTime));
 
            return this;
        }
        
        public HandAnimator AddGrab(GameObject[] hands)
        {
            var animationTime = _parameters.MoveTime * Settings.AnimationSpeedModifier;
            
            _sequence
                .InsertCallback(animationTime/1.15f, () =>
                {
                    HideWrist(hands[0].gameObject);
                    ShowWrist(hands[1].gameObject);
                })
                .OnComplete(() =>
                {
                    HideWrist(hands[1].gameObject);
                    ShowWrist(hands[2].gameObject);
                })
                .SetEase(Ease.InSine);
            
            return this;
        }
        
        private void ShowWrist(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void HideWrist(GameObject obj)
        {
            obj.SetActive(false);
        }


        public void Run()
        {
            _sequence
                .OnComplete(() =>
                {
                    _sequence.Kill();
                    OnAnimationCompleted?.Invoke();
                });
        }
        
        public HandAnimator AddYoyo(List<Vector3> yoyoPoints, int yoyoCount)
        {
            _sequence.AppendCallback(() => OnYoyoStarted?.Invoke());

            yoyoPoints.ForEach(x => AddMoving(x, _parameters.MoveTime / 10));
            
            _sequence.SetLoops(yoyoCount, LoopType.Yoyo);

            return this;
        }

        public void Clear()
        {
            _sequence.Kill();
        }

        private void MoveStarted()
        {
            OnMoveStarted?.Invoke();
        }

        private void MovingCompleted()
        {
            OnMoveEnded?.Invoke();
        }
    }
}