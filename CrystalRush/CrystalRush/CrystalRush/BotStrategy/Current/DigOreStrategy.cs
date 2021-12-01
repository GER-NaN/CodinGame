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
            var safeOreDistance = safeOre == null ? double.MaxValue : safeOre.Position.ManhattenDistanceTo(robot.Position);

            //Find the closest ore 
            var unsafeOre = map.FindNearest(tile => tile.Item.SafeOreAvailable(true), robot.Position);
            var unsafeOreDistance = unsafeOre == null ? double.MaxValue : unsafeOre.Position.ManhattenDistanceTo(robot.Position);

            //Do a random dig because we cant see any ore
            var alternative = map.FindNearest(tile => tile.Position.X > AlternativeDigXLimit && !tile.Item.RadarCoverage && tile.Item.SafeToDig(), robot.Position);

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