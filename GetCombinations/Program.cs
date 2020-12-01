using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using static Combinations.Data;

namespace Combinations
{
	partial class Program
	{
		private static Random StaticRandom = new Random();

		public static string GetCombinationTitle(uint combination)
		{
			switch(combination)
			{
				case Nothing: return "Nothing";
				case HighCard: return "A High";
				case WeakPair: return "Weak Pair";
				case MiddlePair: return "Middle Pair";
				case HighPair: return "High Pair";
				case TwoPairs: return "Two Pairs";
				case Trips: return "Trips";
				case Set: return "Set";
				case Straight: return "Straight";
				case Flush: return "Flush";
				case FullHouse: return "Full House";
				case Nuts: return "Nuts";
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

				ulong board3 = DefaultCardMask[cards[0]] | DefaultCardMask[cards[1]] | DefaultCardMask[cards[2]];

				ulong board4 = board3 | DefaultCardMask[cards[3]];

				ulong board5 = board4 | DefaultCardMask[cards[4]];

				ulong hand1 = DefaultCardMask[cards[5]] | DefaultCardMask[cards[6]];

				ulong hand2 = DefaultCardMask[cards[7]] | DefaultCardMask[cards[8]];

				uint flop1 = GetCombination(board3, hand1);
				uint turn1 = GetCombination(board4, hand1);
				uint river1 = GetCombination(board5, hand1);

				uint flop2 = GetCombination(board3, hand2);
				uint turn2 = GetCombination(board4, hand2);
				uint river2 = GetCombination(board5, hand2);

				uint rang1 = GetTexasHoldemCombinationRank(board5 | hand1);
				uint rang2 = GetTexasHoldemCombinationRank(board5 | hand2);

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

				ulong board3 = DefaultCardMask[cards[0]] | DefaultCardMask[cards[1]] | DefaultCardMask[cards[2]];

				ulong board4 = board3 | DefaultCardMask[cards[3]];

				ulong board5 = board4 | DefaultCardMask[cards[4]];

				ulong hand1 = DefaultCardMask[cards[5]] | DefaultCardMask[cards[6]];

				ulong hand2 = DefaultCardMask[cards[7]] | DefaultCardMask[cards[8]];

				uint Player1Street1 = GetCombination(board3, hand1);
				uint Player1Street2 = GetCombination(board4, hand1);
				uint Player1Street3 = GetCombination(board5, hand1);

				uint Player2Street1 = GetCombination(board3, hand2);
				uint Player2Street2 = GetCombination(board4, hand2);
				uint Player2Street3 = GetCombination(board5, hand2);

				uint rang1 = GetTexasHoldemCombinationRank(board5 | hand1);
				uint rang2 = GetTexasHoldemCombinationRank(board5 | hand2);

				if((board5 & hand1) != 0)
				{
					throw new Exception();
				}

				if((board5 & hand2) != 0)
				{
					throw new Exception();
				}
				
				if((hand1 & hand2) != 0)
				{
					throw new Exception();
				}

				bool show = true;

				bool levels = false;

				bool win1 = rang1 > rang2;
				bool win2 = rang2 > rang1;
				bool tie = rang1 == rang2;

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

					if(levels)
					{
						Console.Clear();

						Console.Write("Flop: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + "\n\n");

						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street1) + ")\n\n");

						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street1) + ")\n\n");

						Console.Write("FlushLevel: " + GetBoardFlushLevel(board3) + "\n\n");

						Console.Write("StraightLevel: " + GetBoardStraightLevel(board3) + "\n\n");

						Console.Write("Turn: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + "\n\n");

						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street2) + ")\n\n");

						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street2) + ")\n\n");

						Console.Write("FlushLevel: " + GetBoardFlushLevel(board4) + "\n\n");

						Console.Write("StraightLevel: " + GetBoardStraightLevel(board4) + "\n\n");

						Console.Write("River: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + " " + BoardCard5 + "\n\n");

						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street3) + ")\n\n");

						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street3) + ")\n\n");

						Console.Write("FlushLevel: " + GetBoardFlushLevel(board5) + "\n\n");

						Console.Write("StraightLevel: " + GetBoardStraightLevel(board5) + "\n\n");
					}
					else
					{

						Console.Clear();

						Console.Write("Flop: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + "\n\n");

						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street1) + ")\n\n");

						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street1) + ")\n\n");

						Console.Write("Turn: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + "\n\n");

						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street2) + ")\n\n");

						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street2) + ")\n\n");

						Console.Write("River: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + " " + BoardCard5 + "\n\n");

						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street3) + ")\n\n");

						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street3) + ")\n\n");
					}

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
			//Console.WriteLine(GetCombinationTitle(GetCombination(new Board("9dTcJhQdKd").Mask, new Hand("Ad5s").Mask)));
			
			//Console.ReadKey();

			Test();
		}
	}
}

