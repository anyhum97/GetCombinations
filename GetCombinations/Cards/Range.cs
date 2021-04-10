using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Action_Equity_
{
	public class Range
	{
		public float[] Probability;

		public int Count;

		public Range()
		{
			Probability = new float[1326];
			Count = 0;
		}

		public Range(string[] str)
		{
			Probability = new float[1326];
			Import(str);
		}

		public void Import(string[] str)
		{
			Count = 0;

			foreach(var hnd in str)
			{
				Hand hand = new Hand(hnd);

				if(hand.IsValid())
				{
					int index = Hand.GetHandIndex(hand);

					Probability[index] = 1.0f;

					++Count;
				}
			}
		}

		public void Add(string[] str)
		{
			foreach(var hnd in str)
			{
				Hand hand = new Hand(hnd);

				if(hand.IsValid())
				{
					int index = Hand.GetHandIndex(hand);

					if(Probability[index] == 0.0f)
					{
						Probability[index] = 1.0f;
						++Count;
					}					
				}
			}
		}

		public bool IsValid()
		{
			if(Probability != null)
			{
				if(Count > 0)
				{
					return true;
				}
			}

			return false;
		}

		public float this[int key]
		{
			get => Probability[key];
		}
	}
}
