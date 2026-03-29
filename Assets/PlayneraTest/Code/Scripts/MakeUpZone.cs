using System.Collections.Generic;
using PlayneraTest.Code.Scripts.Interfaces;
using UnityEngine;

namespace PlayneraTest.Code.Scripts
{
    public class MakeUpZone: MonoBehaviour, IYoyoMakeup
    {
        [SerializeField] List<RectTransform> _yoyoPoints = new ();

        public List<Vector3> YoyoPoints
        {
            get
            {
                List<Vector3> yoyoPoints = new List<Vector3>();
                _yoyoPoints.ForEach(p => yoyoPoints.Add(p.transform.position));
                return yoyoPoints;  
            }
        }
    }
}