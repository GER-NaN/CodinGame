using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CodingGame.TileMap
{
    public class TileMap<T>
    {
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

        /// <summary>Gets the tile at the specified x y coordinates</summary>
        public Tile<T> TileAt(int x, int y)
        {
            return Tiles.FirstOrDefault(t => t.Point.X == x && t.Point.Y == y);
        }

        /// <summary>Gets the tile at the Point including an optional offset in y and x directions</summary>
        /// <param name="p">The point where the tile is located</param>
        /// <param name="xOffset">Any offset in the x direction, bounds are not checked and will return null Tile if outside the map</param>
        /// <param name="yOffset">Any offset in the y direction, bounds are not checked and will return null Tile if outside the map</param>
        public Tile<T> TileAt(Point p, int xOffset = 0, int yOffset = 0)
        {
            return TileAt(p.X + xOffset, p.Y + yOffset);
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
    }
}
