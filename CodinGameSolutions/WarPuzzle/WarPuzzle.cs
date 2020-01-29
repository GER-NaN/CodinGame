using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Common.PlayingCards;
using Common.Core;


//https://www.codingame.com/ide/puzzle/winamax-battle
namespace WarPuzzle
{

    class WarPuzzle
    {
        static void Main(string[] args)
        {
            Deck p1Hand = new Deck();
            Deck p2Hand = new Deck();

            int p1CardCount = int.Parse(Console.ReadLine()); // the number of cards for player 1

            for (int i = 0; i < p1CardCount; i++)
            {
                Card c = new Card(Console.ReadLine());
                p1Hand.AddCard(c);
                DebugTool.Print(c);
            }

            int p2CardCount = int.Parse(Console.ReadLine()); // the number of cards for player 2
            for (int i = 0; i < p2CardCount; i++)
            {
                Card c = new Card(Console.ReadLine());
                p2Hand.AddCard(c);
                DebugTool.Print(c);
            }

            int rounds = 0;

            List<Card> p1CardPile = new List<Card>();
            List<Card> p2CardPile = new List<Card>();

            //Play the game
            while (true)
            {

                //Step 1: first card drawn
                Card p1Card = p1Hand.Deal1();
                Card p2Card = p2Hand.Deal1();

                p1CardPile.Add(p1Card);
                p2CardPile.Add(p2Card);

                if(p1Card > p2Card)
                {
                    p1Hand.AddCards(p1CardPile);
                    p1Hand.AddCards(p2CardPile);
                    p1CardPile.Clear();
                    p2CardPile.Clear();
                    rounds++;

                }
                else if(p2Card > p1Card)
                {
                    p2Hand.AddCards(p1CardPile);
                    p2Hand.AddCards(p2CardPile);
                    p1CardPile.Clear();
                    p2CardPile.Clear();
                    rounds++;
                }
                else//War
                {
                    if(p1Hand.Count() >= 4 && p2Hand.Count() >= 4)
                    {
                        //Put 3 cards into their pile
                        p1CardPile.AddRange(p1Hand.Deal(3));
                        p2CardPile.AddRange(p2Hand.Deal(3));
                    }
                    else
                    {
                        //Clear decks to provide an equally first
                        p1Hand.ClearDeck();
                        p2Hand.ClearDeck();
                    }

                }

                if(p1Hand.Count() == 0 || p2Hand.Count() == 0)
                {
                    break;//we're done
                }
            }


            if(p1Hand.Count() > p2Hand.Count())
            {
                Output.Print("1 " + rounds);
            }
            else if(p2Hand.Count() > p1Hand.Count())
            {
                Output.Print("2 " + rounds);
            }
            else
            {
                Output.Print("PAT");//Tie
            }

            Console.ReadLine();
        }
    }
}



/*
Rules
War is a card game played between two players. Each player gets a variable number 
of cards of the beginning of the game: that's the player's deck. Cards are placed 
face down on top of each deck.
 
Step 1 : the fight
At each game round, in unison, each player reveals the top card of their deck – this 
is a "battle" – and the player with the higher card takes both the cards played and 
moves them to the bottom of their stack. The cards are ordered by value as follows, 
from weakest to strongest:2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A.
 
Step 2 : war
If the two cards played are of equal value, then there is a "war". 
First, both players place the three next cards of their pile face down. 
Then they go back to step 1 to decide who is going to win the war (several "wars" can be chained). 
As soon as a player wins a "war", the winner adds all the cards from the "war" to their deck.
 

Special cases:
If a player runs out of cards during a "war" (when giving up the three cards or when doing the battle), 
then the game ends and both players are placed equally first. The test cases provided in this puzzle 
are built in such a way that a game always ends (you do not have to deal with infinite games)
Each card is represented by its value followed by its suit: D, H, C, S. For example: 4H, 8C, AS.

When a player wins a battle, they put back the cards at the bottom of their deck in a precise order. 
First the cards from the first player, then the one from the second player (for a "war", all the 
cards from the first player then all the cards from the second player).


For example, if the card distribution is the following:
Player 1 : 10D 9S 8D KH 7D 5H 6S
Player 2 : 10H 7H 5C QC 2C 4H 6D
Then after one game turn, it will be:
Player 1 : 5H 6S 10D 9S 8D KH 7D 10H 7H 5C QC 2C
Player 2 : 4H 6D
 
Victory Conditions
A player wins when the other player no longer has cards in their deck.
 	Game Input
Input
Line 1: the number N of cards for player one.

N next lines: the cards of player one.

Next line: the number M of cards for player two.

M next lines: the cards of player two.

Output
If players are equally first: PAT
Otherwise, the player number (1 or 2) followed by the number of game rounds separated by a space character. A war or a succession of wars count as one game round.
Constraints
0 < N, M < 1000
*/
