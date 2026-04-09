using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
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

        List<Vector3> IMakeupModel.GetYoyoPoints(RectTransform yoyo)
        {
            throw new System.NotImplementedException();
        }

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