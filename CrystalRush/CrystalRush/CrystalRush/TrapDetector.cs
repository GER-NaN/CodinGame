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
                    DebugTool.Print(bot.Id, "picked up", " ");
                }
                else if (placedItem && BotsCurrentlyTracking.Contains(bot.Id))
                {

                    //New holes are highly suspect (may be useful later for fine tuning)
                    var newHoles = map.FindAll(cell => cell.Item.IsHole && cell.Position.ManhattenDistanceTo(bot.Position) <= 1 && !ExistingHoles.Contains(cell.Position));
                    foreach(var cell in newHoles)
                    {
                        Traps.Add(cell.Position);
                        DebugTool.Print("Trap @", cell.Position, " ");
                    }

                    //Also mark adjacent holes
                    //var adjacentHoles = map.GetNeighbors(bot.Position, 1,tile => tile.Item.IsHole);
                    var adjacentHoles = map.FindAll(cell => cell.Item.IsHole && cell.Position.ManhattenDistanceTo(bot.Position) <= 1);
                    foreach(var hole in adjacentHoles)
                    {
                        Traps.Add(hole.Position);
                        DebugTool.Print("Trap @", hole.Position, " ");
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
            var holesToAdd = map.FindAll(t => t.Item.IsHole);

            foreach(var hole in holesToAdd)
            {
                ExistingHoles.Add(hole.Position);
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
