
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

    public enum Ingredient
    {
        Croissant,
        ChoppedStrawberry,
        Blueberry,
        IceCream,
        Tart
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
        public Oven Oven;

        public List<CustomerOrder> CustomerOrders = new List<CustomerOrder>();
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

    public class CustomerOrder
    {
        public string Content;
        public List<Item> Items = new List<Item>();
        public int Score;

        public CustomerOrder(string content, int score)
        {
            Content = content;
            Score = score;

            //Parse content
            if(Content.Contains("ICE_CREAM"))
            {
                Items.Add(new Item("ICE_CREAM"));
            }
            if (Content.Contains("BLUEBERRIES"))
            {
                Items.Add(new Item("BLUEBERRIES"));
            }
            if (Content.Contains("CHOPPED_STRAWBERRIES"))
            {
                Items.Add(new Item("CHOPPED_STRAWBERRIES"));
            }
            if (Content.Contains("CROISSANT"))
            {
                Items.Add(new Item("CROISSANT"));
            }
            if (Content.Contains("TART"))
            {
                Items.Add(new Item("TART"));
            }
        }

        public bool HasIceCream()
        {
            return Items.Any(item => item.IsIceCream());
        }

        public bool HasBlueberry()
        {
            return Items.Any(item => item.IsBlueberry());
        }

        public bool HasChoppedBerry()
        {
            return Items.Any(item => item.IsChoppedStrawberry());
        }

        public bool HasCroissant()
        {
            return Items.Any(item => item.IsCroissant());
        }

        public bool IsIceCreamBerry()
        {
            return HasIceCream() && HasBlueberry() && Items.Count == 2;
        }

        public bool IsIceCreamStrawBerry()
        {
            return HasIceCream() && HasChoppedBerry() && Items.Count == 2;
        }

        public bool IsIceCreamCroissant()
        {
            return HasIceCream() && HasCroissant() && Items.Count == 2;
        }

        public bool IsCroissantBerry()
        {
            return HasCroissant() && HasBlueberry() && Items.Count == 2;
        }

        public bool IsCroissantStrawberry()
        {
            return HasCroissant() && HasChoppedBerry() && Items.Count == 2;
        }

        public bool IsDoubleBerry()
        {
            return HasBlueberry() && HasChoppedBerry() && Items.Count == 2;
        }

        public bool IsIceCreamDoubleBerry()
        {
            return HasBlueberry() && HasChoppedBerry() && HasIceCream() && Items.Count == 3;
        }

        public bool IsCroissantIceCreamBerry()
        {
            return HasBlueberry() && HasCroissant() && HasIceCream() && Items.Count == 3;
        }

        public bool IsCroissantDoubleBerry()
        {
            return HasBlueberry() && HasCroissant() && HasChoppedBerry() && Items.Count == 3;
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

        public bool ContainsIngredient(Ingredient ingredient)
        {
            bool contains = false;
            switch (ingredient)
            {
                case Ingredient.Croissant:
                    contains =  Content.Contains(MainClass.Croissant);
                    break;
                case Ingredient.Blueberry:
                    contains = Content.Contains(MainClass.Blueberries);
                    break;
                case Ingredient.ChoppedStrawberry:
                    contains = Content.Contains(MainClass.ChoppedStrawberries);
                    break;
                case Ingredient.IceCream:
                    contains = Content.Contains(MainClass.IceCream);
                    break;
                case Ingredient.Tart:
                    contains = Content.Contains(MainClass.Tart);
                    break;
            }

            return contains;
        }

        public bool IsIngredient(Ingredient ingredient)
        {
            bool contains = false;
            switch (ingredient)
            {
                case Ingredient.Croissant:
                    contains = Content == MainClass.Croissant;
                    break;
                case Ingredient.Blueberry:
                    contains = Content == MainClass.Blueberries;
                    break;
                case Ingredient.ChoppedStrawberry:
                    contains = Content == MainClass.ChoppedStrawberries;
                    break;
                case Ingredient.IceCream:
                    contains = Content == MainClass.IceCream;
                    break;
                case Ingredient.Tart:
                    contains = Content == MainClass.Tart;
                    break;
            }

            return contains;
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

        internal bool IsStrawberry()
        {
            return Content == MainClass.Strawberries;
        }

        public bool IsIceCream()
        {
            return Content == MainClass.IceCream;
        }

        public bool IsBlueberry()
        {
            return Content == MainClass.Blueberries;
        }

        internal bool IsChoppedStrawberry()
        {
            return Content == MainClass.ChoppedStrawberries;
        }

        internal bool IsChoppedStrawberryAndPlate()
        {
            return Content == MainClass.Dish + "-" + MainClass.ChoppedStrawberries;
        }

        internal bool IsCroissant()
        {
            return Content == MainClass.Croissant;
        }

        internal bool IsCroissantAndPlate()
        {
            return Content == MainClass.Dish + "-" + MainClass.Croissant;
        }


        internal bool HasIcecream()
        {
            return Content.Contains(MainClass.IceCream);
        }

        internal bool HasBlueberry()
        {
            return Content.Contains(MainClass.Blueberries);
        }

        internal bool HasChoppedStrawberry()
        {
            return Content.Contains(MainClass.ChoppedStrawberries);
        }

        internal bool IsCroissantStrawberry()
        {
            return Content == "DISH-CROISSANT-CHOPPED_STRAWBERRIES" || Content == "DISH-CHOPPED_STRAWBERRIES-CROISSANT";
        }

        internal bool IsCroissantDoubleBerry()
        {
            return
                Content == "DISH-CHOPPED_STRAWBERRIES-BLUEBERRIES-CROISSANT" ||
                Content == "DISH-CHOPPED_STRAWBERRIES-CROISSANT-BLUEBERRIES" ||
                Content == "DISH-BLUEBERRIES-CROISSANT-CHOPPED_STRAWBERRIES" ||
                Content == "DISH-BLUEBERRIES-CHOPPED_STRAWBERRIES-CROISSANT" ||
                Content == "DISH-CROISSANT-CHOPPED_STRAWBERRIES-BLUEBERRIES" ||
                Content == "DISH-CROISSANT-BLUEBERRIES-CHOPPED_STRAWBERRIES";

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

        internal bool HasCroissantOnPlate()
        {
            return (Item != null && Item.IsCroissantAndPlate());
        }

        internal bool HasChoppedDough()
        {
            return (Item != null && Item.Content == MainClass.ChoppedDough);
        }

        internal bool HasStrawberry()
        {
            return (Item != null && Item.Content == MainClass.Strawberries);
        }

        internal bool HasChoppedStrawberries()
        {
            return (Item != null && Item.Content == MainClass.ChoppedStrawberries);
        }

        internal bool HasPlate()
        {
            return (Item != null && Item.IsPlate);
        }

        internal bool HasBlueberryOnPlate()
        {
            return (Item != null && Item.IsPlate && Item.HasBlueberry());
        }

        internal bool HasIceCreanOnPlate()
        {
            return (Item != null && Item.IsPlate && Item.HasIcecream());
        }

        internal bool HasChoppedStrawberryOnPlate()
        {
            return (Item != null && Item.IsPlate && Item.HasChoppedStrawberry());
        }

        /// <summary>Checks if the chef has a plate and the specified ingredient </summary>
        /// <param name="ingredient">The ingredient that should be on the plates</param>
        /// <returns>True if the chef has a plate and the ingredient.</returns>
        public bool IsItemOnPlate(Ingredient ingredient)
        {
            bool itemFound = false;

            if(Item.IsPlate)
            {
                itemFound = Item.ContainsIngredient(ingredient);
            }
            return itemFound;
        }

        public bool HasSingleIngredient(Ingredient ingredient)
        {
            bool itemFound = false;

            if (!Item.IsPlate)
            {
                itemFound = Item.IsIngredient(ingredient);
            }
            return itemFound;
        }
        



        //Need to make clearer logic to look at whats in hand vs whats on the plate.
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
        public static int SwitchFlag = 1;

        public static bool Debug = true;//prints the input
        public const string Dish = "DISH";
        public const string Dough = "DOUGH";
        public const string Croissant = "CROISSANT";
        public const string Blueberries = "BLUEBERRIES";
        public const string IceCream = "ICE_CREAM";
        public const string Strawberries = "STRAWBERRIES";
        public const string ChoppedStrawberries = "CHOPPED_STRAWBERRIES";
        public const string ChoppedDough = "CHOPPED_DOUGH";
        public const string Tart = "TART";


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

        private static string ReadLine()
        {
            var s = Console.ReadLine();
            if (Debug)
                Console.Error.WriteLine(s);
            return s;
        }

        public static void ResetState(Game game)
        {
            game.State = State.Idle;
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
            var prevTurn = 200;
            while (true)
            {
                int turnsRemaining = int.Parse(ReadLine());

                if(prevTurn < turnsRemaining)
                {
                    ResetState(game);
                }
                prevTurn = turnsRemaining;
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
                //Cean customer orders
                game.CustomerOrders.Clear();

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

                    game.CustomerOrders.Add(new CustomerOrder(customerItem, customerAward));
                }

                // GAME LOGIC
                game.MyChef = game.Players[0];

                StartLogic(game);
            }
        }


        public static void StartLogic(Game game)
        {
            if (game.CustomerOrders.Any(order => order.IsIceCreamBerry()))
            {
                MakeIceCreamBerry(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsIceCreamStrawBerry()))
            {
                MakeIceCreamStrawberry(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsIceCreamCroissant()))
            {
                MakeIceCreamCroissant(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsCroissantBerry()))
            {
                MakeCroissantBerry(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsDoubleBerry()))
            {
                MakeDoubleBerry(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsCroissantStrawberry()))
            {
                MakeCroissantStrawberry(game);
            }

            /************      3 Tier Items ****************/
            else if (game.CustomerOrders.Any(order => order.IsIceCreamDoubleBerry()))
            {
                MakeIceCreamDoubleBerry(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsCroissantIceCreamBerry()))
            {
                MakeCroissantIceCreamBerry(game);
            }
            else if (game.CustomerOrders.Any(order => order.IsCroissantDoubleBerry()))
            {
                MakeCroissantDoubleBerry(game);
            }

            /************      4 Tier Items ****************/

            else
            {
                MakeSupportItems(game);
            }
        }

        public static bool MakeCroissantDoubleBerry(Game game)
        {
            if (game.MyChef.Item.IsCroissantDoubleBerry())
            {
                ServeDish(game);
            }
            else if (game.MyChef.HasSingleIngredient(Ingredient.Croissant) || game.MyChef.IsItemOnPlate(Ingredient.Croissant) || game.Tables.Any(t => (t.Item?.IsCroissant() ?? false) || (t.Item?.IsCroissantAndPlate() ?? false)))
            {
                if (game.MyChef.HasSingleIngredient(Ingredient.ChoppedStrawberry) || game.MyChef.IsItemOnPlate(Ingredient.ChoppedStrawberry) || game.Tables.Any(t => (t.Item?.IsChoppedStrawberry() ?? false) || (t.Item?.IsChoppedStrawberryAndPlate() ?? false)))
                {
                    //Both things are available somewhere. Get a plate and then build the dish
                    if (game.MyChef.HasPlate())
                    {
                        if (!game.MyChef.IsItemOnPlate(Ingredient.Croissant))
                        {
                            GetCroissant(game);
                        }
                        else if (!game.MyChef.IsItemOnPlate(Ingredient.ChoppedStrawberry))
                        {
                            GetChoppedStrawberry(game);
                        }
                        else if(!game.MyChef.IsItemOnPlate(Ingredient.Blueberry))
                        {
                            PutBlueberryOnPlate(game);
                        }
                    }
                    else
                    {
                        GetPlate(game);
                    }
                }
                else
                {
                    ChopBerries(game);
                }
            }
            else
            {
                GetCroissant(game);
            }
            return false;
        }

        public static bool MakeCroissantIceCreamBerry(Game game)
        {
            if (game.MyChef.Item.HasCroissant())
            {
                if (game.MyChef.HasPlate())
                {
                    if (game.MyChef.Item.HasBlueberry())
                    {
                        if (game.MyChef.Item.HasIcecream())
                        {
                            ServeDish(game);
                        }
                        else
                        {
                            PutIceCreamOnPlate(game);
                        }
                    }
                    else
                    {
                        PutBlueberryOnPlate(game);
                    }
                }
                else
                {
                    GetPlate(game);
                }
            }
            else
            {
                GetCroissant(game);
            }

            return false;
        }

        public static bool MakeIceCreamDoubleBerry(Game game)
        {
            if (game.MyChef.Item.HasChoppedStrawberry())
            {
                if (game.MyChef.HasPlate())
                {
                    if (game.MyChef.Item.HasBlueberry())
                    {
                        if(game.MyChef.Item.HasIcecream())
                        {
                            ServeDish(game);
                        }
                        else
                        {
                            PutIceCreamOnPlate(game);
                        }
                    }
                    else
                    {
                        PutBlueberryOnPlate(game);
                    }
                }
                else
                {
                    GetPlate(game);
                }
            }
            else
            {
                ChopBerries(game);
            }

            return false;
        }


        public static bool MakeDoubleBerry(Game game)
        {
            if (game.MyChef.Item.HasChoppedStrawberry())
            {
                if (game.MyChef.HasPlate())
                {
                    if (game.MyChef.Item.HasBlueberry())
                    {
                        ServeDish(game);
                    }
                    else
                    {
                        PutBlueberryOnPlate(game);
                    }
                }
                else
                {
                    GetPlate(game);
                }
            }
            else
            {
                ChopBerries(game);
            }

            return false;
        }


        public static bool PutIceCreamOnPlate(Game game)
        {
            if (game.MyChef.HasIceCreanOnPlate())
            {
                Wait("We have IceCream");
                return true;
            }

            //Move to Blueberry
            Table target = null;
            foreach (var table in game.Tables)
            {
                if (table.Item?.IsIceCream() ?? false)
                {
                    target = table;
                    break;
                }
            }

            if (target == null)
            {
                target = game.IceCream;//Go to the crate
            }


            //Go to the target
            var atTarget = game.MyChef.CanReach(target);
            var isAvailable = (target == game.IceCream || target.Item.IsIceCream());

            if (!atTarget && isAvailable)
            {
                MoveTo(target.Position, "Move to IceCream");
            }

            //We're there and its available
            if (atTarget && isAvailable)
            {
                Use(target.Position, "Pick Up IceCream");
            }

            return false;
        }

        public static bool MakeSupportItems(Game game)
        {
            //Prepare stuff
            if (game.MyChef.HasChoppedDough() || game.MyChef.HasCroissant() || game.MyChef.HasChoppedStrawberries())
            {
                SetItemDown(game);
                SwitchFlag++;
                Console.Error.WriteLine("Switch: " + SwitchFlag);
            }
            else
            {
                if (SwitchFlag % 3 == 0)
                {
                    CookCroissant(game);
                }
                else if (SwitchFlag % 3 == 1)
                {
                    ChopDough(game);
                }
                else if (SwitchFlag % 3 == 2)
                {
                    ChopBerries(game);
                }
            }

            return false;
        }

        public static bool MakeIceCreamBerry(Game game)
        {
            //make the simple order
            if (game.MyChef.HasPlate())
            {
                if (!game.MyChef.Item.HasIcecream())
                {
                    PutIceCreamOnPlate(game);
                }
                else if (!game.MyChef.Item.HasBlueberry())
                {
                    PutBlueberryOnPlate(game);
                }
                else
                {
                    ServeDish(game);
                }
            }
            else
            {
                GetPlate(game);
            }
            return false;
        }

        public static bool MakeIceCreamStrawberry(Game game)
        {
            if (game.MyChef.Item.HasChoppedStrawberry())
            {
                if (game.MyChef.HasPlate())
                {
                    if (game.MyChef.Item.HasIcecream())
                    {
                        ServeDish(game);
                    }
                    else
                    {
                        PutIceCreamOnPlate(game);
                    }
                }
                else
                {
                    GetPlate(game);
                }
            }
            else
            {
                ChopBerries(game);
            }

            return false;
        }

        public static bool MakeIceCreamCroissant(Game game)
        {
            if (game.MyChef.Item.HasCroissant())
            {
                if (game.MyChef.HasPlate())
                {
                    if (game.MyChef.Item.HasIcecream())
                    {
                        ServeDish(game);
                    }
                    else
                    {
                        PutIceCreamOnPlate(game);
                    }
                }
                else
                {
                    GetPlate(game);
                }
            }
            else
            {
                GetCroissant(game);
            }

            return false;
        }

        public static bool MakeCroissantBerry(Game game)
        {
            if (game.MyChef.Item.HasCroissant())
            {
                if (game.MyChef.HasPlate())
                {
                    if (game.MyChef.Item.HasBlueberry())
                    {
                        ServeDish(game);
                    }
                    else
                    {
                        PutBlueberryOnPlate(game);
                    }
                }
                else
                {
                    GetPlate(game);
                }
            }
            else
            {
                GetCroissant(game);
            }

            return false;
        }

        public static bool MakeCroissantStrawberry(Game game)
        {
            if(game.MyChef.Item.IsCroissantStrawberry())
            {
                ServeDish(game);
            }
            else if(game.MyChef.HasCroissant() || game.MyChef.HasCroissantOnPlate() || game.Tables.Any(t => (t.Item?.IsCroissant() ?? false) || (t.Item?.IsCroissantAndPlate() ?? false)))
            {
                if(game.MyChef.HasChoppedStrawberries() || game.MyChef.HasChoppedStrawberryOnPlate() || game.Tables.Any(t => (t.Item?.IsChoppedStrawberry() ?? false) || (t.Item?.IsChoppedStrawberryAndPlate() ?? false)))
                {
                    //Both things are available somewhere. Get a plate and then build the dish
                    if(game.MyChef.HasPlate())
                    {
                        if(!game.MyChef.HasCroissantOnPlate())
                        {
                            GetCroissant(game);
                        }
                        else if (!game.MyChef.HasChoppedStrawberryOnPlate())
                        { 
                            GetChoppedStrawberry(game);
                        }
                    }
                    else
                    {
                        GetPlate(game);
                    }
                }
                else
                {
                    ChopBerries(game);
                }
            }
            else
            {
                GetCroissant(game);
            }
            return false;
        }



        public static bool PutBlueberryOnPlate(Game game)
        {
            if (game.MyChef.HasBlueberryOnPlate())
            {
                Wait("We have Blueberry");
                return true;
            }

            //Move to Blueberry
            Table target = null;
            foreach (var table in game.Tables)
            {
                if (table.Item?.IsBlueberry() ?? false)
                {
                    target = table;
                    break;
                }
            }

            if (target == null)
            {
                target = game.Blueberry;//Go to the crate
            }


            //Go to the target
            var atTarget = game.MyChef.CanReach(target);
            var isAvailable = (target == game.Blueberry || target.Item.IsBlueberry());

            if (!atTarget && isAvailable)
            {
                MoveTo(target.Position, "Move to Blueberry");
            }

            //We're there and its available
            if (atTarget && isAvailable)
            {
                Use(target.Position, "Pick Up Blueberry");
            }

            return false;
        }

        public static bool ServeDish(Game game)
        {
            var target = game.Window.Position;
            var atTarget = game.MyChef.CanReach(target);

            if (!atTarget)
            {
                MoveTo(target, "Move to Window");
            }

            //We're there and its available
            if (atTarget)
            {
                Use(target, "Serve Dish");
            }

            return false;
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

        public static bool ChopDough(Game game)
        {
            if (game.MyChef.HasDough())
            {
                var target = game.Chopping.Position;
                var atTarget = game.MyChef.CanReach(target);

                if (!atTarget)
                {
                    MoveTo(target, "Move to Copping Board");
                }

                //We're there and its available
                if (atTarget)
                {
                    Use(target, "Chop Dough");
                }
            }
            else
            {
                FindDough(game);
            }

            return game.MyChef.HasChoppedDough();
        }

        public static bool ChopBerries(Game game)
        {
            if (game.MyChef.HasStrawberry())
            {
                var target = game.Chopping.Position;
                var atTarget = game.MyChef.CanReach(target);

                if (!atTarget)
                {
                    MoveTo(target, "Move to Copping Board");
                }

                //We're there and its available
                if (atTarget)
                {
                    Use(target, "Chop Strawberries");
                }
            }
            else
            {
                FindStrawberry(game);
            }

            return game.MyChef.HasChoppedStrawberries();
        }


        public static bool GetCroissant(Game game)
        {
            Console.Error.WriteLine("GetCroissant");
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
                CookCroissant(game);
            }

            return false;
        }

        public static bool GetChoppedStrawberry(Game game)
        {
            //Look for existing Chopped
            Table target = null;
            foreach (var table in game.Tables)
            {
                if (table.Item != null && table.Item.IsChoppedStrawberry())//cant get strawberry off a plate
                {
                    target = table;
                    break;
                }
            }

            if (target != null)
            {
                //Go to the target
                var atTarget = game.MyChef.CanReach(target);
                var isAvailable = (target.Item != null && target.Item.IsChoppedStrawberry());

                if (!atTarget && isAvailable)
                {
                    MoveTo(target.Position, "Move to Chopped Strawberry");
                }

                //We're there and its available
                if (atTarget && isAvailable)
                {
                    Use(target.Position, "Pick Up Chopped Strawberry");
                }
            }
            else
            {
                ChopBerries(game);
            }

            return false;
        }

        public static bool CookCroissant(Game game)
        {
            Console.Error.WriteLine("Cook Croissant");
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
            Console.Error.WriteLine("FindDough");
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

        public static bool FindStrawberry(Game game)
        {
            if (game.MyChef.HasStrawberry())
            {
                Wait("We have Strawberry");
                return true;
            }

            //Move to strawberry
            Table target = null;
            foreach (var table in game.Tables)
            {
                if (table.Item?.IsStrawberry() ?? false)
                {
                    target = table;
                    break;
                }
            }

            if (target == null)
            {
                target = game.Strawberry;//Go to the crate
            }


            //Go to the target
            var atTarget = game.MyChef.CanReach(target);
            var isAvailable = (target == game.Strawberry || target.Item.IsStrawberry());

            if (!atTarget && isAvailable)
            {
                MoveTo(target.Position, "Move to Strawberry");
            }

            //We're there and its available
            if (atTarget && isAvailable)
            {
                if (!game.MyChef.IsEmpty())
                {
                    SetItemDown(game);
                }
                else
                {
                    Use(target.Position, "Pick Up Strawberry");
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
