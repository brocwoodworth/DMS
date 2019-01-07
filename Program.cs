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
			bool gameOver = false;

			Player Broc = new Player("Broc");
			Kingdom myKingdom = new Kingdom(1, false);
			do
			{
				Broc.StartTurn();
				do
				{
					myKingdom.PrintKingdom();
					if (Broc.HasActions())
					{
						Broc.PrintHand();
						Console.Write("\r\nA to play an action. B to go to buy phase. T to print Trash. H for help. Q to quit.\r\n");
						Console.Write("Actions: " + Broc.Actions + ", Buys: " + Broc.Buys + ", Dollars: " + Broc.Dollars + ".\r\n");
						switch (Console.ReadLine().ToLower())
						{
							case "a":
								Broc.ActionPhase(myKingdom);
								break;
							case "b":
								Broc.BuyPhase(myKingdom);
								break;
							case "t":
								myKingdom.PrintTrash();
								break;
							case "printplayer":
								Broc.PrintPlayer();
								break;
							case "q":
								gameOver = true;
								break;
						}
					}
					else
						Broc.BuyPhase(myKingdom);
				} while (!Broc.TurnComplete && !gameOver);

				Broc.Cleanup(myKingdom);

			} while (!gameOver && !myKingdom.GameOver());

			Console.WriteLine("Game Over! " + Broc.PlayerName + " won with " + Broc.CalculatePoints() + " points, taking " + Broc.Turns + " turns.\r\nPress enter to continue...");
			Console.ReadLine();
		}
	}
}
