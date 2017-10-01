using UnityEngine;

using System;
using System.Collections.Generic;

namespace Codari.TTT
{
    [Serializable]
    public struct Coordinate : IEquatable<Coordinate>
    {
        public const byte MaxValue = 2;

        public static Coordinate Invalid => new Coordinate(byte.MaxValue, byte.MaxValue);

        public static Coordinate TopLeft => new Coordinate(0, 0);

        public static Coordinate MiddleLeft => new Coordinate(0, 1);

        public static Coordinate BottomLeft => new Coordinate(0, 2);

        public static Coordinate TopCenter => new Coordinate(1, 0);

        public static Coordinate MiddleCenter => new Coordinate(1, 1);

        public static Coordinate BottomCenter => new Coordinate(1, 2);

        public static Coordinate TopRight => new Coordinate(2, 0);

        public static Coordinate MiddleRight => new Coordinate(2, 1);

        public static Coordinate BottomRight => new Coordinate(2, 2);

        public static IEnumerable<Coordinate> All
        {
            get
            {
                yield return TopLeft;
                yield return MiddleLeft;
                yield return BottomLeft;
                yield return TopCenter;
                yield return MiddleCenter;
                yield return BottomCenter;
                yield return TopRight;
                yield return MiddleRight;
                yield return BottomRight;
            }
        }

        public static IEnumerable<Coordinate> TopHorizontal
        {
            get
            {
                yield return TopLeft;
                yield return TopCenter;
                yield return TopRight;
            }
        }

        public static IEnumerable<Coordinate> MiddleHorizontal
        {
            get
            {
                yield return MiddleLeft;
                yield return MiddleCenter;
                yield return MiddleRight;
            }
        }

        public static IEnumerable<Coordinate> BottomHorizontal
        {
            get
            {
                yield return BottomLeft;
                yield return BottomCenter;
                yield return BottomRight;
            }
        }

        public static IEnumerable<Coordinate> LeftVertical
        {
            get
            {
                yield return BottomLeft;
                yield return MiddleLeft;
                yield return TopLeft;
            }
        }

        public static IEnumerable<Coordinate> CenterVertical
        {
            get
            {
                yield return BottomCenter;
                yield return MiddleCenter;
                yield return TopCenter;
            }
        }

        public static IEnumerable<Coordinate> RightVertical
        {
            get
            {
                yield return BottomRight;
                yield return MiddleRight;
                yield return TopRight;
            }
        }

        public static IEnumerable<Coordinate> DiagonalAssending
        {
            get
            {
                yield return BottomLeft;
                yield return MiddleCenter;
                yield return TopRight;
            }
        }

        public static IEnumerable<Coordinate> DiagonalDecending
        {
            get
            {
                yield return TopLeft;
                yield return MiddleCenter;
                yield return BottomRight;
            }
        }

        public static IEnumerable<IEnumerable<Coordinate>> Rows
        {
            get
            {
                yield return TopHorizontal;
                yield return MiddleHorizontal;
                yield return BottomHorizontal;
                yield return LeftVertical;
                yield return CenterVertical;
                yield return RightVertical;
                yield return DiagonalAssending;
                yield return DiagonalDecending;
            }
        }

        [SerializeField, Range(0, MaxValue)]
        private byte x, y;

        public Coordinate(byte x, byte y)
        {
            this.x = x;
            this.y = y;
        }

        public byte X => x;

        public byte Y => y;

        public bool IsValid => x <= MaxValue && y <= MaxValue;

        #region Utility

        public override string ToString() => $"[{X}, {Y}]";

        public override int GetHashCode() => X + Y << 8;

        public override bool Equals(object obj) => obj is Coordinate && Equals((Coordinate) obj);

        public bool Equals(Coordinate other) => X == other.X && Y == other.Y;

        #endregion

        #region Operators

        public static bool operator ==(Coordinate l, Coordinate r) => l.Equals(r);

        public static bool operator !=(Coordinate l, Coordinate r) => !l.Equals(r);

        #endregion
    }
}