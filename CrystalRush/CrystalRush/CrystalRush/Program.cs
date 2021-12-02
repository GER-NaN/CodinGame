using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using CrystalRush;
using Common.TileMap;
using Common.Core;
using CrystalRush.BotStrategy;
using Common.StandardExtensions;
using CrystalRush.GameStrategy;

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
        var enemyRobots = new List<Robot>();
        var map = new TileMap<CrystalRushCell>(width, height);
        var roundNumber = 0;
        TrapDetector trapDetector = null;

        while (true)
        {
            DebugTool.StartTimer("Global");
            /*****************************************************************************
             * Inputs
             *****************************************************************************/
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

                    var tile = map.TileAt(j,i);
                    if (tile.Item == null)
                    {
                        tile.Item = new CrystalRushCell(ore, hole);
                    }
                    else
                    {
                        //Need to reset everything
                        tile.Item.Ore = (ore == "?") ? 0 : int.Parse(ore);
                        tile.Item.IsHole = Convert.ToBoolean(hole);
                        tile.Item.IsTrap = false;
                        tile.Item.IsRadar = false;
                        tile.Item.BotsAssignedToDig = 0;

                        //Update our original ore map
                        //if (map.TileAt(j,i).Item.Ore > 0 && originalOreMap.TileAt(j,i).Item == 0)
                        //{
                        //    originalOreMap.TileAt(j, i).Item = map.TileAt(j, i).Item.Ore;
                        //}
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
                else if ((CrystalRushItemType)entityType == CrystalRushItemType.EnemyRobot)
                {
                    var robot = new Robot(entityId, (CrystalRushItemType)entityType, new System.Drawing.Point(x, y));
                    if (roundNumber == 1)
                    {
                        enemyRobots.Add(robot);
                    }
                    else
                    {
                        enemyRobots.First(r => r.Id == robot.Id).Update(robot);
                    }
                }
                else if ((CrystalRushItemType)entityType == CrystalRushItemType.Radar)
                {
                    map.TileAt(x, y).Item.IsRadar = true;
                }
                else if ((CrystalRushItemType)entityType == CrystalRushItemType.Trap)
                {
                    map.TileAt(x, y).Item.IsTrap = true;
                }
            }

            /*****************************************************************************
             * TRAP DETECTION
             *****************************************************************************/
            if (trapDetector == null)
            {
                trapDetector = new TrapDetector(enemyRobots);
            }
            else
            {
                trapDetector.RunAnalysis(map,enemyRobots);

                foreach(var trap in trapDetector)
                {
                    map.ItemAt(trap).Avoid = true;
                }
            }

            /*****************************************************************************
             * RADAR COVERAGE
             *****************************************************************************/
            foreach (var cell in map.Tiles)
            {
                cell.Item.RadarCoverage = false;
            }

            foreach (var radar in map.FindAll(t => t.Item.IsRadar))
            {
                var neighbors = map.GetNeighbors(radar.Position, 4);
                foreach (var neighbor in neighbors)
                {
                    neighbor.Item.RadarCoverage = true;
                }

                //Mark the radar position as also having coverage
                map.ItemAt(radar.Position).RadarCoverage = true;
            }

            /*****************************************************************************
             * DEAD ROBOTS
             *****************************************************************************/
            foreach (var robot in myRobots)
            {
                if (robot.IsDead())
                {
                    robot.Strategy = new DeadStrategy();
                }
            }



            /*****************************************************************************
             * Calculate Metrics
             *****************************************************************************/
             //Percentage of map I have covered
            var radarCoverage = map.FindAll(t => t.Item.RadarCoverage).Count / (double)map.Tiles.Count();
            //Tiles that have Ore which is known to be safe
            var safeOreCells = map.FindAll(t => !t.Item.IsHole && t.Item.Ore > 0).Count();
            //All tiles with ore that we dont suspect as traps
            var unsafeOreCells = map.FindAll(t => !t.Item.Avoid && t.Item.Ore > 0).Count();

            DebugTool.Print($"Stats>c:{radarCoverage.ToString("P")},so:{safeOreCells},uo:{unsafeOreCells}");





            /*****************************************************************************
             * STRATEGY
             *****************************************************************************/
            //var test = new TestStrategy(map, myRobots, roundNumber);
            //test.RunSingleStrategy(new NoStrategy());


            if (myRobots.Count(b => !b.IsDead()) >= 3)
            {
                var gameStrat = new StarterStrategy(map, myRobots, roundNumber, myScore, opponentScore);
                gameStrat.RunStrategy();
                DebugTool.CheckTimer("Global", "Run Strategy");
            }
            else
            {
                var gameStratLow = new TwoBotStrategy(map, myRobots, roundNumber);
                gameStratLow.RunStrategy();
            }

            DebugTool.StopTimer("Global");
        }
    }
}