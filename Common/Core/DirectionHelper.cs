using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DirectionHelper
    {
        /// <summary>Parses the string into a Direction. This is useful when reading from input.</summary>
        public static Direction Parse(string directionString)
        {
            switch (directionString.ToUpper())
            {
                case "UP":
                    return Direction.Up;
                case "DOWN":
                    return Direction.Down;
                case "LEFT":
                    return Direction.Left;
                case "RIGHT":
                    return Direction.Right;
                case "TOP":
                    return Direction.Top;
                case "BOTTOM":
                    return Direction.Bottom;
                default:
                    throw new InvalidOperationException("Could not parse the string: " + directionString);
            }
        }
    }
}
