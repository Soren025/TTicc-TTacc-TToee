using System;

namespace Codari.TTT
{
    public enum Selection
    {
        None,
        X,
        O,
    }

    public static class SelectionExtensions
    {
        public static bool IsSelected(this Selection selection) => selection != Selection.None;

        public static bool IsX(this Selection selection) => selection == Selection.X;

        public static bool IsO(this Selection selection) => selection == Selection.O;
    }
}
