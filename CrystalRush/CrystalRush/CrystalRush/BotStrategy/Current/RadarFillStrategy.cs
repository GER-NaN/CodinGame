using Common.Core;
using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.BotStrategy
{
    /// <summary>A Radar placement strategy that attempst to discover as many cells as possible</summary>
    public class RadarFillStrategy : IRobotStrategy
    {
        //List of preferred radar stations to start
        private List<Point> RadarStations = new List<Point>()
        {
            //Straight up the middle
            new Point(7,7),
            new Point(15,7),
            new Point(23,7),
            //Middle Sides
            new Point(19,2),
            new Point(19,12),
            //Front Sides
            new Point(12,11),
            new Point(12,3),
            //HQ area
            new Point(2,3),
            new Point(2,11),
            //Back Sides
            new Point(27,12),
            new Point(27,3),
            //Mid Fill
            new Point(7,14),
            new Point(7,0)
        };

        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            var existingRadars = map.FindAll(cell => cell.Item.IsRadar);
            Point newRadar = RadarStations.First();

            //If we only have a few radars, place them at a decent spot
            if(existingRadars.Count() < 2)
            {
                newRadar = map.TileAt(RadarStations.First(s => !map.ItemAt(s).IsRadar && !map.ItemAt(s).IsHole)).Position;//Assume this will return...
            }
            else
            {
                //Calculate total coverage
                int bestLocationCoverage = 0;

                //For each cell that I can put a radar on
                foreach(var tile in map.FindAll(cell => !cell.Item.IsRadar && !cell.Item.IsHole && cell.Position.X > 0))
                {
                    //Count the amount of tiles that would get coverage if we placed a radar here
                    var newCoverage = 0;
                    var neighbors = map.GetNeighbors(tile.Position, 4);

                    foreach (var neighbor in neighbors)
                    {
                        if(!neighbor.Item.RadarCoverage && neighbor.Item.SafeToDig())
                        {
                            newCoverage += 1;
                        }
                    }
                    //Count the radars location as well
                    if(!tile.Item.RadarCoverage && tile.Item.SafeToDig())
                    {
                        newCoverage += 1;
                    }

                    //If this location gets more coverage, save it
                    if(newCoverage > bestLocationCoverage)
                    {
                        bestLocationCoverage = newCoverage;
                        newRadar = tile.Position;
                    }
                }
            }


            //Look for safe ore to grab
            var safeOre = map.FindNearest(tile => tile.Item.SafeOreAvailable(false), robot.Position);

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} rf:hq";
            }
            //If we have a radar, go place it
            else if (robot.ItemHeld == CrystalRushItemType.Radar)
            {
                action = $"DIG {newRadar.X} { newRadar.Y} rf:dr";
                if (robot.Position.Equals(newRadar))
                {
                    map.TileAt(newRadar).Item.IsRadar = true;
                }
            }
            //If at HQ grab a radar
            else if (robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST RADAR rf:get";
            }
            //Dig safe ore if available
            else if (safeOre != null)
            {
                action = $"DIG {safeOre.Position.X} {safeOre.Position.Y} rf:do";
                safeOre.Item.BotsAssignedToDig += 1;
            }
            //Go to HQ for another radar
            else if (safeOre == null)
            {
                action = $"MOVE 0 {robot.Position.Y} rf:hq";
            }

            return action;
        }
    }
}
