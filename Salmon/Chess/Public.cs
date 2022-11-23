namespace Salmon.Chess
{
    public static class Board
    {
        public const int MINIMUM = 0, MAXIMUM = 8;
        public static bool IsInside(Point point)
            => point.X >= Board.MINIMUM && point.Y >= Board.MINIMUM
            && point.X < Board.MAXIMUM && point.Y < Board.MAXIMUM;
    }

    public enum Type { Pawn, Rook, Knight, Bishop, King, Queen }
    public enum Team { First, Last }
}
