using Common.StandardTypeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsZombies.Strategies
{
    //Prevents the loosing condition of every human dying. This is done by sitting at 1 human and waiting (doesnt work in cases that require prediction)
    //Score: 31740 - 95%
    public class StrategySaveOneHuman : IStrategy
    {
        public string GetMove(GameData gameData)
        {
            var closestHuman = gameData.Humans.OrderBy(h => h.DistanceTo(gameData.Player)).First();

            return closestHuman.X + " " + closestHuman.Y;
        }
    }
}
