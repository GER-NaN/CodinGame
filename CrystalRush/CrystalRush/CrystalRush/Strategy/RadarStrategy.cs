using Common.StandardTypeExtensions;
using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.Strategy
{
    //Places radar on the map and only digs known ores
    public class RadarStrategy : IRobotStrategy
    {
        //List of radar placements that will "light up" the entire grid, Grid is 30/15 and Radars illuminate 4 spaces in all direction
        //These could be optimized better, some where added at the end so this strategy would last the entire 200 rounds.
        //Could do this iterativly probably, using a base set
        private List<Point> RadarStations = new List<Point>()
        {
            new Point(8,8),
            new Point(4,3),
            new Point(4,12),
            new Point(12,3),
            new Point(12,12),
            new Point(17,8),
            new Point(21,12),
            new Point(21,3),
            new Point(25,8),
            new Point(28,3),
            new Point(28,12),
            //Extras added to consume time
            new Point(17,0),
            new Point(17,14),
            new Point(15,5),
            new Point(1,8),
            new Point(8,0),
            new Point(25,0),
            //Original Stations, shifted by 1. This helps if our originals have a hole
            new Point(9,8),
            new Point(5,3),
            new Point(5,12),
            new Point(13,3),
            new Point(13,12),
            new Point(18,8),
            new Point(22,12),
            new Point(22,3),
            new Point(24,8),
            new Point(29,3),
            new Point(29,12),
            new Point(9,10),
            new Point(5,4),
            new Point(5,13),
            new Point(13,4),
            new Point(13,13),
            new Point(18,9),
            new Point(22,13),
            new Point(22,4),
            new Point(24,9),
            new Point(29,4),
            new Point(29,13),
        };

        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find a station to place the Radar, dont place in holes or existing stations
            Point station = RadarStations.First(s => map.ItemAt(s).IsRadar == false && map.ItemAt(s).IsHole == false);

            //Some ore to bring back to HQ
            var nearestOre = map.FindNearest(cell => cell.Item.Ore > 0 && cell.Item.MyTrap == false, robot.Position);

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            //If we have a radar, go place it
            else if(robot.ItemHeld == CrystalRushItemType.Radar) 
            {
                action = $"DIG {station.X} { station.Y} BURY:RADAR";

                if(robot.Position.Equals(station))
                {
                    map.TileAt(station).Item.IsRadar = true;
                }
            }
            //If at HQ grab a radar
            else if(robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST RADAR GET:RADAR";
            }
            //Dig any ore if available
            else if(nearestOre != null)
            {
                action = $"DIG {nearestOre.Position.X} {nearestOre.Position.Y} DIG:ORE";
            }
            //Go to HQ for another radar
            else if(nearestOre == null)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }

            return action;
        }
    }
}
