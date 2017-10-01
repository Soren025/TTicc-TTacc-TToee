using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System;
using System.Collections;
using System.Collections.Generic;

using DoozyUI;

using Codari.TTT.Network;

namespace Codari.TTT
{
    public class TTTApplication : MonoBehaviour
    {
        public static TTTApplication Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            LoadSingleton<TTTApplication>("TTT Application");
            LoadSingleton<TTTNetworkManager>("TTT Network Manager");
            LoadSingleton<UIManager>("UI Manager");
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static void LoadSingleton<T>(string prefabName)
            where T : MonoBehaviour
        {
            T prefab = Resources.Load<T>(prefabName);
            T instance = Instantiate(prefab);
            instance.name = prefab.name;
            DontDestroyOnLoad(instance);
        }

        #region Unity Callbacks

        void Awake()
        {
            Instance = this;
        }

        #endregion
    }
}
