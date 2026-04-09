using System;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeupViewModel
    {
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        void StartMakeUp();
    }
}