using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastCrusade1
{
    //https://www.codingame.com/ide/puzzle/the-last-crusade-episode-1
    /* Your objective is to write a program capable of predicting the route Indy will take on his 
     * way down a tunnel. Indy is not in danger of getting trapped in this first mission.
     */
    class TheLastCrusade1
    {
        static void Main(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]); // number of columns.
            int height = int.Parse(inputs[1]); // number of rows.

            for (int i = 0; i < height; i++)
            {
                string LINE = Console.ReadLine(); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
            }

            int EX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

            // game loop
            while (true)
            {
                inputs = Console.ReadLine().Split(' ');
                int XI = int.Parse(inputs[0]);
                int YI = int.Parse(inputs[1]);
                string POS = inputs[2];

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // One line containing the X Y coordinates of the room in which you believe Indy will be on the next turn.
                Console.WriteLine("0 0");
            }
        }
    }
}
