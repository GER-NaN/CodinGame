using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.PlayingCards
{
    public class Card
    {
        private readonly List<string> Suits = new List<string>() { "D", "H", "C", "S" };
        public string Value { get; } = "";
        public string Suit { get; } = "";

        //TODO: Add ParseMethod flag for suitless cards, colorless cards and full specified cards. EX( FullCard, NoSuitSpecified, NoColorSpecified)
        public Card(string fullCard)
        {
            string original = fullCard;
            //We find the suit in the string, save it and then remove it to get the card.
            foreach(string suit in Suits)
            {
                if(fullCard.Contains(suit))
                {
                    Suit = suit;
                    Value = fullCard.Replace(suit, string.Empty);
                    break;//break the for loop
                }
            }

            if(Value == string.Empty)
            {
                throw new ArgumentException("Unable to parse the card. The card value could not be found for argument " + original);
            }
        }

        /// <summary>The card value when they are placed in order from 2 through Ace. The value of 2 is 2 and the value of Ace is 14. Use this to compare ordinal value for comparison.</summary>
        public int OrdinalValue
        {
            get
            {
                //2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A
                int value;

                if (int.TryParse(Value,out value))
                {
                    return value;
                }

                if(Value == "J")
                {
                    return 11;
                }
                else if (Value == "Q")
                {
                    return 12;
                }
                else if (Value == "K")
                {
                    return 13;
                }
                else if(Value == "A")
                {
                    return 14;
                }
                else
                {
                    throw new Exception("Unable to find value for card " + Value);
                }
            }
        }

        public override string ToString()
        {
            return this.Value + this.Suit;
        }

        //TODO: We need to provide a switch for comparison logic, for instance
        // in blackjack the K and J are equal in terms of their value (10) so what
        // would these functions return and how do we switch the comparison logic.
        // One quick thought would be to have a singleton class CardComparison and
        // have the default option to be Oridinal whith an ability to switch it to
        // FaceValue and whatever other comparison options there are. Or dont implement
        // any of this and let the callers deal with it.
        public static bool operator >(Card a, Card b)
        {
            return a.OrdinalValue > b.OrdinalValue;
        }

        public static bool operator <(Card a, Card b)
        {
            return a.OrdinalValue < b.OrdinalValue;
        }

        public static bool operator ==(Card a, Card b)
        {
            return a.OrdinalValue == b.OrdinalValue;
        }

        public static bool operator !=(Card a, Card b)
        {
            return a.OrdinalValue != b.OrdinalValue;
        }

        public override bool Equals(object obj)
        {
            Card card = obj as Card;
            if(obj == null)
            {
                return false;
            }

            return card.Value == this.Value && card.Suit == this.Suit;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Value.GetHashCode();
            hash+=  Suit.GetHashCode();
            
            return hash;
        }
    }
}
