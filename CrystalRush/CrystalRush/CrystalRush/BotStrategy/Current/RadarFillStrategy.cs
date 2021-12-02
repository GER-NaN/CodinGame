using Common.Core;
using Common.StandardTypeExtensions;
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
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT broken";

            var existingRadars = map.FindAll(cell => cell.Item.IsRadar);
            Point newRadar = new Point(7, 7);//Some default point

            //Calculate total coverage
            int bestLocationCoverage = 0;

            //For each cell that I can put a radar on
            foreach(var tile in map.FindAll(cell => !cell.Item.IsRadar && !cell.Item.IsHole && cell.Position.X > 0).OrderBy(cell => cell.Position.X))
            {
                //Count the amount of tiles that would get coverage if we placed a radar here
                var newCoverage = 0;
                var neighbors = map.GetNeighbors(tile.Position, 4);

                foreach (var neighbor in neighbors)
                {
                    if(!neighbor.Item.RadarCoverage)
                    {
                        newCoverage += 1;
                    }
                }
                //Count the radars location as well
                if(!tile.Item.RadarCoverage)
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
            //Only go for ore if its on our way back or we can reach it in 4 turns
            else if (safeOre != null && (safeOre.Position.X <= robot.Position.X || robot.Position.ManhattenDistanceTo(safeOre.Position) <= 16))
            {
                action = $"DIG {safeOre.Position.X} {safeOre.Position.Y} rf:do";
                safeOre.Item.BotsAssignedToDig += 1;
            }
            //Go to HQ for another radar
            else
            {
                action = $"MOVE 0 {robot.Position.Y} rf:hq";
            }

            return action;
        }
    }
}
