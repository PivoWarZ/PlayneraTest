using System;
using PlayneraTest.Code.Scripts.Interfaces;

namespace PlayneraTest.Code.Scripts.Hand
{
    public interface IHandService
    {
        public event Action OnServiceInitialized;
        public bool IsInitialized { get; }
        public void Initialize(HandView hand);
        public IHandView GetHand();
    }
}