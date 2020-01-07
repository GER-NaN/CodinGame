using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://www.codingame.com/ide/puzzle/1d-bush-fire
namespace BushFire1D
{

    public class Cell
    {
        public readonly static string Fire = "f";
        public readonly static string Empty = ".";
    }

    class Solution
    {
        static void Main(string[] args)
        {
            int testCases = int.Parse(Console.ReadLine());
            List<string> tests = new List<string>();

            for (int i = 0; i < testCases; i++)
            {
                tests.Add(Console.ReadLine());
            }

            foreach(var test in tests)
            {
                //* Yea this works.
                //Iterate through the array until the end
                //when you find a fire n,
                //  if fire in n + 2
                //    x = n + 1
                //  else
                //    x = n
                //put water on x
                //continue from x + 2

                string[] fires = new string[test.Length];

                for(int i=0;i<test.Length;i++)
                {
                    fires[i] = test[i].ToString();
                }

                var length = fires.Length;
                var n = 0;
                var x = 0;
                var dumps = 0;

                Console.Error.WriteLine(fires);
                while (n < length)
                {
                    if(fires[n] == Cell.Fire)//when you find a fire n,
                    {
                        Console.Error.WriteLine("Found Fire");
                        x = n;
                        if(n + 2 < length && fires[n+2] == Cell.Fire)// if fire in n + 2
                        {
                            x = n + 1;
                        }

                        //put water on x
                        dumps++;//We dont need to track actual water or which fires are put out. 

                        //continue from x + 2
                        n = x + 2;
                    }
                    else
                    {
                        n++;
                    }
                }

                Console.WriteLine(dumps);
            }
        }
    }
}

/*
 * 
 * 
 * 
 * 

s s s
  x
|_|f|_|_|_|_|_|_
1 2 3 4 5 6 7

  s s s
    x 
|_|f|_|f|_|_|_|_
1 2 3 4 5 6 7

s s s s s s
  x     x
|_|f|_|_|f|_|_|_
1 2 3 4 5 6 7

s s s   s s s
  x     x   x
|_|f|_|_|f|_|f|_
1 2 3 4 5 6 7



|_|_|_|_|_|_|_|_
1 2 3 4 5 6 7



**NOT OPTIMAL
  s s s s s s s s
    x     x   x
|_|_|f|_|f|f|f|f|_|_|_|_|_|_|_|_
1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6

** OPTIMAL
    s s s s s s   
      x     x    
|_|_|f|_|f|f|f|f|_|_|_|_|_|_|_|_
1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6


Steps:
Iterate through the array
when you find a fire n, 
if fire in n + 2
    put water in n + 1
else
    put water in n






     s s s
       x
|_|_|f|f|f|_|_|_
 1 2 3 4 5 6 7

     s s s s
       x x         
|_|_|f|_|f|f|_|_
 1 2 3 4 5 6 7

   s s s     s s s
     x         x
|_|_|f|_|_|_|_|f|_
 1 2 3 4 5 6 7 8


     s s s   s s s
       x       x
|_|_|f|_|f|_|_|f|_
 1 2 3 4 5 6 7 8



 */
