using Common.StandardExtensions;
using Common.TileMap;
using CrystalRush.BotStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.GameStrategy
{
    public class StarterStrategy
    {
        private readonly TileMap<CrystalRushCell> Map;
        private readonly List<Robot> Bots;
        private readonly int Round;
        private readonly int MyScore;
        private readonly int OpponentScore;

        public StarterStrategy(TileMap<CrystalRushCell> map, List<Robot> bots, int roundNumber,int myScore, int oppScore)
        {
            Map = map;
            Bots = bots;
            Round = roundNumber;
            MyScore = myScore;
            OpponentScore = oppScore;
        }

        public void RunStrategy()
        {
            var winning = (MyScore > OpponentScore);
            var preferSafeOre = winning && Round > 100;


            //If we need a trapper, grab one of the diggers  
            var startTrappingAt = 50;
            var stopTrappingAt = 100;
            var needTrapper = Round.IsBetween(startTrappingAt, stopTrappingAt);

            if (needTrapper && !Bots.Any(r => r.Strategy is TrapOreStrategy))
            {
                var robot = Bots.Where(r => r.Strategy is DigOreStrategy || r.Strategy is NoStrategy).First();
                robot.Strategy = new TrapOreStrategy();
            }
            else if (!needTrapper && Bots.Any(r => r.Strategy is TrapOreStrategy))//reset any trappers because we dont need them
            {
                foreach (var bot in Bots.Where(r => r.Strategy is TrapOreStrategy))
                {
                    bot.Strategy = new DigOreStrategy(preferSafeOre);
                }
            }


            //If we need radars, grab one of the diggers
            var radarOreLimit = 20;
            var needRadar = Map.FindAll(cell => cell.Item.Ore > 0 && !cell.Item.Avoid).Sum(cell => cell.Item.Ore) < radarOreLimit;

            if (needRadar && !Bots.Any(r => r.Strategy is RadarClusterStrategy))
            {
                var robot = Bots.Where(r => r.Strategy is DigOreStrategy || r.Strategy is NoStrategy).First();
                robot.Strategy = new RadarClusterStrategy();
            }
            else if (!needRadar && Bots.Any(r => r.Strategy is RadarClusterStrategy))//reset any radar because we dont need them
            {
                foreach (var bot in Bots.Where(r => r.Strategy is RadarClusterStrategy))
                {
                    bot.Strategy = new DigOreStrategy(preferSafeOre);
                }
            }


            //Fix anyone without a strategy and reset (preferSafeOre) on diggers
            foreach (var bot in Bots.Where(r => r.Strategy is NoStrategy || r.Strategy is DigOreStrategy))
            {
                bot.Strategy = new DigOreStrategy(preferSafeOre);
            }


            foreach (var bot in Bots)
            {
                Console.WriteLine(bot.Strategy.GetMove(Map, bot));
            }
        }
    }
}
