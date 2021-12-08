using Common.StandardTypeExtensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://www.codingame.com/ide/puzzle/code-vs-zombies
namespace CodeVsZombies
{
    class Program
    {
        static Random random = new Random(Guid.NewGuid().GetHashCode());
        static void Main(string[] args)
        {
            string[] inputs;

            // game loop
            while (true)
            {

                inputs = Console.ReadLine().Split(' ');
                int x = int.Parse(inputs[0]);
                int y = int.Parse(inputs[1]);
                Point me = new Point(x, y);


                List<Point> humans = new List<Point>();
                int humanCount = int.Parse(Console.ReadLine());
                for (int i = 0; i < humanCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int humanId = int.Parse(inputs[0]);
                    int humanX = int.Parse(inputs[1]);
                    int humanY = int.Parse(inputs[2]);
                    humans.Add(new Point(humanX, humanY));
                }

                List<Point> zombies = new List<Point>();
                int zombieCount = int.Parse(Console.ReadLine());
                for (int i = 0; i < zombieCount; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int zombieId = int.Parse(inputs[0]);
                    int zombieX = int.Parse(inputs[1]);
                    int zombieY = int.Parse(inputs[2]);
                    int zombieXNext = int.Parse(inputs[3]);
                    int zombieYNext = int.Parse(inputs[4]);
                    zombies.Add(new Point(zombieX, zombieY));
                }

                var zombieToKill = zombies.OrderBy(z => z.DistanceTo(me)).First();

                Console.WriteLine(zombieToKill.X + " " + zombieToKill.Y);
            }
        }
    }
}
