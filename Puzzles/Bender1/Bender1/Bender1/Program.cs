using Common.Core;
using Common.StandardTypeExtensions;
using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bender1
{
    //https://www.codingame.com/ide/puzzle/bender-episode-1
    class Program
    {
        static void Main(string[] args)
        {
            //input
            /*********************************/
            string[] inputs = Console.ReadLine().Split(' ');
            int lines = int.Parse(inputs[0]);
            int columns = int.Parse(inputs[1]);

            BenderItem[,] mapInputs = new BenderItem[lines, columns];
            for (int i = 0; i < lines; i++)
            {
                char[] line = Console.ReadLine().ToCharArray();
                for (int j = 0; j < line.Length; j++)
                {
                    mapInputs[i, j] = (BenderItem)line[j];
                }
            }
            /*********************************/

            //Initialize our map and starting position
            var map = new TileMap<BenderItem>(mapInputs);
            var path = new TileMapPath<BenderItem>();
            var currentDirection = "SOUTH";
            var currentPosition = map.Find(BenderItem.Start);


            var output = "";
            var looping = false;
            var completed = false;
            var breaker = false;
            var inverter = false;

            while (!completed && !looping)
            {
                //Add our postion to the path
                path.AddToPath(currentPosition);

                //Check for loop
                //Check if we are looping
                if (path.ContainsLoop(5))
                {
                    completed = true;
                    looping = true;
                }

                // 1.Examine current position and see if we need to do anything
                switch (currentPosition.Item)
                {
                    case BenderItem.SuicideBooth:
                        completed = true;
                        break;
                    case BenderItem.ModifierNorth:
                        currentDirection = "NORTH";
                        break;
                    case BenderItem.ModifierEast:
                        currentDirection = "EAST";
                        break;
                    case BenderItem.ModifierSouth:
                        currentDirection = "SOUTH";
                        break;
                    case BenderItem.ModifierWest:
                        currentDirection = "WEST";
                        break;
                    case BenderItem.Beer:
                        breaker = !breaker;
                        break;
                    case BenderItem.CircuitInverter:
                        inverter = !inverter;
                        break;
                    case BenderItem.Teleporter:
                        var teleporters = map.FindAll(BenderItem.Teleporter); //Guaranteed to only be 2 of them
                        var newPosition = teleporters[0];
                        if (newPosition == currentPosition)
                        {
                            newPosition = teleporters[1];
                        }
                        currentPosition = newPosition;
                        break;
                }

                // 2.Determine Direction to go
                // - Is direction passable
                //      -Move to the tile
                // - Is direction NOT passable
                //      - Check if we can break it(breaker mode on and breakable obstacle
                //          - Break Wall and remove from map
                //          - Move to the tile
                //      - Change direction of travel according to rules

                //var nextTile = map.TileAt()
                if(!completed)
                {
                    var moved = false;
                    while (!moved)
                    {
                        var tileToTry = map.TileAt(currentPosition.Point, currentDirection.ToDirection());

                        if (!tileToTry.Item.IsObstacle())
                        {
                            //Move to the tile
                            currentPosition = tileToTry;
                            moved = true;
                        }
                        else
                        {
                            if (breaker && tileToTry.Item == BenderItem.ObstacleBreakable)
                            {
                                //Break wall
                                tileToTry.Item = BenderItem.Empty;

                                //Move to tile
                                currentPosition = tileToTry;
                                moved = true;
                            }
                            else
                            {
                                //We have hit an obstacle we cannot pass. Try turning according to priority.
                                var testDirection = Direction.South;
                                if (inverter)
                                {
                                    testDirection = Direction.West;
                                }

                                tileToTry = map.TileAt(currentPosition.Point, testDirection);

                                //Find somehwere to go, assume there is one...
                                while(tileToTry.Item.IsObstacle())
                                {
                                    if(inverter)
                                    {
                                        testDirection = testDirection.RelativeRight();
                                        tileToTry = map.TileAt(currentPosition.Point, testDirection);
                                    }
                                    else
                                    {
                                        testDirection = testDirection.RelativeLeft();
                                        tileToTry = map.TileAt(currentPosition.Point, testDirection);
                                    }
                                }

                                currentPosition = tileToTry;
                                currentDirection = testDirection.ToString();
                                moved = true;
                            }
                        }
                    }
                    
                    //output the direction we moved
                    output += currentDirection.ToUpper() + Environment.NewLine;
                }
            }

            if (looping)
            {
                Console.WriteLine("LOOP");
            }
            else
            {
                Console.WriteLine(output);
            }

            Console.ReadLine();
        }
    }
}

/*
Bender is a depressed robot who heals his depression by partying and drinking alcohol. To save him from a life of 
debauchery, his creators have reprogrammed the control system with a more rudimentary intelligence. Unfortunately, 
he has lost his sense of humor and his former friends have now rejected him.

Bender is now all alone and is wandering through the streets of Futurama with the intention of ending it all in a 
suicide booth.

To intercept him and save him from almost certain death, the authorities have given you a mission: write a program 
that will make it possible to foresee the path that Bender follows. To do so, you are given the logic for the new 
intelligence with which Bender has been programmed as well as a map of the city.

Rules
The 9 rules of the new Bender system:

Bender starts from the place indicated by the @ symbol on the map and heads SOUTH.

Bender finishes his journey and dies when he reaches the suicide booth marked $.

Obstacles that Bender may encounter are represented by # or X.

When Bender encounters an obstacle, he changes direction using the following priorities: SOUTH, EAST, NORTH and 
WEST. So he first tries to go SOUTH, if he cannot, then he will go EAST, if he still cannot, then he will go NORTH, 
and finally if he still cannot, then he will go WEST.

Along the way, Bender may come across path modifiers that will instantaneously make him change direction. The S 
modifier will make him turn SOUTH from then on, E, to the EAST, N to the NORTH and W to the WEST.

The circuit inverters (I on map) produce a magnetic field which will reverse the direction priorities that Bender 
should choose when encountering an obstacle. Priorities will become WEST, NORTH, EAST, SOUTH. If Bender returns 
to an inverter I, then priorities are reset to their original state (SOUTH, EAST, NORTH, WEST).

Bender can also find a few beers along his path (B on the map) that will give him strength and put him in “Breaker” 
mode. Breaker mode allows Bender to destroy and automatically pass through the obstacles represented by the 
character X (only the obstacles X). When an obstacle is destroyed, it remains so permanently and Bender maintains 
his course of direction. If Bender is in Breaker mode and passes over a beer again, then he immediately goes out of 
Breaker mode. The beers remain in place after Bender has passed.

2 teleporters T may be present in the city. If Bender passes over a teleporter, then he is automatically teleported 
to the position of the other teleporter and he retains his direction and Breaker mode properties.

Finally, the space characters are blank areas on the map (no special behavior other than those specified above).
Your program must display the sequence of moves taken by Bender according to the map provided as input.



The map is divided into lines (L) and columns (C). The contours of the map are always unbreakable # obstacles. The map 
always has a starting point @ and a suicide booth $.

If Bender cannot reach the suicide booth because he is indefinitely looping, then your program must only display LOOP.
 */
