using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Card
	{
		public enum Type { Treasure, Victory, Action, Attack, Curse, Reaction }

		private string name;
		private List<Type> types;
		private int cost;

		public Card(string name, Type type, int cost, Type? type2 = null, Type? type3 = null)
		{
			this.name = name;
			types = new List<Type>();
			types.Add(type);
			this.cost = cost;

			//Some cards can have multiple types
			if (type2 != null)
				types.Add(type2.Value);
			if (type3 != null)
				types.Add(type3.Value);
		}

		public void PrintCard()
		{
			string type = "";
			foreach (Type cardType in types)
				type = type + " " + cardType;
			Console.WriteLine(name + "," + type + ", cost = " + cost + ".");
		}
	}
}
