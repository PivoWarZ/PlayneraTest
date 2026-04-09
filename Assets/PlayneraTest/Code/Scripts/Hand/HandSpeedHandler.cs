using System;
using PlayneraTest.Code.Scripts.Interfaces;
using R3;
using Zenject;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class HandSpeedHandler: IInitializable, IDisposable
    {
        private IHandService _handService;
        private IHandView _hand;
        private IDisposable _dispose;
        

        public HandSpeedHandler(IHandService handService)
        {
            _handService = handService;
        }
        
        public void Initialize()
        {
            if (_handService.GetHand() == null)
            {
                _handService.OnServiceInitialized += SetHand;
            }
            else
            {
                _hand = _handService.GetHand();
                _dispose = _hand.IsBack.Subscribe(SetAnimationSpeed);
            }
        }

        private void SetHand()
        {
            _hand = _handService.GetHand();
        }

        private void SetAnimationSpeed(bool isBack)
        {
            var parameters = Configs.Instance.Get<AnimationParameters>();
            
            if (isBack)
            {
                parameters.SetBackAnimationsSpeed();
            }
            else
            {
                parameters.RefreshSpeedModifier();
            }
        }

        void IDisposable.Dispose()
        {
            _dispose.Dispose();
        }
    }
}