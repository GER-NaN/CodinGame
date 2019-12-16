﻿/*
 * File generated by SourceCombiner.exe using 10 source files.
 * Created On: 6/6/2017 2:03:52 PM
*/

using CodingGame.Core;
using CodingGame.TileMap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//*** SourceCombiner -> original file TronCg.cs ***
/*
In this game your are a program driving the legendary tron light cycle and fighting against other programs on the game grid.
The light cycle moves in straight lines and only turn in 90° angles while leaving a solid light ribbon in its wake. Each cycle and associated ribbon features a different color.
Should a light cycle stop, hit a light ribbon or goes off the game grid it will be instantly deactivated.
The last cycle in play wins the game. Your goal is to be the best program: once sent to the arena, programs will compete against each-others in battles gathering 2 to 4 cycles. The more battles you win, the better your rank will be.
Rules
Each battle is fought with 2 or 3 players. Each player plays in turn during a battle. When your turn comes, the following happens:
Information about the location of players on the grid is sent on the standard input of your program. So your AI must read information on the standard input at the beginning of a turn.
Once the inputs have been read for the current game turn, your AI must provide its next move information on the standard ouput. The output for a game turn must be a single line stating the next direction of the light cycle: either UP, DOWN, LEFT or RIGHT.
Your light cycle will move in the direction your AI provided.
At this point your AI should wait for your next game turn information and so on and so forth. In the mean time, the AI of the other players will receive information the same way you did.
If your AI does not provide output fast enough when your turn comes, or if you provide an invalid output or if your output would make the light cycle move into an obstacle, then your program loses.
If another AI loses before yours, its light ribbon disappears and the game continues until there is only one player left.
The game grid has a 30 by 20 cells width and height. Each player starts at a random location on the grid.
Victory Conditions
Be the last remaining player
Game Input
Input for one game turn
Line 1: Two integers N and P. Where N is the total number of players and P is your player number for this game.
The N following lines: One line per player. First line is for player 0, next line for player 1, etc. Each line contains four values X0, Y0, X1 and Y1. (X0, Y0) are the coordinates of the initial position of the light ribbon (tail) and (X1, Y1) are the coordinates of the current position of the light ribbon (head) of the player. Once a player loses, his/her X0 Y0 X1 Y1 coordinates are all equal to -1 (no more light ribbon on the grid for this player).
Output for one game turn
A single line with UP, DOWN, LEFT or RIGHT
Constraints
2 ≤ N ≤ 3
0 ≤ P < N
0 ≤ X0, X1 < 30
0 ≤ Y0, Y1 < 20
Your AI must answer in less than 100ms for each game turn.
 * 
 */

namespace TronCg
{

