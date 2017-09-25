using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using DoozyUI;
using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public sealed class TTTMatch : NetworkBehaviour
    {
        public static TTTMatch Instance { get; private set; }

        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField, BoxGroup("Scene Instances")]
        private TTTBoard board;

        [SerializeField, BoxGroup("Icon Coloes")]
        private Color localIconColor;
        [SerializeField, BoxGroup("Icon Coloes")]
        private Color remoteIconColor;

        [Header("Name")]
        [SerializeField, BoxGroup("UI Elements")]
        private Text nameLocal;
        [SerializeField, BoxGroup("UI Elements")]
        private Text nameRemote;
        [Header("Ready Toggle")]
        [SerializeField, BoxGroup("UI Elements")]
        private Toggle readyLocal;
        [SerializeField, BoxGroup("UI Elements")]
        private Toggle readyRemote;
        [Header("X or O Icon")]
        [SerializeField, BoxGroup("UI Elements")]
        private Text iconLocal;
        [SerializeField, BoxGroup("UI Elements")]
        private Text iconRemote;

        [SyncVar]
        private bool isPlaying;

        public TTTBoard Board => board;

        public Color LocalColor => localIconColor;

        public Color RemoteColor => remoteIconColor;

        public bool IsPlaying => isPlaying;

        #region Unity Callbacks

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            readyRemote.interactable = false;

            iconLocal.color = LocalColor;
            iconRemote.color = RemoteColor;
        }

        void Update()
        {
            nameLocal.text = TTTProfile.Local.Name;
            nameRemote.text = TTTProfile.Remote?.Name ?? "<not connected>";

            readyLocal.interactable = !IsPlaying;

            iconLocal.text = TTTPlayer.Local?.Icon.ToText() ?? XOIcon.None.ToText();
            iconLocal.text = TTTPlayer.Remote?.Icon.ToText() ?? XOIcon.None.ToText();
        }

        void OnDestroy()
        {
            Instance = null;
        }

        #endregion
    }
}
