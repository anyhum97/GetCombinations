using System;

namespace Combinations
{
	public class Board
	{
		public string Title { get; protected set; }

		public Card Card1 { get; protected set; }

		public Card Card2 { get; protected set; }

		public Card Card3 { get; protected set; }

		public Card Card4 { get; protected set; }

		public Card Card5 { get; protected set; }

		public int Street { get; protected set; }

		public ulong Mask { get; protected set; }

		public int FlushLevel { get; protected set; }

		public int StraightLevel { get; protected set; }

		public int StraightCount { get; protected set; }

		public Board()
		{
			Invalid();
		}

		public Board(Card сard1, Card сard2, Card сard3)
		{
			Set(сard1, сard2, сard3);
		}

		public Board(Card сard1, Card сard2, Card сard3, Card сard4)
		{
			Set(сard1, сard2, сard3, сard4);
		}

		public Board(Card сard1, Card сard2, Card сard3, Card сard4, Card сard5)
		{
			Set(сard1, сard2, сard3, сard4, сard5);
		}

		public Board(string str)
		{
			Set(str);
		}

		public void Set(Card сard1, Card сard2, Card сard3)
		{
			Invalid();

			if(сard1 == сard2 || сard1 == сard3)
			{
				return;
			}

			if(сard2 == сard3)
			{
				return;
			}

			if(сard1.IsValid() && сard2.IsValid() && сard3.IsValid())
			{
				Card1 = сard1;
				Card2 = сard2;
				Card3 = сard3;

				Street = 1;

				SetProperties();
			}
		}

		public void Set(Card card1, Card card2, Card card3, Card card4)
		{
			Invalid();

			if(card1 == card2 || card1 == card3 || card1 == card4)
			{
				return;
			}

			if(card2 == card3 || card2 == card4)
			{
				return;
			}

			if(card3 == card4)
			{
				return;
			}

			if(card1.IsValid() && card2.IsValid() && card3.IsValid() && card4.IsValid())
			{
				Card1 = card1;
				Card2 = card2;
				Card3 = card3;
				Card4 = card4;

				Street = 2;

				SetProperties();
			}
		}

		public void Set(Card card1, Card card2, Card card3, Card card4, Card card5)
		{
			Invalid();

			if(card1 == card2 || card1 == card3 || card1 == card4 || card1 == card5)
			{
				return;
			}

			if(card2 == card3 || card2 == card4 || card2 == card5)
			{
				return;
			}

			if(card3 == card4 || card3 == card5)
			{
				return;
			}

			if(card4 == card5)
			{
				return;
			}

			if(card1.IsValid() && card2.IsValid() && card3.IsValid() && card4.IsValid() && card5.IsValid())
			{
				Card1 = card1;
				Card2 = card2;
				Card3 = card3;
				Card4 = card4;
				Card5 = card5;

				Street = 3;

				SetProperties();
			}
		}

		public void Set(string str)
		{
			Invalid();

			if(str == null)
			{
				return;
			}

			str = str.Replace(" ", "");
			str = str.Replace(",", "");

			str = str.Replace('t', 'T');
			str = str.Replace('j', 'J');
			str = str.Replace('q', 'Q');
			str = str.Replace('k', 'K');
			str = str.Replace('a', 'A');

			str = str.Replace('C', 'c');
			str = str.Replace('D', 'd');
			str = str.Replace('H', 'h');
			str = str.Replace('S', 's');

			string str1 = "";
			string str2 = "";
			string str3 = "";
			string str4 = "";
			string str5 = "";

			int pos1 = 0;
			int pos2 = 0;
			int pos3 = 0;
			int pos4 = 0;
			int pos5 = 0;

			bool Card1 = CardPattern(str, ref str1, pos1, ref pos1);
			bool Card2 = CardPattern(str, ref str2, pos1, ref pos2);
			bool Card3 = CardPattern(str, ref str3, pos2, ref pos3);
			bool Card4 = CardPattern(str, ref str4, pos3, ref pos4);
			bool Card5 = CardPattern(str, ref str5, pos4, ref pos5);

			if(Card1 && Card2 && Card3 && Card4 && Card5)
			{
				Set(new Card(str1), new Card(str2), new Card(str3), new Card(str4), new Card(str5));
				
				return;
			}

			if(Card1 && Card2 && Card3 && Card4)
			{
				Set(new Card(str1), new Card(str2), new Card(str3), new Card(str4));
				
				return;
			}

			if(Card1 && Card2 && Card3)
			{
				Set(new Card(str1), new Card(str2), new Card(str3));
				
				return;
			}
		}

		public void Invalid()
		{
			Title = "Invalid";

			Card1 = new Card();
			Card2 = new Card();
			Card3 = new Card();
			Card4 = new Card();
			Card5 = new Card();

			Mask = ulong.MaxValue;
		}

