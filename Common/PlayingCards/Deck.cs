using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.PlayingCards
{
    public class Deck
    {
        private Queue<Card> cards;

        public Deck()
        {
            cards = new Queue<Card>();
        }

        public void AddCard(Card c)
        {
            cards.Enqueue(c);
        }
        public void AddCards(IEnumerable<Card> newCards)
        {
            foreach(Card c in newCards)
            {
                cards.Enqueue(c);
            }
        }


        public Card Deal1()
        {
            if(cards.Count == 0)
            {
                return null;
            }

            return cards.Dequeue();
        }

        /// <summary>
        /// Deals the specified number of cards. If the Deck contains less than 
        /// the requested amount of cards then the remaining cards will be returned.
        /// </summary>
        /// <param name="count">The amount of cards to deal</param>
        /// <returns></returns>
        public List<Card> Deal(int count)
        {
            if (cards.Count == 0)
            {
                return null;
            }

            List<Card> cardsDealt = new List<Card>();

            int numberOfCardsAvail = Math.Min(count, cards.Count);
            for(int i = 0; i < numberOfCardsAvail; i++)
            {
                cardsDealt.Add(cards.Dequeue());
            }

            return cardsDealt;
        }

        public int Count()
        {
            return cards.Count();
        }

        public void ClearDeck()
        {
            cards.Clear();
        }
    }
}
