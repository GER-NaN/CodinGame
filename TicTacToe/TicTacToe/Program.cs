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


                var move = board.FindWinningMove(TileState.O);

                if (move == -1)
                {
                    Debug("Find Winning Move to block");
                    move = board.FindWinningMove(TileState.X);
                }
                
                if (move == -1)
                {
                    Debug("Find Random Move: ");
                    move = board.FindEmptyTile();
                }

                board.Set(move, TileState.O);
                Debug(board.Print());

                var position = Board.TileToPoint(move);
                Console.WriteLine(position.Item1 + " " + position.Item2);
                
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

        public static Tuple<int,int> TileToPoint(int tileNumber)
        {
            int row = 0;
            int col = 0;
            
            row = (tileNumber - 1) / 3;
            col = (tileNumber - 1) % 3;

            return new Tuple<int, int>(row, col);
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

        public int FindEmptyTile()
        {
            var tile = -1;

            for(int i=0;i<9;i++)
            {
                if(TileStates[i] == TileState.E)
                {
                    return i + 1;
                }
            }
            return tile;
        }

        //Tries to find a winning tile for the given State
        public int FindWinningMove(TileState tileState)
        {
            if(tileState == TileState.E)
            {
                throw new ArgumentException("TileState.E cannot have a winning state. Pass X or O for the tile state.");
            }

            int winningTile = -1;

            if(TestRowForWin(1, 2, 3, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(4, 5, 6, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(7, 8, 9, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(1, 4, 7, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(2, 5, 8, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(3, 6, 9, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(1, 5, 9, tileState, out winningTile))
            {
                return winningTile;
            }
            else if (TestRowForWin(3, 5, 7, tileState, out winningTile))
            {
                return winningTile;
            }

            return winningTile;
        }

        private bool TestRowForWin(int tile1, int tile2, int tile3, TileState player, out int winningTile)
        {
            List<TileState> states = new List<TileState> { Get(tile1), Get(tile2), Get(tile3) };
            List<TileState> goodStates = new List<TileState> { player, TileState.E };
            
            //If 2 of the cells have my player and all of them are good (player or Empty)
            if (states.Count(state => state == player) == 2 && states.All(state => goodStates.Contains(state)))
            {
                if (IsEmpty(tile1))
                {
                    winningTile = tile1;
                    return true;
                }
                else if (IsEmpty(tile2))
                {
                    winningTile = tile2;
                    return true;
                }
                else
                {
                    winningTile = tile3;
                    return true;
                }
            }
            winningTile = -1;
            return false;
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
