
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//https://www.codingame.com/contests/code-a-la-mode
namespace CodeAlaMode
{
    public class Game
    {
        public Player[] Players = new Player[2];
        public Table Dishwasher;
        public Table Window;
        public Table Blueberry;
        public Table IceCream;
        public Table Strawberry;
        public Table Chopping;
        public Table Dough;
        public Table Oven;

        //When I set it down, maybe dont need this
        public Table ChoppedStrawberry;
        public Table CookedDough;
        public int CookTime;
        
        public List<Table> Tables = new List<Table>();
    }

    public class Table
    {
        public Position Position;
        public bool HasFunction;
        public Item Item;
    }

    public class Item
    {
        public string Content;
        public bool HasPlate;
        public Item(string content)
        {
            Content = content;
            HasPlate = Content.Contains(MainClass.Dish);
        }
    }

    public class Player
    {
        public Position Position;
        public Item Item;
        public Player(Position position, Item item)
        {
            Position = position;
            Item = item;
        }
        public void Update(Position position, Item item)
        {
            Position = position;
            Item = item;
        }

        public bool CanReach(Table t)
        {
            //Pythagorean theorm
            var distance = Math.Sqrt(Math.Pow(Position.X - t.Position.X, 2) + Math.Pow(Position.Y - t.Position.Y, 2));
            return (distance <= 1.5);
        }
    }

    public class Position
    {
        public int X, Y;
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Manhattan(Position p2) => Math.Abs(X - p2.X) + Math.Abs(Y - p2.Y);

        public override string ToString()
        {
            return X + " " + Y;
        }
    }



    public class MainClass
    {
        public static bool Debug = true;
        public const string Dish = "DISH";

