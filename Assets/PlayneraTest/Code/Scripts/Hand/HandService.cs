using System;

namespace PlayneraTest.Code.Scripts.Hand
{
    public class HandService: IHandService
    {
        public event Action OnServiceInitialized;
        
        private HandView _hand;
        
        public bool IsInitialized => _hand != null;

        public HandService() { }
        
        public HandService(HandView hand)
        {
            _hand = hand;
        }
        
        public void Initialize(HandView hand)
        {
            _hand = hand;
            OnServiceInitialized?.Invoke();
        }

        public HandView GetHand()
        {
            return _hand;
        }
    }
}