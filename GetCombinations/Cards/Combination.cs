using System;

namespace Combinations
{
	public class Combination
	{
		public string Title { get; protected set; }

		public Hand Hand { get; protected set; }

		public Board Board { get; protected set; }



		public Combination()
		{
			Invalid();
		}

		public Combination(Hand hand, Board board)
		{
			Invalid();

			if(hand.IsValid() && board.IsValid())
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
			return Hand.IsValid() && Board.IsValid();
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



		private void SetProperties()
		{
			UpdateTitle();


		}
	}
}
