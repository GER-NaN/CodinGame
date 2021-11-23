using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.TileMap;

namespace CrystalRush.BotStrategy
{
    /// <summary>A Radar placement strategy that attempts to discover clusters based on recently discovered ores</summary>
    public class RadarClusterStrategy : IRobotStrategy
    {
        //List of preferred radar stations that will cover most of the map
        private List<Point> RadarStations = new List<Point>()
        {
            //Straight up the middle X
            new Point(7,7),
            new Point(15,7),
            new Point(23,7),
            //Front Sides
            new Point(12,11),
            new Point(12,3),
            //Middle Sides
            new Point(19,2),
            new Point(19,12),
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

            GenerateClusterScores(map);
            GenerateRadarCoverage(map);

            //Find the cell with best surrounding ore density score
            var bestCell = map.Tiles.OrderByDescending(cell => cell.Item.ClusterDensityScore).First();

            //Find the a nearby spot not covered by radar
            var newRadar = map.FindNearest(cell => !cell.Item.RadarCoverage && cell.Position.X > 0 && !cell.Item.Avoid, bestCell.Position);

            //Look for safe ore to grab
            var safeOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.IsTrap == false && tile.Item.IsHole == false, robot.Position);

            //Use a decent starting position if we have no radars or we havent found ore
            var startingRadar = map.TileAt(RadarStations.First(s => map.ItemAt(s).IsRadar == false && map.ItemAt(s).IsHole == false));
            var radarCount = map.Tiles.Where(tile => tile.Item.IsRadar).Count();
            var oreCount = map.Tiles.Where(tile => tile.Item.Ore > 0).Count();

            if(radarCount == 0 || oreCount == 0)
            {
                newRadar = startingRadar;
            }

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} rc:hq";
            }
            //If we have a radar, go place it
            else if (robot.ItemHeld == CrystalRushItemType.Radar)
            {
                action = $"DIG {newRadar.Position.X} { newRadar.Position.Y} rc:dr";
                if(robot.Position.Equals(newRadar.Position))
                {
                    map.TileAt(newRadar.Position).Item.IsRadar = true;
                }
            }
            //If at HQ grab a radar
            else if (robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST RADAR rc:get";
            }
            //Dig safe ore if available
            else if (safeOre != null)
            {
                action = $"DIG {safeOre.Position.X} {safeOre.Position.Y} rc:do";
            }
            //Go to HQ for another radar
            else if (safeOre == null)
            {
                action = $"MOVE 0 {robot.Position.Y} rc:hq";
            }

            return action;
        }

        private void GenerateClusterScores(TileMap<CrystalRushCell> map)
        {
            foreach (var cell in map.Tiles)
            {
                var neighbors = map.GetNeighbors(cell.Position, 2);
                var score = neighbors.Sum(neighbor => neighbor.Item.Ore) + cell.Item.Ore;
                cell.Item.ClusterDensityScore = score;
            }
        }

        private void GenerateRadarCoverage(TileMap<CrystalRushCell> map)
        {
            foreach (var radar in map.FindAll(t => t.Item.IsRadar))
            {
                var neighbors = map.GetNeighbors(radar.Position, 4);
                foreach(var neighbor in neighbors)
                {
                    neighbor.Item.RadarCoverage = true;
                }

                //Mark the radar position as also having coverage
                map.ItemAt(radar.Position).RadarCoverage = true;
            }
        }
    }
}
