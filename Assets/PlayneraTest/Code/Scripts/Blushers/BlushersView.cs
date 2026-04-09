using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private Image _faceBrushLeftImage;
        [SerializeField] private Image _faceBrushRightImage;
        private IBlushersViewModel _viewModel;
        private BlushView _currentBlush;

        [Inject]
        public void Construct(IBlushersViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.OnMakeupCompleted += ApplyMakeup;
        }

        private void Start()
        {
            BlushersConfig config = Configs.Instance.Get<BlushersConfig>();
            
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
            
            _viewModel.OnMakeupCompleted -= ApplyMakeup;
        }
        
        private void StartMakeup(GameObject obj)
        {
            _currentBlush = obj.GetComponent<BlushView>();
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
        
        private void ApplyMakeup()
        {
            var leftColor = _faceBrushLeftImage.color;
            var rightColor = _faceBrushRightImage.color;
            leftColor.a = 0f;
            rightColor.a = 0f;
            
            _faceBrushLeftImage.color = leftColor;
            _faceBrushRightImage.color = rightColor;
            
            _faceBrushLeftImage.sprite = _currentBlush.FaceBrushLeftSprite;
            _faceBrushRightImage.sprite = _currentBlush.FaceBrushRightSprite;
            
            DOTween.To(
                () => _faceBrushLeftImage.color.a,
                a =>
                {
                    Color c = _faceBrushLeftImage.color;
                    c.a = a;
                    _faceBrushLeftImage.color = c;
                },
                1f,
                0.5f
            );
            
            DOTween.To(
                () => _faceBrushRightImage.color.a,
                a =>
                {
                    Color c = _faceBrushRightImage.color;
                    c.a = a;
                    _faceBrushRightImage.color = c;
                },
                1f,
                0.5f
            );
        }
    }
}