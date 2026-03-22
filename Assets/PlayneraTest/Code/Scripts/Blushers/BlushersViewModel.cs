using Cysharp.Threading.Tasks;
using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersViewModel: IBlushersViewModel
    {
        private Transform _brush;
        private HandView _hand;

        public void Init(Transform brush)
        {
            _brush = brush;
        }

        void IMakeUp.StartMakeUp(GameObject obj)
        {
            var handPrefab = Resources.Load<HandView>("Hand");
            _hand = GameObject.Instantiate(handPrefab, _brush.transform);
            Debug.Log(_hand);
            Run().Forget();
        }

        private async UniTaskVoid Run()
        {
            await _hand.MoveAsync(_brush);
        }

        void IMakeUp.BreakMakeUp()
        {
            throw new System.NotImplementedException();
        }
    }
}