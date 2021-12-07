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
        /// <summary>Should the BOT prefer safe ore (un-dug). If false, the bot will pick the closest ore to grab</summary>
        private readonly bool PreferSafeOre;

        public DigOreStrategy(bool preferSafeOre)
        {
            PreferSafeOre = preferSafeOre;
        }

        //TODO: figure out safe ore logic
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT broken";

            //Safe ore does not have a hole on it
            var safeOre = map.FindNearest(tile => tile.Item.SafeOreAvailable(false), robot.Position);
            var safeOreDistance = safeOre == null ? double.MaxValue : safeOre.Position.ManhattenDistanceTo(robot.Position);

            //Find the closest ore 
            var unsafeOre = map.FindNearest(tile => tile.Item.SafeOreAvailable(true), robot.Position);
            var unsafeOreDistance = unsafeOre == null ? double.MaxValue : unsafeOre.Position.ManhattenDistanceTo(robot.Position);

            //Alt1 = Clean hole, not in radar, in our specified zone
            var alternative1 = map.FindNearest(tile => tile.Position.X > 0 && !tile.Item.RadarCoverage && tile.Item.SafeToDig() && tile.Position.Y >= robot.YStart && tile.Position.Y < robot.YStart + 3, robot.Position);

            //Alt2 = Clean hole, not in radar
            var alternative2 = map.FindNearest(tile => tile.Position.X > 0 && tile.Item.SafeToDig() && !tile.Item.RadarCoverage, robot.Position);

            //Alt3 = Any clean hole (last hope)
            var alternative3 = map.FindNearest(tile => tile.Position.X > 0 && tile.Item.SafeToDig(), robot.Position);



            //Fake Pickup at HQ
            var fakePickup = (new Random()).Next(1, 100) < 20; //20% chance to fake a pickup

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} d:hq";
            }
            else if(robot.AtHeadquarters() && fakePickup)
            {
                action = $"WAIT d:fp";
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
            else if(alternative1 != null)
            {
                action = $"DIG {alternative1.Position.X} {alternative1.Position.Y} d:a1";
                alternative1.Item.BotsAssignedToDig += 1;
            }
            else if (alternative2 != null)
            {
                action = $"DIG {alternative2.Position.X} {alternative2.Position.Y} d:a2";
                alternative2.Item.BotsAssignedToDig += 1;
            }
            else if (alternative3 != null)
            {
                action = $"DIG {alternative3.Position.X} {alternative3.Position.Y} d:a3";
                alternative3.Item.BotsAssignedToDig += 1;
            }

            return action;
        }
    }
}