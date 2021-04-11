using System;

using static Combinations.GetCombinations;

namespace Combinations
{
	public static class Equity
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Calculates hand strength on the flop against one opponent's range.<br></br><br></br>
		/// Вычисляет силу руки на флопе против спектра одного оппонента.
		/// </summary>
		/// <param name="Probability">Веса спектра оппонента [1326]</param>
		/// <returns></returns>
		public static float GetFlopEquity(ulong Board, ulong Hand1, float[] Probability)
		{
			ulong Base1 = Board | Hand1;

			float equity = 0.0f;
			float factor = 0.0f;

			for(int h2=0; h2<1326; ++h2)
			{
				float prob2 = Probability[h2];

				if(prob2 > 0.0f)
				{
					ulong Hand2 = DefaultHandMask[h2];

					if((Hand2 & Base1) == 0)
					{
						ulong Base2 = Base1 | Hand2;

						for(int c4=0; c4<52; ++c4)
						{
							ulong Card4 = DefaultCardMask[c4];

							if((Card4 & Base2) == 0)
							{
								ulong Base3 = Base2 | Card4;

								for(int c5=0; c5<52; ++c5)
								{
									ulong Card5 = DefaultCardMask[c5];

									if((Card5 & Base3) == 0)
									{
										uint Rang1 = GetTexasHoldemCombinationRank(Base1 | Card4 | Card5);

										uint Rang2 = GetTexasHoldemCombinationRank(Board | Card4 | Card5 | Hand2);
										
										if(Rang1 > Rang2)
										{
											equity += prob2;
										}

										if(Rang1 == Rang2)
										{
											equity += 0.5f*prob2;
										}

										factor += prob2;
									}
								}
							}
						}
					}
				}
			}

			if(factor == 0.0f)
			{
				return 0.0f;
			}

			return equity / factor;
		}

		/// <summary>
		/// Calculates hand strength on the turn against one opponent's range.<br></br><br></br>
		/// Вычисляет силу руки на тёрне против спектра одного оппонента.
		/// </summary>
		/// <param name="Probability">Веса спектра оппонента [1326]</param>
		/// <returns></returns>
		public static float GetTurnEquity(ulong Board, ulong Hand1, float[] Probability)
		{
			ulong Base1 = Board | Hand1;

			float equity = 0.0f;
			float factor = 0.0f;

			for(int h2=0; h2<1326; ++h2)
			{
				float prob2 = Probability[h2];

				if(prob2 > 0.0f)
				{
					ulong Hand2 = DefaultHandMask[h2];

					if((Hand2 & Base1) == 0)
					{
						ulong Base2 = Base1 | Hand2;

						for(int c5=0; c5<52; ++c5)
						{
							ulong Card5 = DefaultCardMask[c5];

							if((Card5 & Base2) == 0)
							{
								uint Rang1 = GetTexasHoldemCombinationRank(Base1 | Card5);

								uint Rang2 = GetTexasHoldemCombinationRank(Board | Card5 | Hand2);
							
								if(Rang1 > Rang2)
								{
									equity += prob2;
								}

								if(Rang1 == Rang2)
								{
									equity += 0.5f*prob2;
								}

								factor += prob2;
							}
						}
					}
				}
			}

			if(factor == 0.0f)
			{
				return 0.0f;
			}

			return equity / factor;
		}
		
