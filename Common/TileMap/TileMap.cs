using Common.Core;
using Common.StandardTypeExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common.TileMap
{
    public class TileMap<T>
    {
        private Random random = new Random(Guid.NewGuid().GetHashCode());
        private readonly Tile<T>[,] LookupArray;

        public List<Tile<T>> Tiles;
        public readonly int Width;
        public readonly int Height;

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;

            Tiles = new List<Tile<T>>(Width * Height);
            LookupArray = new Tile<T>[Height, Width];

            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var t = new Tile<T>(new Point(j, i));
                    Tiles.Add(t);
                    LookupArray[i,j] = t;
                }
            }
        }

        public TileMap(T[,] map)
        {
            Width = map.GetLength(1);
            Height = map.GetLength(0);

            Tiles = new List<Tile<T>>(Width * Height);
            LookupArray = new Tile<T>[Height, Width];

            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var t = new Tile<T>(new Point(j, i), map[i, j]);
                    Tiles.Add(t);
                    LookupArray[i, j] = t;
                }
            }
        }

        /// <summary>Gets neighboring cells based on distance from the position</summary>
        /// <param name="position">The position of the cell we want neighbors for</param>
        /// <param name="level">How many levels should we extend out from the </param>
        /// <returns></returns>
        public List<Tile<T>> GetNeighbors(Point position, int level)
        {
            List<Tile<T>> tiles = new List<Tile<T>>();
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (tile.Position.ManhattenDistanceTo(position) <= level && tile.Position != position)
                    {
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
            //return this.Tiles.Where(t => t.Position.ManhattenDistanceTo(position) <= level && t.Position != position).ToList();
        }

        /// <summary>Gets neighboring cells based on distance from the position and the predicate condition</summary>
        /// <param name="position">The position of the cell we want neighbors for</param>
        /// <param name="level">How many levels should we extend out from the </param>
        /// <param name="predicate">The predicate condition to also match neighbors on</param>
        /// <returns></returns>
        public List<Tile<T>> GetNeighbors(Point position, int level, Func<Tile<T>, bool> predicate)
        {
            List<Tile<T>> tiles = new List<Tile<T>>();
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (tile.Position.ManhattenDistanceTo(position) <= level && tile.Position != position && predicate.Invoke(tile) )
                    {
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
            //return this.Tiles.Where(predicate).Where(t => t.Position.ManhattenDistanceTo(position) <= level && t.Position != position).ToList();
        }

        /// <summary>Finds all tiles with the item in it.</summary>
        /// <param name="item">The item to look for in the Tiles</param>
        /// <returns>All tiles that contain the item specified</returns>
        public List<Tile<T>> FindAll(T item)
        {
            List<Tile<T>> tiles = new List<Tile<T>>();
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (tile.Item.Equals(item))
                    {
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
            //return Tiles.Where(t => t.Item.Equals(item)).ToList();
        }

        /// <summary>Finds a tile with the item in it. If multiple Tiles contain the requested item the Tile returned is arbitrary and is not guaranteed to be the same every call.</summary>
        /// <param name="item">The item to look for in the Tiles</param>
        public Tile<T> Find(T item)
        {
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (tile.Item.Equals(item))
                    {
                        return (tile);
                    }
                }
            }
            return null;
            //return Tiles.FirstOrDefault(t => t.Item.Equals(item));
        }

        /// <summary> Finds the first tile that matches the predicate condition</summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Tile<T> Find(Func<Tile<T>,bool> predicate)
        {
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (predicate.Invoke(tile))
                    {
                        return tile;
                    }
                }
            }
            return null;
        }

        /// <summary> Finds all tiles that matche the predicate condition</summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Tile<T>> FindAll(Func<Tile<T>, bool> predicate)
        {
            var tiles = new List<Tile<T>>();
            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (predicate.Invoke(tile))
                    {
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
            //return Tiles.Where(predicate).ToList();
        }

        /// <summary> Finds the first tile that matches the predicate condition</summary>
        /// <param name="predicate">The predicate condition for matching tiles</param>
        /// <param name="p">The point to start looking from</param>
        /// <returns></returns>
        public Tile<T> FindNearest(Func<Tile<T>, bool> predicate, Point p)
        {
            Tile<T> nearest = null;
            double nearestDistance = double.MaxValue;

            for (int i = 0; i < Height; i++)//Height (Y)
            {
                for (int j = 0; j < Width; j++)//Width (X)
                {
                    var tile = LookupArray[i, j];
                    if (predicate.Invoke(tile))
                    {
                        if(p.ManhattenDistanceTo(tile.Position) < nearestDistance)
                        {
                            nearestDistance = p.ManhattenDistanceTo(tile.Position);
                            nearest = tile;
                        }
                    }
                }
            }
            return nearest;
            //var tiles = Tiles.Where(predicate);
            //return tiles.OrderBy(tile => p.ManhattenDistanceTo(tile.Position)).FirstOrDefault();
        }


        /// <summary>Gets the tile at the specified x y coordinates</summary>
        public Tile<T> TileAt(int x, int y)
        {
            return LookupArray[y, x];
            //return Tiles.FirstOrDefault(t => t.Position.X == x && t.Position.Y == y);
        }

        /// <summary>Gets the tile at the Point</summary>
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
