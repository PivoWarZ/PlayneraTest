using System;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeUpViewModel
    {
        public event Action OnMakeUpCompleted;
        public event Action OnMakeUpCancelled;
        void StartMakeUp();
    }
}