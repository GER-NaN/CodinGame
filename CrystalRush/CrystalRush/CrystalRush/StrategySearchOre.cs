using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    public class StrategySearchOre
    {
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find the closest ore 
            var ore = map.FindNearest(tile => tile.Item.Ore > 0, robot.Position);

            //Find an un-dug hole in my area
            var alternative = map.FindNearest(tile => tile.Item.IsHole == false && tile.Point.X > 0, robot.Position);


            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            //If we find ore and were not there, go there
            else if (ore != null && robot.Position != ore.Point)
            {
                action = $"MOVE {ore.Point.X} {ore.Point.Y} SEARCH:ORE";
            }
            //If we're at the ore, dig it
            else if(ore != null && robot.Position == ore.Point)
            {
                action = $"DIG {robot.Position.X} {robot.Position.Y} DIG:ORE";
            }
            else if(ore == null && robot.Position != alternative.Point)
            {
                action = $"MOVE {alternative.Point.X} {alternative.Point.Y} SEARCH:Alt";
            }
            else if(ore == null && robot.Position == alternative.Point)
            {
                action = $"DIG {robot.Position.X} {robot.Position.Y} DIG:ALT";
            }

            return action;
        }
    }
}
