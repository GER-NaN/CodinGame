using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This puzzle: https://www.codingame.com/ide/puzzle/nine-mens-morris
namespace NineMensMorris
{
    class Program
    {
        static void Main(string[] args)
        {
            int playerId = int.Parse(Console.ReadLine()); // playerId (0,1)
            int fields = int.Parse(Console.ReadLine()); // number of fields
            for (int i = 0; i < fields; i++)
            {
                string neighbors = Console.ReadLine(); // neighbors of a field (ex: A1:A4;D1)
            }

            // game loop
            while (true)
            {
                string opMove = Console.ReadLine(); // The last move executed from the opponent
                string board = Console.ReadLine(); // Current Board and state(0:Player0 | 1:Player1 | 2:Empty) in format field:state and separated by ;
                int nbr = int.Parse(Console.ReadLine()); // Number of valid moves proposed.
                for (int i = 0; i < nbr; i++)
                {
                    string command = Console.ReadLine(); // An executable command line
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                Console.WriteLine("PLACE;A1");
            }
        }
    }
}
