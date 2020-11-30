using System;
using System.IO;
using System.Text;

namespace Combinations
{
	public class Card
	{
		public int CardIndex { get; protected set; }

		public int Denomination { get; protected set; }

		public int Suit { get; protected set; }

		public ulong Mask { get; protected set; }

		public string Title { get; protected set; }

		public static Card[] DefaultCards { get; protected set; }

		static Card()
		{
			SetDefaultCards();
		}

		public Card()
		{
			Invalid();
		}

		public Card(int index)
		{
			Set(Data.DefaultCardDenomination[index], Data.DefaultCardSuit[index]);

			//CardIndex = index;
			//
			//Mask = 1UL << index;
			//
			//Title = Data.DefaultCardTitle[CardIndex];
			//
			//Denomination = index % 13;
			//
			//Suit = index / 13;
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
			if(denomination < 0 || denomination > 12 || suit < 0 || suit > 3)
			{
				throw new Exception();
			}
			
			CardIndex = 13 * suit + denomination;
			
			Title = Data.DefaultCardTitle[CardIndex];
			
			Mask = 1UL << CardIndex;

			Denomination = denomination;

			Suit = suit;
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
				int denomination = 0;
				int suit = 0;

				switch(str[0])
				{
					case '2': denomination = 0;  break;
					case '3': denomination = 1;  break;
					case '4': denomination = 2;  break;
					case '5': denomination = 3;  break;
					case '6': denomination = 4;  break;
					case '7': denomination = 5;  break;
					case '8': denomination = 6;  break;
					case '9': denomination = 7;  break;
					case 'T': denomination = 8;  break;
					case 'J': denomination = 9;  break;
					case 'Q': denomination = 10; break;
					case 'K': denomination = 11; break;
					case 'A': denomination = 12; break;

					default: return;
				}

				switch(str[1])
				{
					case 'c': suit = 0; break;
					case 'd': suit = 1; break;
					case 'h': suit = 2; break;
					case 's': suit = 3; break;

					default: return;
				}

				Set(denomination, suit);
			}
		}

		public bool IsValid()
		{ 
			if(Mask != ulong.MaxValue)
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

			CardIndex = int.MaxValue;
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
			if(Card1.IsValid() && Card2.IsValid())
			{
				if(Card1.Mask != Card2.Mask)
				{
					return true;
				}
			}

			return false;
		}

		public static void SetDefaultCards()
		{
			DefaultCards = new Card[52];

			int index = 0;

			for(int i=0; i<4; ++i)
			{
				for(int j=0; j<13; ++j)
				{
					DefaultCards[index] = new Card(j, i);

					++index;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("[{0}]: {1}", CardIndex, Title);
		}

		private string Text(int denomination, int suit)
		{
			string str = "";

			if(denomination < 13 && suit < 4)
			{
				switch(denomination)
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

				switch(suit)
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
	}
}

