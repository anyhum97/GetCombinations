using System;

namespace Combinations
{
	public static partial class GetCombinations
	{
		public const uint Nothing = 0;
		public const uint HighCard = 1;
		public const uint WeakPair = 2;
		public const uint MiddlePair = 3;
		public const uint HighPair = 4;
		public const uint OverPair = 5;
		public const uint TwoPairs = 6;
		public const uint Trips = 7;
		public const uint Set = 8;
		public const uint Straight = 9;
		public const uint Flush = 10;
		public const uint FullHouse = 11;
		public const uint Nuts = 12;

		public const uint BoardOnePair = 13;

		/// <summary>
		/// Print the combination title
		/// Выводит название комбинации
		/// </summary>
		public static string GetCombinationTitle(uint combination)
		{
			switch(combination)
			{
				case Nothing: return "Nothing";
				case HighCard: return "A High";

				case WeakPair: return "Weak Pair";
				case MiddlePair: return "Middle Pair";
				case HighPair: return "High Pair";
				case OverPair: return "Over Pair";

				case TwoPairs: return "Two Pairs";
				case Trips: return "Trips";
				case Set: return "Set";
				case Straight: return "Straight";
				case Flush: return "Flush";
				case FullHouse: return "Full House";
				case Nuts: return "Nuts";

				case BoardOnePair: return "One Pair";
			}

			return "Invalid Combination";
		}

