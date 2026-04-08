using System;
using System.Collections.Generic;
using PlayneraTest.Code.Configs.ScriptablesScripts;
using UnityEngine;

namespace PlayneraTest.Code.Scripts
{
    [CreateAssetMenu(fileName = "ConfigsProvider", menuName = "Configs/ConfigsProvider/New ConfigsProvider")]
    public class Configs: ScriptableSingleton<Configs>
    {
        [SerializeField] private List<ScriptableObject> _settings;
        private Dictionary<Type, ScriptableObject> _settingsByType = new ();
        private Dictionary<string, ScriptableObject> _settingsByName = new (); 
        private bool _isInitialized;

        private void Initialize()
        {
            foreach (var setting in _settings)
            {
                if (setting == null) 
                    continue;
                
                var prototype = Instantiate(setting);
                
                _settingsByType.Add(setting.GetType(), prototype);
                _settingsByName.Add(prototype.name, prototype);
            }
            
            _isInitialized = true;
        }

        public T Get<T>() where T : ScriptableObject
        {
            TryInitialize();
            
            Type type = typeof(T);
            
            if (_settingsByType.TryGetValue(type, out var config))
            {
                return (T)config; 
            }
            
            Debug.LogError($"Config of type {type.Name} not found!");
            
            return null;
        }

        public ScriptableObject this[String typeName]
        {
            get
            {
                TryInitialize();
                
                if (_settingsByName.TryGetValue(typeName, out var item))
                {
                    return item;
                }
                
                Debug.LogError($"Config of type {typeName} not found!");
            
                return null;
            }
        }

        private void TryInitialize()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }
    }
}