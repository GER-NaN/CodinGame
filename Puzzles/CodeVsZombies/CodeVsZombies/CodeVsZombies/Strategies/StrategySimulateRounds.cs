using Common.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsZombies.Strategies
{
    /// <summary>Simulates rounds with many random moves and returns the best scoring</summary>
    //Completely broken, doesnt work
    public class StrategySimulateRounds : IStrategy
    {
        private readonly Random Random = new Random();

        public string GetMove(GameData gameData)
        {
            var bestScore = 0;
            var bestMove = gameData.Player;
            var movesToSimulate = 100;

            //Outer loop controls how many simulations we want to run to try finding the best
            for (int i = 0; i < 100; i++)
            {
                var position = gameData.Player;
                var sim = new RoundSimulator();
                var simulatedMoves = new Point[movesToSimulate];
                var simulatedScores = new int[movesToSimulate];

                for (int m = 0; m < movesToSimulate; m++)
                {
                    position = GetRandomPointWithinRadius(position, 1000);
                    simulatedMoves[m] = position;
                    simulatedScores[m] = sim.SimulateRound(gameData, position);
                }

                //Check if any of the moves loses the game
                if(simulatedScores.Any(s => s == int.MinValue))
                {
                    continue;
                }

                //See if its better then our best
                if (simulatedScores.Sum() > bestScore)
                {
                    bestScore = simulatedScores.Sum();
                    bestMove = simulatedMoves[0];
                }
            }

            return bestMove.X + " " + bestMove.Y;
        }

        //https://stackoverflow.com/a/50746409
        //TODO: Move this into Common
        private Point GetRandomPointWithinRadius(Point center, int radius)
        {
            var r = radius * Math.Sqrt(Random.NextDouble());
            var theta = Random.NextDouble() * 2 * Math.PI;

            var x = center.X + r * Math.Cos(theta);
            var y = center.Y + r * Math.Sin(theta);

            //We need positions in our playing field
            x = Math.Min(Math.Max(0, x), 16000);
            y = Math.Min(Math.Max(0, y), 9000);

            return new Point((int)x, (int)y);
        }
    }
}
