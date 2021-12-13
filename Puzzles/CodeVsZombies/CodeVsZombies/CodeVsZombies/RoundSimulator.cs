using Common.Core;
using Common.StandardTypeExtensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsZombies
{
    /// <summary>Simulates a single round in the game</summary>
    /// <remarks>
    ///The order in which actions happens in between two rounds is:
    ///Zombies move towards their targets. (we already have this)
    ///Ash moves towards his target. (this is our target)
    ///Any zombie within a 2000 unit range around Ash is destroyed. (calculated)
    ///Zombies eat any human they share coordinates with.
    ///
    ///Scoring works as follows:
    ///A zombie is worth the number of humans still alive squared x10, not including Ash.
    ///If several zombies are destroyed during on the same round, the nth zombie killed's worth is 
    ///multiplied by the (n+2)th number of the Fibonnacci sequence (1, 2, 3, 5, 8, and so on). As a 
    ///consequence, you should kill the maximum amount of zombies during a same turn.
    /// </remarks>
    public class RoundSimulator
    {
        /// <summary>Simulates a round and returns the points scored</summary>
        /// <param name="gameData">The game data</param>
        /// <param name="target">The position the player should move to during the simulation, assume he can reach it (less than 1000 units)</param>
        public int SimulateRound(GameData gameData, Point target)
        {
            //Count zombies killed this turn, we already have their next position in Item2
            int zombiesKilled = 0;
            foreach (var zombie in gameData.Zombies)
            {
                if(target.DistanceTo(zombie.Item2) < 2000)
                {
                    zombiesKilled += 1;
                }
            }

            //Count humans that will be alive after the zombies move, consider ones that player will kill
            int humansAlive = gameData.Humans.Count;
            foreach (var zombie in gameData.Zombies)
            {
                foreach(var human in gameData.Humans)
                {
                    if(zombie.Item2.Equals(human) && target.DistanceTo(zombie.Item2) >= 2000)
                    {
                        humansAlive -= 1;
                    }
                }
            }

            //Calculate score for zombies
            var score = 0;
            for(int i=0;i<zombiesKilled;i++)
            {
                //Base score = (HumansAlive^2 * 10)
                var zombieScore = humansAlive * humansAlive * 10;

                //Calculate combo = (BaseScore) * Fibonacci[n+2]
                if(i > 0)
                {
                    //Our loop is at zero, so lets add 1
                    var zombieNumber = i + 1;

                    //Combo Fib. starts at 1 instead of 0 so we add 1 more to the indexer than the rules state
                    var fibIndexer = zombieNumber + 3;

                    zombieScore = zombieScore * Fibonacci.GetElementAt(fibIndexer);
                }

                score += zombieScore;
            }

            //We lose if no humans are alive, so return a very low value
            if(humansAlive == 0)
            {
                score = int.MinValue;
            }

            return score;
        }
    }
}
