using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;
using Zenject;

namespace PlayneraTest.Code.Scripts.DI
{
    public class BootsTrap: MonoInstaller
    {
        [SerializeField] private RectTransform _ui;
        private IHandService _handService;
        
        public override void InstallBindings()
        {
            CreateHandService();
        }

        [Inject]
        public void Construct(IHandService handService)
        {
            _handService = handService;
        }

        private void CreateHandService()
        {
            var prefabLink = Resources.Load<HandView>("HandNew");
            var handPrefab = GameObject.Instantiate(prefabLink, _ui);
            _handService.Initialize(handPrefab);
        }
    }
}