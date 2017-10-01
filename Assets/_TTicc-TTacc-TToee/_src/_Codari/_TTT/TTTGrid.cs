using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using DoozyUI;
using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public sealed class TTTGrid : SerializedNetworkBehaviour, IEnumerable<TTTCell>
    {
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField]
        [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis", IsReadOnly = true, ResizableColumns = false)]
        private TTTCell[,] cells = new TTTCell[3, 3];

        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField]
        private Coordinate coordinate;

        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField, BoxGroup("UI")]
        private Text gridIcon;
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField, BoxGroup("UI")]
        private Image gridHighlight;
        [ReadOnly] // Comment this attribute out if editing is required (basicly if you fuck up the scene)
        [SerializeField, BoxGroup("UI")]
        private UIButton gridSelectionButton;

        [SyncVar]
        private XOIcon icon;

        public Coordinate Coordinate => coordinate;

        public XOIcon Icon => icon;

        public TTTCell this[Coordinate coordinate] => cells[coordinate.X, coordinate.Y];

        public bool IsFull
        {
            get
            {
                foreach (TTTCell cell in cells)
                {
                    if (!cell.Icon.IsSelected())
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool IsCurrent => TTTMatch.Instance.CurrentGrid == coordinate;

        [Server]
        public void SetIcon(XOIcon icon)
        {
            this.icon = icon;
        }

        public IEnumerator<TTTCell> GetEnumerator()
        {
            foreach (TTTCell cell in cells)
            {
                yield return cell;
            }
        }

        #region Unity Callbacks

        void Awake()
        {
            gridSelectionButton.OnClick.AddListener(() => TTTPlayer.Local.SelectGrid(coordinate));
        }

        void LateUpdate()
        {
            gridHighlight.enabled = !IsFull
                && (IsCurrent
                    || (TTTMatch.Instance.CurrentGrid.IsValid ? TTTMatch.Instance.Board[TTTMatch.Instance.CurrentGrid].IsFull : false));

            if (gridHighlight.enabled)
            {
                gridHighlight.color = TTTPlayer.IsMyTurn ? TTTMatch.Instance.LocalColor : TTTMatch.Instance.RemoteColor;
            }

            gridIcon.text = icon.ToText(true);
            if (icon.IsSelected())
            {
                float a = gridIcon.color.a;
                Color color = gridIcon.color;

                if (icon == TTTPlayer.Local.Icon)
                {
                    color = TTTPlayer.Local.IconColor;
                }

                if (icon == TTTPlayer.Remote.Icon)
                {
                    color = TTTPlayer.Remote.IconColor;
                }

                color.a = a;
                
                gridIcon.color = color;
                gridIcon.enabled = true;
            }
            else
            {
                gridIcon.enabled = false;
            }

            if (TTTMatch.Instance.IsPlaying && TTTPlayer.Local.IsTurn && !TTTMatch.Instance.IsGridSelected)
            {
                gridSelectionButton.gameObject.SetActive(true);
            }
            else
            {
                gridSelectionButton.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Explicit Definitions

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
