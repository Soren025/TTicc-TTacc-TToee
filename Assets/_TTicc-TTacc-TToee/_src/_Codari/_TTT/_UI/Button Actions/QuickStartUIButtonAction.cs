using Codari.TTT.Network;

namespace Codari.TTT.UI
{
    internal sealed class QuickStartUIButtonAction : UIButtonAction
    {
        protected override void Action() => TTTNetworkManager.Instance.QuickStartMatch();
    }
}
