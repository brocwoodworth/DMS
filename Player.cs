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
		public string PlayerName;
		public int Actions;
		public int Buys;
		public int Dollars;
		public int Turns;
		public bool TurnComplete;

		public Player(string PlayerName)
		{
			this.drawPile = new Deck(Deck.State.drawPile, PlayerName);
			this.inHand = new Deck(Deck.State.inHand, PlayerName);
			this.inPlay = new Deck(Deck.State.inPlay, PlayerName);
			this.discardPile = new Deck(Deck.State.discardPile, PlayerName);
			this.PlayerName = PlayerName;
			this.Turns = 0;
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

		public void StartTurn()
		{
			Actions = 1;
			Buys = 1;
			Dollars = 0;
			TurnComplete = false;
		}

		public void BuyPhase(Kingdom myKingdom)
		{
			bool readyToBuy = false;
			int index = 1;
			foreach (Card card in inHand.Cards)
			{
				if(card.types.Contains(Card.Type.Treasure))
					card.index = index++;
			}

			do
			{
				inHand.PrintDeck();
				Console.WriteLine("\r\nSelect Treasures to play from your hand. [A] = All. [#] = Play specific treasure. [D] = Done playing treasures.");
				Console.WriteLine("Buys: " + Buys + ", Dollars: " + Dollars + ".");
				
				string playTreasureInput = Console.ReadLine().ToLower();
				if (playTreasureInput == "a")
				{
					for (int i = inHand.Cards.Count - 1; i >= 0; i--)
					{
						if (inHand.Cards[i].types.Contains(Card.Type.Treasure))
							PlayCard(inHand.Cards[i]);
					}
					readyToBuy = true;
				}
				else if (playTreasureInput == "d")
					readyToBuy = true;
				else
				{
					int intPlayTreasureInput = -1;
					if (Int32.TryParse(playTreasureInput, out intPlayTreasureInput))
					{
						if (intPlayTreasureInput > 0 && intPlayTreasureInput < index)
						{
							for (int i = 0; i < inHand.Cards.Count; i++)
							{
								if (inHand.Cards[i].index == intPlayTreasureInput)
									PlayCard(inHand.Cards[i]);
							}
						}
						else
							Console.WriteLine("Invalid Input.");
					}
					else
						Console.WriteLine("Invalid Input.");
				}
			} while (!readyToBuy);

			//Ready to buy, reset index
			index = 1;
			bool doneBuying = false;
			foreach (Deck deck in myKingdom.KingdomSupply)
			{
				if(deck.Cards.Count > 0)
				{
					if(deck.Cards[0].cost <= Dollars)
						deck.Cards[0].index = index++;
				}
					
			}
			myKingdom.PrintKingdom();
			while(!doneBuying)
			{
				Console.WriteLine("\r\nChoose a card from the kingdom to buy. Buys: " + Buys + ", Dollars: " + Dollars + ".");
				string buyInput = Console.ReadLine().ToLower();
				int intBuyInput = -1;
				if (Int32.TryParse(buyInput, out intBuyInput))
				{
					if (intBuyInput > 0 && intBuyInput < index)
					{
						foreach (Deck deck in myKingdom.KingdomSupply)
						{
							if (deck.Cards[0].index == intBuyInput)
							{
								Card cardToGain = deck.Cards[deck.Cards.Count - 1];
								GainCard(cardToGain, deck);
								Dollars = Dollars - cardToGain.cost;
								Buys = Buys - 1;
								if (Buys == 0)
									doneBuying = true;
							}
						}
					}
					else
						Console.WriteLine("Invalid Input.");
				}
				else
					Console.WriteLine("Invalid Input.");
			}
			TurnComplete = true;
		}

		public int CalculatePoints()
		{
			int points = 0;
			foreach (Card card in inHand.Cards)
				points = points + card.Points();
			foreach (Card card in inPlay.Cards)
				points = points + card.Points();
			foreach (Card card in discardPile.Cards)
				points = points + card.Points();
			foreach (Card card in drawPile.Cards)
				points = points + card.Points();

			return points;
		}

		//Discard all cards in hand and all cards in play. Draw 5 new cards (and shuffle if necessary)
		//Set number of buys and actions to 1, and dollars to 0
		//Reset index on all cards
		public void Cleanup(Kingdom myKingdom)
		{
			for(int i = inHand.Cards.Count - 1; i >= 0; i--)
				Discard(inHand.Cards[i]);
			for (int i = inPlay.Cards.Count - 1; i >= 0; i--)
				ClearFromPlay(inPlay.Cards[i]);

			foreach (Card card in discardPile.Cards)
				card.index = -1;

			foreach (Card card in drawPile.Cards)
				card.index = -1;

			foreach (Deck deck in myKingdom.KingdomSupply)
			{
				if (deck.Cards.Count > 0)
					deck.Cards[0].index = -1;
			}

			Turns++;

			DrawCard(5);
			Buys = 1;
			Actions = 1;
			Dollars = 0;
		}

		public void Discard(Card card)
		{
			inHand.Cards.Remove(card);
			discardPile.Cards.Add(card);
		}

		public void ClearFromPlay(Card card)
		{
			inPlay.Cards.Remove(card);
			discardPile.Cards.Add(card);
		}

		public void GainCard(Card card, Deck deck)
		{
			discardPile.Cards.Add(card);
			deck.Cards.Remove(card);
		}

		public void PlayCard(Card card)
		{
			if (card.types.Contains(Card.Type.Treasure))
			{
				inHand.Cards.Remove(card);
				inPlay.Cards.Add(card);
				switch (card.name)
				{
					case "Copper":
						Dollars++;
						break;
					case "Silver":
						Dollars = Dollars + 2;
						break;
					case "Gold":
						Dollars = Dollars + 3;
						break;
					default:
						break;
				}
			}
		}
	}
}
