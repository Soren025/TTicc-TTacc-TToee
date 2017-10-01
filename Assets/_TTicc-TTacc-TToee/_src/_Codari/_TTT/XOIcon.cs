using System;

namespace Codari.TTT
{
    public enum XOIcon
    {
        None = 0,
        X = 1,
        O = 2,
    }

    public static class XOIconExtensions
    {
        public static bool IsSelected(this XOIcon icon) => icon != XOIcon.None;

        public static bool IsX(this XOIcon icon) => icon == XOIcon.X;

        public static bool IsO(this XOIcon icon) => icon == XOIcon.O;

        public static string ToText(this XOIcon icon, bool emptyStringIfNone = false)
        {
            switch (icon)
            {
            case XOIcon.X: return "X";
            case XOIcon.O: return "O";
            default: return emptyStringIfNone ? "" : "?";
            }
        }

        public static XOIcon Opposite(this XOIcon icon)
        {
            switch (icon)
            {
            case XOIcon.X: return XOIcon.O;
            case XOIcon.O: return XOIcon.X;
            default: return XOIcon.None;
            }
        }
    }
}
