using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

using Zenject;

using Codari.TTT.DI;
using UnityEngine.Networking.Match;

namespace Codari.TTT.Network
{
    public sealed class TTTNetworkManager : NetworkManager
    {
        [Inject]
        private TTTPlayer.Factory tttPlayerFactory;
        [Inject(Id = TTTInjectId.PlayerPrefab)]
        private NetworkHash128 playerAssetId;

        private Coroutine quickJoinMatchCoroutine;

        public void QuickJoinMatch()
        {
            if (quickJoinMatchCoroutine != null || IsClientConnected() || NetworkServer.active) return;
            quickJoinMatchCoroutine = StartCoroutine(Coroutine_QuickJoinMatch());
        }

        public void CancelQuickJoinMatch()
        {
            if (quickJoinMatchCoroutine == null) return;
            StopQuickJoinMatchCoroutine();
            StopMatchMaker();
        }

        private IEnumerator Coroutine_QuickJoinMatch()
        {
            const int PageSize = 50; // Unity Plus subscribtion only comes with 50 CCU free, so at most only 50 matches can exist.
            const int Attempts = 5;

            for (int i = 0; i < Attempts; i++)
            {
                yield return matchMaker.ListMatches(0, PageSize, "", true, 0, 0, OnMatchList);

                foreach (var match in matches)
                {
                    if (match.currentSize == 1)
                    {
                        yield return matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
                    }
                }
            }

            quickJoinMatchCoroutine = null;
        }

        private void StopQuickJoinMatchCoroutine()
        {
            StopCoroutine(quickJoinMatchCoroutine);
            quickJoinMatchCoroutine = null;
        }

        #region Network Overrides

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            NetworkServer.AddPlayerForConnection(conn, tttPlayerFactory.Create().gameObject, playerControllerId);
        }

        public override void OnStartClient(NetworkClient client)
        {
            base.OnStartClient(client);

            // Registration of spawn handlers
            ClientScene.RegisterSpawnHandler(playerAssetId, SpawnPlayer, UnSpawnPlayer);
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchJoined(success, extendedInfo, matchInfo);

            if (success && quickJoinMatchCoroutine != null)
            {
                StopQuickJoinMatchCoroutine();
            }
        }

        #endregion

        #region Spawn Handlers

        // ----- Player ----- //
        private GameObject SpawnPlayer(Vector3 position, NetworkHash128 assetId) => tttPlayerFactory.Create().gameObject;

        private void UnSpawnPlayer(GameObject gameObject) => Destroy(gameObject);
        
        #endregion
    }
}
