using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StandardTypeExtensions
{
    public static class Array2DExtensions
    {
        /// <summary>Checks if the element exists in the 2D Array</summary>
        /// <typeparam name="T">The type of the element. This must support the Equals method for comparison</typeparam>
        /// <param name="array">The array to search through</param>
        /// <param name="element">The element to find</param>
        /// <returns>True if the element exists in the array</returns>
        public static bool ElementExists<T>(this T[,] array, T element)
        {
            for(int i=0;i<array.GetLength(0);i++)
            {
                for(int j=0;j<array.GetLength(1);j++)
                {
                    if(array[i,j].Equals(element))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>Looks for the postion of the element in the 2D Array. If not found the positions will be flagged with -1</summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="array">The array to search through</param>
        /// <param name="element">The element to find</param>
        /// <returns>A Tuple with the row and column index</returns>
        public static Tuple<int, int> PositionOf<T>(this T[,] array, T element)
        {
            for (int i = 0; i< array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j].Equals(element))
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }

            return new Tuple<int, int>(-1, -1);
        }
    }
}
