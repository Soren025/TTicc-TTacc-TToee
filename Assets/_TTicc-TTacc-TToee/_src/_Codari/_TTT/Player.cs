using UnityEngine;
using UnityEngine.Networking;

using Codari.TTT.Network;

namespace Codari.TTT
{
    [DisallowMultipleComponent]
    public sealed class Player : NetworkBehaviour
    {
        public static Player Me { get; private set; }

        public static Player NotMe { get; private set; }

        [SyncVar]
        private Selection team;

        public Selection Team => team;

        public Color SelectionColor => IsMe ? TTiccTTaccTToee.Instance.MeColor : TTiccTTaccTToee.Instance.NotMeColor;

        public bool IsMe => this == Me;

        public bool IsNotMe => this != Me;

        public bool IsX => team == Selection.X;

        public bool IsO => team == Selection.O;

        public bool IsMyTurn => TTiccTTaccTToee.Instance.CurrentTurn == team;

        public bool IsReady => TTiccTTaccTToee.Instance.IsReady(team);

        #region Commands

        [Command]
        public void CmdSetReady(bool ready)
        {
            if (TTiccTTaccTToee.Instance.IsPlaying) return;
            TTiccTTaccTToee.Instance.SetReady(team, ready);
        }

        [Command]
        public void CmdSelectCell(NetworkCoordinate gridCoordinate, NetworkCoordinate cellCoordinate)
        {
            if (!TTiccTTaccTToee.Instance.IsPlaying || !IsMyTurn) return;
            TTiccTTaccTToee.Instance.SelectCell(gridCoordinate, cellCoordinate, team);
        }

        #endregion

        #region Network Callbacks



        #endregion

        #region Unity Callbacks

        void Start()
        {
            if (isLocalPlayer)
            {
                if (Me == null)
                    Me = this;
                else
                    Debug.Log("More than one `Me`", this);
            }
            else
            {
                if (NotMe == null)
                    NotMe = this;
                else
                    Debug.Log("More than one `NotMe`", this);
            }

            if (isServer)
            {
                if (this == Me)
                {
                    team = (Selection) Random.Range(1, 3);
                }
                else if (this == NotMe)
                {
                    team = Me.team == Selection.O ? Selection.X : Selection.O;
                }
            }
        }

        void OnDestroy()
        {
            if (this == Me)
                Me = null;
            if (this == NotMe)
                NotMe = null;
        }

        #endregion

        #region SyncVar Hooks



        #endregion
    }
}
