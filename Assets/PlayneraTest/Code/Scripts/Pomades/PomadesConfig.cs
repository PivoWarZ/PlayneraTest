using System.Collections.Generic;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Pomades
{
    [CreateAssetMenu(fileName = "PomadesConfig", menuName = "Configs/PomadesConfig/New PomadesConfig")]
    public class PomadesConfig: ScriptableObject
    {
        [SerializeField] private List<Pomade> _pomades;
        public Color ShadowColor;
        public List<Pomade> Pomades => _pomades;
    }
}