		/// <summary>
		/// Determines the strength of the hand relative to other hands in this board.<br></br><br></br>
		/// Определяет силу руки относительно других рук в текущей раздаче.
		/// </summary>
		/// <param name="CardsMask"></param>
		/// <returns></returns>
		public static uint GetTexasHoldemCombinationRank(ulong CardsMask)
		{
			uint cCardMask = (uint)((CardsMask >> 00) & 0x1fffUL);
			uint dCardMask = (uint)((CardsMask >> 13) & 0x1fffUL);
			uint hCardMask = (uint)((CardsMask >> 26) & 0x1fffUL);
			uint sCardMask = (uint)((CardsMask >> 39) & 0x1fffUL);

			uint CardCount = 0;
			
			CardCount += BitCountTable[(CardsMask >> 00) & 0x1fff];
			CardCount += BitCountTable[(CardsMask >> 13) & 0x1fff];
			CardCount += BitCountTable[(CardsMask >> 26) & 0x1fff];
			CardCount += BitCountTable[(CardsMask >> 39) & 0x1fff];

			uint DenominationMask = cCardMask | dCardMask | hCardMask | sCardMask;

			uint DenominationCount = BitCountTable[DenominationMask];

			uint DuplicateCount = CardCount - DenominationCount;
			
			uint CombinationRank = 0;

			if(DenominationCount >= 5)
			{
				if(BitCountTable[sCardMask] >= 5)
				{
					if(StraightValueTable[sCardMask] != 0)
					{
						return 134217728 + (StraightValueTable[sCardMask] << 16);
					}
					
					CombinationRank = 83886080 + FlushValueTable[sCardMask];
				}
				
				else if(BitCountTable[cCardMask] >= 5)
				{
					if(StraightValueTable[cCardMask] != 0)
					{
						return 134217728 + (StraightValueTable[cCardMask] << 16);
					}
		
					CombinationRank = 83886080 + FlushValueTable[cCardMask];
				}
				
				else if (BitCountTable[dCardMask] >= 5)
				{
					if(StraightValueTable[dCardMask] != 0)
					{
						return 134217728 + (StraightValueTable[dCardMask] << 16);
					}
					
					CombinationRank = 83886080 + FlushValueTable[dCardMask];
				}
				
				else if(BitCountTable[hCardMask] >= 5)
				{
					if(StraightValueTable[hCardMask] != 0)
					{
						return 134217728 + (StraightValueTable[hCardMask] << 16);
					}
					
					CombinationRank = 83886080 + FlushValueTable[hCardMask];
				}
				else
				{
					uint StraightValue = StraightValueTable[DenominationMask];
					
					if(StraightValue != 0)
					{
						CombinationRank = 67108864 + (StraightValue << 16);
					}
				}
				
				if(CombinationRank != 0 && DuplicateCount < 3)
				{
					return CombinationRank;
				}
			}

			switch(DuplicateCount)
			{
				case 0: return FlushValueTable[DenominationMask];
				
				case 1:
				{
					uint TwoMask = DenominationMask ^ cCardMask ^ dCardMask ^ hCardMask ^ sCardMask;

					CombinationRank = 16777216 + (FirstCardValueTable[TwoMask] << 16);

					long KickersValue = FlushValueTable[DenominationMask ^ TwoMask] >> 4 & ~0x0000000F;

					CombinationRank += (uint)KickersValue;

					return CombinationRank;
				}

				case 2:
				{
					uint TwoMask = DenominationMask ^ cCardMask ^ dCardMask ^ hCardMask ^ sCardMask;
					
					if(TwoMask != 0)
					{
						uint TempValue = DenominationMask ^ TwoMask;
						
						CombinationRank = (33554432 + (FlushValueTable[TwoMask] & (0x000F0000 | 0x0000F000)) + (FirstCardValueTable[TempValue] << 8));
						
						return CombinationRank;
					}
					else
					{
						uint ThreeMask = (cCardMask & dCardMask | hCardMask & sCardMask) & (cCardMask & hCardMask | dCardMask & sCardMask);
						
						CombinationRank = 50331648 + (FirstCardValueTable[ThreeMask] << 16);
						
						uint TempValue = DenominationMask ^ ThreeMask;

						uint SecondCardValue = FirstCardValueTable[TempValue];
			    
						CombinationRank += SecondCardValue << 12;

						TempValue ^= 1U << (int) SecondCardValue;

						CombinationRank += FirstCardValueTable[TempValue] << 8;
						
						return CombinationRank;
					}
				}

				default:
				{
					uint FourMask = hCardMask & dCardMask & cCardMask & sCardMask;

					if(FourMask != 0)
					{
						uint FirstCardValue = FirstCardValueTable[FourMask];

						CombinationRank = (117440512 + (FirstCardValue << 16) + (FirstCardValueTable[DenominationMask ^ 1U << (int) FirstCardValue] << 12));
	            
						return CombinationRank;
					}

					uint TwoMask = DenominationMask ^ cCardMask ^ dCardMask ^ hCardMask ^ sCardMask;

					if(BitCountTable[TwoMask] != DuplicateCount)
					{	
						uint ThreeMask = (cCardMask & dCardMask | hCardMask & sCardMask) & (cCardMask & hCardMask | dCardMask & sCardMask);
						
						CombinationRank = 100663296;
						
						uint FirstCardValue = FirstCardValueTable[ThreeMask];

						CombinationRank += FirstCardValue << 16;
						
						uint TempValue = (TwoMask | ThreeMask) ^ 1U << (int) FirstCardValue;

						CombinationRank += FirstCardValueTable[TempValue] << 12;
						
						return CombinationRank;
					}

					if(CombinationRank != 0)
					{
						return CombinationRank;
					}
					
					CombinationRank = 33554432;

					uint firstCardValue = FirstCardValueTable[TwoMask];
					
					CombinationRank += firstCardValue << 16;
					
					uint secondCardValue = FirstCardValueTable[TwoMask ^ 1 << (int) firstCardValue];
					
					CombinationRank += secondCardValue << 12;

					CombinationRank += FirstCardValueTable[DenominationMask ^ 1U << (int) firstCardValue ^ 1 << (int) secondCardValue] << 8;
					
					return CombinationRank;
				}
			}
		}

