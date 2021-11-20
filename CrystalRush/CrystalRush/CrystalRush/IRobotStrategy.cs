using Common.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    public interface IRobotStrategy
    {
        string GetMove(TileMap<CrystalRushCell> map, Robot robot);
    }
}
