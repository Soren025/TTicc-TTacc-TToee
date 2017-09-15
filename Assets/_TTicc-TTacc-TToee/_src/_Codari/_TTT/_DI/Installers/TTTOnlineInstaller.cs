using UnityEngine;
using UnityEngine.Networking;

using Sirenix.OdinInspector;
using Zenject;

using Codari.TTT.Network;

namespace Codari.TTT.DI
{
    internal sealed class TTTOnlineInstaller : MonoInstaller
    {
        [SerializeField]
        [BoxGroup("Instance Binding")]
        private TTTMatch tttMatch;

        [SerializeField]
        [BoxGroup("Instance Binding")]
        private TTTBoard tttBoard;

        public override void InstallBindings()
        {
            Container.BindInstance(tttMatch).AsSingle();
            Container.BindInstance(tttBoard).AsSingle();
        }
    }
}
