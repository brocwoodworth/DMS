using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace DMS
{
	class Kingdom
	{
		public List<Deck> KingdomSupply { get; set; }
		private int maxCurses { get; set; }
		private int maxVictory { get; set; }

		public Kingdom(int? players = 2, bool colonyDay = false)
		{
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
			
			KingdomSupply = new List<Deck>();

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

			if(colonyDay)
			{
				KingdomSupply.Add(PlatinumSupply);
				KingdomSupply.Add(ColonySupply);
			}

			for (int i = 0; i < 30; i++)
			{
				Card copper = new Card("Copper", Card.Type.Treasure, 0);
				Card silver = new Card("Silver", Card.Type.Treasure, 3);
				Card gold = new Card("Gold", Card.Type.Treasure, 6);
				Card estate = new Card("Estate", Card.Type.Victory, 2);
				Card duchy = new Card("Duchy", Card.Type.Victory, 5);
				Card province = new Card("Province", Card.Type.Victory, 8);
				Card curse = new Card("Curse", Card.Type.Curse, 0);
				Card platinum = new Card("Platinum", Card.Type.Treasure, 9);
				Card colony = new Card("Colony", Card.Type.Victory, 11);

				if (i < maxVictory)
				{
					EstateSupply.Cards.Add(estate);
					DuchySupply.Cards.Add(duchy);
					ProvinceSupply.Cards.Add(province);
					if(colonyDay)
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

			Deck KingdomPile1 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile2 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile3 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile4 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile5 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile6 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile7 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile8 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile9 = new Deck(Deck.State.inKingdom, "");
			Deck KingdomPile10 = new Deck(Deck.State.inKingdom, "");

			KingdomSupply.Add(KingdomPile1);
			KingdomSupply.Add(KingdomPile2);
			KingdomSupply.Add(KingdomPile3);
			KingdomSupply.Add(KingdomPile4);
			KingdomSupply.Add(KingdomPile5);
			KingdomSupply.Add(KingdomPile6);
			KingdomSupply.Add(KingdomPile7);
			KingdomSupply.Add(KingdomPile8);
			KingdomSupply.Add(KingdomPile9);
			KingdomSupply.Add(KingdomPile10);

			Sets.BaseSet baseSet = new Sets.BaseSet();

			Card KingdomCard1 = baseSet.getRandomCard();
			Card KingdomCard2 = baseSet.getRandomCard();
			Card KingdomCard3 = baseSet.getRandomCard();
			Card KingdomCard4 = baseSet.getRandomCard();
			Card KingdomCard5 = baseSet.getRandomCard();
			Card KingdomCard6 = baseSet.getRandomCard();
			Card KingdomCard7 = baseSet.getRandomCard();
			Card KingdomCard8 = baseSet.getRandomCard();
			Card KingdomCard9 = baseSet.getRandomCard();
			Card KingdomCard10 = baseSet.getRandomCard();

			fillKingdomPile(KingdomCard1, KingdomPile1);
			fillKingdomPile(KingdomCard2, KingdomPile2);
			fillKingdomPile(KingdomCard3, KingdomPile3);
			fillKingdomPile(KingdomCard4, KingdomPile4);
			fillKingdomPile(KingdomCard5, KingdomPile5);
			fillKingdomPile(KingdomCard6, KingdomPile6);
			fillKingdomPile(KingdomCard7, KingdomPile7);
			fillKingdomPile(KingdomCard8, KingdomPile8);
			fillKingdomPile(KingdomCard9, KingdomPile9);
			fillKingdomPile(KingdomCard10, KingdomPile10);
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
	}
}
