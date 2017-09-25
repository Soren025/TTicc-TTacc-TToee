using System;

namespace Codari.TTT
{
    public enum XOIcon
    {
        None,
        X,
        O,
    }

    public static class SelectionExtensions
    {
        public static bool IsSelected(this XOIcon icon) => icon != XOIcon.None;

        public static bool IsX(this XOIcon icon) => icon == XOIcon.X;

        public static bool IsO(this XOIcon icon) => icon == XOIcon.O;

        public static string ToText(this XOIcon icon)
        {
            switch (icon)
            {
            case XOIcon.X: return "X";
            case XOIcon.O: return "O";
            default: return "?";
            }
        }
    }
}
