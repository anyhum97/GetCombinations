using System;

namespace Combinations
{
	partial class Program
	{
		private static void Main()
		{
			Board board1 = new Board("2c4h6d");

			Console.WriteLine(board1 + "\n");

			Console.WriteLine("FlushLevel: " + board1.FlushLevel);

			Console.WriteLine("StraightLevel: " + board1.StraightLevel);

			Console.WriteLine("StraightCount: " + board1.StraightCount);

			Console.ReadKey();
		}
	}
}
