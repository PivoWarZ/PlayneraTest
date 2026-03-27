using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayneraTest.Code.Scripts.Blushers
{
    public class BlushersView: MonoBehaviour
    {
        [SerializeField] private List<BlushView> _blushs;
        [SerializeField] private RectTransform _brushHandle;
        [SerializeField] private RectTransform _brush;
        [SerializeField] private Image _brushShadow;
        private IBlushersViewModel _viewModel;

        public Transform BrushHandle => _brushHandle;

        [Inject]
        public void Construct(IBlushersViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            BlushersConfig config = Resources.Load<Configs>("ConfigsProvider").GetConfig<BlushersConfig>();
            
            for (int i=0; i < _blushs.Count; i++)
            {
                var blush = _blushs[i];
                blush.SetBlush(config.Blushes[i]);
                blush.OnMakeupRequest += StartMakeup;
            }
            
            _brushShadow.color = config.ShadowColor;
        }

        private void OnDestroy()
        {
            for (int i=1; i < _blushs.Count; i++)
            {
                var blush = _blushs[i];
                blush.OnMakeupRequest -= StartMakeup;
            }
        }

        private void StartMakeup(GameObject obj)
        {
            transform.SetAsLastSibling();
            _viewModel.SetMakeupTarget(MakeupTargets(obj));
            _viewModel.StartMakeUp();
        }

        private BlushMakeupTargets MakeupTargets(GameObject blush)
        {
            BlushMakeupTargets targets = new BlushMakeupTargets {
                BrushHandle = _brushHandle,
                Brush = _brush,
                Blush = blush.GetComponent<RectTransform>()
            };
            
            return targets;
        }
    }
}