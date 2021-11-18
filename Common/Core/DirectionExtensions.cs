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
                case Direction.North:
                    return "NORTH";
                case Direction.South:
                    return "SOUTH";
                case Direction.East:
                    return "EAST";
                case Direction.West:
                    return "WEST";
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
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.East:
                    return Direction.West;
                case Direction.West:
                    return Direction.East;
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
                case Direction.North:
                    return Direction.West;
                case Direction.South:
                    return Direction.East;
                case Direction.East:
                    return Direction.North;
                case Direction.West:
                    return Direction.South;
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
                case Direction.North:
                    return Direction.East;
                case Direction.South:
                    return Direction.West;
                case Direction.East:
                    return Direction.South;
                case Direction.West:
                    return Direction.North;
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }


        /// <summary>Converts a string to the Direction Type</summary>
        /// <param name="str">THe string that represents the direction</param>
        public static Direction ToDirection(this string str)
        {
            //Do a little formatting first by capitalizing the first char and lowercasing the rest
            str = str[0].ToString().ToUpper() + str.Substring(1, str.Length - 1).ToLower();

            return (Direction)Enum.Parse(typeof(Direction), str);
        }
        
    }
}
