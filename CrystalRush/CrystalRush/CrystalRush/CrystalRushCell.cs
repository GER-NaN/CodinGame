using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    public class CrystalRushCell
    {
        //Items
        public int Ore = 0;
        public bool IsHole = false;
        public bool IsRadar = false;
        public bool IsTrap = false;
        public bool Avoid = false;

        //Strategy flags
        
        /// <summary>Is the Cell within reach of a radar; can a radar see it</summary>
        public bool RadarCoverage = false;

        /// <summary>A score given to the Cell based on known ores surrounding</summary>
        public int ClusterDensityScore = 0;

        /// <summary> Creates a cell using standard input parameters </summary>
        /// <param name="ore">Input from CG for Ore. ? for unknown or the number of ores</param>
        /// <param name="hole">1 for a hole and 0 for no hole</param>
        public CrystalRushCell(string ore, int hole)
        {
            Ore = (ore == "?") ?  0 : int.Parse(ore);
            IsHole = Convert.ToBoolean(hole);
        }
    }
}
