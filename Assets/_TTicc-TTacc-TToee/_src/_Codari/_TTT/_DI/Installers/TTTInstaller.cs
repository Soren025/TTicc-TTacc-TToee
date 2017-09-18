using UnityEngine;
using UnityEngine.Networking;

using DoozyUI;
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
        [SerializeField, BoxGroup("Prefabs")]
        private UIManager uiManagerPrefab;

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

                // UI Manager
                {
                    // Done like this because it sets it self to `DontDestroyOnLoad` causing a warning
                    // do to Zenject childing it under the `ProjectContext`.
                    UIManager uiManager = Instantiate(uiManagerPrefab);
                    uiManager.name = "UI Manager";

                    Container.BindInstance(uiManager).AsSingle();
                }
            }

            //Container.Bind<TTTProfileManager>()
            //    .FromNewComponentOnNewGameObject()
            //    .WithGameObjectName("TTT Profile Manager")
            //    .AsSingle()
            //    .NonLazy();
        }
    }
}
