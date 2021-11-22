using Common.StandardTypeExtensions;
using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.Strategy
{
    public class DigOreStrategy : IRobotStrategy
    {
        /// <summary>
        /// When doing an alternative search, the limit on where they should start searching. This can be used to avoid the HQ area which is often empty.
        /// </summary>
        private int AlternativeDigXLimit = 5;

        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Safe ore does not have a hole on it
            var safeOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.MyTrap == false && tile.Item.IsHole == false, robot.Position);

            //Find the closest ore 
            var unsafeOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.MyTrap == false, robot.Position);

            //Find an un-dug hole in my area
            var alternative = map.FindNearest(tile => tile.Item.IsHole == false && tile.Position.X > AlternativeDigXLimit && tile.Item.MyTrap == false, robot.Position);

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            else if(safeOre != null)
            {
                action = $"DIG {safeOre.Position.X} {safeOre.Position.Y} DIG:ORE-s";
            }
            //If we find ore dig it
            else if (unsafeOre != null)
            {
                action = $"DIG {unsafeOre.Position.X} {unsafeOre.Position.Y} DIG:ORE-u";
            }
            //If we cant find ore, try digging a non-hole
            else if (safeOre == null && unsafeOre == null && alternative != null)
            {
                action = $"DIG {alternative.Position.X} {alternative.Position.Y} DIG:ALT";
            }

            return action;
        }
    }
}



//public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
//{
//    var action = "WAIT";

//    //Find the closest ore 
//    var nearestOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.MyTrap == false, robot.Position);

//    //Find an un-dug hole in my area
//    var alternative = map.FindNearest(tile => tile.Item.IsHole == false && tile.Position.X > 0 && tile.Item.MyTrap == false, robot.Position);


//    //If we have ore, go to HQ
//    if (robot.ItemHeld == CrystalRushItemType.Ore)
//    {
//        action = $"MOVE 0 {robot.Position.Y} GO:HQ";
//    }
//    //If we find ore and can reach it, dig
//    //An un-specified rule seems to be in place, if you're in the HQ you cannot DIG on adjacent cells (x=1)
//    else if (nearestOre != null && robot.Position.AdjacentTo(nearestOre.Position, true))
//    {
//        action = $"DIG {nearestOre.Position.X} {nearestOre.Position.Y} DIG:ORE";
//    }
//    //If we find ore and we cant reach it go there.
//    else if (nearestOre != null && (!robot.Position.AdjacentTo(nearestOre.Position)))
//    {
//        action = $"MOVE {nearestOre.Position.X} {nearestOre.Position.Y} SEARCH:ORE";
//    }
//    //If we cant find ore, try digging a non-hole
//    else if (nearestOre == null && robot.Position.AdjacentTo(alternative.Position, true))
//    {
//        action = $"DIG {alternative.Position.X} {alternative.Position.Y} DIG:ALT";
//    }
//    //If we cant dig the non-hole, go there
//    else if (nearestOre == null && (!robot.Position.AdjacentTo(alternative.Position, true)))
//    {
//        action = $"MOVE {alternative.Position.X} {alternative.Position.Y} SEARCH:Alt";
//    }

//    return action;
//}
