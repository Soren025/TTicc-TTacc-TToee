using UnityEngine.Networking;
using UnityEditor;

namespace Codari.TTT.Network
{
    [CustomEditor(typeof(TTTNetworkManager))]
    public sealed class TTTNetworkManagerEditor : NetworkManagerEditor
    {
        // Copied from decompiled `NetworkManagerEditor` and modified to remove unneeded elements
        public override void OnInspectorGUI()
        {
            if (m_DontDestroyOnLoadProperty == null || m_DontDestroyOnLoadLabel == null)
                m_Initialized = false;
            Init();
            serializedObject.Update();
            //EditorGUILayout.PropertyField(m_DontDestroyOnLoadProperty, m_DontDestroyOnLoadLabel, new GUILayoutOption[0]);
            //EditorGUILayout.PropertyField(m_RunInBackgroundProperty, m_RunInBackgroundLabel, new GUILayoutOption[0]);
            if (EditorGUILayout.PropertyField(m_LogLevelProperty))
                LogFilter.currentLogLevel = (int) m_NetworkManager.logLevel;
            ShowScenes();
            ShowNetworkInfo();
            //ShowSpawnInfo();
            ShowConfigInfo();
            ShowSimulatorInfo();

            // Below are changes to make sure the removed elements dont cause problems by not being able to edit them
            {
                m_DontDestroyOnLoadProperty.boolValue = false;
                m_RunInBackgroundProperty.boolValue = true;

                // The cached propertys is not exposed, coppied from decompiled `NetworkManagerEditor`.
                serializedObject.FindProperty("m_PlayerPrefab").objectReferenceValue = null;
                serializedObject.FindProperty("m_AutoCreatePlayer").boolValue = false;
            }

            serializedObject.ApplyModifiedProperties();
            ShowDerivedProperties(typeof(NetworkManager), null);
        }
    }
}
