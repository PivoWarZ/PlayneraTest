using System;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeUpViewModel
    {
        public event Action OnMakeUpAnimationCompleted;
        void StartMakeUp();
        void BreakMakeUp();
    }
}