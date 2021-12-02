
using System.Drawing;

namespace CrystalRush
{
    public class CrystalRushItem
    {
        public readonly int Id;
        public readonly CrystalRushItemType Type;
        public Point Position;

        public CrystalRushItem(int id, CrystalRushItemType type, Point position )
        {
            Id = id;
            Type = type;
            Position = position;
        }

        public void SetPosition(Point p)
        {
            Position.X = p.X;
            Position.Y = p.Y;
        }
    }
}
