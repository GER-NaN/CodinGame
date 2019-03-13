
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//https://www.codingame.com/contests/code-a-la-mode
namespace CodeAlaMode
{

    public enum State
    {
        Idle,
        CookingDough
    }

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
   

        //When I set it down, maybe dont need this
        public Table ChoppedStrawberry;
        public Table CookedDough;
        
        public List<Table> Tables = new List<Table>();

        public Player MyChef;
        public State State = State.Idle;
        public Oven Oven;//TODO:Refactor implemention to use this
    }

    public class Table
    {
        public Position Position;
        public bool HasFunction;
        public Item Item;

        public bool IsEmpty()
        {
            return Item == null;
        }
    }

    public class Item
    {
        public string Content;
        public bool IsPlate;
        public Item(string content)
        {
            Content = content;
            IsPlate = Content.Contains(MainClass.Dish);
        }

        /// <summary>Dough is always stand-alone and cannot go on a plate</summary>
        public bool IsDough()
        {
            return Content == MainClass.Dough;
        }

        public bool HasCroissant()
        {
            return Content.Contains(MainClass.Croissant);
        }
    }

    public class Oven
    {
        public readonly Position Position;

        public int Timer;
        public Item Contents;
        public Table Table;

        public Oven(Position position)
        {
            Position = position;
        }

        public bool IsEmpty
        {
            get
            {
                return Contents.Content == "NONE";
            }
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

        public bool CanReach(Table table)
        {
            return CanReach(table.Position);
        }

        public bool CanReach(Position position)
        {
            var distance = Math.Sqrt(Math.Pow(Position.X - position.X, 2) + Math.Pow(Position.Y - position.Y, 2));
            return (distance <= 1.5);//Pythagorean theorm, would manhatten work?
        }

        public bool IsEmpty()
        {
            return (Item == null || Item.Content == "NONE");
        }

        public bool HasDough()
        {
            return (Item != null && Item.IsDough());
        }

        internal bool HasCroissant()
        {
            return (Item != null && Item.Content == MainClass.Croissant);
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
        public static bool Debug = true;//prints the input
        public const string Dish = "DISH";
        public const string Dough = "DOUGH";
        public const string Croissant = "CROISSANT";
        public const string Blueberries = "BLUEBERRIES";
        public const string IceCream = "ICE_CREAM";
        public const string ChoppedStrawberries = "CHOPPED_STRAWBERRIES";


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
                    if (kitchenLine[x] == 'O') game.Oven = new Oven (new Position(x, i));
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

                //Table items
                int numTablesWithItems = int.Parse(ReadLine()); 
                for (int i = 0; i < numTablesWithItems; i++)
                {
                    inputs = ReadLine().Split(' ');
                    var table = game.Tables.First(t => t.Position.X == int.Parse(inputs[0]) && t.Position.Y == int.Parse(inputs[1]));
                    table.Item = new Item(inputs[2]);
                }

                //Oven stuff
                inputs = ReadLine().Split(' ');
                game.Oven.Contents = new Item(inputs[0]);
                game.Oven.Timer = int.Parse(inputs[1]);

                //Customer stuff, the number of customers currently waiting for food
                int numCustomers = int.Parse(ReadLine());
                for (int i = 0; i < numCustomers; i++)
                {
                    inputs = ReadLine().Split(' ');
                    string customerItem = inputs[0];
                    int customerAward = int.Parse(inputs[1]);
                }

                // GAME LOGIC
                game.MyChef = game.Players[0];

                StartLogic(game);
            }
        }

        public static void StartLogic(Game game)
        {
            if(game.MyChef.HasCroissant())
            {
                SetItemDown(game);
            }
            else
            {
                CookCroissant(game);
            }


            //if (game.MyChef.Item == null || !game.MyChef.Item.IsPlate)
            //{
            //    GetPlate(game);
            //}
            //else if(! game.MyChef.Item.HasCroissant())
            //{
            //    GetCroissant(game);
            //}
            //else
            //{
            //    SetItemDown(game);
            //}
        }

        public static bool SetItemDown(Game game)
        {
            //Find the closest spot
            Table target = null;
            double closestEmpty = double.MaxValue;
            foreach (var table in game.Tables)
            {
                if (table.IsEmpty() && table.Position.Manhattan(game.MyChef.Position) < closestEmpty)
                {
                    target = table;
                    closestEmpty = table.Position.Manhattan(game.MyChef.Position);
                }
            }
            
            //Go to the target
            var atTarget = game.MyChef.CanReach(target);

            if (!atTarget)
            {
                MoveTo(target.Position, "Move to Empty Spot");
            }

            //We're there and its available
            if (atTarget)
            {
                Use(target.Position, "Set item Down: " + game.MyChef.Item.Content );
            }

            return false;
        }

        public static bool GetCroissant(Game game)
        {
            //Look for existing Croissant
            Table target = null;
            foreach (var table in game.Tables)
            {
                if (table.Item != null && table.Item.HasCroissant() && !table.Item.IsPlate)//cant get croissant off a plate
                {
                    Console.Error.WriteLine("Croissant Found: " + table.Item.Content);
                    target = table;
                    break;
                }
            }

            if(target != null)
            {
                //Go to the target
                var atTarget = game.MyChef.CanReach(target);
                var isAvailable = (target.Item != null && target.Item.HasCroissant() && !target.Item.IsPlate);

                if (!atTarget && isAvailable)
                {
                    MoveTo(target.Position, "Move to Croissant");
                }

                //We're there and its available
                if (atTarget && isAvailable)
                {
                    Use(target.Position, "Pick Up Croissant");
                }
            }
            else
            {
                FindDough(game);
            }

            return false;
        }

        public static bool CookCroissant(Game game)
        {
            if(game.State == State.CookingDough)
            {
                //Wait for oven to cook
                if(game.Oven.Timer == 10)
                {
                    Use(game.Oven.Position, "Get Croissant from Oven");
                    Console.Error.WriteLine(game.Oven.Timer + " " + game.Oven.Contents.Content);
                    game.State = State.Idle;//were done
                }
                else
                {
                    Wait("Dough is Cooking.");
                }
            }
            else if(game.MyChef.HasDough())
            {
                var target = game.Oven.Position;
                var atTarget = game.MyChef.CanReach(target);
                var isAvailable = game.Oven.IsEmpty;

                if (!atTarget)
                {
                    MoveTo(target, "Move to Oven");
                }

                //We're there and its available
                if (atTarget && isAvailable)
                {
                    Use(target, "Put Dough in Oven");
                    game.State = State.CookingDough;
                }

                if(atTarget && !isAvailable)
                {
                    Wait("Waiting for Oven");
                }
            }
            else
            {
                FindDough(game);
            }

            return game.MyChef.HasCroissant();
        }

        public static bool FindDough(Game game)
        {
            if(game.MyChef.HasDough())
            {
                Wait("We have dough");
                return true;
            }

            //Move to dough
            Table target = null;
            foreach (var table in game.Tables)
            {
                if (table.Item?.IsDough() ?? false)
                {
                    target = table;
                    break;
                }
            }

            if(target == null)
            {
                target = game.Dough;
            }


            //Go to the target
            var atTarget = game.MyChef.CanReach(target);
            var isAvailable = (target == game.Dough || target.Item.IsDough());

            if (!atTarget && isAvailable)
            {
                MoveTo(target.Position, "Move to Dough");
            }

            //We're there and its available
            if (atTarget && isAvailable)
            {
                if(!game.MyChef.IsEmpty())
                {
                    SetItemDown(game);
                }
                else
                {
                    Use(target.Position, "Pick Up Dough");
                }
            }

            return false;
        }

        public static bool GetPlate(Game game)
        {
            //Get a plate, start by looking for an existing plate (not at dishwasher)
            Table target = null;
            //foreach (var table in game.Tables)
            //{
            //    if (table.Item?.IsPlate ?? false)
            //    {
            //        target = table;
            //        break;
            //    }
            //}

            //We couldnt find a plate so go to the dishwasher
            if (target == null)
            {
                target = game.Dishwasher;
            }
            
            //Go to the target
            var atTarget = game.MyChef.CanReach(target);
            var isAvailable = (target == game.Dishwasher || target.Item.IsPlate);

            if(!atTarget && isAvailable)
            {
                MoveTo(target.Position, "Move to Plate");
            }

            //We're there and its available
            if(atTarget && isAvailable)
            {
                Use(target.Position, "Pick Up Plate");
            }

            return game.MyChef.Item?.IsPlate ?? false;
        }

        public static void MoveTo(Position position,string message = "")
        {
            Console.WriteLine("MOVE " + position.ToString() + ";" + message);
        }

        public static void Use(Position position, string message = "")
        {
            Console.WriteLine("USE " + position.ToString() + ";" + message);
        }

        public static void Wait(string message = "")
        {
            Console.WriteLine("WAIT ;" + message);
        }


        public static bool PositionsWithinReach(Position first, Position second)
        {
            //Pythatorean theorm
            var distance = Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
            return (distance <= 1.5);
        }

    }
}
