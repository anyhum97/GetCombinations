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

		private static int GetCombination(ulong board, ulong hand)
		{
			ulong total = board | hand;

			uint cBoardCardMask = (uint)((board >> 00) & 0x1fffUL);
			uint dBoardCardMask = (uint)((board >> 13) & 0x1fffUL);
			uint hBoardCardMask = (uint)((board >> 26) & 0x1fffUL);
			uint sBoardCardMask = (uint)((board >> 39) & 0x1fffUL);
			
			uint cTotalCardMask = (uint)((total >> 00) & 0x1fffUL);
			uint dTotalCardMask = (uint)((total >> 13) & 0x1fffUL);
			uint hTotalCardMask = (uint)((total >> 26) & 0x1fffUL);
			uint sTotalCardMask = (uint)((total >> 39) & 0x1fffUL);

			uint cTotalFlushCards = Data.BitCountTable[cTotalCardMask];
			uint dTotalFlushCards = Data.BitCountTable[dTotalCardMask];
			uint hTotalFlushCards = Data.BitCountTable[hTotalCardMask];
			uint sTotalFlushCards = Data.BitCountTable[sTotalCardMask];

			uint BoardCardCount = 0;
			
			BoardCardCount += Data.BitCountTable[(board >> 00) & 0x1fffUL];
			
			BoardCardCount += Data.BitCountTable[(board >> 13) & 0x1fffUL];
			
			BoardCardCount += Data.BitCountTable[(board >> 26) & 0x1fffUL];
			
			BoardCardCount += Data.BitCountTable[(board >> 39) & 0x1fffUL];

			uint TotalCardCount = 0;
			
			TotalCardCount += Data.BitCountTable[(total >> 00) & 0x1fffUL];
			
			TotalCardCount += Data.BitCountTable[(total >> 13) & 0x1fffUL];
			
			TotalCardCount += Data.BitCountTable[(total >> 26) & 0x1fffUL];
			
			TotalCardCount += Data.BitCountTable[(total >> 39) & 0x1fffUL];

			uint BoardDenominationMask = cBoardCardMask | dBoardCardMask | hBoardCardMask | sBoardCardMask;

			uint TotalDenominationMask = cTotalCardMask | dTotalCardMask | hTotalCardMask | sTotalCardMask;

			uint BoardDenominationCount = Data.BitCountTable[BoardDenominationMask];

			uint TotalDenominationCount = Data.BitCountTable[TotalDenominationMask];

			uint BoardDuplicateCount = BoardCardCount - BoardDenominationCount;

			uint TotalDuplicateCount = TotalCardCount - TotalDenominationCount;

			bool BoardHasFlush = false;

			bool BoardHasStraight = false;

			if(TotalDenominationCount >= 5)
			{
				if(cTotalFlushCards >= 5)
				{
					uint cBoardStraightValue = Data.StraightValueTable[cBoardCardMask];

					uint cTotalStraightValue = Data.StraightValueTable[cTotalCardMask];

					if(cTotalStraightValue > cBoardStraightValue)
					{
						// Hero Has Straight Flush (c)

						return StraightFlush;
					}

					if(cBoardStraightValue > 0)
					{
						// Board Has Straight Flush (c)

						return Nothing;
					}

					uint cBoardFlushValue = Data.FlushValueTable[cBoardCardMask];

					uint cTotalFlushValue = Data.FlushValueTable[cTotalCardMask];

					if(cTotalFlushValue > cBoardFlushValue)
					{
						// Hero Has Flush (c)

						return Flush;
					}

					if(cBoardFlushValue > 0)
					{
						BoardHasFlush = true;
					}
				}

				if(dTotalFlushCards >= 5)
				{
					uint dBoardStraightValue = Data.StraightValueTable[dBoardCardMask];

					uint dTotalStraightValue = Data.StraightValueTable[dTotalCardMask];

					if(dTotalStraightValue > dBoardStraightValue)
					{
						// Hero Has Straight Flush (d)

						return StraightFlush;
					}

					if(dBoardStraightValue > 0)
					{
						// Board Has Straight Flush (d)

						return Nothing;
					}

					uint dBoardFlushValue = Data.FlushValueTable[dBoardCardMask];

					uint dTotalFlushValue = Data.FlushValueTable[dTotalCardMask];

					if(dTotalFlushValue > dBoardFlushValue)
					{
						// Hero Has Flush (d)

						return Flush;
					}

					if(dBoardFlushValue > 0)
					{
						BoardHasFlush = true;
					}
				}

				if(hTotalFlushCards >= 5)
				{
					uint hBoardStraightValue = Data.StraightValueTable[hBoardCardMask];

					uint hTotalStraightValue = Data.StraightValueTable[hTotalCardMask];

					if(hTotalStraightValue > hBoardStraightValue)
					{
						// Hero Has Straight Flush (h)

						return StraightFlush;
					}

					if(hBoardStraightValue > 0)
					{
						// Board Has Straight Flush (h)

						return Nothing;
					}

					uint hBoardFlushValue = Data.FlushValueTable[hBoardCardMask];

					uint hTotalFlushValue = Data.FlushValueTable[hTotalCardMask];

					if(hTotalFlushValue > hBoardFlushValue)
					{
						// Hero Has Flush (h)

						return Flush;
					}

					if(hBoardFlushValue > 0)
					{
						BoardHasFlush = true;
					}
				}
				
				if(sTotalFlushCards >= 5)
				{
					uint sBoardStraightValue = Data.StraightValueTable[sBoardCardMask];

					uint sTotalStraightValue = Data.StraightValueTable[sTotalCardMask];

					if(sTotalStraightValue > sBoardStraightValue)
					{
						// Hero Has Straight Flush (s)

						return StraightFlush;
					}

					if(sBoardStraightValue > 0)
					{
						// Board Has Straight Flush (s)

						return Nothing;
					}

					uint sBoardFlushValue = Data.FlushValueTable[sBoardCardMask];

					uint sTotalFlushValue = Data.FlushValueTable[sTotalCardMask];

					if(sTotalFlushValue > sBoardFlushValue)
					{
						// Hero Has Flush (s)

						return Flush;
					}

					if(sBoardFlushValue > 0)
					{
						BoardHasFlush = true;
					}
				}

				uint BoardStraightValue = Data.StraightValueTable[BoardDenominationMask];

				uint TotalStraightValue = Data.StraightValueTable[TotalDenominationMask];

				if(BoardHasFlush == false)
				{
					if(TotalStraightValue > BoardStraightValue)
					{
						// Hero Has Straight

						return Straight;
					}
				}

				if(BoardStraightValue > 0)
				{
					BoardHasStraight = true;
				}
			}

			if(TotalDuplicateCount == 0)
			{
				return Nothing;
			}

			if(BoardHasFlush || BoardHasStraight)
			{
				if(TotalDuplicateCount < 3)
				{
					return Nothing;
				}
			}

			uint TotalTwoMask = TotalDenominationMask ^ cTotalCardMask ^ dTotalCardMask ^ hTotalCardMask ^ sTotalCardMask;

			if(TotalDuplicateCount > BoardDuplicateCount)
			{
				if(TotalDuplicateCount == 1)
				{
					return OnePair;
				}

				if(TotalDuplicateCount == 2)
				{
					if(TotalTwoMask != 0)
					{
						// Hero Has Two Pairs

						return TwoPairs;
					}
					else
					{
						// Hero Has Trips

						return Trips;
					}
				}
			}

			uint TotalFourMask = hTotalCardMask & dTotalCardMask & cTotalCardMask & sTotalCardMask;

			if(TotalFourMask != 0)
			{
				// Board or Hero Has Four of a Kind

				return Quads;
			}

			if(Data.BitCountTable[TotalTwoMask] != TotalDuplicateCount)
			{
				if(TotalDuplicateCount != 1 && TotalDuplicateCount != 2)
				{
					// Board or Hero Has Full House

					return FullHouse;
				}
			}

			if(TotalDuplicateCount == 3 && BoardDuplicateCount == 2)
			{
				uint BoardTwoMask = BoardDenominationMask ^ cBoardCardMask ^ dBoardCardMask ^ hBoardCardMask ^ sBoardCardMask;

				uint BoardFirstCardValue = Data.FirstCardValueTable[BoardTwoMask];

				uint TotalFirstCardValue = Data.FirstCardValueTable[TotalTwoMask];

				uint BoardSecondCardValue = Data.FirstCardValueTable[BoardTwoMask ^ 1 << (int) BoardFirstCardValue];

				uint TotalSecondCardValue = Data.FirstCardValueTable[TotalTwoMask ^ 1 << (int) TotalFirstCardValue];

				if(TotalSecondCardValue > BoardSecondCardValue)
				{
					// Hero Has Two Pairs

					return TwoPairs;
				}
			}

			return Nothing;
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
			}

			int b = 0;

			for(int i=0; i<1000000; ++i)
			{
				int[] cards = GetCards();

				Card card1 = new Card(cards[0]);
				Card card2 = new Card(cards[1]);
				Card card3 = new Card(cards[2]);
				Card card4 = new Card(cards[3]);
				Card card5 = new Card(cards[4]);
				Card card6 = new Card(cards[5]);
				Card card7 = new Card(cards[6]);
				Card card8 = new Card(cards[7]);
				Card card9 = new Card(cards[8]);
			}

			int c = 0;
		}

		private static void Main()
		{
			GetCardsTest();
		}
	}
}

