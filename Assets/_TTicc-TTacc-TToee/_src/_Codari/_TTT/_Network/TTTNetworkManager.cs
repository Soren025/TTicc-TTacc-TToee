using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

using System.Collections;

using DoozyUI;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Codari.TTT.Network
{
    public sealed class TTTNetworkManager : NetworkManager
    {
        [Header("TTT Properties")]
        [Range(1, 10)]
        [SerializeField]
        private int quickStartAttempts = 5;
        [Range(0f, 5f)]
        [SerializeField]
        private float quickStartAttemptDelay = 1f;
        [SerializeField]
        private UINotification quickStartCreateMatchNotification;

        public static TTTNetworkManager Instance => singleton as TTTNetworkManager;

        private bool isQuickStarting;
        private int quickStartMatchIndex;
        private int quickStartAttemptCount;

        public bool IsQuickStarting => isQuickStarting;

        public void CreateMatch(string matchName, string password = "")
        {
            StartMatchMaker();
            matchMaker.CreateMatch(matchName, 2, true, password, "", "", 0, 0, OnMatchCreate);
        }

        public void QuickStartMatch()
        {
            if (isQuickStarting || IsClientConnected()) return;

            isQuickStarting = true;
            quickStartAttemptCount = 0;

            StartMatchMaker();
            QuickStart_ListMatches();
        }

        public void CancelQuickStartMatch()
        {
            if (isQuickStarting)
            {
                isQuickStarting = false;
                StopMatchMaker();

                if (IsInvoking(nameof(QuickStart_ListMatches)))
                {
                    CancelInvoke(nameof(QuickStart_ListMatches));
                }
            }
        }

        public void LeaveMatch()
        {
            if (NetworkServer.active)
                StopHost();
            else if (IsClientConnected())
                StopClient();

            if (matchMaker != null)
                StopMatchMaker();
        }

        private void QuickStart_ListMatches()
        {
            quickStartAttemptCount++;
            matchMaker.ListMatches(0, 50, "", true, 0, 1, OnMatchList);
        }

        private void QuickStart_AttemptToJoinNextMatch()
        {
            if (quickStartMatchIndex < matches.Count)
            {
                matchMaker.JoinMatch(matches[quickStartMatchIndex++].networkId, "", "", "", 0, 1, OnMatchJoined);
            }
            else if (quickStartAttemptCount < quickStartAttempts)
            {
                Invoke(nameof(QuickStart_ListMatches), quickStartAttemptDelay);
            }
            else
            {
                StopMatchMaker();
                isQuickStarting = false;

                // This is really ugly, but aperently is how to do it.
                UIManager.NotificationManager.ShowNotification(quickStartCreateMatchNotification.gameObject, -1f, false,
                    "No Match Found", "There were no matches to quick join, would you like to create one instead?", null,
                    new string[] { "Go To Multiplayer Menu", "Quick Start Create Match" }, new string[] { "No", "Yes" });
            }
        }

        #region Network Overrides

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            //ClientAddPlayer(conn);
            ClientScene.AddPlayer(conn, 0);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            if (!clientLoadedScene)
            {
                //ClientAddPlayer(conn);
                ClientScene.AddPlayer(conn, 0);
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            //OnServerAddPlayerInternal(conn, playerControllerId, null);
            NetworkServer.AddPlayerForConnection(conn, Instantiate(playerPrefab).gameObject, playerControllerId);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            //OnServerAddPlayerInternal(conn, playerControllerId, extraMessageReader);
            NetworkServer.AddPlayerForConnection(conn, Instantiate(playerPrefab).gameObject, playerControllerId);
        }

        public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
        {
            base.OnMatchList(success, extendedInfo, matchList);

            if (isQuickStarting)
            {
                quickStartMatchIndex = 0;
                QuickStart_AttemptToJoinNextMatch();
            }
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (isQuickStarting)
            {
                if (success)
                {
                    isQuickStarting = false;
                    base.OnMatchJoined(success, extendedInfo, matchInfo);
                }
                else
                {
                    QuickStart_AttemptToJoinNextMatch();
                }
            }
        }

        //private void ClientAddPlayer(NetworkConnection conn)
        //{
        //    if (conn.IsLocalConnectionToServer())
        //    {
        //        ClientScene.AddPlayer(conn, 0);
        //    }
        //    else
        //    {
        //        ClientScene.AddPlayer(conn, 0, new TTTProfileMessage { json = TTTProfile.Local.ToJson() });
        //    }
        //}

        //private void OnServerAddPlayerInternal(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        //{
        //    if (extraMessageReader != null && !conn.IsLocalConnectionToClient())
        //    {
        //        TTTProfile.ParseRemoteProfile(extraMessageReader.ReadMessage<TTTProfileMessage>().json);
        //    }

        //    NetworkServer.AddPlayerForConnection(conn, Instantiate(playerPrefab).gameObject, playerControllerId);
        //}

        #endregion
    }
}
