using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    public class CrystalRushItem
    {
        public readonly int Id;
        public readonly CrystalRushItemType Type;
        public readonly Point Position;

        public CrystalRushItem(int id, CrystalRushItemType type, Point position )
        {
            Id = id;
            Type = type;
            Position = position;
        }
    }
}
