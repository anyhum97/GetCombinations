using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combinations
{
	partial class Program
	{
		private static void EqualsTest()
		{
			Card[] cards = new Card[52];

			int index = 0;

			for(int i=0; i<4; ++i)
			{
				for(int j=0; j<13; ++j)
				{
					cards[index] = new Card(j, i);
					++index;
				}
			}

			// Если карты равны, то их сравнение a & b не равно нулю.
			
			// Если a и b - разные карты, то их сравнение a & b равно нулю.

			Console.Write("Equals: \n\n");

			for(int i=0; i<index; ++i)
			{
				for(int j=0; j<index; ++j)
				{
					if(i == j)
					{
						if((cards[i].Mask & cards[j].Mask) == 0)
						{
							Console.Write("[True]\t");
						}
						else
						{
							Console.Write("[False]\t");
						}
					}
				}
			}

			// Если карты равны, то их сравнение a & b не равно нулю.
			
			// Если a и b - разные карты, то их сравнение a & b равно нулю.

			Console.Write("\n\nNot equals: \n\n");

			for(int i=0; i<index; ++i)
			{
				for(int j=0; j<index; ++j)
				{
					if(i != j)
					{
						if((cards[i].Mask & cards[j].Mask) == 0)
						{
							Console.Write("[True]\t");
						}
						else
						{
							Console.Write("[False]\t");
						}
					}
				}
			}

			Console.ReadKey();
		}

		private static void StraightTest()
		{
			Random random = new Random();

			for(int i=0; i<10; ++i)
			{
				Card Card1 = new Card(0, random.Next(4));

			}
		}

		private static void BoardTest()
		{
			Board board = new Board("2c2hKdKsKh");

			Console.WriteLine(board + "\n");

			Console.WriteLine("FlushLevel: " + board.FlushLevel);

			Console.WriteLine("StraightLevel: " + board.StraightLevel);

			Console.WriteLine("StraightCount: " + board.StraightCount);

			Console.WriteLine("PairLevel: " + board.PairLevel);

			Console.WriteLine("CardLevel: " + board.CardLevel);

			Console.ReadKey();
		}

		private static void CombinationTest()
		{
			Combination combination = new Combination(new Hand("Ks8c"), new Board("Qd, Jh, 8d"));

			Console.WriteLine(combination + "\n");

			Console.WriteLine("FlushLevel: " + combination.FlushLevel);

			Console.WriteLine("StraightLevel: " + combination.StraightLevel);

			Console.WriteLine("StraightCount: " + combination.StraightCount);

			Console.WriteLine("PairLevel: " + combination.PairLevel);

			Console.WriteLine("CardLevel: " + combination.CardLevel);

			Console.WriteLine();

			Console.WriteLine("FlushLevel: " + combination.Board.FlushLevel);

			Console.WriteLine("StraightLevel: " + combination.Board.StraightLevel);

			Console.WriteLine("StraightCount: " + combination.Board.StraightCount);

			Console.WriteLine("PairLevel: " + combination.Board.PairLevel);

			Console.WriteLine("CardLevel: " + combination.Board.CardLevel);

			Console.ReadKey();
		}
	}
}
