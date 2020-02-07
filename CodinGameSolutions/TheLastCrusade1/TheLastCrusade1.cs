using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Common.Core;

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

            List<int> size = Input.ReadItems<int>();
            int width = size[0]; // number of columns.
            int height = size[1]; // number of rows.

            int[,] rooms = new int[height,width];

            //Read the rooms in each row
            for (int i = 0; i < height; i++)
            {
                List<int> row = Input.ReadItems<int>();
                int col = 0;
                foreach(var room in row)
                {
                    rooms[i, col] = room;
                    col++;
                }
            }

            int finalExit = Input.ReadInt(); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

            // game loop
            while (true)
            {
                List<string> input = Input.ReadItems<string>();
                int x = int.Parse(input[0]);
                int y = int.Parse(input[1]);

                var currentRoom = rooms[y, x];
                Direction entry = DirectionHelper.Parse(input[2]);
                Direction exit = FindExit(entry, currentRoom);


                //Output
                int exitY;
                int exitX;


                //Check if he is going to exit the bottom
                if (exit == Direction.Down)
                {
                    exitX = x;
                    exitY = y + 1;
                }
                else if(exit == Direction.Right)//Hes going to exit left
                {
                    exitX = x + 1;
                    exitY = y;
                }
                else//He is going to exit right (no other option)
                {
                    exitX = x - 1;
                    exitY = y;
                }

                // One line containing the X Y coordinates of the room in which you believe Indy will be on the next turn.
                Output.Print(exitX, exitY);
            }


        }
        public static Direction FindExit(Direction entry, int roomType)
        {
            Direction exit = Direction.Down;

            switch (roomType)
            {
                case 1:
                    exit = Direction.Down;//The only exit
                    break;
                case 2:
                    if(entry == Direction.Left)
                    {
                        exit = Direction.Right;
                    }
                    else
                    {
                        exit = Direction.Left;
                    }
                    break;
                case 3:
                    exit = Direction.Down;//The only exit
                    break;
                case 4:
                    if (entry == Direction.Top)
                    {
                        exit = Direction.Left;
                    }
                    else
                    {
                        exit = Direction.Down;
                    }
                    break;
                case 5:
                    if (entry == Direction.Top)
                    {
                        exit = Direction.Right;
                    }
                    else
                    {
                        exit = Direction.Down;
                    }
                    break;
                case 6:
                    if (entry == Direction.Left)
                    {
                        exit = Direction.Right;
                    }
                    else
                    {
                        exit = Direction.Left;
                    }
                    break;
                case 7:
                    exit = Direction.Down;//only option
                    break;
                case 8:
                    exit = Direction.Down;//only option
                    break;
                case 9:
                    exit = Direction.Down;//only option
                    break;
                case 10:
                    exit = Direction.Left;//only option
                    break;
                case 11:
                    exit = Direction.Right;//only option
                    break;
                case 12:
                    exit = Direction.Down;//only option
                    break;
                case 13:
                    exit = Direction.Down;//only option
                    break;
            }

            return exit;
        }
    }
}
