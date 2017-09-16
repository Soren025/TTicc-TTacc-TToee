namespace Codari.TTT.UI
{
    internal sealed class QuitApplicationUIButtonAction : UIButtonAction
    {
        protected override void Action() => TTTApplication.Quit();
    }
}
