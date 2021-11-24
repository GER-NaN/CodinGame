using Common.StandardExtensions;
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
    /// <summary>
    /// A "Safe" radar strategy that places radars at optimal safe positions and digs safe ores on the way back.
    /// </summary>
    /// <remarks>A better strategy would be to radar around ore clusters and use some kind of intelligence</remarks>
    public class RadarStrategy : IRobotStrategy
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

        public RadarStrategy()
        {
            var xMin = 1;
            var xMax = 29;
            var yMin = 0;
            var yMax = 14;

            //Add 1 to the X
            var offsetX = new List<Point>();
            foreach (var point in RadarStations)
            {
                var x = (point.X + 1).RestrainWithin(xMin, xMax);
                var y = (point.Y).RestrainWithin(yMin, yMax);
                offsetX.Add(new Point(x, y));
            }

            //Add 1 to the Y
            var offsetY = new List<Point>();
            foreach (var point in RadarStations)
            {
                var x = (point.X).RestrainWithin(xMin, xMax);
                var y = (point.Y + 1).RestrainWithin(yMin, yMax);
                offsetY.Add(new Point(x, y));
            }

            //Add the alternatives back to our single list
            foreach (var point in offsetX.Union(offsetY))
            {
                RadarStations.Add(point);
            }
        }

        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find a station to place the Radar, dont place in holes or existing stations
            Point station = RadarStations.First(s => map.ItemAt(s).IsRadar == false && map.ItemAt(s).IsHole == false);

            //Look for safe ore to grab
            var safeOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.IsTrap == false && tile.Item.IsHole == false, robot.Position);

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} r:hq";
            }
            //If we have a radar, go place it
            else if (robot.ItemHeld == CrystalRushItemType.Radar)
            {
                action = $"DIG {station.X} { station.Y} r:d";

                if (robot.Position.Equals(station))
                {
                    map.TileAt(station).Item.IsRadar = true;
                }
            }
            //If at HQ grab a radar
            else if (robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST RADAR r:get";
            }
            //Dig any ore if available
            else if (safeOre != null)
            {
                action = $"DIG {safeOre.Position.X} {safeOre.Position.Y} r:d";
            }
            //Go to HQ for another radar
            else if (safeOre == null)
            {
                action = $"MOVE 0 {robot.Position.Y} r:hq";
            }

            return action;
        }
    }
}
