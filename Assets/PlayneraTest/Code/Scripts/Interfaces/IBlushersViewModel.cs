using PlayneraTest.Code.Scripts.Blushers;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IBlushersViewModel: IMakeUpViewModel
    {
        void SetMakeupTarget(BlushMakeupTargets targets);
    }
}