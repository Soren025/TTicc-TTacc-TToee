using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using Codari.TTT.Network;

namespace Codari.TTT
{
    public class TTiccTTaccTToee : NetworkBehaviour, IEnumerable<Grid>
    {
        public static TTiccTTaccTToee Instance { get; private set; }

        [SerializeField]
        private Text winnerText;
        [SerializeField]
        private Grid[] gridsIGuess;

        [Header("Audio")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip yourTurn;
        [SerializeField]
        private AudioClip notYourTurn;
        [SerializeField]
        private AudioClip youWin;
        [SerializeField]
        private AudioClip youLost;

        [Header("Info Backgrounds")]
        [SerializeField]
        private Image meBackground;
        [SerializeField]
        private Image notMeBackground;

        [Header("Team Colors")]
        [SerializeField]
        private Color meColor;
        [SerializeField]
        private Color notMeColor;

        [Header("Selection Icon Texts")]
        [SerializeField]
        private Text meIcon;
        [SerializeField]
        private Text notMeIcon;

        [Header("Ready Toggles")]
        [SerializeField]
        private Toggle meReadyToggle;
        [SerializeField]
        private Toggle notMeReadyToggle;

        private Grid[,] grids = new Grid[3,3];

        private Selection whoGoesFirst = Selection.X;
        private Color infoBackgroundBase;

        [SyncVar]
        private NetworkCoordinate currentGrid = Coordinate.Invalid;

        [SyncVar(hook = nameof(Hook_TurnChange))]
        private Selection currentTurn = Selection.None;

        [SyncVar]
        private bool xReady = false;

        [SyncVar]
        private bool oReady = false;

        [SyncVar]
        private bool isPlaying = false;

        [SyncVar(hook = nameof(Hook_OnWinnerSet))]
        private Selection currentWinner = Selection.None;

        private Coroutine yourTurnLoop;

        public Color MeColor => meColor;

        public Color NotMeColor => notMeColor;

        public Selection CurrentTurn => currentTurn;

        public Coordinate CurrentGrid => currentGrid;

        public bool XReady => xReady;

        public bool OReady => oReady;

        public bool IsPlaying => isPlaying;

        public Grid this[Coordinate coordinate] => grids[coordinate.X, coordinate.Y];

        public Cell this[Coordinate gridCoordinate, Coordinate cellCoordinate] => this[gridCoordinate][cellCoordinate];

        public bool IsReady(Selection team)
        {
            switch (team)
            {
            case Selection.X: return xReady;
            case Selection.O: return oReady;
            default: return false;
            }
        }

        public void SetReady(Selection team, bool ready)
        {
            switch (team)
            {
            case Selection.X:
                xReady = ready;
                break;
            case Selection.O:
                oReady = ready;
                break;
            }

            if (xReady && oReady)
            {
                StartGame();
            }
        }

        [Server]
        public void StartGame()
        {
            if (isPlaying) return;

            foreach (Grid grid in this)
            {
                grid.SetGridWinner(Selection.None);

                foreach (Cell cell in grid)
                {
                    cell.SetSelection(Selection.None);
                }
            }

            currentWinner = Selection.None;
            currentTurn = whoGoesFirst;
            whoGoesFirst = whoGoesFirst == Selection.X ? Selection.O : Selection.X;
            currentGrid = new Coordinate((byte) Random.Range(0, 3), (byte) Random.Range(0, 3));
            isPlaying = true;
        }

        [Server]
        public void EndGame()
        {
            isPlaying = false;
            currentTurn = Selection.None;
            currentGrid = Coordinate.Invalid;

            Rpc_ResetReadyToggles();
        }

        [Server]
        public void SelectCell(Coordinate gridCoordinate, Coordinate cellCoordinate, Selection selection)
        {
            if (!isPlaying) return;
            if (!CurrentGrid.IsValid) return;

            this[gridCoordinate, cellCoordinate].SetSelection(selection);

            foreach (IEnumerable<Coordinate> row in Coordinate.Rows)
            {
                int rowCount = 0;

                foreach (Coordinate rowCoord in row)
                {
                    if (this[gridCoordinate, rowCoord].Selection == selection)
                    {
                        rowCount++;
                    }
                }

                if (rowCount == 3)
                {

                    // Remove this `if` if you want to be able to override a grid winner with a new row
                    //if (!this[gridCoordinate].GridWinner.IsSelected())
                    {
                        this[gridCoordinate].SetGridWinner(selection);
                    }

                    if (TestForGameWin(selection)) return;
                }
            }

            currentGrid = cellCoordinate;
            currentTurn = selection == Selection.X ? Selection.O : Selection.X;
        }

        private bool TestForGameWin(Selection team)
        {
            foreach (IEnumerable<Coordinate> row in Coordinate.Rows)
            {
                int rowCount = 0;

                foreach (Coordinate rowCoord in row)
                {
                    if (this[rowCoord].GridWinner == team)
                    {
                        rowCount++;
                    }
                }

                if (rowCount == 3)
                {
                    currentWinner = team;
                    EndGame();
                    return true;
                }
            }

            return false;
        }

        #region Unity Callbacks

        void Awake()
        {
            Instance = this;
            meReadyToggle.onValueChanged.AddListener(UIListener_OnReadyToggle);

            foreach (Grid grid in gridsIGuess)
            {
                grids[grid.Coordinate.X, grid.Coordinate.Y] = grid;
            }

            infoBackgroundBase = meBackground.color;
        }

        void LateUpdate()
        {
            meReadyToggle.interactable = isClient && !isPlaying;
            notMeReadyToggle.isOn = ((Player.NotMe?.IsX ?? false) && xReady) || ((Player.NotMe?.IsO ?? false) && oReady);

            meIcon.text = Player.Me?.Team.ToString() ?? "?";
            notMeIcon.text = Player.NotMe?.Team.ToString() ?? "?";

            winnerText.enabled = currentWinner.IsSelected();
            if (winnerText.enabled)
            {
                winnerText.text = $"{currentWinner.ToString()}\n\nW\nI\nN\nS";
            }

            meBackground.color = (Player.Me?.IsMyTurn ?? false) ? new Color(.75f, .75f, 0, .5f) : infoBackgroundBase;
            notMeBackground.color = (Player.NotMe?.IsMyTurn ?? false) ? new Color(.75f, .75f, 0, .5f) : infoBackgroundBase;
        }

        #endregion

        #region Network Callbacks



        #endregion

        #region RPCs

        [ClientRpc]
        private void Rpc_ResetReadyToggles()
        {
            meReadyToggle.isOn = false;
        }

        #endregion

        #region UI Listeners

        void UIListener_OnReadyToggle(bool ready)
        {
            Player.Me.CmdSetReady(ready);
        }

        #endregion

        #region SyncVar Hooks

        void Hook_TurnChange(Selection turn)
        {
            currentTurn = turn;

            if (turn.IsSelected())
            {
                if (Player.Me.Team == turn)
                {
                    AudioSource.PlayClipAtPoint(yourTurn, Vector3.zero);
                    yourTurnLoop = StartCoroutine(YourTurnLoop());
                }
                else
                {
                    AudioSource.PlayClipAtPoint(notYourTurn, Vector3.zero);
                    if (yourTurnLoop != null)
                        StopCoroutine(yourTurnLoop);
                }
            }
        }

        void Hook_OnWinnerSet(Selection winner)
        {
            currentWinner = winner;
            
            if (winner.IsSelected())
            {
                if (Player.Me.Team == winner)
                    AudioSource.PlayClipAtPoint(youWin, Vector3.zero);
                else
                    AudioSource.PlayClipAtPoint(youLost, Vector3.zero);
            }
        }

        #endregion

        private IEnumerator YourTurnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(30);
                AudioSource.PlayClipAtPoint(yourTurn, Vector3.zero);
            }
        }

        public IEnumerator<Grid> GetEnumerator()
        {
            foreach (Grid grid in grids)
            {
                yield return grid;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
