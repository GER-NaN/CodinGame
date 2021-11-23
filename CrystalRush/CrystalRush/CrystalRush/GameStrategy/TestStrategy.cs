using Common.TileMap;
using CrystalRush.BotStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.GameStrategy
{
    public class TestStrategy
    {
        private readonly TileMap<CrystalRushCell> Map;
        private readonly List<Robot> Bots;
        private readonly int Round;

        public TestStrategy(TileMap<CrystalRushCell> map, List<Robot> bots, int roundNumber)
        {
            Map = map;
            Bots = bots;
            Round = roundNumber;
        }

        public void RunSingleStrategy(IRobotStrategy strategy)
        {
            //If we dont have a bot with the strategy, make one
            var stratType = strategy.GetType();

            if (!Bots.Any(b => b.Strategy.GetType() == stratType))
            {
                Bots.First(b => !b.IsDead()).Strategy = strategy;
            }

            //Make all the other bots do nothing
            foreach (var bot in Bots.Where(b => !b.IsDead() && b.Strategy.GetType() != stratType))
            {
                bot.Strategy = new NoStrategy();
            }


            foreach (var bot in Bots)
            {
                Console.WriteLine(bot.Strategy.GetMove(Map, bot));
            }
        }
    }
}