		public bool IsValid()
		{
			if(Street < 1 || Street > 3)
			{
				return false;
			}

			if(Street == 1)
			{
				return Card1.IsValid() && Card2.IsValid() && Card3.IsValid();
			}

			if(Street == 2)
			{
				return Card1.IsValid() && Card2.IsValid() && Card3.IsValid() && Card4.IsValid();
			}

			if(Street == 3)
			{
				return Card1.IsValid() && Card2.IsValid() && Card3.IsValid() && Card4.IsValid() && Card5.IsValid();
			}

			return false;
		}

		public static bool operator == (Board Board1, Board Board2)
		{
			if(Board1.IsValid() == false || Board2.IsValid() == false)
			{
				return false;
			}

			if(Board1.Street != Board2.Street)
			{
				return false;
			}

			if(Board1.Mask == Board2.Mask)
			{
				return true;
			}
			
			return false;
		}

		public static bool operator != (Board Board1, Board Board2)
		{
			if(Board1.IsValid() == false || Board2.IsValid() == false)
			{
				return false;
			}

			if(Board1.Street != Board2.Street)
			{
				return true;
			}

			if(Board1.Mask != Board2.Mask)
			{
				return true;
			}

			return false;
		}

		public override string ToString()
		{
			return Title;
		}

		public string Text()
		{
			if(IsValid())
			{
				if(Street == 1)
				{
					return Card1.Title + " " + Card2.Title + " " + Card3.Title;
				}

				if(Street == 2)
				{
					return Card1.Title + " " + Card2.Title + " " + Card3.Title + " " + Card4.Title;
				}

				if(Street == 3)
				{
					return Card1.Title + " " + Card2.Title + " " + Card3.Title + " " + Card4.Title + " " + Card5.Title;
				}
			}
			
			return "Invalid";
		}

		private bool CardPattern(string str, ref string title, int start, ref int pos)
		{
			title = null;

			int len = Math.Min(str.Length, 32);

			for(int i=start; i<len-1; ++i)
			{
				if((str[i] >= '2' && str[i] <= '9') || str[i] == 'T' || str[i] == 'J' || str[i] == 'Q' || str[i] == 'K' || str[i] == 'A')
				{
					if(str[i+1] == 'c' || str[i+1] == 'd' || str[i+1] == 'h' || str[i+1] == 's')
					{
						pos = i+2;
						title += str[i];
						title += str[i+1];
						return true;
					}
				}
			}

			return false;
		}

		private void UpdateTitle()
		{
			if(Street == 1)
			{
				Title = Card1.Title + " " + Card2.Title + " " + Card3.Title;
				
				return;
			}

			if(Street == 2)
			{
				Title = Card1.Title + " " + Card2.Title + " " + Card3.Title + " " + Card4.Title;
				
				return;
			}

			if(Street == 3)
			{
				Title = Card1.Title + " " + Card2.Title + " " + Card3.Title + " " + Card4.Title + " " + Card5.Title;
				
				return;
			}

			Title = "Invalid";
		}

		private void UpdateMask()
		{
			if(Street == 1)
			{
				Mask = Card1.Mask | Card2.Mask | Card3.Mask;

				return;
			}

			if(Street == 2)
			{
				Mask = Card1.Mask | Card2.Mask | Card3.Mask | Card4.Mask;

				return;
			}

			if(Street == 3)
			{
				Mask = Card1.Mask | Card2.Mask | Card3.Mask | Card4.Mask | Card5.Mask;

				return;
			}
		}

		private void UpdateFlushLevel()
		{
			// Сколько карт одной масти содержит данный борд.

			int[] levels = new int[4];
			
			++levels[Card1.Suit];
			++levels[Card2.Suit];
			++levels[Card3.Suit];
			
			if(Street > 1)
			{
				++levels[Card4.Suit];
			}
			
			if(Street > 2)
			{
				++levels[Card5.Suit];
			}
			
			FlushLevel = Math.Max(Math.Max(levels[0], levels[1]), Math.Max(levels[2], levels[3]));
		}

		private void UpdateStraightLevel()
		{
			// A 2 3 4 5 6 7 8 9 T  J  Q  K  A

			// 0 1 2 3 4 5 6 7 8 9 10 11 12 13

			bool[] positions = new bool[14];

			positions[Card1.Denomination+1] = true;
			positions[Card2.Denomination+1] = true;
			positions[Card3.Denomination+1] = true;

			if(Street > 1)
			{
				positions[Card4.Denomination+1] = true;
			}
			
			if(Street > 2)
			{
				positions[Card5.Denomination+1] = true;
			}

			positions[0] = positions[13];

			// Какое максимальное число карт стоит подряд:

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

			// Максимальное число карт, которые могут составить стрит:

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

		private void SetProperties()
		{
			UpdateTitle();
			UpdateMask();

			UpdateFlushLevel();
			UpdateStraightLevel();
		}
	}
}
