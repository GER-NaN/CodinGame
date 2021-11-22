using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.Strategy
{
    /// <summary>Places traps on Ore</summary>
    class TrapOreStrategy : IRobotStrategy
    {
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find ore that doesnt have a hole on it
            var freshOre = map.FindNearest(tile => tile.Item.Ore > 0 && !tile.Item.IsHole, robot.Position);

            //Find ore that does have a hole
            var usedOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.IsHole && !tile.Item.MyTrap, robot.Position);

            //Empty spot near HQ
            var empty = map.FindNearest(tile => tile.Item.Ore == 0 && !tile.Item.IsHole && !tile.Item.MyTrap && tile.Position.X < 5 && tile.Position.X > 0, robot.Position);

            //If we have a trap, place it on fresh ore
            if(robot.ItemHeld == CrystalRushItemType.Trap && freshOre != null)
            {
                action = $"DIG {freshOre.Position.X} { freshOre.Position.Y} TRAP:f";
                map.TileAt(freshOre.Position).Item.MyTrap = true;
            }
            //If we have a trap, place on used ore
            else if (robot.ItemHeld == CrystalRushItemType.Trap && usedOre != null)
            {
                action = $"DIG {usedOre.Position.X} { usedOre.Position.Y} TRAP:u";
                map.TileAt(usedOre.Position).Item.MyTrap = true;
            }
            //If we still have a trap, place it on an empty spot near HQ
            else if(robot.ItemHeld == CrystalRushItemType.Trap && empty != null)
            {
                action = $"DIG {empty.Position.X} { empty.Position.Y} TRAP:hq";
                map.TileAt(empty.Position).Item.MyTrap = true;
            }
            //If we have ore, go to HQ
            else if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            //Request a trap at HQ
            else if (robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST TRAP GET:TRAP";
            }
            else
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }

            return action;
        }
    }
}
