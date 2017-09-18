using Zenject;

using Codari.TTT.Network;

namespace Codari.TTT.UI
{
    internal sealed class CancelQuickStartUIButtonAction : UIButtonAction
    {
        [Inject]
        private TTTNetworkManager tttNetworkManager;

        protected override void Action() => tttNetworkManager.CancelQuickStartMatch();
    }
}