		/// <summary>
		/// Determines the players combination.<br></br><br></br>
		/// Определяет комбинацию, которую имеет игрок.
		/// </summary>
		public static uint GetCombination(ulong Board, ulong Hand)
		{
			ulong Total = Board | Hand;

			uint cBoardCardMask = (uint)((Board >> 00) & 0x1fffUL);
			uint dBoardCardMask = (uint)((Board >> 13) & 0x1fffUL);
			uint hBoardCardMask = (uint)((Board >> 26) & 0x1fffUL);
			uint sBoardCardMask = (uint)((Board >> 39) & 0x1fffUL);
			
			uint cTotalCardMask = (uint)((Total >> 00) & 0x1fffUL);
			uint dTotalCardMask = (uint)((Total >> 13) & 0x1fffUL);
			uint hTotalCardMask = (uint)((Total >> 26) & 0x1fffUL);
			uint sTotalCardMask = (uint)((Total >> 39) & 0x1fffUL);

			uint cBoardFlushCards = BitCountTable[cBoardCardMask];
			uint dBoardFlushCards = BitCountTable[dBoardCardMask];
			uint hBoardFlushCards = BitCountTable[hBoardCardMask];
			uint sBoardFlushCards = BitCountTable[sBoardCardMask];

			uint cTotalFlushCards = BitCountTable[cTotalCardMask];
			uint dTotalFlushCards = BitCountTable[dTotalCardMask];
			uint hTotalFlushCards = BitCountTable[hTotalCardMask];
			uint sTotalFlushCards = BitCountTable[sTotalCardMask];

			uint BoardCardCount = 0;
			
			BoardCardCount += BitCountTable[(Board >> 00) & 0x1fff];
			
			BoardCardCount += BitCountTable[(Board >> 13) & 0x1fff];
			
			BoardCardCount += BitCountTable[(Board >> 26) & 0x1fff];
			
			BoardCardCount += BitCountTable[(Board >> 39) & 0x1fff];

			uint TotalCardCount = 0;
			
			TotalCardCount += BitCountTable[(Total >> 00) & 0x1fff];
			
			TotalCardCount += BitCountTable[(Total >> 13) & 0x1fff];
			
			TotalCardCount += BitCountTable[(Total >> 26) & 0x1fff];
			
			TotalCardCount += BitCountTable[(Total >> 39) & 0x1fff];

			uint BoardDenominationMask = cBoardCardMask | dBoardCardMask | hBoardCardMask | sBoardCardMask;

			uint TotalDenominationMask = cTotalCardMask | dTotalCardMask | hTotalCardMask | sTotalCardMask;

			uint BoardDenominationCount = BitCountTable[BoardDenominationMask];

			uint TotalDenominationCount = BitCountTable[TotalDenominationMask];

			uint BoardDuplicateCount = BoardCardCount - BoardDenominationCount;

			uint TotalDuplicateCount = TotalCardCount - TotalDenominationCount;

			bool BoardHasFlush = false;

			bool BoardHasStraight = false;

			if(TotalDenominationCount >= 5)
			{
				if(cTotalFlushCards >= 5)
				{
					uint cBoardStraightValue = StraightValueTable[cBoardCardMask];

					uint cTotalStraightValue = StraightValueTable[cTotalCardMask];

					if(cTotalStraightValue > cBoardStraightValue)
					{
						// Hero Has Straight Flush (c)

						return Nuts;
					}

					if(cBoardStraightValue > 0)
					{
						// Board Has Straight Flush (c)
						
						return Nothing;
					}

					if(cBoardFlushCards == 5)
					{
						uint cBoardFlushValue = FlushValueTable[cBoardCardMask];

						uint cTotalFlushValue = FlushValueTable[cTotalCardMask];

						if(cTotalFlushValue > cBoardFlushValue)
						{
							// Hero Has Flush (c)

							return Flush;
						}

						BoardHasFlush = true;
					}
					else
					{
						// Hero Has Flush (c)

						return Flush;
					}
				}

				if(dTotalFlushCards >= 5)
				{
					uint dBoardStraightValue = StraightValueTable[dBoardCardMask];

					uint dTotalStraightValue = StraightValueTable[dTotalCardMask];

					if(dTotalStraightValue > dBoardStraightValue)
					{
						// Hero Has Straight Flush (d)

						return Nuts;
					}

					if(dBoardStraightValue > 0)
					{
						// Board Has Straight Flush (d)
						
						return Nothing;
					}

					if(dBoardFlushCards == 5)
					{
						uint dBoardFlushValue = FlushValueTable[dBoardCardMask];

						uint dTotalFlushValue = FlushValueTable[dTotalCardMask];

						if(dTotalFlushValue > dBoardFlushValue)
						{
							// Hero Has Flush (d)

							return Flush;
						}

						BoardHasFlush = true;
					}
					else
					{
						// Hero Has Flush (d)

						return Flush;
					}
				}

				if(hTotalFlushCards >= 5)
				{
					uint hBoardStraightValue = StraightValueTable[hBoardCardMask];

					uint hTotalStraightValue = StraightValueTable[hTotalCardMask];

					if(hTotalStraightValue > hBoardStraightValue)
					{
						// Hero Has Straight Flush (h)

						return Nuts;
					}

					if(hBoardStraightValue > 0)
					{
						// Board Has Straight Flush (h)
						
						return Nothing;
					}

					if(hBoardFlushCards == 5)
					{
						uint hBoardFlushValue = FlushValueTable[hBoardCardMask];

						uint hTotalFlushValue = FlushValueTable[hTotalCardMask];

						if(hTotalFlushValue > hBoardFlushValue)
						{
							// Hero Has Flush (h)

							return Flush;
						}

						BoardHasFlush = true;
					}
					else
					{
						// Hero Has Flush (h)

						return Flush;
					}
				}
				
				if(sTotalFlushCards >= 5)
				{
					uint sBoardStraightValue = StraightValueTable[sBoardCardMask];

					uint sTotalStraightValue = StraightValueTable[sTotalCardMask];

					if(sTotalStraightValue > sBoardStraightValue)
					{
						// Hero Has Straight Flush (s)

						return Nuts;
					}

					if(sBoardStraightValue > 0)
					{
						// Board Has Straight Flush (s)
						
						return Nothing;
					}

					if(sBoardFlushCards == 5)
					{
						uint sBoardFlushValue = FlushValueTable[sBoardCardMask];

						uint sTotalFlushValue = FlushValueTable[sTotalCardMask];

						if(sTotalFlushValue > sBoardFlushValue)
						{
							// Hero Has Flush (s)

							return Flush;
						}

						BoardHasFlush = true;
					}
					else
					{
						// Hero Has Flush (s)

						return Flush;
					}
				}

				uint BoardStraightValue = StraightValueTable[BoardDenominationMask];

				uint TotalStraightValue = StraightValueTable[TotalDenominationMask];

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
				// Nothing or High Card

				return GetHighCardRang(Hand);
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
					// Hero Has One Pair

					return GetPairLevel(Board, Hand);
				}

				if(TotalDuplicateCount == 2)
				{
					if(TotalTwoMask != 0)
					{
						if(BoardDuplicateCount == 0)
						{
							// Hero Has Two Pairs

							return TwoPairs;
						}
						else
						{
							// Hero Has One Pair

							return GetPairLevel(Board, Hand);
						}
					}
					else
					{
						// Hero Has Set or Trips

						if(BoardDuplicateCount == 0)
						{
							return Set;
						}
						else
						{
							return Trips;
						}
					}
				}
			}
			
