using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using CrystalRush;
using Common.TileMap;
using Common.Core;
using CrystalRush.Strategy;
using Common.StandardExtensions;

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

                    if (map.TileAt(j, i).Item == null)
                    {
                        map.TileAt(j, i).Item = new CrystalRushCell(ore, hole);
                    }
                    else
                    {
                        //Need to reset everything
                        map.TileAt(j, i).Item.Ore = (ore == "?") ? 0 : int.Parse(ore);
                        map.TileAt(j, i).Item.IsHole = Convert.ToBoolean(hole);
                        map.TileAt(j, i).Item.MyTrap = false;
                        map.TileAt(j, i).Item.IsRadar = false;
                    }
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
                if ((CrystalRushItemType)entityType == CrystalRushItemType.MyRobot)
                {
                    var robot = new Robot(entityId, (CrystalRushItemType)entityType, new System.Drawing.Point(x, y));
                    robot.SetItemHeld((CrystalRushItemType)item);
                    if (roundNumber == 1)
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
                else if ((CrystalRushItemType)entityType == CrystalRushItemType.Radar)
                {
                    map.TileAt(x, y).Item.IsRadar = true;
                }
                else if ((CrystalRushItemType)entityType == CrystalRushItemType.Trap)
                {
                    map.TileAt(x, y).Item.MyTrap = true;
                }
            }


            //Mark Dead robots
            foreach (var robot in myRobots)
            {
                if (robot.IsDead())
                {
                    robot.Strategy = new DeadStrategy();
                }
            }

            //If we need a trapper, grab one of the diggers  
            var startTrappingAt = 50;
            var stopTrappingAt = 100;
            var needTrapper = roundNumber.Between(startTrappingAt, stopTrappingAt);

            if (needTrapper && !myRobots.Any(r => r.Strategy is TrapOreStrategy))
            {
                var robot = myRobots.Where(r => r.Strategy is DigOreStrategy || r.Strategy is NoStrategy).First();
                robot.Strategy = new TrapOreStrategy();
            }
            else if (!needTrapper && myRobots.Any(r => r.Strategy is TrapOreStrategy))//reset any trappers because we dont need them
            {
                foreach (var bot in myRobots.Where(r => r.Strategy is TrapOreStrategy))
                {
                    bot.Strategy = new DigOreStrategy();
                }
            }


            //If we need radars, grab one of the diggers
            var radarOreLimit = 20;
            var needRadar = map.FindAll(cell => cell.Item.Ore > 0).Sum(cell => cell.Item.Ore) < radarOreLimit;

            if (needRadar && !myRobots.Any(r => r.Strategy is RadarStrategy))
            {
                var robot = myRobots.Where(r => r.Strategy is DigOreStrategy || r.Strategy is NoStrategy).First();
                robot.Strategy = new RadarStrategy();
            }
            else if (!needRadar && myRobots.Any(r => r.Strategy is RadarStrategy))//reset any radar because we dont need them
            {
                foreach (var bot in myRobots.Where(r => r.Strategy is RadarStrategy))
                {
                    bot.Strategy = new DigOreStrategy();
                }
            }


            //Fix anyone without a strategy
            foreach (var bot in myRobots.Where(r => r.Strategy is NoStrategy))
            {
                bot.Strategy = new DigOreStrategy();
            }


            foreach (var bot in myRobots)
            {
                Console.WriteLine(bot.Strategy.GetMove(map, bot));
            }

        }
    }
}