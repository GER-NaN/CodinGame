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

            // game loop
            while (true)
            {
                inputs = Console.ReadLine().Split(' ');
                int opponentRow = int.Parse(inputs[0]);
                int opponentCol = int.Parse(inputs[1]);
                Debug("Opponent Played: " + opponentRow + ", " + opponentCol);

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


}
