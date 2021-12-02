using Common.Core;
using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.BotStrategy
{
    /// <summary>Places traps on Ore</summary>
    class TrapOreStrategy : IRobotStrategy
    {
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT broken";

            //Find ore that doesnt have a hole on it
            var freshOre = map.FindNearest(tile => tile.Item.Ore > 0 && !tile.Item.IsHole, robot.Position);

            //Find ore that does have a hole, this gets be blown up a bit
            var usedOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.IsHole && !tile.Item.IsTrap && !tile.Item.Avoid, robot.Position);

            //Empty spot near HQ
            var empty = map.FindNearest(tile => tile.Item.Ore == 0 && !tile.Item.IsHole && !tile.Item.IsTrap && tile.Position.X < 5 && tile.Position.X > 0 && !tile.Item.Avoid, robot.Position);

            //If we have a trap, place it on fresh ore
            if(robot.ItemHeld == CrystalRushItemType.Trap && freshOre != null)
            {
                action = $"DIG {freshOre.Position.X} { freshOre.Position.Y} t:s";
                map.TileAt(freshOre.Position).Item.IsTrap = true;
            }
            //If we have a trap, place on used ore
            else if (robot.ItemHeld == CrystalRushItemType.Trap && usedOre != null)
            {
                action = $"DIG {usedOre.Position.X} { usedOre.Position.Y} t:u";
                map.TileAt(usedOre.Position).Item.IsTrap = true;
            }
            //If we still have a trap, place it on an empty spot near HQ
            else if(robot.ItemHeld == CrystalRushItemType.Trap && empty != null)
            {
                action = $"DIG {empty.Position.X} { empty.Position.Y} t:hq";
                map.TileAt(empty.Position).Item.IsTrap = true;
            }
            //If we have ore, go to HQ
            else if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} t:hq";
            }
            //Request a trap at HQ
            else if (robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST TRAP t:get";
            }
            else
            {
                action = $"MOVE 0 {robot.Position.Y} t:hq";
            }

            return action;
        }
    }
}
