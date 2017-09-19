using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using DoozyUI;
using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public sealed class TTTCell : NetworkBehaviour
    {
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField]
        private Coordinate coordinate;

        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField, BoxGroup("UI")]
        private UIButton cellButton;
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField, BoxGroup("UI")]
        private Text cellIcon;

        public Coordinate Coordinate => coordinate;

        #region Unity Callbacks

        void Awake()
        {

        }

        #endregion

        #region Network Callbacks

        public override void OnStartLocalPlayer()
        {

        }

        #endregion
    }
}
