using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS
{
	public class Card
	{
		public enum Type { Treasure, Victory, Action }

		private string name;
		private Type type; //enum this later
		private int cost;

		public Card(string name, Type type, int cost)
		{
			this.name = name;
			this.type = type;
			this.cost = cost;
		}

		public void PrintCard()
		{
			Console.WriteLine("Name = " + name + ", Type = " + type + ", cost = " + cost + ".");
		}
	}
}
