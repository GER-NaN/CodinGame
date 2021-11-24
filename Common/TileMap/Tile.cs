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

        public override bool Equals(object obj)
        {
            var t1 = obj as Tile<T>;
            return (t1.Position.X == this.Position.X && t1.Position.Y == this.Position.Y);
        }

        //https://stackoverflow.com/a/4630550
        public override int GetHashCode()
        {
            return new { this.Position.X, this.Position.Y }.GetHashCode();
        }
    }
}
