using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TileMap
{
    public class TileMapPath<T>
    {
        private List<TileMapPathStep<T>> Steps;
        private int StepNumber = 0;

        public TileMapPath()
        {
            Steps = new List<TileMapPathStep<T>>();
        }

        public void AddToPath(Tile<T> tile)
        {
            StepNumber++;
            Steps.Add(new TileMapPathStep<T>(StepNumber, tile)); 
        }

        /// <summary> Poor Mans loop checker. If we have passed the same 2 tiles more than x times, consider us looping. </summary>
        /// <param name="loopCounter">How many times to count passing over the same two tiles before we consider us looping.</param>
        /// <returns></returns>
        public bool ContainsLoop(int loopCounter)
        {
            var loopFound = false;

            for (int i = 0; i < Steps.Count - 2; i += 2)
            {
                var tile1 = Steps[i];
                var tile2 = Steps[i + 1];

                //Check if tile1 and tile 2 repeat more than loopCounter times
                var loopCount = 0;
                for (int j = i + 2; j < Steps.Count - 2; j+=2)
                {
                    if (tile1.Tile.Equals(Steps[j].Tile) &&
                        tile2.Tile.Equals(Steps[j + 1].Tile))
                    {
                        loopCount++;
                    }

                    if (loopCount >= loopCounter)
                    {
                        loopFound = true;
                        break;
                    }

                }

                if(loopFound)
                {
                    break;
                }
            }

            return loopFound;

        }
    }
}
