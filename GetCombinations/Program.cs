using System;

using static Combinations.Data;
using static Combinations.Equity;

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

		private static void GetCardsTest()
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

				uint BoardStreet1 = GetBoardCombination(board3);
				uint BoardStreet2 = GetBoardCombination(board4);
				uint BoardStreet3 = GetBoardCombination(board5);

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

				const bool show = true;

				bool levels = true;

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
						
						Console.Write("Flop: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " (" + GetCombinationTitle(BoardStreet1) + ")\n\n");
						
						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street1) + ")\n\n");
						
						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street1) + ")\n\n");
						
						Console.Write("FlushLevel: " + GetFlushLevel(board3) + "\n\n");
						
						Console.Write("StraightCount: " + GetStraightCount(board3) + "\n\n");
						
						Console.Write("StraightLevel: " + GetStraightLevel(board3) + "\n\n");

						Console.Write("Turn: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + " (" + GetCombinationTitle(BoardStreet2) + ")\n\n");
						
						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street2) + ")\n\n");
						
						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street2) + ")\n\n");
						
						Console.Write("FlushLevel: " + GetFlushLevel(board4) + "\n\n");
						
						Console.Write("StraightCount: " + GetStraightCount(board4) + "\n\n");
						
						Console.Write("StraightLevel: " + GetStraightLevel(board4) + "\n\n");

						Console.Write("River: " + BoardCard1 + " " + BoardCard2 + " " + BoardCard3 + " " + BoardCard4 + " " + BoardCard5 + " (" + GetCombinationTitle(BoardStreet3) + ")\n\n");
						
						Console.Write("Player1: " + Player1Card1  + " " + Player1Card2 + " (" + GetCombinationTitle(Player1Street3) + ")\n\n");
						
						Console.Write("Player2: " + Player2Card1  + " " + Player2Card2 + " (" + GetCombinationTitle(Player2Street3) + ")\n\n");
						
						Console.Write("FlushLevel: " + GetFlushLevel(board5) + "\n\n");
						
						Console.Write("StraightCount: " + GetStraightCount(board5) + "\n\n");

						Console.Write("StraightLevel: " + GetStraightLevel(board5) + "\n\n");
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
			GetCardsTest();
		}
	}
}

