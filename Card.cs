using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Card
	{
		public enum Type { Treasure, Victory, Action, Attack, Curse, Reaction } //, Duration, Knight, Looter, Prize, Reserve, Ruins, Shelter, Traveller, Castle, Gathering, Event, Landmark, Artifact, Project }

		public string name;
		public List<Type> types;
		public int cost;
		public int index;

		public Card(string name, Type type, int cost, Type? type2 = null, Type? type3 = null, Type? type4 = null)
		{
			this.name = name;
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

		public int Points()
		{
			switch (name)
			{
				case "Estate":
					return 1;
				case "Duchy":
					return 3;
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
