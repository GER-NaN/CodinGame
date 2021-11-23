using Common.Core;
using Common.StandardTypeExtensions;
using Common.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRush
{
    /// <summary>Analyzes the map each round and tries to determine where Traps might have been laid</summary>
    public class TrapDetector : IEnumerable<Point>
    {
        private List<Point> Traps = new List<Point>();
        private Dictionary<int, Point> BotLastPosition = new Dictionary<int, Point>();
        private HashSet<int> BotsCurrentlyTracking = new HashSet<int>();
        private HashSet<Point> ExistingHoles = new HashSet<Point>();

        public TrapDetector(List<Robot> enemyBots)
        {
            //Setup initial positions
            foreach(var bot in enemyBots)
            {
                BotLastPosition.Add(bot.Id, bot.Position);
            }
        }

        //TODO: Deal with Bots placing items at x=1 while in HQ
        //TODO: Try to weed out Radars placements (placing an item on empty site may indicate a radar and not a trap)
        //TODO: This is currently very conservative and marks a lot of false positives as traps
        public void RunAnalysis(TileMap<CrystalRushCell> map, List<Robot> enemyBots)
        {
            foreach(var bot in enemyBots.Where(b => !b.IsDead()))
            {
                var lastPosition = BotLastPosition[bot.Id];
                var botPaused = lastPosition.Equals(bot.Position);
                var pickedUp = botPaused && bot.AtHeadquarters();
                var placedItem = botPaused && !pickedUp;

                if (pickedUp)
                {
                    BotsCurrentlyTracking.Add(bot.Id);
                }
                else if (placedItem && BotsCurrentlyTracking.Contains(bot.Id))
                {
                    //Bots dont always place an item on their position, they can place items adjacent (up/down/left,right) as well.
                    var newHoles = map.Where(cell => cell.Item.IsHole && !ExistingHoles.Contains(cell.Position));

                    //If there is a new hole within 1 distance of the bot, consider that the trap
                    if(newHoles.Any(newHole => newHole.Position.DistanceTo(bot.Position) <= 1))
                    {
                        Traps.Add(newHoles.First(newHole => newHole.Position.DistanceTo(bot.Position) <= 1).Position);
                    }
                    else
                    {
                        var adjacentHoles = map.GetNeighbors(bot.Position, 1,tile => tile.Item.IsHole);
                        foreach(var hole in adjacentHoles)
                        {
                            Traps.Add(hole.Position);
                        }

                        //Check the bots position
                        if(map.TileAt(bot.Position).Item.IsHole)
                        {
                            Traps.Add(bot.Position);
                        }
                    }

                    //Remove the bot from our tracking list
                    BotsCurrentlyTracking.Remove(bot.Id);
                }
            }

            //Remember positions
            foreach (var bot in enemyBots)
            {
                BotLastPosition[bot.Id] = bot.Position;
            }

            //Update hole locations
            foreach(var tile in map)
            {
                if(tile.Item.IsHole)
                {
                    ExistingHoles.Add(tile.Position);
                }
            }
        }

        public bool IsTrap(Point pointToTest)
        {
            return Traps.Any(p => p.Equals(pointToTest));
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return Traps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
