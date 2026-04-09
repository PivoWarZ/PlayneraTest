using PlayneraTest.Code.Scripts.Pomades;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IPomadesViewModel: IMakeupViewModel
    {
        void Initialize(PomadeView pomadeView);
    }
}