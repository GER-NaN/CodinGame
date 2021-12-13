using Common.StandardTypeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsZombies.Strategies
{
    //Kills the zombie which is closest to a human
    //Score: 42560 - 85%
    class StrategyZombieNearHuman : IStrategy
    {
        public string GetMove(GameData gameData)
        {
            //Default zombie
            var zombieToKill = gameData.Zombies.OrderBy(z => z.DistanceTo(gameData.Player)).First();

            //Find the zombie that is closest to a human
            double minDistance = double.MaxValue;

            foreach(var human in gameData.Humans)
            {
                foreach(var zombie in gameData.Zombies)
                {
                    if(zombie.DistanceTo(human) < minDistance)
                    {
                        minDistance = zombie.DistanceTo(human);
                        zombieToKill = zombie;
                    }
                }
            }

            return zombieToKill.X + " " + zombieToKill.Y;
        }
    }
}
