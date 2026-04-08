using UnityEngine;

namespace PlayneraTest.Code.Configs.ScriptablesScripts
{
    public class ScriptableSingleton<T>: ScriptableObject where T: ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] singletons = Resources.LoadAll<T>(string.Empty);

                    if (singletons == null || singletons.Length < 1)
                    {
                        throw new System.Exception($"No {typeof(T).Name} singletons object found");
                    }

                    if (singletons.Length > 1)
                    {
                        Debug.LogWarning($"Multiple {typeof(T).Name} singletons found");
                    }

                    _instance = singletons[0];
                    Debug.Log($"Instance {typeof(T).Name} singleton");
                }
                
                return _instance;
            }
        }
    }
}