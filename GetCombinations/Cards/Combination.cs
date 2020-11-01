using System;

namespace Combinations
{
	public class Combination
	{
		public string Title { get; protected set; }

		public Hand Hand { get; protected set; }

		public Board Board { get; protected set; }

		public int FlushLevel { get; protected set; }

		public int StraightLevel { get; protected set; }

		public int StraightCount { get; protected set; }

		public int PairLevel { get; protected set; }

		public int CardLevel1 { get; protected set; }

		public int CardLevel2 { get; protected set; }

		public Combination()
		{
			Invalid();
		}

		public Combination(Hand hand, Board board)
		{
			Invalid();

			if(hand.IsValid())
			{
				Hand = hand;
				Board = board;

				SetProperties();
			}
		}

		public void Invalid()
		{
			Title = "Invalid";

			Hand = new Hand();

			Board = new Board();
		}

		public bool IsValid()
		{
			return Hand.IsValid();
		}

		public static bool operator == (Combination combination1, Combination combination2)
		{
			if(combination1.IsValid() == false || combination2.IsValid() == false)
			{
				return false;
			}

			if(combination1.Hand == combination2.Hand && combination1.Board == combination2.Board)
			{
				return true;
			}

			return false;
		}

		public static bool operator != (Combination combination1, Combination combination2)
		{
			if(combination1.IsValid() == false || combination2.IsValid() == false)
			{
				return false;
			}

			if(combination1.Hand != combination2.Hand || combination1.Board != combination2.Board)
			{
				return true;
			}

			return false;
		}

		public override string ToString()
		{
			return Title;
		}

		private void UpdateTitle()
		{
			Title = Hand.Title + " + " + Board.Title;
		}

		private void UpdateFlushLevel()
		{
			int[] levels = new int[4];
			
			++levels[Hand.Card1.Suit];
			++levels[Hand.Card2.Suit];

			if(Board.Street > 0)
			{
				++levels[Board.Card1.Suit];
				++levels[Board.Card2.Suit];
				++levels[Board.Card3.Suit];
			}

			if(Board.Street > 1)
			{
				++levels[Board.Card4.Suit];
			}
			
			if(Board.Street > 2)
			{
				++levels[Board.Card5.Suit];
			}
			
			FlushLevel = 1;

			for(int i=0; i<4; ++i)
			{
				if(levels[i] > FlushLevel)
				{
					FlushLevel = levels[i];
				}
			}
		}

		private void UpdateStraightLevel()
		{
			// A 2 3 4 5 6 7 8 9 T  J  Q  K  A

			// 0 1 2 3 4 5 6 7 8 9 10 11 12 13

			bool[] positions = new bool[14];

			positions[Hand.Card1.Denomination+1] = true;
			positions[Hand.Card2.Denomination+1] = true;

			if(Board.Street > 0)
			{
				positions[Board.Card1.Denomination+1] = true;
				positions[Board.Card2.Denomination+1] = true;
				positions[Board.Card3.Denomination+1] = true;
			}

			if(Board.Street > 1)
			{
				positions[Board.Card4.Denomination+1] = true;
			}
			
			if(Board.Street > 2)
			{
				positions[Board.Card5.Denomination+1] = true;
			}

			positions[0] = positions[13];

			StraightLevel = 0;

			for(int i=0; i<13; ++i)
			{
				int count = 0;

				for(int j=i; j<i+5 && j<14; ++j)
				{
					if(!positions[j])
					{
						break;
					}

					++count;
				}

				StraightLevel = Math.Max(StraightLevel, count);
			}

			StraightCount = 0;

			for(int i=0; i<10; ++i)
			{
				int count = 0;

				for(int j=i; j<i+5; ++j)
				{
					if(positions[j])
					{
						++count;
					}
				}

				StraightCount = Math.Max(StraightCount, count);
			}
		}

		private void UpdatePairLevel()
		{
			int[] denominations = new int[13];

			++denominations[Hand.Card1.Denomination];
			++denominations[Hand.Card2.Denomination];

			if(Board.Street > 0)
			{
				++denominations[Board.Card1.Denomination];
				++denominations[Board.Card2.Denomination];
				++denominations[Board.Card3.Denomination];
			}

			if(Board.Street > 1)
			{
				++denominations[Board.Card4.Denomination];
			}
			
			if(Board.Street > 2)
			{
				++denominations[Board.Card5.Denomination];
			}

			// 0 => Nothing
			// 1 => Week Pair
			// 2 => Middle Pair
			// 3 => High Pair
			// 4 => Two pair
			// 5 => Three of a Kind
			// 6 => Full House
			// 7 => Four of a Kind

			PairLevel = 0;

			for(int i=0; i<13; ++i)
			{
				if(denominations[i] == 4)
				{
					PairLevel = 5;
					return;
				}

				if(denominations[i] == 3)
				{
					PairLevel = 3;

					for(int j=0; j<13; ++j)
					{
						if(i != j)
						{
							if(denominations[j] == 2)
							{
								PairLevel = 4;
								return;
							}
						}
					}

					return;
				}

				if(denominations[i] == 2)
				{
					if(i > 9)
					{
						PairLevel = Math.Max(PairLevel, 3);
					}

					if(i > 5)
					{
						PairLevel = Math.Max(PairLevel, 2);
					}

					PairLevel = Math.Max(PairLevel, 1);

					for(int j=0; j<13; ++j)
					{
						if(i != j)
						{
							if(denominations[j] == 2)
							{
								PairLevel = Math.Max(PairLevel, 4);
								break;
							}
						}
					}
				}
			}
		}

		private void UpdateCardLevel()
		{
			CardLevel1 = 0;
			CardLevel2 = 0;

			CardLevel1 = Math.Max(CardLevel1, Hand.Card1.Denomination);
			CardLevel1 = Math.Max(CardLevel1, Hand.Card2.Denomination);

			if(Board.Street > 0)
			{
				CardLevel1 = Math.Max(CardLevel1, Board.Card1.Denomination);
				CardLevel1 = Math.Max(CardLevel1, Board.Card2.Denomination);
				CardLevel1 = Math.Max(CardLevel1, Board.Card3.Denomination);
			}
			else
			{
				CardLevel2 = CardLevel1;

				CardLevel2 = Math.Min(CardLevel2, Hand.Card1.Denomination);
				CardLevel2 = Math.Min(CardLevel2, Hand.Card2.Denomination);
			}

			if(Board.Street > 1)
			{
				CardLevel1 = Math.Max(CardLevel1, Board.Card4.Denomination);
			}

			if(Board.Street > 2)
			{
				CardLevel1 = Math.Max(CardLevel1, Board.Card5.Denomination);
			}
		}

		private void SetProperties()
		{
			UpdateTitle();

			UpdateFlushLevel();
			UpdateStraightLevel();
			UpdatePairLevel();
			UpdateCardLevel();
		}
	}
}

