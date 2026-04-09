using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeupViewModel
    {
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        void StartMakeUp();
        UniTask RunMakeupAsync(CancellationToken token);
    }
}