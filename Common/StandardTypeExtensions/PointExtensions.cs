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
        public static bool IsWithin(this Point point,Point topLeft, Point bottomRight)
        {
            return (point.Y >= topLeft.Y && point.Y <= bottomRight.Y && point.X >= topLeft.X && point.X <= bottomRight.X);
        }

        public static double DistanceTo(this Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X,2) + Math.Pow(p2.Y - p1.Y,2));
        }
    }
}
