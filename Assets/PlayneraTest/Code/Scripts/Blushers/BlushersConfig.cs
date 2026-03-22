using System;
using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Blushers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayneraTest.Code.Configs
{
    [CreateAssetMenu(fileName = "BlushersConfig", menuName = "Configs/BlushersConfig/New BlushersConfig")]
    public class BlushersConfig: ScriptableObject
    {
        [SerializeField] List<Blush> _blushes;
        public Color BorderColor;
        public Color ShadowColor;

        public List<Blush> Blushes => _blushes;

        [Button]
        private void SetBorderAndShadowColor()
        {
            for (int i = 0; i < _blushes.Count; i++)
            {
                Blush blush = _blushes[i];
                blush.BorderColor = BorderColor;
                _blushes[i] = blush;
            }
        }
    }
}