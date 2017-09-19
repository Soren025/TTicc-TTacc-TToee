using Codari.TTT.Network;

namespace Codari.TTT.UI
{
    internal sealed class CancelQuickStartUIButtonAction : UIButtonAction
    {
        protected override void Action() => TTTNetworkManager.Instance.CancelQuickStartMatch();
    }
}
