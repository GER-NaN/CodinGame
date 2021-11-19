using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    public class CrystalRushCell
    {
        public readonly bool IsKnown;
        public readonly int Ore;
        public readonly bool IsHole;
        public readonly List<CrystalRushItem> Items;

        public CrystalRushCell(bool known, int ore, bool hole)
        {
            IsKnown = known;
            Ore = ore;
            IsHole = hole;
        }

        public CrystalRushCell(string ore, int hole)
        {
            IsKnown = !(ore == "?");
            Ore = IsKnown ? int.Parse(ore) : 0;
            IsHole = Convert.ToBoolean(hole);
        }

        public void AddItemToCell(CrystalRushItem item)
        {
            Items.Add(item);
        }
    }
}
