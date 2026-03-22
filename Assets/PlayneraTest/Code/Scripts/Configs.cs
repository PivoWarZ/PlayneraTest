using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayneraTest.Code.Scripts
{
    [CreateAssetMenu(fileName = "ConfigsProvider", menuName = "Configs/ConfigsProvider/New ConfigsProvider")]
    public class Configs: ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> _configs;

        public T GetConfig<T>() where T : ScriptableObject
        {
            foreach (var scriptableObject in _configs)
            {
                if (scriptableObject is T config)
                    return config;
            }
            
            throw new Exception("Config not found");
        }
    }
}