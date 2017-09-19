using System;
using Codari.TTT.Network;

namespace Codari.TTT.UI
{
    internal sealed class LeaveMatchUIButtonAction : UIButtonAction
    {
        protected override void Action() => TTTNetworkManager.Instance.LeaveMatch();
    }
}
