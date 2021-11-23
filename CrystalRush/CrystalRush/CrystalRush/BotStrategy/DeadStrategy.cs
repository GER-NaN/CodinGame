using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.TileMap;

namespace CrystalRush.BotStrategy
{
    public class DeadStrategy : IRobotStrategy
    {
        public string GetMove(TileMap<CrystalRushCell> map, Robot robot)
        {
            return "WAIT Im' dead";
        }
    }
}
