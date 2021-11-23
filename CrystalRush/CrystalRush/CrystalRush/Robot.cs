using Common.Core;
using Common.TileMap;
using CrystalRush.BotStrategy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush 
{
    /*
        Each robot can hold 1 item in its inventory.

        A robot may:

        REQUEST an item from the headquarters.
        MOVE towards a given cell.
        DIG on a cell. This will, in order:
            Create a hole on this cell if there isn't one already.
            Bury any item the robot is holding into the hole.
            If digging on a vein cell and ore was not buried on step 2, place one unit of ore into the robot's inventory.
        WAIT to do nothing.
        Details:

        Robots may only dig on the cell they occupy or neighbouring cells. Cells have 4 neighbours: up, left, right, and down.
        Robots on any cell part of the headquarters will automatically deliver any ore it is holding.
        Robots can occupy the same cell.
        Robots cannot leave the grid.
        Robots' inventories are not visible to the opponent.
     */
    public class Robot : CrystalRushItem
    {

        public CrystalRushItemType ItemHeld;
        public Point TopLeft;
        public Point BottomRight;
        public IRobotStrategy Strategy = new NoStrategy();

        public Robot(int id, CrystalRushItemType type, Point position) : base(id,type,position)
        {

        }

        public void SetItemHeld(CrystalRushItemType item)
        {
            ItemHeld = item;
        }

        public void Update(Robot robot)
        {
            this.SetPosition(robot.Position);
            this.SetItemHeld(robot.ItemHeld);
        }

        public bool AtHeadquarters()
        {
            return this.Position.X == 0;
        }

        public bool IsDead()
        {
            return this.Position.X == -1;
        }

        public int YStart = 0;
    }
}
