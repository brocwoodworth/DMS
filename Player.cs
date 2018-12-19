using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	class Player
	{
		public Deck drawPile { get; set; }
		public Deck inHand { get; set; }
		public Deck inPlay { get; set; }
		public Deck discardPile { get; set; }
		private string PlayerName;

		public Player(string PlayerName)
		{
			this.drawPile = new Deck(Deck.State.drawPile, PlayerName);
			this.inHand = new Deck(Deck.State.inHand, PlayerName);
			this.inPlay = new Deck(Deck.State.inPlay, PlayerName);
			this.discardPile = new Deck(Deck.State.discardPile, PlayerName);
			this.PlayerName = PlayerName;
		}

		public void PrintPlayer()
		{
			drawPile.PrintDeck();
			inHand.PrintDeck();
			inPlay.PrintDeck();
			discardPile.PrintDeck();
		}
	}
}
