using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IBlushersModel
    {
        List<Vector3> GetYoyoPoints();
        IHandView Hand { get; }
        int YoyoCount { get; }

        RotateParameters GetRotateParameters();
    }
}