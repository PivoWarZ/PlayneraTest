using System;
using System.Collections.Generic;
using PlayneraTest.Code.Configs;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersView: MonoBehaviour
    {
        [SerializeField] private List<BlushView> blushs;
        [SerializeField] private Transform brush;
        [SerializeField] private Image _brushShadow;
        private IBlushersViewModel _viewModel;

        public Transform Brush => brush;

        [Inject]
        public void Construct(IBlushersViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            BlushersConfig config = Resources.Load<Configs>("ConfigsProvider").GetConfig<BlushersConfig>();
            
            for (int i=0; i < blushs.Count; i++)
            {
                var blush = blushs[i];
                blush.SetBlush(config.Blushes[i]);
                blush.OnMakeupRequest += StartMakeup;
            }
            
            _brushShadow.color = config.ShadowColor;
        }

        private void OnDestroy()
        {
            for (int i=1; i < blushs.Count; i++)
            {
                var blush = blushs[i];
                blush.OnMakeupRequest -= StartMakeup;
            }
        }

        private void StartMakeup(GameObject obj)
        {
            _viewModel.StartMakeUp(obj);
        }
    }
}