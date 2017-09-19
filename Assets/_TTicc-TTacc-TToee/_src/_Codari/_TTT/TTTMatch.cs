using UnityEngine;
using UnityEngine.Networking;

using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public sealed class TTTMatch : NetworkBehaviour
    {
        public TTTMatch Instance { get; private set; }

        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField]
        private TTTBoard board;

        public TTTBoard Board => board;

        #region Unity Callbacks

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        #endregion
    }
}
