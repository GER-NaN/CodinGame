using Common.TileMap;
using CrystalRush.BotStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush.GameStrategy
{
    public class TwoBotStrategy
    {
        private readonly TileMap<CrystalRushCell> Map;
        private readonly List<Robot> Bots;
        private readonly int Round;

        public TwoBotStrategy(TileMap<CrystalRushCell> map, List<Robot> bots, int roundNumber)
        {
            Map = map;
            Bots = bots;
            Round = roundNumber;
        }

        public void RunStrategy()
        {
            //If we need radars, assign it to someone
            var radarOreLimit = 20;
            var oreIsLow = Map.FindAll(cell => cell.Item.Ore > 0).Sum(cell => cell.Item.Ore) < radarOreLimit;

            if (oreIsLow && !Bots.Any(r => r.Strategy is RadarClusterStrategy))
            {
                var robot = Bots.First();
                robot.Strategy = new RadarClusterStrategy(Map);
            }
            //Ore is not low, everyone should be digging
            else if (!oreIsLow)
            {
                foreach (var bot in Bots.Where(b => !b.IsDead()))
                {
                    bot.Strategy = new DigOreStrategy(false);
                }
            }

            foreach (var bot in Bots)
            {
                Console.WriteLine(bot.Strategy.GetMove(Map, bot));
            }
        }
    }
}
