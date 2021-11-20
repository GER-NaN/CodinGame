using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Core;
using Common.TileMap;

namespace CrystalRush
{
    //Searches an area and digs all Tiles that are not a hole
    public class StrategySearchArea : IRobotStrategy
    {
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find the closest tile in my area 
            var destination = map.FindNearest(tile => tile.Item.IsHole == false && tile.Point.X > 6 && tile.Point.Y >= robot.YStart && tile.Point.Y < robot.YStart + 3, robot.Position);

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            //If we dont have any tiles in our area, go somehwere random
            else if (destination == null)
            {
                destination = map.Find(tile => tile.Item.IsHole == false && tile.Point.X > 0);
                action = $"MOVE {destination.Point.X} {destination.Point.Y} SEARCH:Rand";
            }
            //If we are not at our destiation, go there
            else if (robot.Position != destination.Point)
            {
                action = $"MOVE {destination.Point.X} {destination.Point.Y} SEARCH:Area";
            }
            //We're at our destination, Dig it
            else
            {
                action = $"DIG {robot.Position.X} {robot.Position.Y} DIG";
            }

            return action;
        }
    }
}
