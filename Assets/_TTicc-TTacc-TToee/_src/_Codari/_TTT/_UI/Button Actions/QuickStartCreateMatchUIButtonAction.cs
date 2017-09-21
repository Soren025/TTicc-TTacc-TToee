using UnityEngine;

using Codari.TTT.Network;

namespace Codari.TTT.UI
{
    internal sealed class QuickStartCreateMatchUIButtonAction : UIButtonAction
    {
        protected override void Action() => TTTNetworkManager.Instance.CreateMatch($"{TTTProfile.Local.Name}'s Match [{Random.Range(0, short.MaxValue + 1)}]");
    }
}
