using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.Strategy
{
    public class TrapXStrategy : IRobotStrategy
    {
        private int XTrapLine = 1;

        /// <summary>Creates a trapper that sets traps along the specified x axis lne </summary>
        /// <param name="xTrapLine">The x coordinate to place a line of traps</param>
        public TrapXStrategy(int xTrapLine)
        {
            XTrapLine = xTrapLine;
        }

        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            var action = "WAIT";

            //Find the closest safe ore 
            var nearestOre = map.FindNearest(tile => tile.Item.Ore > 0 && tile.Item.IsHole == false && tile.Item.MyTrap == false, robot.Position);

            //Find somewhere to place my trap
            var placeTrap = map.FindNearest(tile => tile.Item.MyTrap == false && tile.Position.X == XTrapLine, robot.Position);

            //If we cant place traps, convert to ore gathering
            if (placeTrap == null)
            {
                var gatherOre = new DigOreStrategy();
                return gatherOre.GetMove(map, robot);
            }

            //If we have ore, go to HQ
            if (robot.ItemHeld == CrystalRushItemType.Ore)
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }
            //If we have a trap, go place it
            else if (robot.ItemHeld == CrystalRushItemType.Trap)
            {
                action = $"DIG {robot.Position.X} { robot.Position.Y} BURY:TRAP";
                map.TileAt(robot.Position).Item.MyTrap = true;
            }
            //Request a trap at HQ
            else if (robot.AtHeadquarters() && robot.ItemHeld == CrystalRushItemType.None)
            {
                action = $"REQUEST TRAP GET:TRAP";
            }
            else if (nearestOre != null)
            {
                action = $"DIG {nearestOre.Position.X} {nearestOre.Position.Y} GET:ORE";
            }
            else
            {
                action = $"MOVE 0 {robot.Position.Y} GO:HQ";
            }

            return action;
        }
    }
}
