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
        /// <returns>True if any two tiles appear adjacent to eachother anywhere in the path more than loopCounter times</returns>
        /// <remarks>
        /// This is a flawed algorithm but it works for simple cases and our Bender1 puzzle.
        /// 
        /// For example, consider the path below. If we look for Tiles [A] [B] repeating, we will find two instances of them. The
        /// function would return as "looping = true" when in fact the path continues on without loopoing.
        /// 
        /// [A] [B] [C] [D] [Z] [A] [G] [T] [A] [B] [Z] [V] [V] [A] [B] [T] [Q] [R]...
        ///  ^   ^                           ^   ^               ^   ^
        /// T1  T2                         First Repeat       Second Repeat 
        /// </remarks>
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