        public static Game ReadGame()
        {
            var game = new Game();
            game.Players[0] = new Player(null, null);
            game.Players[1] = new Player(null, null);

            for (int i = 0; i < 7; i++)
            {
                string kitchenLine = ReadLine();
                for (var x = 0; x < kitchenLine.Length; x++)
                {
                    if (kitchenLine[x] == 'W') game.Window = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'D') game.Dishwasher = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'I') game.IceCream = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'B') game.Blueberry = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'S') game.Strawberry = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'C') game.Chopping = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'H') game.Dough = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == 'O') game.Oven = new Table { Position = new Position(x, i), HasFunction = true };
                    if (kitchenLine[x] == '#') game.Tables.Add(new Table { Position = new Position(x, i) });
                }
            }

            return game;
        }

        private static void Move(Position p) => Console.WriteLine("MOVE " + p);

        private static void Use(Position p)
        {
            Console.WriteLine("USE " + p + "; C# Starter AI");
        }

        private static string ReadLine()
        {
            var s = Console.ReadLine();
            if (Debug)
                Console.Error.WriteLine(s);
            return s;
        }

        private static void Wait()
        {
            Console.WriteLine("WAIT");
        }


        static void Main()
        {
            string[] inputs;

            // ALL CUSTOMERS INPUT: to ignore until Bronze
            int numAllCustomers = int.Parse(ReadLine());
            for (int i = 0; i < numAllCustomers; i++)
            {
                inputs = ReadLine().Split(' ');
                string customerItem = inputs[0]; // the food the customer is waiting for
                int customerAward = int.Parse(inputs[1]); // the number of points awarded for delivering the food
            }

            // KITCHEN INPUT
            var game = ReadGame();
            var step = -11;
            while (true)
            {
                int turnsRemaining = int.Parse(ReadLine());

                // PLAYERS INPUT
                inputs = ReadLine().Split(' ');
                game.Players[0].Update(new Position(int.Parse(inputs[0]), int.Parse(inputs[1])), new Item(inputs[2]));
                inputs = ReadLine().Split(' ');
                game.Players[1].Update(new Position(int.Parse(inputs[0]), int.Parse(inputs[1])), new Item(inputs[2]));

                //Clean other tables
                foreach (var t in game.Tables)
                {
                    t.Item = null;
                }
                int numTablesWithItems = int.Parse(ReadLine()); // the number of tables in the kitchen that currently hold an item
                for (int i = 0; i < numTablesWithItems; i++)
                {
                    inputs = ReadLine().Split(' ');
                    var table = game.Tables.First(t => t.Position.X == int.Parse(inputs[0]) && t.Position.Y == int.Parse(inputs[1]));
                    table.Item = new Item(inputs[2]);
                }

                inputs = ReadLine().Split(' ');
                string ovenContents = inputs[0]; // ignore until bronze league
                int ovenTimer = int.Parse(inputs[1]);
                int numCustomers = int.Parse(ReadLine()); // the number of customers currently waiting for food
                for (int i = 0; i < numCustomers; i++)
                {
                    inputs = ReadLine().Split(' ');
                    string customerItem = inputs[0];
                    int customerAward = int.Parse(inputs[1]);
                }

                // GAME LOGIC
                var myChef = game.Players[0];


                //Check if we completed our step, if so then we move on
                var stepDone = false;
                switch (step)
                {
                    case -11:
                        stepDone = myChef.CanReach(game.Dough);
                        break;
                    case -10:
                        stepDone = true;
                        break;
                    case -9:
                        stepDone = myChef.CanReach(game.Oven);

                        break;
                    case -8:
                        if (game.CookTime == 10)
                        {
                            stepDone = true;
                            game.CookTime = 0;
                        }
                        else
                        {
                            game.CookTime++;
                        }
                        break;
                    case -7:
                        stepDone = myChef.CanReach(game.CookedDough);
                        break;
                    case -6:
                        stepDone = true;
                        break;
                    case -5:
                        stepDone = myChef.CanReach(game.Strawberry);
                        break;
                    case -4:
                        stepDone = true;
                        break;
                    case -3:
                        stepDone = myChef.CanReach(game.Chopping);
                        break;
                    case -2:
                        stepDone = true;
                        break;
                    case -1:
                        stepDone = myChef.CanReach(game.ChoppedStrawberry);
                        break;
                    case 0:
                        stepDone = true;
                        break;

                    case 1:
                        stepDone = myChef.CanReach(game.Dishwasher);
                        break;

                    case 2:
                        stepDone = true;//If we get to step 2 that means we were able to execute it and it should always succeed
                        break;

                    case 3:
                        stepDone = myChef.CanReach(game.Blueberry);
                        break;

                    case 4:
                        stepDone = true;
                        break;

                    case 5:
                        stepDone = myChef.CanReach(game.IceCream);
                        
                        break;

                    case 6:
                        stepDone = true;//If we get to step 2 that means we were able to execute it and it should always succeed
                        break;

                    case 7:
                        stepDone = myChef.CanReach(game.ChoppedStrawberry);
                        break;

                    case 8:
                        stepDone = true;
                        break;

                    case 9:
                        stepDone = myChef.CanReach(game.CookedDough);
                        break;
                    case 10:
                        stepDone = true;
                        break;

                    case 11:
                        stepDone = myChef.CanReach(game.Window);
                        break;
                    case 12:
                        stepDone = true;
                        break;
                }

                //If our current step is done, move to the next one
                if (stepDone)
                {
                    step++;

                    //We have delivered to the window, reset
                    if (step > 12)
                    {
                        step = -11;
                    }
                }

                if(step == -11)
                {
                    Console.WriteLine("MOVE " + game.Dough.Position.ToString() + ";Move Dough");
                }
                if(step == -10)
                {
                    Console.WriteLine("USE " + game.Dough.Position.ToString() + ";Use Dough");
                }
                if (step == -9)
                {
                    Console.WriteLine("MOVE " + game.Oven.Position.ToString() + ";Move Oven");
                }
                if (step == -8)
                {
                    if(game.CookTime == 1)
                    {
                        Console.WriteLine("USE " + game.Oven.Position.ToString() + ";Use Oven"); //Put the dough in
                    }
                    else if(game.CookTime < 10)
                    {
                        Console.WriteLine("WAIT");
                    }
                    else
                    {
                        Console.WriteLine("USE " + game.Oven.Position.ToString() + ";Use Oven"); //Get dough out
                    }
                }
                if (step == -7)
                {
                    if (game.CookedDough == null)
                    {
                        //Find an empty spot
                        foreach (var t in game.Tables.OrderByDescending(t => myChef.Position.Manhattan(t.Position)))
                        {
                            if (t.Item == null)
                            {
                                game.CookedDough = t;
                                break;
                            }
                        }
                    }

                    Console.WriteLine("MOVE " + game.CookedDough.Position.ToString() + ";MOVE Empty (Cooked Dough)");
                }
                if (step == -6)
                {
                    Console.WriteLine("USE " + game.CookedDough.Position.ToString() + ";USE Cooked Dough (Set down)");
                }
                if (step == -5)
                {
                    //Go to strawberry
                    Console.WriteLine("MOVE " + game.Strawberry.Position.ToString() + ";Move Strawberry");
                }

                if(step == -4)
                {
                    //Use Strawberry
                    Console.WriteLine("USE " + game.Strawberry.Position.ToString() + ";Use Strawberry");
                }

                if (step == -3)
                {
                    //Go to chopping
                    Console.WriteLine("MOVE " + game.Chopping.Position.ToString() + ";Move Chopping");
                }

                if (step == -2)
                {
                    //Use Chopping
                    Console.WriteLine("USE " + game.Chopping.Position.ToString() + ";Use Chopping");
                }

                if (step == -1)//Move empty spot
                {
                    if(game.ChoppedStrawberry == null)
                    {
                        //Find an empty spot
                        foreach(var t in game.Tables.OrderByDescending(t => myChef.Position.Manhattan(t.Position)))
                        {
                            if(t.Item == null)
                            {
                                game.ChoppedStrawberry = t;
                                break;
                            }
                        }
                    }
                    Console.WriteLine("MOVE " + game.ChoppedStrawberry.Position.ToString() + ";Move Empty Spot");
                }

                if (step == 0)
                {
                    //Use empty spot
                    Console.WriteLine("USE " + game.ChoppedStrawberry.Position.ToString() + ";Use Empty Spot - Place Strawberry)");
                }

                   //**************************

                if (step == 1)
                {
                    //Go to Dishwasher
                    Console.WriteLine("MOVE " + game.Dishwasher.Position.X + " " + game.Dishwasher.Position.Y + ";Move dishwasher");
                }
                if (step == 2)
                {
                    //USE th Dishwasher
                    Console.WriteLine("USE " + game.Dishwasher.Position.X + " " + game.Dishwasher.Position.Y + ";Use dishwasher");
                }
                if (step == 3)
                {
                    //Go to Blueberry
                    Console.WriteLine("MOVE " + game.Blueberry.Position.X + " " + game.Blueberry.Position.Y + ";Move blueberries");
                }
                if (step == 4)
                {
                    Console.WriteLine("USE " + game.Blueberry.Position.X + " " + game.Blueberry.Position.Y + ";Use blueberries");
                }
                if (step == 5)
                {
                    //Move to icecream
                    Console.WriteLine("MOVE " + game.IceCream.Position.X + " " + game.IceCream.Position.Y + "; Move Icecream");
                }
                if (step == 6)
                {
                    //Use to icecream
                    Console.WriteLine("USE " + game.IceCream.Position.X + " " + game.IceCream.Position.Y + "; Use Icecream");
                }

                if (step == 7)
                {
                    //Move to chipped strawberry
                    Console.WriteLine("MOVE " + game.ChoppedStrawberry.Position.ToString() + ";Move Chopped Strawberry");
                }
                if (step == 8)
                {
                    //Use chopped strawberry
                    Console.WriteLine("USE " + game.ChoppedStrawberry.Position.ToString() + ";USE Chopped Strawberry");
                    game.ChoppedStrawberry = null;
                }

                if (step == 9)
                {
                    //Move to chipped strawberry
                    Console.WriteLine("MOVE " + game.CookedDough.Position.ToString() + ";Move Cooked Dough");
                }
                if (step == 10)
                {
                    //Use chopped strawberry
                    Console.WriteLine("USE " + game.CookedDough.Position.ToString() + ";Get Cooked Dough");
                    game.CookedDough = null;
                }

                if (step == 11)
                {
                    //Move to Window
                    Console.WriteLine("MOVE " + game.Window.Position.X + " " + game.Window.Position.Y + "; Move Window");
                }
                if (step == 12)
                {
                    //Use to Window
                    Console.WriteLine("USE " + game.Window.Position.X + " " + game.Window.Position.Y + "; Use Window");
                }


            }
        }

        public static bool PositionsWithinReach(Position first, Position second)
        {
            //Pythatorean theorm
            var distance = Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
            return (distance <= 1.5);
        }

    }
}
