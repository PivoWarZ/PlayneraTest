using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using PlayneraTest.Code.Scripts.MakeupGirl;
using UnityEngine;
using Zenject;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersModel: IInitializable, IDisposable
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

        public List<Vector3> GetYoyoPoints()
        {
            List<Vector3> yoyoPoints = new List<Vector3>();
            yoyoPoints.Add(GirlFaceMakeupPositions.FaceBrushLeft.position);
            yoyoPoints.Add(GirlFaceMakeupPositions.FaceBrushRight.position);
            
            return yoyoPoints;
        }
        
        public RotateParameters GetRotateParameters()
        {
            RotateParameters parameters = new RotateParameters
            {
                RotateDirection = new Vector3(0, 0, -90),
                RotateTime = 0.2f,
                ScaleTime = 0.2f,
                ScaleFactor = 1.15f,
            };
            
            return parameters;
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
        
    }
}