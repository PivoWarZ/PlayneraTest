using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeupModel
    {
        List<Vector3> GetYoyoPoints(RectTransform yoyo);
        IHandView Hand { get; }
        int YoyoCount { get; }
    }
}