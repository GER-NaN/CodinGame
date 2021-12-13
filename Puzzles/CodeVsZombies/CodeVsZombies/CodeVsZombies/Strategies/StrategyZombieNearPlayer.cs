using Common.StandardTypeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsZombies.Strategies
{
    //Kills the closest zombie
    //Score: 43770 - 80%
    class StrategyZombieNearPlayer : IStrategy
    {
        public string GetMove(GameData gameData)
        {
            var zombieToKill = gameData.Zombies.OrderBy(z => z.DistanceTo(gameData.Player)).First();

            return zombieToKill.X + " " + zombieToKill.Y;
        }
    }
}
