using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardTypeExtensions
{
    /// <summary>
    /// Extensions for the System.Drawing.Point class
    /// </summary>
    public static class PointExtensions
    {
        //Untested
        /// <summary>Determines if this point is within the bounding box described by topLeft and bottomRight</summary>
        /// <param name="point">The Point to check</param>
        /// <param name="topLeft">The top left corner of the bounding box</param>
        /// <param name="bottomRight">The bottom right corner of the bounding box</param>
        /// <returns></returns>
        public static bool IsWithin(this Point point, Point topLeft, Point bottomRight)
        {
            return (point.Y >= topLeft.Y && point.Y <= bottomRight.Y && point.X >= topLeft.X && point.X <= bottomRight.X);
        }

        /// <summary>Calcualtes the Distance from p1 to p2</summary>
        /// <param name="p1">The starting point</param>
        /// <param name="p2">The ending point</param>
        /// <returns></returns>
        public static double DistanceTo(this Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        /// <summary>Checks if the Point is adjacent to another point in one of the standard directinos (Up, Down, Left, Right or North, South, East, West)</summary>
        /// <param name="p1">The starting point</param>
        /// <param name="p2">The Point to check</param>
        /// <param name="equalIsAdjacent">Whether or not equal points should be considered adjacent, the default is false</param>
        /// <returns></returns>
        public static bool AdjacentTo(this Point p1, Point p2, bool equalIsAdjacent = false)
        {
            return
                (equalIsAdjacent && p1.Equals(p2)) ||   //equal
                (p1.X == p2.X && p1.Y == p2.Y - 1) ||   //up
                (p1.X == p2.X && p1.Y == p2.Y + 1) ||   //down
                (p1.Y == p2.Y && p1.X == p2.X - 1) ||   //left
                (p1.Y == p2.Y && p1.X == p2.X + 1);     //right
        }
    }
}
