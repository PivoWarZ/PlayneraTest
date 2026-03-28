using PlayneraTest.Code.Scripts.Hand;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace PlayneraTest.Code.Scripts.DI
{
    public class BootsTrap: MonoInstaller
    {
        [SerializeField] private RectTransform _ui;
        private IHandService _handService;
        private INeedHandService[] _handNeeds;
        
        public override void InstallBindings()
        {
            CreateHandService();
            InitializeHandServiceNeededs();
        }

        [Inject]
        public void Construct(IHandService handService, INeedHandService[] handServices)
        {
            _handService = handService;
            _handNeeds = handServices;
        }

        private void CreateHandService()
        {
            var prefabLink = Resources.Load<HandView>("HandNew");
            var handPrefab = GameObject.Instantiate(prefabLink, _ui);
            _handService.Initialize(handPrefab);
        }

        private void InitializeHandServiceNeededs()
        {
            foreach (var need in _handNeeds)
            {
                need.Initialize(_handService);
            }
        }
    }
}