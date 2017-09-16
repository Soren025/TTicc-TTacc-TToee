using UnityEngine;
using UnityEngine.Networking;

using Sirenix.OdinInspector;
using Zenject;

using Codari.TTT.Network;

namespace Codari.TTT.DI
{
    internal sealed class TTTInstaller : MonoInstaller
    {
        [SerializeField, BoxGroup("Prefabs")]
        private TTTApplication tttApplicationPrefab;
        [SerializeField, BoxGroup("Prefabs")]
        private TTTNetworkManager tttNetworkManagerPrefab;
        [SerializeField, BoxGroup("Prefabs")]
        private TTTPlayer tttPlayerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<TTTApplication>()
                .FromComponentInNewPrefab(tttApplicationPrefab)
                .WithGameObjectName("TTT Application")
                .AsSingle()
                .NonLazy();

            // Network Stuff
            {
                Container.Bind<TTTNetworkManager>()
                    .FromComponentInNewPrefab(tttNetworkManagerPrefab)
                    .WithGameObjectName("TTT Network Manager")
                    .AsSingle()
                    .NonLazy();

                Container.BindFactory<TTTPlayer, TTTPlayer.Factory>()
                    .FromComponentInNewPrefab(tttPlayerPrefab)
                    .UnderTransformGroup("Players")
                    .WhenInjectedInto<TTTNetworkManager>();

                Container.BindInstance(tttPlayerPrefab.GetComponent<NetworkIdentity>().assetId)
                    .WithId(TTTInjectId.PlayerPrefab)
                    .WhenInjectedInto<TTTNetworkManager>();
            }
        }
    }
}
