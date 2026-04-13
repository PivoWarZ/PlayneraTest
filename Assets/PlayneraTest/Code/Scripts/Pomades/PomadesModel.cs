using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Base;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using UnityEngine;
using Zenject;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadesModel: IPomadesModel, IInitializable, IDisposable
    {
        private IHandService _handService;
        private IHandView _hand;
        private int _yoyoCount;

        public PomadesModel(IHandService handService)
        {
            _handService = handService;
        }

        public RectTransform MakeupZone => GirlFaceMakeupPositions.Lips;
        public RotateParameters GetRotateParameters() => new RotateParameters();

        public Vector3 DragPosition => GirlFaceMakeupPositions.BottomMakeup.position;
        IHandView IMakeupModel.Hand => _hand;
        int IMakeupModel.YoyoCount => _yoyoCount;
        
        void IInitializable.Initialize()
        {
            TrySetHand();
            _yoyoCount = Configs.Instance.Get<AnimationParameters>().YoyoCount;
        }
        
        private void TrySetHand()
        {
            var hand = _handService.GetHand();

            if (hand == null)
            {
                _handService.OnServiceInitialized += TrySetHand;
            }
            else
            {
                _hand = hand;
            }
        }

        void IDisposable.Dispose()
        {
            _handService.OnServiceInitialized -= TrySetHand;
        }
    }
}