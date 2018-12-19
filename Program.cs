using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DMS
{
	public class Program
	{
		static void Main(string[] args)
		{
			/*string testString;
			Console.Write("Enter a string - ");
			testString = Console.ReadLine();
			Console.WriteLine("You entered '{0}'", testString);*/

			bool gameOver = false;

			Player Broc = new Player("Broc");
			do
			{
				Broc.PrintPlayer();
				Console.Write("\r\nPress Q to quit...");
				string testString = Console.ReadLine();
				if (testString == "Q" || testString == "q")
					gameOver = true;
			} while (!gameOver);
		}
	}
}
