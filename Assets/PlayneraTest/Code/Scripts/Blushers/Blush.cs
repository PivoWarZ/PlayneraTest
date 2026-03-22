using System;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Blushers
{
    [Serializable]
    public struct Blush
    {
        public Color BlushColor;
        public Sprite FaceBrushLeft;
        public Sprite FaceBrushRight;
        public Color BorderColor;
    }
}