    class TronCg
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;
        private static TronMap map = new TronMap(MapWidth, MapHeight);
        private static Point me = new Point(-1, -1);
        private static Direction travelingDir = Direction.Right;       //Current travel direction (could be any)
        private static Direction attemptToTurnDir = Direction.Right;  //The direction we want to check for open space (this will be relative to travel dir)
        private static Direction goalTurnDir = Direction.Right;        //Always left or right (circle right or circle left)
        private static int myPlayerId;
        static void Main(string[] args)
        {
            while (true)
            {
                var inputs = Console.ReadLine().Split(' ');
                int playerCount = int.Parse(inputs[0]);
                myPlayerId = int.Parse(inputs[1]);
                for (int i = 0; i < playerCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int x0 = int.Parse(inputs[0]); // starting X coordinate of lightcycle (or -1)
                    int y0 = int.Parse(inputs[1]); // starting Y coordinate of lightcycle (or -1)
                    int x1 = int.Parse(inputs[2]); // starting X coordinate of lightcycle (can be the same as X0 if you play before this player)
                    int y1 = int.Parse(inputs[3]); // starting Y coordinate of lightcycle (can be the same as Y0 if you play before this player)
                    if (x0 == -1)
                    {
                        map.KillPlayer(i);
                    }
                    else
                    {
                        map.UpdatePlayerTrail(i, x0, y0);//Starting position / doesnt change
                        map.UpdatePlayerTrail(i, x1, y1);
                        if (i == myPlayerId)
                        {
                            me = new Point(x1, y1);
                        }
                    }
                }
                Tile<int> tileStraight = map.GetTileInDirection(me, travelingDir);
                if (tileStraight != null && map.IsTileEmpty(tileStraight.Point))
                {
                    //Update and output
                    map.UpdatePlayerTrail(myPlayerId, tileStraight.Point);
                    Console.WriteLine(travelingDir.Print());
                }
                else
                {
                    WallHuggerStrategy();
                }
            }
        }
        public static void WallHuggerStrategy()
        {
            //Update my directions
            attemptToTurnDir = (goalTurnDir == Direction.Right ? travelingDir.RelativeRight() : travelingDir.RelativeLeft());
            Tile<int> tileToCheck = map.GetTileInDirection(me, attemptToTurnDir);
            Tile<int> tileStraight = map.GetTileInDirection(me, travelingDir);
            //Check if we can turn in our desired direction
            if (tileToCheck != null && map.IsTileEmpty(tileToCheck.Point))
            {
                //Update and output
                travelingDir = attemptToTurnDir;
                map.UpdatePlayerTrail(myPlayerId, tileToCheck.Point);
                Console.WriteLine(travelingDir.Print());
            }
            else if (tileStraight != null && map.IsTileEmpty(tileStraight.Point))
            {
                map.UpdatePlayerTrail(myPlayerId, tileStraight.Point);
                Console.WriteLine(travelingDir.Print());
            }
            else
            {
                //Change goal direction, if any of these fail it means we've reached a dead end and failed.
                goalTurnDir = goalTurnDir.Opposite();
                attemptToTurnDir = attemptToTurnDir.Opposite();
                travelingDir = attemptToTurnDir;
                tileToCheck = map.GetTileInDirection(me, attemptToTurnDir);
                map.UpdatePlayerTrail(myPlayerId, tileToCheck.Point);
                Console.WriteLine(travelingDir.Print());
            }
        }
    }
}
//*** SourceCombiner -> original file TronMap.cs ***
namespace TronCg
{
    class TronMap : TileMap<int>
    {
        public static readonly int EmptyTile = -1;
        public TronMap(int width, int height) : base(width, height)
        {
            foreach (var tile in Tiles)
            {
                tile.Item = EmptyTile;
            }
        }
        public void KillPlayer(int playerId)
        {
            foreach (var tile in Tiles)
            {
                if (tile.Item == playerId)
                {
                    tile.Item = EmptyTile;
                }
            }
        }
        public void UpdatePlayerTrail(int playerId, int x, int y)
        {
            Tile<int> tile = this.TileAt(x, y);
            if (tile != null)
            {
                tile.Item = playerId;
            }
        }
        public void UpdatePlayerTrail(int playerId, Point p)
        {
            this.TileAt(p).Item = playerId;
        }
        public bool IsTileEmpty(int x, int y)
        {
            return TileAt(x, y).Item == EmptyTile;
        }
        public bool IsTileEmpty(Point p)
        {
            return TileAt(p).Item == EmptyTile;
        }
        public Tile<int> GetTileInDirection(Point p, Direction direction)
        {
            int dx = direction == Direction.Left ? -1 : direction == Direction.Right ? 1 : 0;
            int dy = direction == Direction.Up ? -1 : direction == Direction.Down ? 1 : 0;
            return TileAt(p, dx, dy);
        }
        public bool IsTileEmptyInDirection(Point p, Direction d)
        {
            Tile<int> tile = GetTileInDirection(p, d);
            return IsTileEmpty(tile.Point);
        }
    }
}
//*** SourceCombiner -> original file DebugTool.cs ***
namespace CodingGame.Core
{
    public static class DebugTool
    {
        public static void Print(object o)
        {
            Console.Error.WriteLine(o?.ToString());
        }
        public static void Print(string label, object o)
        {
            Print(label + ": " + o);
        }
    }
}
//*** SourceCombiner -> original file Direction.cs ***
namespace CodingGame.Core
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
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
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }
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
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }
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
                default:
                    throw new InvalidOperationException("Default case statement on enum Direction");
            }
        }
    }
}
//*** SourceCombiner -> original file Graph.cs ***
/*
https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005
Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/
namespace CodingGame.Graph
{
    public class Graph<T>
    {
        private readonly NodeList<T> _nodeSet;
        public Graph() : this(null) { }
        public Graph(NodeList<T> initialNodes)
        {
            if (initialNodes == null)
            {
                this._nodeSet = new NodeList<T>();
            }
            else
            {
                this._nodeSet = initialNodes;
            }
        }
        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            _nodeSet.Add(node);
        }
        public void AddNode(T value)
        {
            // adds a node to the graph
            _nodeSet.Add(new GraphNode<T>(value));
        }
        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }
        public bool Contains(T value)
        {
            return _nodeSet.FindByValue(value) != null;
        }
        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)_nodeSet.FindByValue(value);
            if (nodeToRemove == null)
            {
                return false;
            }
            _nodeSet.Remove(nodeToRemove);
            // removing edges to this node
            foreach (var node in _nodeSet)
            {
                var gnode = (GraphNode<T>)node;
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }
            return true;
        }
        public NodeList<T> Nodes
        {
            get
            {
                return _nodeSet;
            }
        }
        public int Count
        {
            get { return _nodeSet.Count; }
        }
    }
}
//*** SourceCombiner -> original file GraphNode.cs ***
/*
https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005
Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/
namespace CodingGame.Graph
{
    /*
     * Neighbors and Costs are used as parallel lists where the index of an item relates it to the counterpart.
     * Example: Neighor at 'Neighbors[4]' has a Cost of 'Costs[4]'.
     */
    public class GraphNode<T> : Node<T>
    {
        private List<int> _costs;
        public GraphNode() : base() { }
        public GraphNode(T value) : base(value) { }
        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }
        new public NodeList<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                {
                    base.Neighbors = new NodeList<T>();
                }
                return base.Neighbors;
            }
        }
        public List<int> Costs
        {
            get
            {
                if (_costs == null)
                {
                    _costs = new List<int>();
                }
                return _costs;
            }
        }
    }
}
//*** SourceCombiner -> original file Node.cs ***
/*
https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005
Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/
namespace CodingGame.Graph
{
    public class Node<T>
    {
        public Node() { }
        public Node(T data) : this(data, null) { }
        public Node(T data, NodeList<T> neighbors)
        {
            this.Value = data;
            this.Neighbors = neighbors;
        }
        public T Value { get; set; }
        protected NodeList<T> Neighbors { get; set; } = null;
    }
}
//*** SourceCombiner -> original file NodeList.cs ***
/*
https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005
Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/
namespace CodingGame.Graph
{
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }
        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
            {
                base.Items.Add(default(Node<T>));
            }
        }
        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            // if we reached here, we didn't find a matching node
            return null;
        }
    }
}
//*** SourceCombiner -> original file Tile.cs ***
namespace CodingGame.TileMap
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
//*** SourceCombiner -> original file TileMap.cs ***
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