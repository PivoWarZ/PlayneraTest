using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using UnityEngine;
using Zenject;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersModel: IInitializable, IDisposable, IBlushersModel
    {
        private IHandView _hand;
        private IHandService _handService;
        private int _yoyoCount;

        public BlushersModel(IHandService handService)
        {
            _handService = handService;
        }

        public IHandView Hand => _hand;

        public int YoyoCount => _yoyoCount;

        void IInitializable.Initialize()
        {
            TrySetHand();
            _yoyoCount = Configs.Instance.Get<AnimationParameters>().YoyoCount;
        }
        
        void IDisposable.Dispose()
        {
            _handService.OnServiceInitialized -= TrySetHand;
        }

        public RectTransform MakeupZone => GirlFaceMakeupPositions.Cheeks;
  

        public RotateParameters GetRotateParameters
        {
            get
            {
                var config = Configs.Instance.Get<AnimationParameters>();
                return config.GetRotateParameters(new Vector3(0f, 0f, -90f));
            }
        }

        public Vector3 DragPosition => GirlFaceMakeupPositions.BottomMakeup.position;

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
    }
}