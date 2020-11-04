using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Combinations
{
	partial class Program
	{
		private static Random StaticRandom = new Random();

		private static int[] GetCards()
		{
			bool[] positions = new bool[52];

			int[] cards = new int[9];

			for(int i=0; i<9; ++i)
			{
				int index = StaticRandom.Next(52);

				while(positions[index])
				{
					index = StaticRandom.Next(52);
				}

				positions[index] = true;
				
				cards[i] = index;
			}

			return cards;
		}

		private static void GetTexasHoldemCombinationRank()
		{
			Hand hand1 = new Hand("KsJc");
			Hand hand2 = new Hand("2d8c");

			Card card1 = new Card("Td");
			Card card2 = new Card("5d");
			Card card3 = new Card("Ah");
			Card card4 = new Card("5c");
			Card card5 = new Card("8s");

			ulong mask1 = hand1.Mask | card1.Mask | card2.Mask | card3.Mask | card4.Mask | card5.Mask;
			ulong mask2 = hand2.Mask | card1.Mask | card2.Mask | card3.Mask | card4.Mask | card5.Mask;

			uint rang1 = Data.GetTexasHoldemCombinationRank(mask1);
			uint rang2 = Data.GetTexasHoldemCombinationRank(mask2);

			if(rang1 > rang2)
			{
				Console.WriteLine("[1]");
			}

			if(rang1 == rang2)
			{
				Console.WriteLine("[=]");
			}

			if(rang1 < rang2)
			{
				Console.WriteLine("[2]");
			}

			Console.ReadKey();
		}

		private static void Main()
		{
			
		}
	}
}

