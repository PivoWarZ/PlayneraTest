using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;

namespace PlayneraTest.Code.Scripts
{
    [CreateAssetMenu(fileName = "AnimationParameters", menuName = "Configs/Parameters/New AnimationParameters")]
    public class AnimationParameters: ScriptableObject
    {
        public int YoyoCount = 3;
        [SerializeField] private float _moveTime = 2;
        [SerializeField, Range(0, 1)] private float _animationSpeedModifier = 1;
        [SerializeField, Range(0, 1)] private float _backAnimationSpeedModifier = 0.3f;
        [SerializeField, Range(0, 1)] private float _yoyoSpeedModifier = 0.3f;
        
        [Header("Rotate Parameters")]
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _scaleTime = 0.5f;
        [SerializeField] private float _rotateTime = 0.5f;

        public float MoveTime => _moveTime;

        public float AnimationSpeedModifier => _animationSpeedModifier;

        public float BackAnimationSpeedModifier => _backAnimationSpeedModifier;

        public float YoyoSpeed => MoveTime * _yoyoSpeedModifier;


        private RotateParameters GetRotateParameters(Vector3 rotateDirection)
        {
            RotateParameters rotateParameters = new RotateParameters
            {
                RotateDirection = rotateDirection,
                ScaleFactor = _scaleFactor,
                ScaleTime = _scaleTime * AnimationSpeedModifier,
                RotateTime = _rotateTime * AnimationSpeedModifier,
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
            SetSpeedModifier(BackAnimationSpeedModifier);
        }
    }
}