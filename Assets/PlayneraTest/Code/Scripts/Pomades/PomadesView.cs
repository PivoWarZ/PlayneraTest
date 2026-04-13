
using System.Collections.Generic;
using DG.Tweening;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayneraTest.Code.Scripts.Pomades
{
    public class PomadesView: MonoBehaviour
    {
        [SerializeField] private Image _makeUpImage;
        [SerializeField] List<PomadeView> _pomades;
        private IPomadesViewModel _viewModel;
        private PomadeView _currentPomade;

        [Inject]
        public void Construct(IPomadesViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.OnMakeupCompleted += MakeUp;
        }

        private void MakeUp()
        {
            _makeUpImage.color = _currentPomade.LipColor;
            
            DOTween.To(
                () => _makeUpImage.color.a,
                a =>
                {
                    Color c = _makeUpImage.color;
                    c.a = a;
                    _makeUpImage.color = c;
                },
                1f,
                0.5f
            );
        }

        private void Start()
        {
            var config = Configs.Instance.Get<PomadesConfig>();

            for (var index = 0; index < _pomades.Count; index++)
            {
                var pomade = _pomades[index];
                pomade.Image.sprite = config.Pomades[index].Lipstick;
                pomade.LipColor = config.Pomades[index].LipColor;

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
            
            _viewModel.OnMakeupCompleted -= MakeUp;
        }
        
        private void StartMakeup(GameObject obj)
        {
            _currentPomade = obj.GetComponent<PomadeView>();
            transform.SetAsLastSibling();
            _viewModel.Initialize(_currentPomade);
            _viewModel.StartMakeUp();
        }
    }
}