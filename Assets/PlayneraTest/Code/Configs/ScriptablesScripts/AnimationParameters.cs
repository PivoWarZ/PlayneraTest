using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;

namespace PlayneraTest.Code.Scripts
{
    [CreateAssetMenu(fileName = "AnimationParameters", menuName = "Configs/Parameters/New AnimationParameters")]
    public class AnimationParameters: ScriptableObject
    {
        [SerializeField] private float _moveTime = 2;
        [SerializeField, Range(0, 1)] private float _animationSpeedModifier = 1;
        [SerializeField, Range(0, 1)] private float _backAnimationSpeedModifier = 0.3f;
        [SerializeField] private float _yoyoSpeedModifier = 12;
        
        [Header("Rotate Parameters")]
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _scaleTime = 2;
        [SerializeField] private float _rotateTime = 2;

        public int YoyoCount = 3;
        
        public float MoveTime => _moveTime * _animationSpeedModifier;
        public float YoyoSpeed => _moveTime * _animationSpeedModifier / _yoyoSpeedModifier;

        private RotateParameters GetRotateParameters(Vector3 rotateDirection)
        {
            RotateParameters rotateParameters = new RotateParameters
            {
                RotateDirection = rotateDirection,
                ScaleFactor = _scaleFactor,
                ScaleTime = _scaleTime * _animationSpeedModifier,
                RotateTime = _rotateTime * _animationSpeedModifier,
            };
            
            return rotateParameters;
        }

        public void RefreshSpeedModifier()
        {
            SetSpeedModifier(1f);
        }

        private void SetSpeedModifier(float speed)
        {
            _animationSpeedModifier = speed;
        }

        public void SetBackAnimationsSpeed()
        {
            SetSpeedModifier(_backAnimationSpeedModifier);
        }
    }
}