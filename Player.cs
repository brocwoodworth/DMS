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
		public int Actions;
		public int Buys;
		public string PlayerName;

		public Player(string PlayerName)
		{
			this.drawPile = new Deck(Deck.State.drawPile, PlayerName);
			this.inHand = new Deck(Deck.State.inHand, PlayerName);
			this.inPlay = new Deck(Deck.State.inPlay, PlayerName);
			this.discardPile = new Deck(Deck.State.discardPile, PlayerName);
			this.PlayerName = PlayerName;
			Shuffle();
			DrawCard(5);
		}

		public void PrintPlayer()
		{
			drawPile.PrintDeck();
			inHand.PrintDeck();
			inPlay.PrintDeck();
			discardPile.PrintDeck();
		}

		public void PrintHand()
		{
			inHand.PrintDeck();
		}

		//Moves all cards in discard pile to draw pile, then shuffle
		public void Shuffle()
		{
			Random rng = new Random();

			if(discardPile.Cards.Count > 0)
			{
				foreach (Card card in discardPile.Cards)
					drawPile.Cards.Add(card);
				discardPile.Cards = new List<Card>();
			}

			int n = drawPile.Cards.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				Card value = drawPile.Cards[k];
				drawPile.Cards[k] = drawPile.Cards[n];
				drawPile.Cards[n] = value;
			}
		}

		public void DrawCard(int? numberOfDraws = null)
		{
			if (numberOfDraws == null)
				numberOfDraws = 1;
			for(int i = 0; i < numberOfDraws; i++)
			{
				if (drawPile.Cards.Count == 0 && discardPile.Cards.Count != 0)
					Shuffle();
				if (drawPile.Cards.Count != 0)
				{
					Card card = drawPile.Cards[0];
					inHand.Cards.Add(card);
					drawPile.Cards.Remove(card);
				}
			}
		}


	}
}
