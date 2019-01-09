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
			Kingdom.SetName chosenSet = Kingdom.SetName.Random;
			Console.Write("\r\nWelcome to Dominion Mimicing Software!\r\n\r\n Please choose a recommended Set of 10 from the list below, or anything else for Random.\r\n\r\n");
			Console.Write("[1]: First Game\r\n[2]: Size Distortion\r\n[3]: Deck Top\r\n[4]: Sleight of Hand\r\n[5]: Improvements\r\n[6]: Silver & Gold\r\n\r\n");
			string chosenSetInput = Console.ReadLine().ToLower();
			int intChosenSet = -1;
			if (Int32.TryParse(chosenSetInput, out intChosenSet))
			{
				if (intChosenSet >= 0 && intChosenSet <= 6)
					chosenSet = (Kingdom.SetName)intChosenSet;
			}
			Kingdom myKingdom = new Kingdom((Kingdom.SetName)intChosenSet, 1, false);
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
