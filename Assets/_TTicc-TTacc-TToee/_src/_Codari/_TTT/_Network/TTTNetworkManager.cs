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

        private void StopQuickJoinMatchCoroutine()
        {
            StopCoroutine(quickStartMatchCoroutine);
            quickStartMatchCoroutine = null;
        }

        private IEnumerator Coroutine_QuickJoinMatch()
        {
            const int PageSize = 50; // Unity Plus subscribtion only comes with 50 CCU free, so at most only 50 matches can exist.
            const int Attempts = 5; // Lets only make 5 attempts

            StartMatchMaker();

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

                yield return new WaitForSeconds(0.75f + Random.Range(0f, 0.5f));
            }

            quickStartMatchCoroutine = null;
            CreateMatch(TTTProfile.Local.Name + "'s Match");
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

            if (success && IsQuickStarting)
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
