using System;

namespace Combinations
{
	public class Card
	{
		public string Title { get; protected set; }

		public int Denomination { get; protected set; }

		public int Suit { get; protected set; }

		public ulong Mask { get; protected set; }

		public Card()
		{
			Invalid();
		}

		public Card(int denomination, int suit)
		{
			Set(denomination, suit);
		}

		public Card(string str)
		{
			Set(str);
		}

		public void Set(int denomination, int suit)
		{
			if(denomination > 12 || suit > 3)
			{
				Invalid();
				return;
			}

			Denomination = denomination;
			Suit = suit;

			UpdateMask();
			UpdateTitle();
		}

		public void Set(string str)
		{
			Invalid();

			if(str == null)
			{
				return;
			}

			if(str.Length >= 2)
			{
				switch(str[0])
				{
					case '2': Denomination = 0;  break;
					case '3': Denomination = 1;  break;
					case '4': Denomination = 2;  break;
					case '5': Denomination = 3;  break;
					case '6': Denomination = 4;  break;
					case '7': Denomination = 5;  break;
					case '8': Denomination = 6;  break;
					case '9': Denomination = 7;  break;
					case 'T': Denomination = 8;  break;
					case 'J': Denomination = 9;  break;
					case 'Q': Denomination = 10; break;
					case 'K': Denomination = 11; break;
					case 'A': Denomination = 12; break;

					default: Invalid(); return;
				}

				switch(str[1])
				{
					case 'c': Suit = 0; break;
					case 'd': Suit = 1; break;
					case 'h': Suit = 2; break;
					case 's': Suit = 3; break;

					default: Invalid(); return;
				}

				UpdateMask();
				UpdateTitle();
			}
		}

		public bool IsValid()
		{ 
			if(Denomination < 13 && Suit < 4)
			{
				return true;
			}

			return false;
		}

		public void Invalid()
		{
			Title = "Invalid";

			Denomination = int.MaxValue;
			Suit = int.MaxValue;
			
			Mask = ulong.MaxValue;
		}

		public static bool operator == (Card Card1, Card Card2)
		{
			if(Card1.IsValid() && Card2.IsValid())
			{
				if(Card1.Mask == Card2.Mask)
				{
					return true;
				}
			}

			return false;
		}

		public static bool operator != (Card Card1, Card Card2)
		{
			if(Card1.IsValid() == false || Card2.IsValid() == false)
			{
				return true;
			}

			if(Card1.Mask != Card2.Mask)
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
			string str = "";

			if(Denomination < 13 && Suit < 4)
			{
				switch(Denomination)
				{
					case 0: str += '2'; break;
					case 1: str += '3'; break;
					case 2: str += '4'; break;
					case 3: str += '5'; break;
					case 4: str += '6'; break;
					case 5: str += '7'; break;
					case 6: str += '8'; break;
					case 7: str += '9'; break;
					case 8: str += 'T'; break;
					case 9: str += 'J'; break;
					case 10: str += 'Q'; break;
					case 11: str += 'K'; break;
					case 12: str += 'A'; break;
				}

				switch(Suit)
				{
					case 0: str += 'c'; break;
					case 1: str += 'd'; break;
					case 2: str += 'h'; break;
					case 3: str += 's'; break;
				}
			}
			else
			{
				str = "Invalid";
			}

			return str;
		}

		private void UpdateMask()
		{
			Mask = 1UL << (Denomination + 13 * Suit);
		}

		private void UpdateTitle()
		{
			Title = Text();
		}
	}
}


