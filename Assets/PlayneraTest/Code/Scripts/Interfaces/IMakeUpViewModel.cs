using System;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeUpViewModel
    {
        public event Action OnMakeUpAnomationCompleted;
        void StartMakeUp();
        void BreakMakeUp();
    }
}