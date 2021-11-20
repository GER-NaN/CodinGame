using Common.StandardTypeExtensions;
using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    //Places radar on the map and only digs known ores
    public class StrategyRadarSearch
    {
        //List of radar placements that will "light up" the entire grid, Grid is 30/15 and Radars illuminate 4 spaces in all direction
        private List<Point> RadarStations = new List<Point>()
        {
            new Point(4,3),
            new Point(4,12),
            new Point(12,3),
            new Point(12,12),
            new Point(21,3),
            new Point(21,12),
            new Point(28,3),
            new Point(28,12)
        };

        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";
            var nearestOre = map.FindNearest(cell => cell.Item.Ore > 0,robot.Position);

            //Remove RadarStations that have one already
            foreach(var tile in map.FindAll(cell => cell.Item.IsRadar))
            {
                RadarStations.Remove(tile.Point);
            }


            var nearestStation = RadarStations.OrderBy(station => robot.Position.DistanceTo(station)).First();


            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            else if(robot.ItemHeld == CrystalRushItemType.Radar && robot.Position.Equals(nearestStation)) //If we have a radar and are at a location, place it
            {
                action = $"DIG {robot.Position.X} { robot.Position.Y} BURY:RADAR";
                map.TileAt(robot.Position).Item.IsRadar = true;
            }
            else if(robot.ItemHeld == CrystalRushItemType.Radar)//Continue to the nearest station
            {
                action = $"MOVE {nearestStation.X} {nearestStation.Y} GO:RADAR";
            }
            else if(robot.Position.X == 0 && robot.ItemHeld == CrystalRushItemType.None)//At headquarters, get a radar
            {
                action = $"REQUEST RADAR GET:RADAR";
            }
            else if(nearestOre != null && !robot.Position.Equals(nearestOre.Point))//Go to the nearest Ore
            {
                action = $"MOVE {nearestOre.Point.X} {nearestOre.Point.Y} GO:ORE";
            }
            else if (nearestOre != null && robot.Position.Equals(nearestOre.Point))//Dig the ore
            {
                action = $"DIG {nearestOre.Point.X} {nearestOre.Point.Y} GET:ORE";
            }
            else if(nearestOre == null)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }


            return action;
        }
    }
}
