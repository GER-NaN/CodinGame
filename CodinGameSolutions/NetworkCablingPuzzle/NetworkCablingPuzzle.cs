﻿using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Common.Core;
using System.Drawing;
using Common.StandardExtensions;
using System.Data.Common;

//https://www.codingame.com/ide/puzzle/network-cabling
/*
 * The only path optimization needed is where the main pipe goes from left to right, this is the Y median
 * Then we calculate the total X distance traveled, Buildings.MAX(X) - Buildings.MIN(X)
 * Then we calculate the distance from the median to each building along the X axis
 */
class Solution
{
    static void Main(string[] args)
    {
        long buildingCount = Input.ReadInt();
        List<Point> buildings = new List<Point>();

        for (int i = 0; i < buildingCount; i++)
        {
            var coords = Input.ReadPair<int, int>();
            buildings.Add(new Point(coords.Item1, coords.Item2));
        }

        //Get the median of the houses relative in the Y-axis. This is our main pipe location
        long median = buildings.Select(b => (long)b.Y).Median();

        //Order by X as thats how we'll process them
        buildings = buildings.OrderBy(b => b.X).ToList();

        //In all cases the Main Line has to trave from MIN(Building.x) to MAX(Building.x)
        int left = buildings.Min(b => b.X);
        int right = buildings.Max(b => b.X);
        long cableLength = Math.Abs(right - left);

        foreach(Point building in buildings)
        {
            //Find the segment legnth to the building from the main line y
            cableLength += Math.Abs(median - building.Y);
        }

        Console.WriteLine(cableLength);
        Console.ReadLine();
    }
}
/*
An internet operator plans to connect a business park to the optical fiber network. The area 
to be covered is large and the operator is asking you to write a program that will calculate 
the minimum length of optical fiber cable required to connect all buildings.
 
Rules
For the implementation of the works, the operator has technical constraints whereby it is 
forced to proceed in the following manner:
A main cable will cross through the park from the West to the East (from the position x of 
the most westerly building to the position x of the most easterly building).

For each building, a dedicated cable will connect from the building to the main cable by a 
minimal path (North or South), as shown in the following example:
 
In this example, the green lines represent the cables. 
The minimum length will therefore depend on the position of the main cable.

Game Input

Line 1: The number N of buildings that need to be connected to the optical fiber network

On the N following lines: The coordinates x and y of the buildings

Output
The minimum length L of cable required to connect all of the buildings. In other words, the length 
of the main cable plus the length of the cables dedicated to all the buildings.

Note: the buildings with the same position x should not in any case share the same dedicated cable.
Constraints
0 < N ≤ 100000
0 ≤ L ≤ 263
-230 ≤ x ≤ 230
-230 ≤ y ≤ 230

*/
