using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using TMPro;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: IBlushersViewModel, IDisposable, INeedHandService
    {
        private BlushMakeupTargets _makeup = new BlushMakeupTargets();
        private IHandView _hand;
        private bool _isMakeupProcessing;
        private CancellationTokenSource _cancell = new CancellationTokenSource();
        private IHandService _handService;

        void IBlushersViewModel.SetMakeupTarget(BlushMakeupTargets targets)
        {
            _makeup = targets;
        }
        
        void IMakeUpViewModel.StartMakeUp()
        {
            _isMakeupProcessing = true;
            _hand = _handService.GetHand();
            RunMakeupRequest(_cancell.Token).Forget();
        }
        
        void IDisposable.Dispose()
        {
            _cancell.Cancel();

        }
        
        private async UniTask RunMakeUpEventAnimation(UniTaskCompletionSource<bool> task, CancellationToken cancellToken)
        {
            Debug.Log("<color=green>CHEEKS/color><>");
            List<Vector3> yoyoPoints = new();
            yoyoPoints = Girl.Cheeks.GetComponent<MakeUpZone>().YoyoPoints;
            _hand.MoveTime = _hand.MoveTime / 12;
            await _hand.PlayYoyoAnimationAsync(yoyoPoints, 6, _cancell.Token);
            task.TrySetResult(true);
        }

        private async UniTask RunMakeupRequest(CancellationToken token)
        {
            var brushHandle = _makeup.BrushHandle;
            var brush = _makeup.Brush;
            var blush = _makeup.Blush;
            var yoyoPoints = _makeup.Blush.GetComponent<IYoyoMakeup>().YoyoPoints;
            int yoyoCount = 6;
            float yoyoSpeed = _hand.MoveTime / 12;

            var rotateParameters = GetRotateParameters();
            
            await _hand.GrabAndRotate(brushHandle, rotateParameters, token);
            
           brushHandle.SetParent(_hand.RectTransform);
           brushHandle.SetAsLastSibling();
            var offset =  brush.position - _hand.RectTransform.position;
             _hand.SetOffset(offset);
             Debug.Log($"Brush position {brush.position} Hand {_hand.RectTransform.position}");
            Debug.Log(offset);
            Debug.Log($"Blush position {blush.position}");
            
             await _hand.MoveAsync(blush.position, token);
             await _hand.MoveToBottomMakeupPosition(token);

        }

        private RotationParameters GetRotateParameters()
        {
            RotationParameters parameters = new RotationParameters
            {
                RotateDirection = new Vector3(0, 0, -90),
                RotateTime = 0.2f,
                ScaleTime = 0.2f,
                ScaleFactor = 1.15f,
            };
            
            return parameters;
        }

        void IMakeUpViewModel.BreakMakeUp()
        {
            throw new System.NotImplementedException();
        }

        void INeedHandService.Initialize(IHandService handService)
        {
            _handService = handService;
        }
    }
}