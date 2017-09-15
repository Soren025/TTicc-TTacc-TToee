namespace Codari.TTT.Network
{
    [System.Serializable]
    public struct NetworkCoordinate
    {
        public byte x, y;

        public static implicit operator Coordinate(NetworkCoordinate c) => new Coordinate(c.x, c.y);

        public static implicit operator NetworkCoordinate(Coordinate c) => new NetworkCoordinate { x = c.X, y = c.Y };
    }
}
