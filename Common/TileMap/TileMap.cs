using Common.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common.TileMap
{
    public class TileMap<T>
    {
        private Random random = new Random(Guid.NewGuid().GetHashCode());

        public List<Tile<T>> Tiles;
        public readonly int Width;
        public readonly int Height;

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;

            Tiles = new List<Tile<T>>(Width * Height);
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    Tiles.Add(new Tile<T>(new Point(j, i)));
                }
            }
        }

        public TileMap(T[,] map)
        {
            Width = map.GetLength(1);
            Height = map.GetLength(0);

            Tiles = new List<Tile<T>>(Width * Height);
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    //A lil' confusing here.... Points are in formatted in X,Y style. Array access is Y,X style, ie. Row,Column. Thats why they are flipped when reading the item from the Array
                    Tiles.Add(new Tile<T>(new Point(j, i),map[i,j]));
                }
            }
        }

        /// <summary>Finds all tiles with the item in it.</summary>
        /// <param name="item">The item to look for in the Tiles</param>
        /// <returns>All tiles that contain the item specified</returns>
        public List<Tile<T>> FindAll(T item)
        {
            return Tiles.Where(t => t.Item.Equals(item)).ToList();
        }

        /// <summary>Finds a tile with the item in it. If multiple Tiles contain the requested item the Tile returned is arbitrary and is not guaranteed to be the same every call.</summary>
        /// <param name="item">The item to look for in the Tiles</param>
        public Tile<T> Find(T item)
        {
            return Tiles.FirstOrDefault(t => t.Item.Equals(item));
        }

        //Untested - Finds a tile based on a function of the item it contains.
        public Tile<T> Find(Func<Tile<T>,bool> predicate)
        {
            return Tiles.FirstOrDefault(predicate);
        }


        /// <summary>Gets the tile at the specified x y coordinates</summary>
        public Tile<T> TileAt(int x, int y)
        {
            return Tiles.FirstOrDefault(t => t.Point.X == x && t.Point.Y == y);
        }

        /// <summary>Gets the tile at the Point including an optional offset in y and x directions</summary>
        /// <param name="p">The point where the tile is located</param>
        public Tile<T> TileAt(Point p)
        {
            return TileAt(p.X, p.Y);
        }

        /// <summary>Gets the tile at the Point including an optional offset in y and x directions</summary>
        /// <param name="p">The point where the tile is located</param>
        /// <param name="xOffset">Any offset in the x direction, bounds are not checked and will return null Tile if outside the map</param>
        /// <param name="yOffset">Any offset in the y direction, bounds are not checked and will return null Tile if outside the map</param>
        public Tile<T> TileAt(Point p, int xOffset = 0, int yOffset = 0)
        {
            return TileAt(p.X + xOffset, p.Y + yOffset);
        }

        /// <summary> Gets the tile at the Point offset by 1 in the Direction specified</summary>
        /// <param name="p">The Point to start from</param>
        /// <param name="direction">The Direction to step in by 1 to get the final Point</param>
        /// <returns>The Tile at Point + Direction by 1</returns>
        public Tile<T> TileAt(Point p, Direction direction)
        {
            var xOffset = 0;
            var yOffset = 0;

            switch (direction)
            {
                case Direction.Up:
                case Direction.North:
                case Direction.Top:
                    yOffset = -1;
                    break;
                case Direction.Down:
                case Direction.Bottom:
                case Direction.South:
                    yOffset = 1;
                    break;
                case Direction.Left:
                case Direction.West:
                    xOffset = -1;
                    break;
                case Direction.Right:
                case Direction.East:
                    xOffset = 1;
                    break;
            }

            return TileAt(p, xOffset, yOffset);

        }

        public T ItemAt(int x, int y)
        {
            Tile<T> tile = TileAt(x, y);
            T val = tile == null ? default(T) : tile.Item;

            return val;
        }

        public T ItemAt(Point p, int xOffset = 0, int yOffset = 0)
        {
            return ItemAt(p.X + xOffset, p.Y + yOffset);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Tile<T> t = TileAt(j, i);
                    sb.Append(t.Item);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public Tile<T> GetRandomTile()
        {
            var x = random.Next() % Width;
            var y = random.Next() % Height;

            return this.TileAt(x, y);
        }
    }
}
