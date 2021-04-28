using System;

using static Combinations.GetCombinations;
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

				Card BoardCard1 = Card.DefaultCards[cards[0]];
				Card BoardCard2 = Card.DefaultCards[cards[1]];
				Card BoardCard3 = Card.DefaultCards[cards[2]];
				Card BoardCard4 = Card.DefaultCards[cards[3]];
				Card BoardCard5 = Card.DefaultCards[cards[4]];

				Card Player1Card1 = Card.DefaultCards[cards[5]];
				Card Player1Card2 = Card.DefaultCards[cards[6]];

				Card Player2Card1 = Card.DefaultCards[cards[7]];
				Card Player2Card2 = Card.DefaultCards[cards[8]];

				Board board = new Board(BoardCard1, BoardCard2, BoardCard3, BoardCard4, BoardCard5);

				Hand Hand1 = new Hand(Player1Card1, Player1Card2);

				Hand Hand2 = new Hand(Player2Card1, Player2Card2);

				if(ShowSthraight4(board.Mask, Hand1.Mask, out string str1))
				{
					Console.Clear();

					Console.Write("Board: " + board.Title + "\n\n");

					Console.Write("Hand: " + Hand1.Title + "\n\n");

					Console.Write(str1);
				}

				if(ShowSthraight4(board.Mask, Hand2.Mask, out string str2))
				{
					Console.Clear();

					Console.Write("Board: " + board.Title + "\n\n");

					Console.Write("Hand: " + Hand2.Title + "\n\n");

					Console.Write(str2);
				}

				Console.ReadKey();
			}
		}

		private static bool ShowSthraight4(ulong Board, ulong Hand, out string str)
		{
			str = default;

			uint heroStraightCount = GetStraightCount(Board | Hand);

			uint heroStraightLevel = GetStraightLevel(Board | Hand);			

			if(heroStraightCount == 4)
			{
				if(heroStraightLevel == 4)
				{
					str = "Player Has OESD\n\n";
				}
				else
				{
					str = "Player Has Gutshot\n\n";
				}

				return true;
			}

			return false;
		}

		private static void Main()
		{
			GetCardsTest();

			Console.ReadKey();
		}
	}
}

