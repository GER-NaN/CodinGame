using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class Fibonacci
    {
        public static int GetElementAt(int index)
        {
            int[] fibonacciList = new int[index + 1];

            fibonacciList[0] = 0;
            fibonacciList[1] = 1;
            for (int i = 2; i <= index; i++)
            {
                fibonacciList[i] = fibonacciList[i - 2] + fibonacciList[i - 1];
            }
            return fibonacciList[index];
        }
    }
}
