using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: MonoBehaviour, IBlushersViewModel
    {
        private Transform _brush;

        public void Init(Transform brush)
        {
            _brush = brush;
        }

        void IMakeUp.StartMakeUp(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        void IMakeUp.BreakMakeUp()
        {
            throw new System.NotImplementedException();
        }
    }
}