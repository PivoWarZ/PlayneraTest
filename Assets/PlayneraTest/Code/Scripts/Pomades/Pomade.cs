using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Pomades
{
    [Serializable]
    public struct Pomade
    {
        [Header("----------------------------------------------")]
        [PreviewField(ObjectFieldAlignment.Center, Height = 50)]
        public Sprite Lipstick;
        [Space]
        [PreviewField(ObjectFieldAlignment.Center, Height = 50)]
        public Sprite LipColor;
    }
}