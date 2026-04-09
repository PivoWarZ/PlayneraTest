using PlayneraTest.Code.Scripts.Blushers;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IBlushersViewModel: IMakeupViewModel
    {
        void SetMakeupTarget(BlushMakeupTargets targets);
    }
}