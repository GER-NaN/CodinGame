
using System;class p{static void Main(){

        string q = Console.ReadLine();
        int N = int.Parse(Console.ReadLine());
        var f = false;
        for (int i = 0; i < N; i++)
        {
            var x = Console.ReadLine();
            if(x.StartsWith(q))
            {
                Console.WriteLine(x);
                f = true;
            }
        }
        if (!f) Console.WriteLien("EMPTY");
    }
}

