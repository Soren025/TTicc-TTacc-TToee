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

        [SyncVar]
        private XOIcon icon;

        private TTTGrid grid;

        public XOIcon Icon => icon;

        public TTTGrid Grid => grid;

        public Coordinate Coordinate => coordinate;

        [Server]
        public void SetIcon(XOIcon icon)
        {
            this.icon = icon;
        }

        #region Unity Callbacks

        void Awake()
        {
            grid = GetComponentInParent<TTTGrid>();

            cellButton.OnClick.AddListener(() => TTTPlayer.Local.SelectCell(grid.Coordinate, coordinate));
            cellIcon.text = "";
        }

        void LateUpdate()
        {
            cellButton.Interactable = TTTMatch.Instance.IsPlaying && TTTPlayer.IsMyTurn && TTTMatch.Instance.IsGridSelected
                && (grid.IsCurrent || (TTTMatch.Instance.Board[TTTMatch.Instance.CurrentGrid].IsFull && !grid.IsFull))
                && !icon.IsSelected();

            cellIcon.text = icon.ToText(true);
            if (icon.IsSelected())
            {
                if (icon == TTTPlayer.Local.Icon)
                {
                    cellIcon.color = TTTPlayer.Local.IconColor;
                }

                if (icon == TTTPlayer.Remote.Icon)
                {
                    cellIcon.color = TTTPlayer.Remote.IconColor;
                }
            }
        }

        #endregion

        #region Network Callbacks

        public override void OnStartLocalPlayer()
        {

        }

        #endregion
    }
}
