using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace DMS
{
	public class Kingdom
	{
		public enum SetName {Random = 0, FirstGame, SizeDistortion, DeckTop, SleightOfHand, Improvements, SilverAndGold };
		public List<Deck> KingdomSupply { get; set; }
		private int maxCurses { get; set; }
		private int maxVictory { get; set; }
		public Deck Trash { get; set; }

		public Kingdom(SetName setName = 0, int? players = 2, bool colonyDay = false)
		{
			Trash = new Deck(Deck.State.inTrash, "KINGDOM");
			KingdomSupply = new List<Deck>();
			this.maxCurses = 10;
			this.maxVictory = 8;

			if(players == 3)
			{
				this.maxCurses = 20;
				this.maxVictory = 12;
			}
			else if(players == 4)
			{
				this.maxCurses = 30;
				this.maxVictory = 12;
			}

			populateBasicCards(KingdomSupply, colonyDay);
			populateNonBasicCards(KingdomSupply, setName);

			//Sort by cost for easier reading
			KingdomSupply = KingdomSupply.OrderBy(o => o.Cards[0].cost).ToList();
		}

		private void populateBasicCards(List<Deck> KingdomSupply, bool colonyDay = false)
		{
			Deck CopperSupply = new Deck(Deck.State.inKingdom, "Copper");
			Deck SilverSupply = new Deck(Deck.State.inKingdom, "Silver");
			Deck GoldSupply = new Deck(Deck.State.inKingdom, "Gold");
			Deck EstateSupply = new Deck(Deck.State.inKingdom, "Estate");
			Deck DuchySupply = new Deck(Deck.State.inKingdom, "Duchy");
			Deck ProvinceSupply = new Deck(Deck.State.inKingdom, "Province");
			Deck CurseSupply = new Deck(Deck.State.inKingdom, "Curse");
			Deck PlatinumSupply = new Deck(Deck.State.inKingdom, "Platinum");
			Deck ColonySupply = new Deck(Deck.State.inKingdom, "Colony");

			KingdomSupply.Add(CopperSupply);
			KingdomSupply.Add(SilverSupply);
			KingdomSupply.Add(GoldSupply);
			KingdomSupply.Add(EstateSupply);
			KingdomSupply.Add(DuchySupply);
			KingdomSupply.Add(ProvinceSupply);
			KingdomSupply.Add(CurseSupply);

			if (colonyDay)
			{
				KingdomSupply.Add(PlatinumSupply);
				KingdomSupply.Add(ColonySupply);
			}

			for (int i = 0; i < 30; i++)
			{
				Card copper = new Card("Copper", Card.Expansion.Basic, Card.Type.Treasure, 0);
				Card silver = new Card("Silver", Card.Expansion.Basic, Card.Type.Treasure, 3);
				Card gold = new Card("Gold", Card.Expansion.Basic, Card.Type.Treasure, 6);
				Card estate = new Card("Estate", Card.Expansion.Basic, Card.Type.Victory, 2);
				Card duchy = new Card("Duchy", Card.Expansion.Basic, Card.Type.Victory, 5);
				Card province = new Card("Province", Card.Expansion.Basic, Card.Type.Victory, 8);
				Card curse = new Card("Curse", Card.Expansion.Basic, Card.Type.Curse, 0);
				Card platinum = new Card("Platinum", Card.Expansion.Basic, Card.Type.Treasure, 9);
				Card colony = new Card("Colony", Card.Expansion.Basic, Card.Type.Victory, 11);

				if (i < maxVictory)
				{
					EstateSupply.Cards.Add(estate);
					DuchySupply.Cards.Add(duchy);
					ProvinceSupply.Cards.Add(province);
					if (colonyDay)
						ColonySupply.Cards.Add(colony);
				}
				if (i < maxCurses)
					CurseSupply.Cards.Add(curse);
				if (i < 12 && colonyDay)
					PlatinumSupply.Cards.Add(platinum);

				CopperSupply.Cards.Add(copper);
				SilverSupply.Cards.Add(silver);
				GoldSupply.Cards.Add(gold);
			}
		}

		private void populateNonBasicCards(List<Deck> KingdomSupply, SetName setName)
		{
			Sets.BaseSet baseSet = new Sets.BaseSet();
			List<Card> KingdomSupplyTemplate = new List<Card>();

			switch (setName)
			{
				case SetName.Random:
					for (int i = 0; i < 10; i++)
					{
						//Uncomment to test a specific card
						if (i == 0)
							KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Spy));
						else
							KingdomSupplyTemplate.Add(baseSet.getBaseSetCard());
					}
					break;
				case SetName.FirstGame:
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Cellar));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Market));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Merchant));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Militia));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Mine));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Moat));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Remodel));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Smithy));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Village));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Workshop));
					break;
				case SetName.SizeDistortion:
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Artisan));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Bandit));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Bureaucrat));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Chapel));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Festival));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Gardens));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Sentry));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.ThroneRoom));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Witch));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Workshop));
					break;
				case SetName.DeckTop:
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Artisan));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Bureaucrat));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.CouncilRoom));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Festival));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Harbinger));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Laboratory));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Moneylender));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Sentry));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Vassal));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Village));
					break;
				case SetName.SleightOfHand:
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Cellar));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.CouncilRoom));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Festival));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Gardens));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Library));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Harbinger));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Militia));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Poacher));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Smithy));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.ThroneRoom));
					break;
				case SetName.Improvements:
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Artisan));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Cellar));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Market));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Merchant));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Mine));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Moat));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Moneylender));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Poacher));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Remodel));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Witch));
					break;
				case SetName.SilverAndGold:
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Bandit));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Bureaucrat));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Chapel));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Harbinger));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Laboratory));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Merchant));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Mine));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Moneylender));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.ThroneRoom));
					KingdomSupplyTemplate.Add(baseSet.getBaseSetCard(Sets.BaseSet.BaseSetCards.Vassal));
					break;
			}

			foreach (Card card in KingdomSupplyTemplate)
			{
				Deck KingdomPile = new Deck(Deck.State.inKingdom, "");
				KingdomSupply.Add(KingdomPile);
				fillKingdomPile(card, KingdomPile);
			}
		}

		private void fillKingdomPile(Card card, Deck deck)
		{
			int maxFill = 10;
			if(card.types.Contains(Card.Type.Victory))
				maxFill = maxVictory;

			for (int i = 0; i < maxFill; i++)
				deck.Cards.Add(card);

			deck.PlayerName = card.name;
		}

		public bool GameOver(int? numPlayers = null)
		{
			int requiredPiles = 3;//number of piles that need to be empty to end the game (3 piles for 1-4 players, 4 piles for 5-6 players)
			if (numPlayers > 4)
				requiredPiles = 4;

			int emptyPiles = 0;
			foreach (Deck deck in KingdomSupply)
			{
				if (deck.Cards.Count == 0)
				{
					if (deck.PlayerName == "Province" || deck.PlayerName == "Colony")
						return true;
					else
						emptyPiles++;
				}
			}

			if (emptyPiles >= requiredPiles)
				return true;

			return false;
		}

		public void PrintKingdom()
		{
			Console.WriteLine("\r\n--- Kingdom Cards ---\r\n");
			foreach (Deck deck in KingdomSupply)
				deck.PrintDeck();
		}

		public void PrintTrash()
		{
			Console.WriteLine("\r\n--- Trash Pile ---\r\n");
			if (Trash.Cards.Count > 0)
				Trash.PrintDeck();
			else
				Console.WriteLine("--- (Empty) ---\r\n");
		}
	}
}
