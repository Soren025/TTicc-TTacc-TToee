using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

using UnityEngine.Networking.Match;

namespace Codari.TTT.Network
{
    public sealed class TTTNetworkManager : NetworkManager
    {
        public static TTTNetworkManager Instance => singleton as TTTNetworkManager;

        private Coroutine quickStartMatchCoroutine;

        public bool IsQuickStarting => quickStartMatchCoroutine != null;

        public void CreateMatch(string matchName, string password = "")
        {
            StartMatchMaker();
            matchMaker.CreateMatch(matchName, 2, true, password, "", "", 0, 0, OnMatchCreate);
        }

        public void QuickStartMatch()
        {
            if (IsQuickStarting || IsClientConnected() || NetworkServer.active) return;
            quickStartMatchCoroutine = StartCoroutine(Coroutine_QuickJoinMatch());
        }

        public void CancelQuickStartMatch()
        {
            if (!IsQuickStarting) return;
            StopQuickJoinMatchCoroutine();
            StopMatchMaker();
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

        private void StopQuickJoinMatchCoroutine()
        {
            StopCoroutine(quickStartMatchCoroutine);
            quickStartMatchCoroutine = null;
        }

        private IEnumerator Coroutine_QuickJoinMatch()
        {
            StartMatchMaker();

            for (int i = 0; i < 5; i++)
            {
                yield return matchMaker.ListMatches(0, 50, "", true, 0, 0, OnMatchList);

                foreach (var match in matches)
                {
                    if (match.currentSize == 1)
                    {
                        yield return matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }

            quickStartMatchCoroutine = null;
            CreateMatch(TTTProfile.Local.Name + "'s Match");
        }

        #region Network Overrides

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchJoined(success, extendedInfo, matchInfo);

            if (success && IsQuickStarting)
            {
                StopQuickJoinMatchCoroutine();
            }
        }

        #endregion
    }
}
