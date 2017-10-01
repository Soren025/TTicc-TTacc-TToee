using UnityEngine;
using UnityEngine.Networking;

using Codari.TTT.Network;

namespace Codari.TTT
{
    [DisallowMultipleComponent]
    public sealed class TTTPlayer : NetworkBehaviour
    {
        public static TTTPlayer Local { get; private set; }

        public static TTTPlayer Remote { get; private set; }

        public static bool LocalReady => Local?.IsReady ?? false;

        public static bool RemoteReady => Remote?.IsReady ?? false;

        public static bool IsMyTurn => Local?.IsTurn ?? false;

        public static bool HasLocal => Local != null;

        public static bool HasRemote => Remote != null;

        public static bool HasTwoPlayers => HasLocal && HasRemote;

        public TTTProfile Profile => isLocalPlayer ? TTTProfile.Local : TTTProfile.Remote;

        [SyncVar]
        private XOIcon icon;
        [SyncVar]
        private bool isReady;

        public bool IsLocal => this == Local;

        public bool IsRemote => this == Remote;

        public XOIcon Icon => icon;

        public bool IsReady => isReady;

        public Color IconColor => IsLocal ? TTTMatch.Instance.LocalColor : IsRemote ? TTTMatch.Instance.RemoteColor : Color.magenta;

        public bool IsTurn => TTTMatch.Instance.CurrentTurn == icon;

        public bool IconSelected => icon.IsSelected();

        [Server]
        public void SetIcon(XOIcon icon)
        {
            print(icon + " : " + (this == Remote));

            if (icon != this.icon)
            {
                this.icon = icon;
            }
        }

        [Server]
        public void SetOppositeIcon()
        {
            icon = icon.Opposite();
        }

        [Client]
        public void SetReady(bool ready)
        {
            if (!TTTMatch.Instance.IsPlaying)
            {
                Cmd_SetReady(ready);
            }
        }

        [Client]
        public void SelectGrid(Coordinate coordinate)
        {
            if (TTTMatch.Instance.IsPlaying)
            {
                Cmd_SelectGrid(coordinate);
            }
        }

        [Client]
        public void SelectCell(Coordinate gridCoordinate, Coordinate cellCoordinate)
        {
            if (TTTMatch.Instance.IsPlaying)
            {
                Cmd_SelectCell(gridCoordinate, cellCoordinate);
            }
        }

        #region Unity Callbacks

        void Start()
        {
            if (isLocalPlayer)
            {
                Local = this;
            }
            else
            {
                Remote = this;
            }
        }

        void OnDestroy()
        {
            if (isLocalPlayer)
            {
                Local = null;
            }
            else
            {
                Remote = null;
                TTTProfile.ClearRemoteProfile();
            }
        }

        #endregion

        #region Network Callbacks

        public override void OnStartLocalPlayer()
        {
            if (!isServer)
            {
                Cmd_SendProfile(TTTProfile.Local.ToJson());
            }
        }

        #endregion

        #region Network Commands

        [Command]
        void Cmd_SendProfile(string profileJson)
        {
            Rpc_RecieveProfile(profileJson);

            if (!isLocalPlayer)
            {
                Local.Cmd_SendProfile(TTTProfile.Local.ToJson());
            }
        }

        [Command]
        void Cmd_SetReady(bool ready)
        {
            isReady = ready;
            TTTMatch.Instance.ReadyCheck();
        }

        [Command]
        void Cmd_SelectGrid(NetworkCoordinate coordinate)
        {
            TTTMatch.Instance.SelectGrid(coordinate, icon);
        }

        [Command]
        void Cmd_SelectCell(NetworkCoordinate gridCoordinate, NetworkCoordinate cellCoordinate)
        {
            TTTMatch.Instance.SelectCell(gridCoordinate, cellCoordinate, icon);
        }

        #endregion

        #region Network RPCs

        [ClientRpc]
        void Rpc_RecieveProfile(string profileJson)
        {
            if (!isLocalPlayer)
            {
                TTTProfile.ParseRemoteProfile(profileJson);
            }
        }

        #endregion

        //public static TTTPlayer Me { get; private set; }

        //public static TTTPlayer NotMe { get; private set; }

        //[SyncVar]
        //private Selection team;

        //public Selection Team => team;

        //public Color SelectionColor => IsMe ? TTTApplication.Instance.MeColor : TTTApplication.Instance.NotMeColor;

        //public bool IsMe => this == Me;

        //public bool IsNotMe => this != Me;

        //public bool IsX => team == Selection.X;

        //public bool IsO => team == Selection.O;

        //public bool IsMyTurn => TTTApplication.Instance.CurrentTurn == team;

        //public bool IsReady => TTTApplication.Instance.IsReady(team);

        //#region Commands

        //[Command]
        //public void CmdSetReady(bool ready)
        //{
        //    if (TTTApplication.Instance.IsPlaying) return;
        //    TTTApplication.Instance.SetReady(team, ready);
        //}

        //[Command]
        //public void CmdSelectCell(NetworkCoordinate gridCoordinate, NetworkCoordinate cellCoordinate)
        //{
        //    if (!TTTApplication.Instance.IsPlaying || !IsMyTurn) return;
        //    TTTApplication.Instance.SelectCell(gridCoordinate, cellCoordinate, team);
        //}

        //#endregion

        //#region Network Callbacks



        //#endregion

        //#region Unity Callbacks

        //void Start()
        //{
        //    if (isLocalPlayer)
        //    {
        //        if (Me == null)
        //            Me = this;
        //        else
        //            Debug.Log("More than one `Me`", this);
        //    }
        //    else
        //    {
        //        if (NotMe == null)
        //            NotMe = this;
        //        else
        //            Debug.Log("More than one `NotMe`", this);
        //    }

        //    if (isServer)
        //    {
        //        if (this == Me)
        //        {
        //            team = (Selection) Random.Range(1, 3);
        //        }
        //        else if (this == NotMe)
        //        {
        //            team = Me.team == Selection.O ? Selection.X : Selection.O;
        //        }
        //    }
        //}

        //void OnDestroy()
        //{
        //    if (this == Me)
        //        Me = null;
        //    if (this == NotMe)
        //        NotMe = null;
        //}

        //#endregion

        //#region SyncVar Hooks



        //#endregion
    }
}
