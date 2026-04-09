using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadesView: MonoBehaviour
    {
        [SerializeField] List<PomadeView> _pomades;
        private IPomadesViewModel _viewModel;

        [Inject]
        public void Construct(IPomadesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            var config = Configs.Instance.Get<PomadesConfig>();

            for (var index = 0; index < _pomades.Count; index++)
            {
                var pomade = _pomades[index];
                pomade.Image.sprite = config.Pomades[index].Lipstick;
                pomade.LipkColor = config.Pomades[index].LipColor;

                pomade.OnMakeupRequest += StartMakeup;
            }
        }

        private void OnDestroy()
        {
            for (var index = 0; index < _pomades.Count; index++)
            {
                var pomade = _pomades[index];
                pomade.OnMakeupRequest -= StartMakeup;
            }
        }
        
        private void StartMakeup(GameObject obj)
        {
            var pomade = obj.GetComponent<PomadeView>();
            transform.SetAsLastSibling();
            _viewModel.Initialize(pomade);
            _viewModel.StartMakeUp();
        }
    }
}