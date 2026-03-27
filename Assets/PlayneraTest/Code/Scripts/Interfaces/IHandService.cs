using System;

namespace PlayneraTest.Code.Scripts.Hand
{
    public interface IHandService
    {
        public event Action OnServiceInitialized;
        public bool IsInitialized { get; }
        public void Initialize(HandView hand);
        public HandView GetHand();
    }
}