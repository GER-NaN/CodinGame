using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
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
                case Direction.Top:
                    return "TOP";
                case Direction.Bottom:
                    return "BOTTOM";
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
                case Direction.Top:
                    return Direction.Bottom;
                case Direction.Bottom:
                    return Direction.Top;
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }

        /// <summary>
        /// When oriented in a top down view, returns the direction to the left relative to the current direction.
        /// </summary>
        /// <param name="current">The current direction to orient ourselves</param>
        /// <returns>The direction to the left relative to the current direction</returns>
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


        /// <summary>
        /// When oriented in a top down view, returns the direction to the right relative to the current direction.
        /// </summary>
        /// <param name="current">The current direction to orient ourselves</param>
        /// <returns>The direction to the right relative to the current direction</returns>
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
