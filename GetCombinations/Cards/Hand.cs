using System;

namespace Combinations
{
	public class Hand
	{
		public string Title { get; protected set; }

		public Card Card1 { get; protected set; }

		public Card Card2 { get; protected set; }

		public ulong Mask { get; protected set; }

		public static Hand[] DefaultHands { get; protected set; }

		static Hand()
		{
			DefaultHands = new Hand[1326];

			for(int i=0; i<1326; ++i)
			{
				DefaultHands[i] = new Hand(Data.HandIndex[i]);
			}
		}

		public Hand()
		{
			Invalid();
		}

		public Hand(Card card1, Card card2)
		{
			Set(card1, card2);
		}

		public Hand(string str)
		{
			Set(str);
		}

		public void Set(Card card1, Card card2)
		{
			Invalid();

			if(card1 == card2)
			{
				return;
			}

			if(card1.IsValid() && card2.IsValid())
			{
				if(card1.Denomination >= card2.Denomination)
				{
					Card1 = card1;
					Card2 = card2;
				}
				else
				{
					Card1 = card2;
					Card2 = card1;
				}

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

			int pos1 = 0;
			int pos2 = 0;

			bool Card1 = CardPattern(str, ref str1, pos1, ref pos1);
			bool Card2 = CardPattern(str, ref str2, pos1, ref pos2);

			if(Card1 && Card2)
			{
				Set(new Card(str1), new Card(str2));
			}
		}

		public void Invalid()
		{
			Title = "Invalid";

			Card1 = new Card();
			Card2 = new Card();
			
			Mask = ulong.MaxValue;
		}

		public bool IsValid()
		{
			return Card1.IsValid() && Card2.IsValid();
		}

		public static bool operator == (Hand Hand1, Hand Hand2)
		{
			if(Hand1.IsValid() && Hand2.IsValid())
			{
				if(Hand1.Mask == Hand2.Mask)
				{
					return true;
				}
			}

			return false;
		}

		public static bool operator != (Hand Hand1, Hand Hand2)
		{
			if(Hand1.IsValid() && Hand2.IsValid())
			{
				if(Hand1.Mask != Hand2.Mask)
				{
					return true;
				}
			}

			return false;
		}

		public override string ToString()
		{
			return Title;
		}

		public string Text()
		{
			return Card1.Title + " " + Card2.Title;
		}

		private bool CardPattern(string str, ref string title, int start, ref int pos)
		{
			title = null;

			int len = str.Length;

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
			Title = Card1.Title + " " + Card2.Title;
		}

		private void UpdateMask()
		{
			Mask = Card1.Mask | Card2.Mask;
		}

		private void SetProperties()
		{
			UpdateTitle();
			UpdateMask();
		}
	}
}
