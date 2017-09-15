using UnityEngine;
using UnityEngine.Networking;

using Zenject;

using Codari.TTT.Network;

namespace Codari.TTT.DI
{
    internal sealed class TTTInstaller : MonoInstaller
    {
        [SerializeField]
        private TTTApplication tttApplicationPrefab;
        [SerializeField]
        private TTTNetworkManager tttNetworkManagerPrefab;
        [SerializeField]
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
