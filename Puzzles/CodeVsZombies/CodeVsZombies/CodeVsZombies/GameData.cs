using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeVsZombies
{
    public class GameData
    {
        public readonly Point Player;
        public readonly List<Point> Humans;
        public readonly List<Point> Zombies;

        public GameData(Point player, List<Point> humans, List<Point> zombies)
        {
            Player = player;
            Humans = humans;
            Zombies = zombies;
        }
    }
}
