using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Card
	{
		public enum Type { None = 0, Treasure, Victory, Action, Attack, Curse, Reaction } //, Duration, Knight, Looter, Prize, Reserve, Ruins, Shelter, Traveller, Castle, Gathering, Event, Landmark, Artifact, Project }
		public enum Expansion { Basic, BaseSet }

		public string name;
		public List<Type> types;
		public int cost;
		public int index;
		public Expansion expansion;

		public Card(string name, Expansion expansion, Type type, int cost, Type? type2 = null, Type? type3 = null, Type? type4 = null)
		{
			this.name = name;
			this.expansion = expansion;
			types = new List<Type>();
			types.Add(type);
			this.cost = cost;
			this.index = -1;

			//Some cards can have multiple types
			if (type2 != null)
				types.Add(type2.Value);
			if (type3 != null)
				types.Add(type3.Value);
			if (type4 != null)
				types.Add(type4.Value);
		}

		//just make a default card that will be modified in some way
		public Card()
		{
		}

		public int Points(Player player)
		{
			switch (name)
			{
				case "Estate":
					return 1;
				case "Duchy":
					return 3;
				case "Gardens":
					return (player.inHand.Cards.Count + player.inPlay.Cards.Count + player.discardPile.Cards.Count + player.drawPile.Cards.Count) / 10;
				case "Province":
					return 6;
				case "Curse":
					return -1;
				default:
					return 0;
			}
		}

		public string PrintCard()
		{
			string type = types[0].ToString();
			if (types.Count > 1)
			{
				for (int i = 1; i < types.Count; i++)
					type = type + " - " + types[i].ToString();
			}
			string returnValue = name + ", " + type + ", cost = " + cost + " ";
			if (index != -1)
				returnValue = "(#" + index + ") " + returnValue;
			return returnValue;
		}
	}
}
