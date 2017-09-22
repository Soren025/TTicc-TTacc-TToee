using UnityEngine;
using UnityEngine.Networking;

namespace Codari.TTT
{
    [DisallowMultipleComponent]
    public sealed class TTTPlayer : NetworkBehaviour
    {
        public TTTProfile Profile => isLocalPlayer ? TTTProfile.Local : TTTProfile.Remote;

        #region Unity Callbacks



        #endregion

        #region Network Callbacks

        public override void OnStartLocalPlayer()
        {

        }

        #endregion

        #region Network Commands



        #endregion

        #region Network RPCs



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