			uint TotalFourMask = hTotalCardMask & dTotalCardMask & cTotalCardMask & sTotalCardMask;

			if(TotalFourMask != 0)
			{
				// Board or Hero Has Four of a Kind

				return Nuts;
			}

			if(BitCountTable[TotalTwoMask] != TotalDuplicateCount)
			{
				if(TotalDuplicateCount != 1 && TotalDuplicateCount != 2)
				{
					// Board or Hero Has Full House

					return FullHouse;
				}
			}

			if(TotalDuplicateCount == 3)
			{
				uint BoardTwoMask = BoardDenominationMask ^ cBoardCardMask ^ dBoardCardMask ^ hBoardCardMask ^ sBoardCardMask;

				uint BoardFirstCardValue = FirstCardValueTable[BoardTwoMask];

				uint TotalFirstCardValue = FirstCardValueTable[TotalTwoMask];

				uint BoardSecondCardValue = FirstCardValueTable[BoardTwoMask ^ 1 << (int) BoardFirstCardValue];

				uint TotalSecondCardValue = FirstCardValueTable[TotalTwoMask ^ 1 << (int) TotalFirstCardValue];

				if(TotalSecondCardValue > BoardSecondCardValue)
				{
					if(BoardDuplicateCount == 0)
					{
						return TwoPairs;
					}
					else
					{
						// Hero Has One Pair

						return GetPairLevel(Board, Hand);
					}
				}
			}

			return GetHighCardRang(Hand);
		}
		
