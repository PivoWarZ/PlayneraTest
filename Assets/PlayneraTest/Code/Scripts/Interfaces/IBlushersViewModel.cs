using PlayneraTest.Code.Scripts.Blushers;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IBlushersViewModel: IMakeUpViewModel
    {
        void SetMakeupTarget(BlushMakeupTargets targets);
    }
}