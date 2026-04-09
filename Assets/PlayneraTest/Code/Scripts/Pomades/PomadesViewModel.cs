using System;
using PlayneraTest.Code.Scripts.Interfaces;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadesViewModel: IPomadesViewModel
    {
        public event Action OnMakeupCompleted;
        public event Action OnMakeupCancelled;
        public void StartMakeUp()
        {
            throw new NotImplementedException();
        }
    }
}