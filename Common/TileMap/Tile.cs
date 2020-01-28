using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TileMap
{
    public class Tile<T>
    {
        public Point Point;
        public T Item;

        public Tile(Point p)
        {
            Point = p;
        } 

        public Tile(Point p, T item)
        {

            Item = item;
        }

        public Tile(Tile<T> tile)
        {
            Point = new Point(tile.Point.X, tile.Point.Y);
            Item = tile.Item;
        }

        public override string ToString()
        {
            return $"({Point.X},{Point.Y}) > {Item}";
        }
    }
}
