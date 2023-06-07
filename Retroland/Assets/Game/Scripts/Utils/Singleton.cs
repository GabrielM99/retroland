using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                //If the instance hasn't been initialized (like if we are on editor).
                if (_instance == null)
                {
                    //Search for this component on the scene.
                    _instance = FindObjectOfType<T>(true);
                }

                return _instance;
            }

            private set => _instance = value;
        }

        protected virtual void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
        }
    }
}
