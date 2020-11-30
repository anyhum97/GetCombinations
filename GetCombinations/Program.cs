using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Combinations
{
	partial class Program
	{
		public const int Nothing = 0;
		public const int OnePair = 1;
		public const int TwoPairs = 2;
		public const int Trips = 3;
		public const int Straight = 4;
		public const int Flush = 5;
		public const int FullHouse = 6;
		public const int Quads = 7;
		public const int StraightFlush = 8;

		private static Random StaticRandom = new Random();

		public static string GetCombinationTitle(int combination)
		{
			switch(combination)
			{
				case Nothing: return "Nothing";
				case OnePair: return "One Pair";
				case TwoPairs: return "Two Pairs";
				case Trips: return "Trips";
				case Straight: return "Straight";
				case Flush: return "Flush";
				case FullHouse: return "Full House";
				case Quads: return "Quads";
				case StraightFlush: return "Straight Flush";
			}

			return "Invalid Combination";
		}

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

		private static void GetCardsTest()
		{
			int a = 0;

			for(int i=0; i<1000000; ++i)
			{
				int[] cards = GetCards();

				ulong board3 = Data.DefaultCardMask[cards[0]] | Data.DefaultCardMask[cards[1]] | Data.DefaultCardMask[cards[2]];

				ulong board4 = board3 | Data.DefaultCardMask[cards[3]];

				ulong board5 = board4 | Data.DefaultCardMask[cards[4]];

				ulong hand1 = Data.DefaultCardMask[cards[5]] | Data.DefaultCardMask[cards[6]];

				ulong hand2 = Data.DefaultCardMask[cards[7]] | Data.DefaultCardMask[cards[8]];

				int flop1 = Data.GetCombination(board3, hand1);
				int turn1 = Data.GetCombination(board4, hand1);
				int river1 = Data.GetCombination(board5, hand1);

				int flop2 = Data.GetCombination(board3, hand2);
				int turn2 = Data.GetCombination(board4, hand2);
				int river2 = Data.GetCombination(board5, hand2);

				uint rang1 = Data.GetTexasHoldemCombinationRank(board5 | hand1);
				uint rang2 = Data.GetTexasHoldemCombinationRank(board5 | hand2);

				if((board5 & hand1) != 0)
				{
					throw new Exception();
				}

				if((board5 & hand2) != 0)
				{
					throw new Exception();
				}
			}

			int b = 0;
		}

		private static void Test()
		{
			for(int i=0; i<10000000; ++i)
			{
				int[] cards = GetCards();

				ulong board3 = Data.DefaultCardMask[cards[0]] | Data.DefaultCardMask[cards[1]] | Data.DefaultCardMask[cards[2]];

				ulong board4 = board3 | Data.DefaultCardMask[cards[3]];

				ulong board5 = board4 | Data.DefaultCardMask[cards[4]];

				ulong hand1 = Data.DefaultCardMask[cards[5]] | Data.DefaultCardMask[cards[6]];

				ulong hand2 = Data.DefaultCardMask[cards[7]] | Data.DefaultCardMask[cards[8]];

				int flop1 = Data.GetCombination(board3, hand1);
				int turn1 = Data.GetCombination(board4, hand1);
				int river1 = Data.GetCombination(board5, hand1);

				int flop2 = Data.GetCombination(board3, hand2);
				int turn2 = Data.GetCombination(board4, hand2);
				int river2 = Data.GetCombination(board5, hand2);

				uint rang1 = Data.GetTexasHoldemCombinationRank(board5 | hand1);
				uint rang2 = Data.GetTexasHoldemCombinationRank(board5 | hand2);

				if((board5 & hand1) != 0)
				{
					throw new Exception();
				}

				if((board5 & hand2) != 0)
				{
					throw new Exception();
				}
				
				bool show = false;
				
				//show = show || (rang1 > rang2 && river1 < river2);
				//
				//show = show || (rang2 > rang1 && river2 < river1);
				//
				//show = show || (rang2 == rang1 && river2 != river1);

				show = show || (flop1 == TwoPairs || turn1 == TwoPairs ||  river1 == TwoPairs);

				show = show || (flop2 == TwoPairs || turn2 == TwoPairs ||  river2 == TwoPairs);

				if(show)
				{
					Card BoardCard1 = Card.DefaultCards[cards[0]];
					Card BoardCard2 = Card.DefaultCards[cards[1]];
					Card BoardCard3 = Card.DefaultCards[cards[2]];
					Card BoardCard4 = Card.DefaultCards[cards[3]];
					Card BoardCard5 = Card.DefaultCards[cards[4]];

					Card Player1Card1 = Card.DefaultCards[cards[5]];
					Card Player1Card2 = Card.DefaultCards[cards[6]];

					Card Player2Card1 = Card.DefaultCards[cards[7]];
					Card Player2Card2 = Card.DefaultCards[cards[8]];

					Console.Clear();

					Console.Write("Flop: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + "\n\n");

					Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(flop1) + ")\n\n");

					Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(flop2) + ")\n\n");

					Console.Write("Turn: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + "\n\n");

					Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(turn1) + ")\n\n");

					Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(turn2) + ")\n\n");

					Console.Write("River: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + " " + BoardCard5 + "\n\n");

					Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(river1) + ")\n\n");

					Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(river2) + ")\n\n");

					if(rang1 > rang2)
					{
						Console.Write("Player1 Wins\n\n");
					}

					if(rang1 == rang2)
					{
						Console.Write("Tie\n\n");
					}

					if(rang2 > rang1)
					{
						Console.Write("Player2 Wins\n\n");
					}

					Console.ReadKey();
				}
			}
		}

		private static void Main()
		{
			Test();
		}
	}
}

