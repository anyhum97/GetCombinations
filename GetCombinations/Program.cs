using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Combinations
{
	partial class Program
	{
		private static Dictionary<Tuple<int, int>, int> HandIndex;

		private static readonly int[] HashIndex = new int[51]
		{
			-1, 49, 98, 146, 193, 239, 284, 328, 371, 413, 454, 494, 533, 571, 608, 644, 679, 713, 746, 778, 809, 839, 868, 896, 923, 949, 974, 998, 1021, 1043, 1064, 1084, 1103, 1121, 1138, 1154, 1169, 1183, 1196, 1208, 1219, 1229, 1238, 1246, 1253, 1259, 1264, 1268, 1271, 1273, 1274
		};

		static Program()
		{
			HandIndex = new Dictionary<Tuple<int, int>, int>();

			int index = 0;

			for(int c1=0; c1<52; ++c1)
			{
				for(int c2=0; c2<52; ++c2)
				{
					if(c2 > c1)
					{
						HandIndex.Add(new Tuple<int, int>(c1, c2), index);

						++index;
					}
				}
			}
		}

		private static void WriteHandIndex()
		{
			StringBuilder stringBuilder = new StringBuilder();
			
			int index1 = 0;

			for(int c1=0; c1<52; ++c1)
			{
				for(int c2=0; c2<52; ++c2)
				{
					if(c2 > c1)
					{
						int index2 = HandIndex[new Tuple<int, int>(c1, c2)];

						if(index1 != index2)
						{
							throw new Exception();
						}

						string str = string.Format("({0}, {1}) => {2}\n", c1, c2, index1);

						stringBuilder.Append(str);

						++index1;
					}
				}

				stringBuilder.Append("\n");
			}

			File.WriteAllText("hands.txt", stringBuilder.ToString());
		}

		private static void WriteHashIndex()
		{
			StringBuilder stringBuilder = new StringBuilder();

			for(int i=0; i<51; ++i)
			{
				stringBuilder.Append(HashIndex[i]);
				stringBuilder.Append(", ");
			}

			File.WriteAllText("hash.txt", stringBuilder.ToString());
		}

		private static int GetHandIndex(int card1, int card2)
		{
			if(card1 > card2)
			{
				card1 = card1 ^ card2;
				card2 = card1 ^ card2;
				card1 = card1 ^ card2;
			}

			return HashIndex[card1] + card2;
		}

		private static void SwapTest(int card1, int card2)
		{
			Console.WriteLine(card1 + ", " + card2);

			card1 = card1 ^ card2;
			card2 = card1 ^ card2;
			card1 = card1 ^ card2;

			Console.WriteLine(card1 + ", " + card2);
			Console.WriteLine();
		}

		private static void TestHandIndex()
		{
			StringBuilder stringBuilder = new StringBuilder();

			int index1 = 0;

			for(int c1=0; c1<52; ++c1)
			{
				for(int c2=0; c2<52; ++c2)
				{
					int card1 = Math.Min(c1, c2);
					int card2 = Math.Max(c1, c2);

					if(c2 > c1)
					{
						int index2 = HandIndex[new Tuple<int, int>(card1, card2)];

						int index3 = GetHandIndex(card1, card2);
						int index4 = GetHandIndex(card2, card1);

						if(index1 != index2 || index1 != index3 || index1 != index4)
						{
							throw new Exception();
						}

						++index1;
					}
				}
			}
		}

		private static void Main()
		{
			Hand.WritePreflopHandIndex();
		}
	}
}

