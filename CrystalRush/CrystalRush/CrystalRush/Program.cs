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



        // game loop
        while (true)
        {
            //Reset map
            var map = new TileMap<CrystalRushCell>(width, height);
            var myRobots = new List<Robot>();

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
                    map.TileAt(j, i).Item = new CrystalRushCell(ore, hole);
                }
            }

            inputs = Console.ReadLine().Split(' ');
            int entityCount = int.Parse(inputs[0]); // number of entities visible to you
            int radarCooldown = int.Parse(inputs[1]); // turns left until a new radar can be requested
            int trapCooldown = int.Parse(inputs[2]); // turns left until a new trap can be requested

            //Entities
            for (int i = 0; i < entityCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');

                int entityId = int.Parse(inputs[0]); // unique id of the entity
                int entityType = int.Parse(inputs[1]); // 0 for your robot, 1 for other robot, 2 for radar, 3 for trap
                int x = int.Parse(inputs[2]);
                int y = int.Parse(inputs[3]); // position of the entity
                int item = int.Parse(inputs[4]); // if this entity is a robot, the item it is carrying (-1 for NONE, 2 for RADAR, 3 for TRAP, 4 for ORE)

                CrystalRushItem entity;
                if (entityType == 0 || entityType == 1)
                {
                    entity = new Robot(entityId, (CrystalRushItemType)entityType, new System.Drawing.Point(x, y));
                    (entity as Robot).SetItemHeld((CrystalRushItemType)item);


                    if (entity.Type == CrystalRushItemType.MyRobot)
                    {
                        myRobots.Add(entity as Robot);
                    }
                }
                else
                {
                    entity = new CrystalRushItem(entityId, (CrystalRushItemType) entityType, new System.Drawing.Point(x, y));
                    //Not doing anything with the enemy robots
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Robot robot = myRobots[i];

                //The lowest Robot will do radar stuff
                if(robot.Id == myRobots.OrderBy(r => r.Id).First().Id)
                {
                    if(robot.ItemHeld != CrystalRushItemType.Radar && robot.Position.X != 0)
                    {
                        Console.WriteLine($"MOVE 0 {robot.Position.Y} (R)GO:HQ");
                    }
                    else if(robot.ItemHeld != CrystalRushItemType.Radar && robot.Position.X == 0)
                    {
                        Console.WriteLine("REQUEST RADAR");
                    }
                    else if(robot.ItemHeld == CrystalRushItemType.Radar && robot.Position.X == 0)
                    {
                        //Go somewhere random
                        var destination = map.GetRandomTile();
                        Console.WriteLine($"MOVE {destination.Point.X} {destination.Point.Y} (R)GO");
                    }
                    else
                    {
                        //Make sure the robot goes out into the map, we havent saved state or travel yet.
                        if(robot.Position.X < 9)
                        {
                            //Go somewhere random
                            var destination = map.GetRandomTile();
                            Console.WriteLine($"MOVE {destination.Point.X} {destination.Point.Y} (R)GO");
                        }
                        else
                        {
                            Console.WriteLine($"DIG {robot.Position.X} {robot.Position.Y} (R)PLANT");
                        }
                    }
                }
                else
                {
                    if (robot.ItemHeld == CrystalRushItemType.Ore)
                    {
                        Console.WriteLine($"MOVE 0 {robot.Position.Y} GO:HQ");
                    }
                    else if (map.TileAt(robot.Position).Item.Ore > 0)
                    {
                        Console.WriteLine($"DIG {robot.Position.X} {robot.Position.Y} DIG");
                    }
                    else
                    {
                        //Go somewhere random
                        var destination = map.GetRandomTile();
                        Console.WriteLine($"MOVE {destination.Point.X} {destination.Point.Y} SEARCH");
                    }
                }
 
            }
        }
    }
}