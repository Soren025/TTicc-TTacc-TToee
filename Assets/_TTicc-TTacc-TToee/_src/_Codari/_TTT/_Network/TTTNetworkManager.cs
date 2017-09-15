using UnityEngine;
using UnityEngine.Networking;

using Zenject;

using Codari.TTT.DI;

namespace Codari.TTT.Network
{
    public sealed class TTTNetworkManager : NetworkManager
    {
        [Inject]
        private TTTPlayer.Factory tttPlayerFactory;
        [Inject(Id = TTTInjectId.PlayerPrefab)]
        private NetworkHash128 playerAssetId;

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

        #region Spawn Handlers

        // ----- Player ----- //
        private GameObject SpawnPlayer(Vector3 position, NetworkHash128 assetId) => tttPlayerFactory.Create().gameObject;

        private void UnSpawnPlayer(GameObject gameObject) => Destroy(gameObject);
        
        #endregion
    }
}
