using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    public class CrystalRushCell
    {
        public bool IsKnown;
        public int Ore;
        public bool IsHole;
        public bool IsRadar;

        public CrystalRushCell(bool known, int ore, bool hole, bool radar)
        {
            IsKnown = known;
            Ore = ore;
            IsHole = hole;
            IsRadar = radar;
        }

        public CrystalRushCell(string ore, int hole)
        {
            IsKnown = !(ore == "?");
            Ore = IsKnown ? int.Parse(ore) : 0;
            IsHole = Convert.ToBoolean(hole);
            IsRadar = false;
        }
    }
}
