using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using CrystalRush;
using Common.TileMap;

//https://www.codingame.com/ide/puzzle/crystal-rush
class Program
{
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]);
        int height = int.Parse(inputs[1]); // size of the map


        var myRobots = new List<Robot>();
        var map = new TileMap<CrystalRushCell>(width, height);
        var roundNumber = 0;

        // game loop
        while (true)
        {
            roundNumber++;


            inputs = Console.ReadLine().Split(' ');
            int myScore = int.Parse(inputs[0]); // Amount of ore delivered
            int opponentScore = int.Parse(inputs[1]);

            //Cells
            for (int i = 0; i < height; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                for (int j = 0; j < width; j++)
                {
                    string ore = inputs[2 * j];// amount of ore or "?" if unknown
                    int hole = int.Parse(inputs[2 * j + 1]);// 1 if cell has a hole

                    if(map.TileAt(j,i).Item == null)
                    {
                        map.TileAt(j, i).Item = new CrystalRushCell(false,0,false,false);
                    }

                    map.TileAt(j, i).Item.Ore = (ore == "?" ? 0 : int.Parse(ore));
                    map.TileAt(j, i).Item.IsHole = Convert.ToBoolean(hole);
                }
            }

            inputs = Console.ReadLine().Split(' ');
            int entityCount = int.Parse(inputs[0]); // number of entities visible to you
            int radarCooldown = int.Parse(inputs[1]); // turns left until a new radar can be requested
            int trapCooldown = int.Parse(inputs[2]); // turns left until a new trap can be requested

            //Entities
            var ySearchArea = 0;
            for (int i = 0; i < entityCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');

                int entityId = int.Parse(inputs[0]); // unique id of the entity
                int entityType = int.Parse(inputs[1]); // 0 for your robot, 1 for other robot, 2 for radar, 3 for trap
                int x = int.Parse(inputs[2]);
                int y = int.Parse(inputs[3]); // position of the entity
                int item = int.Parse(inputs[4]); // if this entity is a robot, the item it is carrying (-1 for NONE, 2 for RADAR, 3 for TRAP, 4 for ORE)

                //Only handle my robots for now
                if (entityType == 0)
                {
                    var robot = new Robot(entityId, (CrystalRushItemType)entityType, new System.Drawing.Point(x, y));
                    robot.SetItemHeld((CrystalRushItemType)item);
                    if(roundNumber == 1)
                    {
                        myRobots.Add(robot);
                        robot.YStart = ySearchArea * 3;
                        ySearchArea++;
                    }
                    else
                    {
                        myRobots.First(r => r.Id == robot.Id).Update(robot);
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                //Only have 1 robot do work
                if(i == 0)
                {
                    Robot robot = myRobots[i];

                    var strategy = new StrategyRadarSearch();
                    var move = strategy.GetMove(map, robot);

                    Console.WriteLine(move);
                }
                else
                {
                    Console.WriteLine("WAIT");
                }

            }
        }
    }
}