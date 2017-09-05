using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Codari.TTT
{
    public sealed class Cell : NetworkBehaviour
    {
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("cellCoordinate")]
        private Coordinate coordinate;

        private Button selectButton;
        private Text selectionText;

        [SyncVar(hook = nameof(Hook_SelectionChanged))]
        private Selection selection;

        private Grid grid;

        public Grid Grid => grid;

        public Coordinate Coordinate => coordinate;

        public Selection Selection => selection;

        public bool Interactable => selectButton.interactable;

        [Server]
        public void SetSelection(Selection selection)
        {
            if (selection != this.selection)
            {
                this.selection = selection;
            }
        }

        #region Unity Callbacks

        void Awake()
        {
            grid = GetComponentsInParent<Grid>(true)[0];

            selectButton = GetComponentInChildren<Button>();
            selectButton.onClick.AddListener(UIListener_OnSelectButtonPress);
            selectButton.interactable = false;

            selectionText = GetComponentInChildren<Text>();
            selectionText.enabled = false;
        }

        void LateUpdate()
        {
            selectButton.interactable = (Player.Me?.IsMyTurn ?? false)
                && (grid.IsCurrent || (TTiccTTaccTToee.Instance[TTiccTTaccTToee.Instance.CurrentGrid].IsFull && !grid.IsFull))
                && !selection.IsSelected();
        }

        #endregion

        #region UI Listeners

        void UIListener_OnSelectButtonPress()
        {
            Player.Me.CmdSelectCell(grid.Coordinate, coordinate);
        }

        #endregion

        #region Hooks

        void Hook_SelectionChanged(Selection newSelection)
        {
            switch (newSelection)
            {
            case Selection.X:
                selectionText.text = "X";
                selectionText.color = Player.Me.IsX ? TTiccTTaccTToee.Instance.MeColor : TTiccTTaccTToee.Instance.NotMeColor;
                selectionText.enabled = true;
                selectButton.interactable = false;
                break;
            case Selection.O:
                selectionText.text = "O";
                selectionText.color = Player.Me.IsO ? TTiccTTaccTToee.Instance.MeColor : TTiccTTaccTToee.Instance.NotMeColor;
                selectionText.enabled = true;
                selectButton.interactable = false;
                break;
            default:
                selectionText.enabled = false;
                selectButton.interactable = true;
                break;
            }

            selection = newSelection;
        }

        #endregion
    }
}
