using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://www.codingame.com/multiplayer/bot-programming/tic-tac-toe
namespace TicTacToe
{
    /*
        Tic-tac-toe is a turn-based game, where the objective is to get three in a row.
 	
        Rules
        The game is played on a 3x3 grid. You must output the coordinate of the cell you 
        want to mark. The first player to get 3 of their mark in a row (vertically, 
        horizontally or diagonally) wins.

 	    Game Input
        Input for one game turn
        Line 1: 2 space separated integers opponentRow and opponentCol, the opponent's 
        last action (-1 -1 for the first turn). 
        Line 2: the number of valid actions for this turn, validActionCount. 
        Next validActionCount lines: 2 space separated integers row and col, the coordinates 
        you're allowed to play at.
        
        Output for one game turn
        Line 1: 2 space separated integers row and col.
        
        Constraints
        Response time for first turn ≤ 1000ms 
        Response time for one turn ≤ 100ms

        *My Notes*
        * For this implementation the opponent is always X and I will always be O.
        
     */
    class Program
    {
        static void Main(string[] args)
        {
            var t = new TicTacToe();
            t.Run();
        }
    }

    class TicTacToe
    {
        public void Run()
        {
            string[] inputs;

            Board board = new Board();

            // game loop
            while (true)
            {
                inputs = Console.ReadLine().Split(' ');
                int opponentRow = int.Parse(inputs[0]);
                int opponentCol = int.Parse(inputs[1]);
                
                if(opponentCol > -1)
                {
                    board.Set(opponentRow, opponentCol, TileState.X);
                    Debug("Opponent Played: " + opponentRow + ", " + opponentCol);
                }

                int validActionCount = int.Parse(Console.ReadLine());
                var validMoves = new List<Tuple<int, int>>();
                for (int i = 0; i < validActionCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int row = int.Parse(inputs[0]);
                    int col = int.Parse(inputs[1]);
                    validMoves.Add(new Tuple<int, int>(row, col));
                }

                var randomMove = GetRandomMove(validMoves);

                board.Set(randomMove.Item1, randomMove.Item2, TileState.O);
                Debug(board.Print());
                Console.WriteLine(randomMove.Item1 + " " + randomMove.Item2);
                
            }
        }

        private Tuple<int,int> GetRandomMove(List<Tuple<int,int>> validMoves)
        {
            return validMoves.OrderBy(r => Guid.NewGuid()).First();
        }

        public void Debug(string message)
        {
            Console.Error.WriteLine(message);
        }
    }

    internal class Board
    {
        /*
        1	2	3
        4	5	6
        7	8	9
        */
        public readonly TileState[] TileStates;

        /// <summary> Initializes an empty board </summary>
        public Board()
        {
            TileStates = new TileState[] { TileState.E, TileState.E, TileState.E, TileState.E, TileState.E, TileState.E, TileState.E, TileState.E, TileState.E };
        }

        public Board(TileState[] tileStates)
        {
            if(tileStates.Count() != 9)
            {
                throw new ArgumentOutOfRangeException("There must be exactly nine TileState's passed to the constructor. Found " + tileStates.Count() + " states in the array.");
            }

            TileStates = tileStates;
        }

        /// <summary>Gets the state of the tile</summary>
        public TileState Get(int tileNumber)
        {
            return TileStates[tileNumber - 1];
        }

        public bool IsEmpty(int tileNumber)
        {
            return Get(tileNumber) == TileState.E;
        }

        /// <summary> Sets the state of the specified tile</summary>
        public void Set(int tileNumber, TileState state)
        {
            if (TileStates[tileNumber - 1] != TileState.E)
            {
                throw new InvalidOperationException("The tile has already been played and is not empty. Current state is " + TileStates[tileNumber - 1]);
            }
            TileStates[tileNumber - 1] = state;
        }

        public void Set(int row, int col, TileState state)
        {
            Set(PointToTile(row, col), state);
        }

        public static int PointToTile(int row, int col)
        {
            return (row * 3) + col + 1;
        }

        public string Print()
        {
            var output = "";
            for(int i=0; i<9; i++)
            {
                output += TileStates[i].ToString();

                if(i == 2 || i == 5 || i == 9)
                {
                    output += Environment.NewLine;
                }
                else
                {
                    output += " ";
                }
            }
            return output;
        }
    }

    internal enum TileState
    {
        /// <summary>Player X </summary>
        X,

        /// <summary>Player X </summary>
        O,

        /// <summary>Empty</summary>
        E
    }
}
