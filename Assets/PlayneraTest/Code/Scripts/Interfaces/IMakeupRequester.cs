using System;
using UnityEngine;

namespace PlayneraTest.Code.Scripts.Interfaces
{
    public interface IMakeupRequester
    {
        public event Action<GameObject> OnMakeupRequest;
    }
}