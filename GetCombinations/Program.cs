using System;

namespace Combinations
{
	partial class Program
	{
		private static void Main()
		{
			Board board1 = new Board("2c 7h 4d8cJc                           2");
			Board board2 = new Board("2c 7h 4d8cJc");
			
			Hand hand = new Hand("9h8c");

			bool s = board1 != board2;
		}
	}
}
