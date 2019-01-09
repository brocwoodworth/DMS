using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Player
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
		public int SilverBonus;
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

		public bool HasActions()
		{
			foreach (Card card in inHand.Cards)
			{
				if (card.types.Contains(Card.Type.Action))
					return true;
			}
			return false;
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
			SilverBonus = 0;
			TurnComplete = false;
		}

		public void ActionPhase(Kingdom myKingdom)
		{
			bool readyForBuyPhase = false;

			do
			{
				int index = 1;
				ClearIndex(myKingdom);
				foreach (Card card in inHand.Cards)
				{
					if (card.types.Contains(Card.Type.Action))
						card.index = index++;
				}

				inHand.PrintDeck();
				Console.WriteLine("\r\nSelect Actions to play from your hand. [#] = Play specific action. [D] = Done playing actions.");
				Console.WriteLine("Actions:" + Actions + ", Buys: " + Buys + ", Dollars: " + Dollars + ".");

				string playActionInput = Console.ReadLine().ToLower();

				if (playActionInput == "d")
					readyForBuyPhase = true;
				else
				{
					int intplayActionInput = -1;
					if (Int32.TryParse(playActionInput, out intplayActionInput))
					{
						if (intplayActionInput > 0 && intplayActionInput < index)
						{
							for (int i = 0; i < inHand.Cards.Count; i++)
							{
								if (inHand.Cards[i].index == intplayActionInput)
								{
									PlayAction(inHand.Cards[i], myKingdom);
									Actions--;
									break;
								}
							}
						}
						else
							Console.WriteLine("Invalid Input.");
					}
					else
						Console.WriteLine("Invalid Input.");
				}

				if (Actions <= 0 || !HasActions())
					readyForBuyPhase = true;
			} while (!readyForBuyPhase);
		}

		public void BuyPhase(Kingdom myKingdom)
		{
			bool readyToBuy = false;
			int index = 1;
			ClearIndex(myKingdom);
			foreach (Card card in inHand.Cards)
			{
				if(card.types.Contains(Card.Type.Treasure))
					card.index = index++;
			}

			do
			{
				inHand.PrintDeck();
				Console.WriteLine("\r\nSelect Treasures to play from your hand. [A] = All. [#] = Play specific treasure. [D] = Done playing treasures. [T] = Print trash.");
				Console.WriteLine("Buys: " + Buys + ", Dollars: " + Dollars + ".");
				
				string playTreasureInput = Console.ReadLine().ToLower();
				if (playTreasureInput == "a")
				{
					for (int i = inHand.Cards.Count - 1; i >= 0; i--)
					{
						if (inHand.Cards[i].types.Contains(Card.Type.Treasure))
							PlayTreasure(inHand.Cards[i]);
					}
					readyToBuy = true;
				}
				else if (playTreasureInput == "d")
					readyToBuy = true;
				else if (playTreasureInput == "t")
					myKingdom.PrintTrash();
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
									PlayTreasure(inHand.Cards[i]);
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
				Console.WriteLine("\r\nChoose a card from the kingdom to buy. [N] for none. Buys: " + Buys + ", Dollars: " + Dollars + ".");
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
				else if (buyInput == "n")
					doneBuying = true;
				else
					Console.WriteLine("Invalid Input.");
			}
			TurnComplete = true;
		}

		public int CalculatePoints()
		{
			int points = 0;
			foreach (Card card in inHand.Cards)
				points = points + card.Points(this);
			foreach (Card card in inPlay.Cards)
				points = points + card.Points(this);
			foreach (Card card in discardPile.Cards)
				points = points + card.Points(this);
			foreach (Card card in drawPile.Cards)
				points = points + card.Points(this);

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

			ClearIndex(myKingdom);

			Turns++;

			DrawCard(5);
			Buys = 1;
			Actions = 1;
			Dollars = 0;
		}

		public void ClearIndex(Kingdom myKingdom)
		{
			foreach (Card card in discardPile.Cards)
				card.index = -1;

			foreach (Card card in drawPile.Cards)
				card.index = -1;

			foreach (Deck deck in myKingdom.KingdomSupply)
			{
				if (deck.Cards.Count > 0)
					deck.Cards[0].index = -1;
			}
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

		public void HandToTopOfDrawPile(Card card)
		{
			drawPile.Cards.Insert(0, card);
			inHand.Cards.Remove(card);
		}

		public void GainCard(Card card, Deck deck, Deck.State state = 0)
		{
			if (state == 0 || state == Deck.State.discardPile)
				discardPile.Cards.Add(card);
			else if (state == Deck.State.inHand)
				inHand.Cards.Add(card);
			else if (state == Deck.State.drawPile)
				drawPile.Cards.Insert(0, card);
			deck.Cards.Remove(card);
		}

		public void Trash(Card card, Kingdom myKingdom)
		{
			if (inPlay.Cards.Contains(card))
				inPlay.Cards.Remove(card);
			if (inHand.Cards.Contains(card))
				inHand.Cards.Remove(card);
			if (discardPile.Cards.Contains(card))
				discardPile.Cards.Remove(card);
			if (drawPile.Cards.Contains(card))
				drawPile.Cards.Remove(card);
			myKingdom.Trash.Cards.Add(card);
		}

		public void PlayTreasure(Card card)
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
						if(SilverBonus > 0)
						{
							Dollars = Dollars + SilverBonus;
							SilverBonus = 0;
						}
						break;
					case "Gold":
						Dollars = Dollars + 3;
						break;
					default:
						break;
				}
			}
		}

		public void PlayAction(Card card, Kingdom myKingdom)
		{
			if (card.types.Contains(Card.Type.Action))
			{
				switch (card.expansion)
				{
					case Card.Expansion.BaseSet:
						inHand.Cards.Remove(card);
						inPlay.Cards.Add(card);
						PlayBaseSetAction(card, myKingdom);
						break;
				}
			}
		}

		public void PlayBaseSetAction(Card card, Kingdom myKingdom)
		{
			Sets.BaseSet baseSet = new Sets.BaseSet();
			baseSet.PlayAction(card, this, myKingdom);
		}

		public void AddDollars(int dollars)
		{
			Dollars = Dollars + dollars;
		}

		public void AddActions(int actions)
		{
			Actions = Actions + actions;
		}

		public void AddBuys(int buys)
		{
			Buys = Buys + buys;
		}
		public void GainCardByName(string cardName, bool optional, Kingdom myKingdom, Deck.State state = Deck.State.discardPile)
		{
			foreach(Deck deck in myKingdom.KingdomSupply)
			{
				if(deck.Cards.Count > 0)
				{
					if(deck.Cards[0].name == cardName)
					{
						Card cardToGain = deck.Cards[deck.Cards.Count - 1];
						GainCard(cardToGain, deck, state);
						return;
					}
				}
			}
			Console.WriteLine("\r\nNo " + cardName + " available.\r\n");
		}

		public void GainCardByCost(int cost, bool exactCost, bool optional, Kingdom myKingdom, Deck.State state = Deck.State.discardPile, Card.Type type = 0)
		{
			ClearIndex(myKingdom);
			bool done = false;
			int index = 1;
			foreach (Deck deck in myKingdom.KingdomSupply)
			{
				if (deck.Cards.Count > 0)
				{
					if ((deck.Cards[0].cost <= cost && !exactCost) || (deck.Cards[0].cost == cost && exactCost))
					{
						if (type == 0 || deck.Cards[0].types.Contains(type))
							deck.Cards[0].index = index++;
					}
				}
			}
			myKingdom.PrintKingdom();
			string optionalText = "";
			if (optional)
				optionalText = " Or, press [n] to gain no card.";
			if (exactCost)
				Console.WriteLine("\r\nChoose a card from the kingdom to gain costing exactly " + cost + "." + optionalText);
			else
				Console.WriteLine("\r\nChoose a card from the kingdom to gain costing up to " + cost + "." + optionalText);

			do
			{
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
								GainCard(cardToGain, deck, state);
								done = true;
							}
						}
					}
					else
						Console.WriteLine("Invalid Input.");
				}
				else if (optional && buyInput == "n")
					done = true;
				else
					Console.WriteLine("Invalid Input");
			} while (!done);
		}
	}
}
