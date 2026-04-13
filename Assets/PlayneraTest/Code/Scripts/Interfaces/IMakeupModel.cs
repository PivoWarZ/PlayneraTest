using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Hand;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeupModel
    {
        RectTransform MakeupZone { get; }
        RotateParameters GetRotateParameters();
        Vector3 DragPosition { get; }
        IHandView Hand { get; }
        int YoyoCount { get; }
    }
}