		/// <summary>
		/// Determines the boards combination.<br></br><br></br>
		/// Определяет комбинацию, которую составляет борд.
		/// </summary>
		public static uint GetBoardCombination(ulong Board)
		{
			uint cBoardCardMask = (uint)((Board >> 00) & 0x1fffUL);
			uint dBoardCardMask = (uint)((Board >> 13) & 0x1fffUL);
			uint hBoardCardMask = (uint)((Board >> 26) & 0x1fffUL);
			uint sBoardCardMask = (uint)((Board >> 39) & 0x1fffUL);

			uint cBoardFlushCards = BitCountTable[cBoardCardMask];
			uint dBoardFlushCards = BitCountTable[dBoardCardMask];
			uint hBoardFlushCards = BitCountTable[hBoardCardMask];
			uint sBoardFlushCards = BitCountTable[sBoardCardMask];

			uint BoardCardCount = 0;
			
			BoardCardCount += BitCountTable[(Board >> 00) & 0x1fff];
			
			BoardCardCount += BitCountTable[(Board >> 13) & 0x1fff];
			
			BoardCardCount += BitCountTable[(Board >> 26) & 0x1fff];
			
			BoardCardCount += BitCountTable[(Board >> 39) & 0x1fff];

			uint BoardDenominationMask = cBoardCardMask | dBoardCardMask | hBoardCardMask | sBoardCardMask;

			uint BoardDenominationCount = BitCountTable[BoardDenominationMask];

			uint BoardDuplicateCount = BoardCardCount - BoardDenominationCount;

			if(BoardDenominationCount >= 5)
			{
				if(cBoardFlushCards >= 5)
				{
					uint cBoardStraightValue = StraightValueTable[cBoardCardMask];

					if(cBoardStraightValue >= 5)
					{
						// Board Has Straight Flush (c)

						return Nuts;
					}

					// Board Has Flush (c)

					return Flush;
				}

				if(dBoardFlushCards >= 5)
				{
					uint dBoardStraightValue = StraightValueTable[dBoardCardMask];

					if(dBoardStraightValue >= 5)
					{
						// Board Has Straight Flush (d)

						return Nuts;
					}

					// Board Has Flush (d)

					return Flush;
				}

				if(hBoardFlushCards >= 5)
				{
					uint hBoardStraightValue = StraightValueTable[hBoardCardMask];

					if(hBoardStraightValue >= 5)
					{
						// Board Has Straight Flush (h)

						return Nuts;
					}

					// Board Has Flush (h)

					return Flush;
				}
				
				if(sBoardFlushCards >= 5)
				{
					uint sBoardStraightValue = StraightValueTable[sBoardCardMask];

					if(sBoardStraightValue >= 5)
					{
						// Board Has Straight Flush (s)

						return Nuts;
					}

					// Board Has Flush (s)

					return Flush;
				}

				uint BoardStraightValue = StraightValueTable[BoardDenominationMask];
				
				if(BoardStraightValue >= 5)
				{
					// Board Has Straight
					
					return Straight;
				}
			}

			if(BoardDuplicateCount == 0)
			{
				return Nothing;
			}

			if(BoardDuplicateCount == 1)
			{
				return BoardOnePair;
			}

			uint BoardTwoMask = BoardDenominationMask ^ cBoardCardMask ^ dBoardCardMask ^ hBoardCardMask ^ sBoardCardMask;
			
			if(BoardDuplicateCount == 2)
			{
				if(BoardTwoMask != 0)
				{
					// Board Has Two Pairs

					return TwoPairs;
				}
				else
				{
					// Board Has Set or Trips

					return Set;
				}
			}

			uint BoardFourMask = hBoardCardMask & dBoardCardMask & cBoardCardMask & sBoardCardMask;
			
			if(BoardFourMask != 0)
			{
				// Board Has Four of a Kind
				
				return Nuts;
			}

			if(BitCountTable[BoardTwoMask] != BoardDuplicateCount)
			{
				// Board Has Full House
				
				return FullHouse;
			}

			return Nothing;
		}

		/// <summary>
		/// Determines how many cardscan make a flush.<br></br><br></br>
		/// Определяет, сколько карт могут составить флеш.
		/// </summary>
		public static uint GetFlushLevel(ulong cards)
		{
			uint cCardMask = (uint)((cards >> 00) & 0x1fffUL);
			uint dCardMask = (uint)((cards >> 13) & 0x1fffUL);
			uint hCardMask = (uint)((cards >> 26) & 0x1fffUL);
			uint sCardMask = (uint)((cards >> 39) & 0x1fffUL);

			uint cFlushCards = BitCountTable[cCardMask];
			uint dFlushCards = BitCountTable[dCardMask];
			uint hFlushCards = BitCountTable[hCardMask];
			uint sFlushCards = BitCountTable[sCardMask];

			uint FlushLevel = Math.Max(Math.Max(cFlushCards, dFlushCards), Math.Max(hFlushCards, sFlushCards));

			return FlushLevel;
		}

