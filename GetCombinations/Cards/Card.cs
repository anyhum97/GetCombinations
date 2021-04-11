using System;

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

		/// <summary>
		/// Denominations:<br></br>
		/// 0 => 2<br></br> 1 => 3<br></br> 2 => 4<br></br>
		/// 3 => 5<br></br> 4 => 6<br></br> 5 => 7<br></br>
		/// 6 => 8<br></br> 7 => 9<br></br> 8 => T<br></br>
		/// 9 => J<br></br> 10 => Q<br></br> 11 => K<br></br>
		/// 12 => A<br></br><br></br>
		/// Suits:<br></br>
		/// 0 => c<br></br> 1 => d<br></br> 2 => h<br></br> 3 => s<br></br>
		/// </summary>
		/// <param name="denomination"></param>
		/// <param name="suit"></param>
		public Card(int denomination, int suit)
		{
			Set(denomination, suit);
		}

		public Card(string str)
		{
			Set(str);
		}

		private static void SetDefaultCards()
		{
			DefaultCards = new Card[52];

			int index = default;

			for(int i=0; i<4; ++i)
			{
				for(int j=0; j<13; ++j)
				{
					DefaultCards[index] = new Card(j, i);

					++index;
				}
			}
		}

		private void Set(int denomination, int suit)
		{
			if(denomination < 0 || denomination > 12)
			{
				throw new Exception("Card.Set: Invalid Argument \"denomination\". Valid values from 0 to 12.");
			}
			
			if(suit < 0 || suit > 3)
			{
				throw new Exception("Card.Set: Invalid Argument \"suit\". Valid values from от 0 to 3.");
			}

			CardIndex = 13 * suit + denomination;
			
			Title = GetCombinations.DefaultCardTitle[CardIndex];
			
			Mask = 1UL << CardIndex;

			Denomination = denomination;

			Suit = suit;
		}

		private void Set(string str)
		{
			Invalid();

			if(str == null)
			{
				return;
			}

			if(str.Length >= 2)
			{
				int denomination = default;

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

				int suit = default;

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

		public string Text(int denomination, int suit)
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

			CardIndex = default;

			Denomination = default;

			Suit = default;

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

		public override string ToString()
		{
			return Title;
		}
	}
}

