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
    /// <summary> A DIG ore strategy where it prefers safe ore</summary>
    public class DigOreStrategy : IRobotStrategy
    {
        /// <summary>
        /// When doing an alternative search, the limit on where they should start searching. This can be used to avoid the HQ area which is often empty.
        /// </summary>
        private int AlternativeDigXLimit = 5;

        /// <summary>Should the BOT prefer safe ore (un-dug). If false, the bot will pick the closest ore to grab</summary>
        private readonly bool PreferSafeOre;

        public DigOreStrategy(bool preferSafeOre)
        {
            PreferSafeOre = preferSafeOre;
        }

        //TODO: figure out safe ore logic
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Safe ore does not have a hole on it
            var safeOre = map.FindNearest(tile => tile.Item.SafeOreAvailable(false), robot.Position);
            var safeOreDistance = safeOre == null ? double.MaxValue : safeOre.Position.DistanceTo(robot.Position);

            //Find the closest ore 
            var unsafeOre = map.FindNearest(tile => tile.Item.SafeOreAvailable(true), robot.Position);
            var unsafeOreDistance = unsafeOre == null ? double.MaxValue : unsafeOre.Position.DistanceTo(robot.Position);

            //Do a random dig because we cant see any ore
            var alternative = map.FindNearest(tile => tile.Position.X > AlternativeDigXLimit && !tile.Item.RadarCoverage && tile.Item.SafeToDig(), robot.Position);

            DebugTool.Print("DIG - Begin If");
            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} d:hq";
            }
            else if (PreferSafeOre && safeOre != null)
            {
                action = $"DIG {safeOre.Position.X} {safeOre.Position.Y} d:s";
                safeOre.Item.BotsAssignedToDig += 1;
            }
            else if (unsafeOre != null)
            {
                action = $"DIG {unsafeOre.Position.X} {unsafeOre.Position.Y} d:u";
                unsafeOre.Item.BotsAssignedToDig += 1;
            }
            else if(alternative != null)
            {
                action = $"DIG {alternative.Position.X} {alternative.Position.Y} d:a";
                alternative.Item.BotsAssignedToDig += 1;
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