		/// <summary>
		/// Determines how many cardscan make a straight.<br></br><br></br>
		/// Определяет, сколько карт могут составить стрит.
		/// </summary>
		public static uint GetStraightCount(ulong cards)
		{
			uint cCardMask = (uint)((cards >> 00) & 0x1fffUL);
			uint dCardMask = (uint)((cards >> 13) & 0x1fffUL);
			uint hCardMask = (uint)((cards >> 26) & 0x1fffUL);
			uint sCardMask = (uint)((cards >> 39) & 0x1fffUL);

			uint BoardDenominationMask = cCardMask | dCardMask | hCardMask | sCardMask;

			uint StraightLevel = StraightCountTable[BoardDenominationMask];

			return StraightLevel;
		}

		/// <summary>
		/// Determines how many cardscan make a straight. 
		/// Only consecutive cards are taken into account.<br></br><br></br>
		/// Определяет, сколько карт подряд могут составить стрит.
		/// </summary>
		public static uint GetStraightLevel(ulong cards)
		{
			uint cCardMask = (uint)((cards >> 00) & 0x1fffUL);
			uint dCardMask = (uint)((cards >> 13) & 0x1fffUL);
			uint hCardMask = (uint)((cards >> 26) & 0x1fffUL);
			uint sCardMask = (uint)((cards >> 39) & 0x1fffUL);

			uint BoardDenominationMask = cCardMask | dCardMask | hCardMask | sCardMask;

			uint StraightLevel = StraightLevelTable[BoardDenominationMask];

			return StraightLevel;
		}

		public static uint GetPairLevel(ulong Board, ulong Hand)
		{
			uint cBoardCardMask = (uint)((Board >> 00) & 0x1fffUL);
			uint dBoardCardMask = (uint)((Board >> 13) & 0x1fffUL);
			uint hBoardCardMask = (uint)((Board >> 26) & 0x1fffUL);
			uint sBoardCardMask = (uint)((Board >> 39) & 0x1fffUL);

			uint cHandCardMask = (uint)((Hand >> 00) & 0x1fffUL);
			uint dHandCardMask = (uint)((Hand >> 13) & 0x1fffUL);
			uint hHandCardMask = (uint)((Hand >> 26) & 0x1fffUL);
			uint sHandCardMask = (uint)((Hand >> 39) & 0x1fffUL);

			uint BoardDenominationMask = cBoardCardMask | dBoardCardMask | hBoardCardMask | sBoardCardMask;

			uint HandDenominationMask = cHandCardMask | dHandCardMask | hHandCardMask | sHandCardMask;

			ulong PairBit = HandDenominationMask & BoardDenominationMask;

			ulong HighHeroBit = GetHighBit(HandDenominationMask);

			ulong HighBoardBit = GetHighBit(BoardDenominationMask);

			ulong HighPairBit = GetHighBit((uint)PairBit);

			ulong LowHeroBit = GetLowBit(HandDenominationMask);

			ulong LowBoardBit = GetLowBit(BoardDenominationMask);

			ulong LowPairBit = GetLowBit((uint)PairBit);

			if(PairBit == 0)
			{
				if(HighHeroBit > HighBoardBit)
				{
					return OverPair;
				}
			}

			if(HighHeroBit == HighBoardBit)
			{
				return HighPair;
			}

			if(PairBit > 0)
			{
				if(LowPairBit > LowBoardBit)
				{
					return MiddlePair;
				}
			}
			else
			{
				if(LowHeroBit > LowBoardBit)
				{
					return MiddlePair;
				}
			}

			return WeakPair;
		}

		public static uint GetHighCardRang(ulong Hand)
		{
			uint cHandCardMask = (uint)((Hand >> 00) & 0x1fffUL);
			uint dHandCardMask = (uint)((Hand >> 13) & 0x1fffUL);
			uint hHandCardMask = (uint)((Hand >> 26) & 0x1fffUL);
			uint sHandCardMask = (uint)((Hand >> 39) & 0x1fffUL);

			uint HandDenominationMask = cHandCardMask | dHandCardMask | hHandCardMask | sHandCardMask;

			if((HandDenominationMask & 4096) != 0)
			{
				// Ace High

				return HighCard;
			}

			return Nothing;
		}

		private static uint GetHighBit(uint bits)
		{
			bits |= bits >> 1;
			bits |= bits >> 2;
			bits |= bits >> 4;
			bits |= bits >> 8;
			bits |= bits >> 16;

			return bits - (bits >> 1);
		}

		private static uint GetLowBit(uint bits)
		{
		    uint bit = 1;

			if(bits == 0)
			{
				return 0;
			}

		    while((bits & bit) == 0)
			{
		        bit = bit << 1;
		    }

			return bit;
		}
	}
}

