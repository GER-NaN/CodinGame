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
        public Point Position;
        public T Item;

        public Tile(Point p)
        {
            Position = p;
        } 

        public Tile(Point p, T item): this(p)
        {
            Item = item;
        }

        public Tile(Tile<T> tile)
        {
            Position = new Point(tile.Position.X, tile.Position.Y);
            Item = tile.Item;
        }

        public override string ToString()
        {
            return $"({Position.X},{Position.Y}) > {Item}";
        }
    }
}
