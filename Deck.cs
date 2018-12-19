using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Deck
	{
		public enum State { drawPile, inHand, inPlay, discardPile, inTrash, inKingdom }

		public List<Card> Cards { get; set; }
		public State state;
		public string PlayerName;

		public Deck(State state, string PlayerName)
		{
			if (state == State.drawPile)
			{
				Cards = new List<Card>() { new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Copper", Card.Type.Treasure, 0),
										   new Card("Estate", Card.Type.Victory, 2),
										   new Card("Estate", Card.Type.Victory, 2),
										   new Card("Estate", Card.Type.Victory, 2)};
			}
			else
				Cards = new List<Card>();

			this.state = state;
			this.PlayerName = PlayerName;
		}

		public void PrintDeck()
		{
			Console.WriteLine("State of " + PlayerName + "'s deck: " + state + ".");

			if (Cards.Count > 0)
			{
				foreach (Card card in Cards)
					card.PrintCard();
				Console.WriteLine("\r\n");
			}
		}
	}
}