		/// <summary>
		/// Calculates hand strength on the river against one opponent's range.<br></br><br></br>
		/// Вычисляет силу руки на ривере против спектра одного оппонента.
		/// </summary>
		/// <param name="Probability">Веса спектра оппонента [1326]</param>
		/// <returns></returns>
		public static float GetRiverEquity(ulong Board, ulong Hand1, float[] Probability)
		{
			ulong Base1 = Board | Hand1;

			float equity = 0.0f;
			float factor = 0.0f;

			for(int h2=0; h2<1326; ++h2)
			{
				ulong Hand2 = DefaultHandMask[h2];

				if((Hand2 & Base1) == 0)
				{
					float prob = Probability[h2];

					if(prob > 0.0f)
					{
						uint Rang1 = GetTexasHoldemCombinationRank(Base1);

						uint Rang2 = GetTexasHoldemCombinationRank(Board | Hand2);

						if(Rang1 > Rang2)
						{
							equity += prob;
						}

						if(Rang1 == Rang2)
						{
							equity += 0.5f*prob;
						}

						factor += prob;
					}
				}
			}

			if(factor == 0.0f)
			{
				return 0.0f;
			}

			return equity / factor;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Calculates hand strength on the flop against two opponent's ranges.<br></br><br></br>
		/// Вычисляет силу руки на флопе против спектра двух оппонентов.
		/// </summary>
		public static float GetFlopEquity(ulong Board, ulong Hand1, float[] Probability2, float[] Probability3, int iterations)
		{
			Random Random = new Random();

			iterations = Math.Max(iterations, 1);

			ulong Base1 = Board | Hand1;

			double equity = 0.0;
			double factor = 0.0;

			for(int h2=0; h2<1326; ++h2)
			{
				float prob2 = Probability2[h2];

				if(prob2 > 0.0f)
				{
					ulong Hand2 = DefaultHandMask[h2];

					if((Hand2 & Base1) == 0)
					{
						ulong Base2 = Base1 | Hand2;

						for(int h3=0; h3<1326; ++h3)
						{
							float prob3 = Probability3[h3];

							if(prob3 > 0.0f)
							{
								ulong Hand3 = DefaultHandMask[h3];

								if((Hand3 & Base2) == 0)
								{
									ulong Base3 = Base2 | Hand3;

									for(int r1=0; r1<iterations; ++r1)
									{
										int index = Random.Next(51);

										ulong Card4 = DefaultCardMask[index];

										while((Card4 & Base3) > 0)
										{
											index = Random.Next(51);

											Card4 = DefaultCardMask[index];
										}

										ulong Base4 = Base3 | Card4;

										ulong Card5 = DefaultCardMask[index];

										while((Card5 & Base4) > 0)
										{
											index = Random.Next(51);

											Card5 = DefaultCardMask[index];
										}

										uint Rang1 = GetTexasHoldemCombinationRank(Board | Card4 | Card5 | Hand1);

										uint Rang2 = GetTexasHoldemCombinationRank(Board | Card4 | Card5 | Hand2);

										uint Rang3 = GetTexasHoldemCombinationRank(Board | Card4 | Card5 | Hand3);

										if(Rang1 > Rang2 && Rang1 > Rang3)
										{
											equity += prob2*prob3;
										}

										if(Rang1 == Rang2 && Rang1 > Rang3)
										{
											equity += 0.5*(double)prob2*prob3;
										}
										
										if(Rang1 == Rang3 && Rang1 > Rang2)
										{
											equity += 0.5*(double)prob2*prob3;
										}
										
										if(Rang1 == Rang2 && Rang1 == Rang3)
										{
											equity += 0.333*(double)prob2*prob3;
										}

										factor += prob2*prob3;
									}
								}
							}
						}
					}
				}
			}

			if(factor == 0.0)
			{
				return 0.0f;
			}

			return (float)(equity / factor);
		}

		/// <summary>
		/// Calculates hand strength on the turn against two opponent's ranges.<br></br><br></br>
		/// Вычисляет силу руки на тёрне против спектра двух оппонентов.
		/// </summary>
		public static float GetTurnEquity(ulong Board, ulong Hand1, float[] Probability2, float[] Probability3)
		{
			ulong Base1 = Board | Hand1;

			double equity = 0.0;
			double factor = 0.0;

			for(int h2=0; h2<1326; ++h2)
			{
				float prob2 = Probability2[h2];

				if(prob2 > 0.0f)
				{
					ulong Hand2 = DefaultHandMask[h2];

					if((Hand2 & Base1) == 0)
					{
						ulong Base2 = Base1 | Hand2;

						for(int h3=0; h3<1326; ++h3)
						{
							float prob3 = Probability3[h3];

							if(prob3 > 0.0f)
							{
								ulong Hand3 = DefaultHandMask[h3];

								if((Hand3 & Base2) == 0)
								{
									ulong Base3 = Base2 | Hand3;

									for(int c5=0; c5<52; ++c5)
									{
										ulong Card5 = DefaultCardMask[c5];

										if((Base3 & Card5) == 0)
										{
											uint Rang1 = GetTexasHoldemCombinationRank(Board | Card5 | Hand1);

											uint Rang2 = GetTexasHoldemCombinationRank(Board | Card5 | Hand2);

											uint Rang3 = GetTexasHoldemCombinationRank(Board | Card5 | Hand3);

											if(Rang1 > Rang2 && Rang1 > Rang3)
											{
												equity += prob2*prob3;
											}

											if(Rang1 == Rang2 && Rang1 > Rang3)
											{
												equity += 0.5f*prob2*prob3;
											}
											
											if(Rang1 == Rang3 && Rang1 > Rang2)
											{
												equity += 0.5f*prob2*prob3;
											}
											
											if(Rang1 == Rang2 && Rang1 == Rang3)
											{
												equity += 0.333f*prob2*prob3;
											}

											factor += prob2*prob3;
										}
									}
								}
							}
						}
					}
				}
			}

			if(factor == 0.0)
			{
				return 0.0f;
			}

			return (float)(equity / factor);
		}

		/// <summary>
		/// Calculates hand strength on the river against two opponent's ranges.<br></br><br></br>
		/// Вычисляет силу руки на ривере против спектра двух оппонентов
		/// </summary>
		public static float GetRiverEquity(ulong Board, ulong Hand1, float[] Probability2, float[] Probability3)
		{
			ulong Base1 = Board | Hand1;

			double equity = 0.0;
			double factor = 0.0;

			for(int h2=0; h2<1326; ++h2)
			{
				float prob2 = Probability2[h2];

				if(prob2 > 0.0f)
				{
					ulong Hand2 = DefaultHandMask[h2];

					if((Hand2 & Base1) == 0)
					{
						ulong Base2 = Base1 | Hand2;

						for(int h3=0; h3<1326; ++h3)
						{
							float prob3 = Probability3[h3];

							if(prob3 > 0.0f)
							{
								ulong Hand3 = DefaultHandMask[h3];

								if((Hand3 & Base2) == 0)
								{
									uint Rang1 = GetTexasHoldemCombinationRank(Board | Hand1);

									uint Rang2 = GetTexasHoldemCombinationRank(Board | Hand2);

									uint Rang3 = GetTexasHoldemCombinationRank(Board | Hand3);

									if(Rang1 > Rang2 && Rang1 > Rang3)
									{
										equity += prob2*prob3;
									}

									if(Rang1 == Rang2 && Rang1 > Rang3)
									{
										equity += 0.5f*prob2*prob3;
									}

									if(Rang1 == Rang3 && Rang1 > Rang2)
									{
										equity += 0.5f*prob2*prob3;
									}

									if(Rang1 == Rang2 && Rang1 == Rang3)
									{
										equity += 0.333f*prob2*prob3;
									}

									factor += prob2*prob3;
								}
							}
						}
					}
				}
			}

			if(factor == 0.0)
			{
				return 0.0f;
			}

			return (float)(equity / factor);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
}

