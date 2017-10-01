using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;
using System.Collections.Generic;
using System.Text;

using DoozyUI;
using Sirenix.OdinInspector;

using Codari.TTT.Network;

using Random = UnityEngine.Random;

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
        [SerializeField, BoxGroup("Player UI Elements")]
        private Text nameLocal;
        [SerializeField, BoxGroup("Player UI Elements")]
        private Text nameRemote;
        [Header("Ready Toggle")]
        [SerializeField, BoxGroup("Player UI Elements")]
        private UIToggle readyLocal;
        [SerializeField, BoxGroup("Player UI Elements")]
        private UIToggle readyRemote;
        [Header("X or O Icon")]
        [SerializeField, BoxGroup("Player UI Elements")]
        private Text iconLocal;
        [SerializeField, BoxGroup("Player UI Elements")]
        private Text iconRemote;
        [Header("Turn Highlight")]
        [SerializeField, BoxGroup("Player UI Elements")]
        private Image turnHighlightLocal;
        [SerializeField, BoxGroup("Player UI Elements")]
        private Image turnHighlightRemote;
        [Header("Winner Text")]
        [SerializeField, BoxGroup("Player UI Elements")]
        private Text winnerTextRight;
        [SerializeField, BoxGroup("Player UI Elements")]
        private Text winnerTextLeft;

        [SerializeField, BoxGroup("Menu UI Elements")]
        private UIButton drawButton;
        [SerializeField, BoxGroup("Menu UI Elements")]
        private UIButton resignButton;

        [SyncVar]
        private bool isPlaying;

        [SyncVar]
        private XOIcon whoGoesFirst;
        [SyncVar]
        private XOIcon currentTurn = XOIcon.None;
        [SyncVar]
        private XOIcon currentWinner = XOIcon.None;
        [SyncVar]
        private string winnerText = string.Empty;

        [SyncVar]
        private NetworkCoordinate currentGrid = Coordinate.Invalid;

        public TTTBoard Board => board;

        public Color LocalColor => localIconColor;

        public Color RemoteColor => remoteIconColor;

        public bool IsPlaying => isPlaying;

        public XOIcon CurrentTurn => currentTurn;

        public XOIcon CurrentWinner => currentWinner;

        public Coordinate CurrentGrid => currentGrid;

        public bool IsGridSelected => currentGrid != Coordinate.Invalid;

        [Server]
        public void ReadyCheck()
        {
            if (isPlaying) return;

            if (TTTPlayer.LocalReady && TTTPlayer.RemoteReady)
            {
                StartGame();
            }
        }

        [Server]
        public void SelectGrid(Coordinate coordinate, XOIcon player)
        {
            if (isPlaying && !IsGridSelected && player == currentTurn)
            {
                currentGrid = coordinate;
                currentTurn = currentTurn.Opposite();
            }
        }

        [Server]
        public void SelectCell(Coordinate gridCoordinate, Coordinate cellCoordinate, XOIcon player)
        {
            if (isPlaying && IsGridSelected && player == currentTurn)
            {
                board[gridCoordinate, cellCoordinate].SetIcon(player);

                bool gridWin = false;

                // Remove this `if` if you want to be able to override a grid winner with a new row
                //if (!board[gridCoordinate].Icon.IsSelected())
                {

                    foreach (IEnumerable<Coordinate> row in Coordinate.Rows)
                    {
                        int rowCount = 0;

                        foreach (Coordinate rowCoord in row)
                        {
                            if (board[gridCoordinate, rowCoord].Icon == player)
                            {
                                rowCount++;
                            }
                        }

                        if (rowCount == 3)
                        {
                            if (player != board[gridCoordinate].Icon)
                            {
                                board[gridCoordinate].SetIcon(player);
                                gridWin = true;
                                if (TestForGameWin(player)) return;
                            }
                        }
                    }
                }

                currentGrid = cellCoordinate;

                if (!gridWin)
                {
                    currentTurn = currentTurn.Opposite();
                }
            }
        }

        [Server]
        public void StartGame()
        {
            if (isPlaying) return;

            foreach (TTTGrid grid in board)
            {
                grid.SetIcon(XOIcon.None);

                foreach (TTTCell cell in grid)
                {
                    cell.SetIcon(XOIcon.None);
                }
            }

            if (TTTPlayer.Local.IconSelected && TTTPlayer.Remote.IconSelected)
            {
                TTTPlayer.Local.SetOppositeIcon();
                TTTPlayer.Remote.SetOppositeIcon();
            }
            else
            {
                TTTPlayer.Local.SetIcon(RandomIcon());
                TTTPlayer.Remote.SetIcon(TTTPlayer.Local.Icon.Opposite());
            }

            whoGoesFirst = whoGoesFirst.Opposite();
            currentTurn = whoGoesFirst;
            currentWinner = XOIcon.None;
            winnerText = string.Empty;

            currentGrid = Coordinate.Invalid;

            isPlaying = true;
        }

        [Server]
        public void EndGame()
        {
            if (!isPlaying) return;

            isPlaying = false;
            currentTurn = XOIcon.None;
            currentGrid = Coordinate.Invalid;

            Rpc_ResetReadyToggles();
        }

        private bool TestForGameWin(XOIcon player)
        {
            foreach (IEnumerable<Coordinate> row in Coordinate.Rows)
            {
                int rowCount = 0;

                foreach (Coordinate rowCoord in row)
                {
                    if (board[rowCoord].Icon == player)
                    {
                        rowCount++;
                    }
                }

                if (rowCount == 3)
                {
                    currentWinner = player;

                    string playerName = TTTPlayer.Local.Icon == currentWinner ? TTTPlayer.Local.Profile.Name : TTTPlayer.Remote.Profile.Name;
                    StringBuilder winnerTextBuilder = new StringBuilder();

                    foreach (char nameChar in playerName)
                    {
                        winnerTextBuilder.Append(nameChar).Append(Environment.NewLine);
                    }

                    winnerTextBuilder.Append(' ').Append(Environment.NewLine)
                        .Append('W').Append(Environment.NewLine)
                        .Append('I').Append(Environment.NewLine)
                        .Append('N').Append(Environment.NewLine)
                        .Append('S');

                    winnerText = winnerTextBuilder.ToString();

                    EndGame();
                    return true;
                }
            }

            return false;
        }

        private XOIcon RandomIcon() => (XOIcon) (Random.Range(0, 2) + 1);

        #region Unity Callbacks

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            readyLocal.OnClickToggleOn.AddListener(() => TTTPlayer.Local.SetReady(true));
            readyLocal.OnClickToggleOff.AddListener(() => TTTPlayer.Local.SetReady(false));

            readyRemote.Interactable = false;

            //drawButton.OnClick.AddListener(() => { });
            //resignButton.OnClick.AddListener(() => { });

            iconLocal.color = LocalColor;
            iconRemote.color = RemoteColor;
        }

        void LateUpdate()
        {
            nameLocal.text = TTTProfile.Local.Name;
            nameRemote.text = TTTProfile.Remote?.Name ?? "<not connected>";

            readyLocal.Interactable = !IsPlaying;

            if (isPlaying)
            {
                readyLocal.IsOn = true;
            }
            readyRemote.IsOn = TTTPlayer.Remote?.IsReady ?? false;

            iconLocal.text = TTTPlayer.Local?.Icon.ToText() ?? XOIcon.None.ToText();
            iconRemote.text = TTTPlayer.Remote?.Icon.ToText() ?? XOIcon.None.ToText();

            if (currentTurn.IsSelected())
            {
                turnHighlightLocal.enabled = currentTurn == (TTTPlayer.Local?.Icon ?? XOIcon.None);
                turnHighlightRemote.enabled = currentTurn == (TTTPlayer.Remote?.Icon ?? XOIcon.None);
            }
            else
            {
                turnHighlightLocal.enabled = false;
                turnHighlightRemote.enabled = false;
            }

            winnerTextRight.enabled = currentWinner.IsSelected();
            winnerTextLeft.enabled = currentWinner.IsSelected();

            winnerTextRight.text = winnerText;
            winnerTextLeft.text = winnerText;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        #endregion

        #region Network Callbacks

        public override void OnStartServer()
        {
            whoGoesFirst = RandomIcon();
        }

        #endregion

        #region RPCs

        [ClientRpc]
        private void Rpc_ResetReadyToggles()
        {
            readyLocal.IsOn = false;
        }

        #endregion

        #region UI Listeners



        #endregion
    }
}
