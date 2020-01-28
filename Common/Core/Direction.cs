using System;

namespace Common.Core
{
    public enum Direction
    {
         Up,
         Down,
         Left,
         Right
    }

    public static class DirectionExtensions
    {
        public static string Print(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return "UP";
                case Direction.Down:
                    return "DOWN";
                case Direction.Left:
                    return "LEFT";
                case Direction.Right:
                    return "RIGHT";
                default://Including this default so the function is happy with all return statements, but it should never execute... right?
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }

        public static Direction Opposite(this Direction current)
        {
            switch (current)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }

        public static Direction RelativeLeft(this Direction current)
        {
            switch (current)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Up;
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }

        public static Direction RelativeRight(this Direction current)
        {
            switch (current)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Down;
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }
    }
}
