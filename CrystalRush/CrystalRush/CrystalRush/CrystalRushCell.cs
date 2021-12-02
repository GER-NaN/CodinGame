using System;

namespace CrystalRush
{
    public class CrystalRushCell
    {
        /// <summary>Ore Count for the Cell</summary>
        public int Ore = 0;

        /// <summary>Does the cell have a hole</summary>
        public bool IsHole = false;

        /// <summary>Is one of my radars on this hole</summary>
        public bool IsRadar = false;

        /// <summary>Is one of my traps on this hole</summary>
        public bool IsTrap = false;

        /// <summary>Should this Cell be avoided</summary>
        public bool Avoid = false;

        /// <summary>How many of my Robots are targeting this cell for their next dig</summary>
        public int BotsAssignedToDig = 0;

        /// <summary>Is the Cell within reach of a radar; can a radar see it</summary>
        public bool RadarCoverage = false;

        /// <summary>A score given to the Cell based on known ores surrounding</summary>
        public int ClusterDensityScore = 0;

        /// <summary> Creates a cell using standard input parameters </summary>
        /// <param name="ore">Input from CG for Ore. ? for unknown or the number of ores</param>
        /// <param name="hole">1 for a hole and 0 for no hole</param>
        public CrystalRushCell(string ore, int hole)
        {
            Ore = (ore == "?") ? 0 : int.Parse(ore);
            IsHole = Convert.ToBoolean(hole);
        }

        /// <summary>Checks if ore is available to collect based on game rules</summary>
        /// <param name="holesAreSafe">Should holes be considered safe to dig on</param>
        /// <returns></returns>
        public bool SafeOreAvailable(bool holesAreSafe = false)
        {
            bool available = true;

            //Explcitly not available
            if (IsTrap || Avoid || Ore == 0 || BotsAssignedToDig >= Ore)
            {
                available = false;
            }
            else if(!holesAreSafe && IsHole)
            {
                available = false;
            }
            return available;
        }

        /// <summary>Checks if the cell is safe to dig on</summary>
        /// <returns></returns>
        public bool SafeToDig()
        {
            bool available = true;

            //Explcitly not available
            if (IsTrap || Avoid || IsHole)
            {
                available = false;
            }
            return available;
        }
    }
}
