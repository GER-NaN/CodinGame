using CrystalRush.BotStrategy;
using System.Drawing;

namespace CrystalRush 
{
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
