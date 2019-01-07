using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Deck
	{
		public enum State { None = 0, drawPile, inHand, inPlay, discardPile, inTrash, inKingdom, temp }

		public List<Card> Cards { get; set; }
		public State state;
		public string PlayerName;

		public Deck(State state, string PlayerName)
		{
			if (state == State.drawPile)
			{
				Cards = new List<Card>() { new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0),
										   new Card("Estate", Card.Expansion.Basic, Card.Type.Victory, 2),
										   new Card("Estate", Card.Expansion.Basic, Card.Type.Victory, 2),
										   new Card("Estate", Card.Expansion.Basic, Card.Type.Victory, 2)};
			}
			else
				Cards = new List<Card>();

			this.state = state;
			this.PlayerName = PlayerName;
		}

		public void PrintDeck()
		{
			if (state == State.inHand)
			{
				Console.WriteLine("\r\n" + PlayerName + "'s Hand: \r\n");

				if (Cards.Count > 0)
				{
					foreach (Card card in Cards)
						Console.WriteLine(card.PrintCard());
				}
			}
			else if (state != State.inKingdom)
			{
				Console.WriteLine("State of " + PlayerName + "'s deck: " + state + ".");

				if (Cards.Count > 0)
				{
					foreach (Card card in Cards)
						Console.WriteLine(card.PrintCard());
				}
			}
			else //if State.inKingdom
			{
				if (Cards.Count > 0)
				{
					if(Cards.Count < 10)
						Console.WriteLine("(Q:  " + Cards.Count + ") " + Cards[0].PrintCard());
					else
						Console.WriteLine("(Q: " + Cards.Count + ") " + Cards[0].PrintCard());
				}
				else
					Console.WriteLine("Empty Pile - " + PlayerName);
			}
		}
	}
}
