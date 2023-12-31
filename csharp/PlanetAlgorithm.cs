using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public abstract class PlanetAlgorithm
{
	// Token: 0x06003A86 RID: 14982 RVA: 0x0030D444 File Offset: 0x0030B644
	public void Reset(int _seed, PlanetData _planet)
	{
		this.seed = _seed;
		this.planet = _planet;
	}

	// Token: 0x06003A87 RID: 14983
	public abstract void GenerateTerrain(double modX, double modY);

	// Token: 0x06003A88 RID: 14984
	public abstract void GenerateVegetables();

	// Token: 0x06003A89 RID: 14985 RVA: 0x0030D454 File Offset: 0x0030B654
	public virtual void CalcWaterPercent()
	{
		if (this.planet.type == EPlanetType.Gas)
		{
			this.planet.windStrength = 0f;
		}
		PlanetAlgorithm.CalcLandPercent(this.planet);
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x0030D480 File Offset: 0x0030B680
	public virtual void GenerateVeins()
	{
		PlanetData obj = this.planet;
		lock (obj)
		{
			ThemeProto themeProto = LDB.themes.Select(this.planet.theme);
			if (themeProto != null)
			{
				DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
				dotNet35Random.Next();
				dotNet35Random.Next();
				dotNet35Random.Next();
				dotNet35Random.Next();
				int birthSeed = dotNet35Random.Next();
				DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
				PlanetRawData data = this.planet.data;
				float num = 2.1f / this.planet.radius;
				VeinProto[] veinProtos = PlanetModelingManager.veinProtos;
				int[] veinModelIndexs = PlanetModelingManager.veinModelIndexs;
				int[] veinModelCounts = PlanetModelingManager.veinModelCounts;
				int[] veinProducts = PlanetModelingManager.veinProducts;
				int[] array = new int[veinProtos.Length];
				float[] array2 = new float[veinProtos.Length];
				float[] array3 = new float[veinProtos.Length];
				if (themeProto.VeinSpot != null)
				{
					Array.Copy(themeProto.VeinSpot, 0, array, 1, Math.Min(themeProto.VeinSpot.Length, array.Length - 1));
				}
				if (themeProto.VeinCount != null)
				{
					Array.Copy(themeProto.VeinCount, 0, array2, 1, Math.Min(themeProto.VeinCount.Length, array2.Length - 1));
				}
				if (themeProto.VeinOpacity != null)
				{
					Array.Copy(themeProto.VeinOpacity, 0, array3, 1, Math.Min(themeProto.VeinOpacity.Length, array3.Length - 1));
				}
				float p = 1f;
				ESpectrType spectr = this.planet.star.spectr;
				EStarType type = this.planet.star.type;
				if (type == EStarType.MainSeqStar)
				{
					if (spectr == ESpectrType.M)
					{
						p = 2.5f;
					}
					else if (spectr == ESpectrType.K)
					{
						p = 1f;
					}
					else if (spectr == ESpectrType.G)
					{
						p = 0.7f;
					}
					else if (spectr == ESpectrType.F)
					{
						p = 0.6f;
					}
					else if (spectr == ESpectrType.A)
					{
						p = 1f;
					}
					else if (spectr == ESpectrType.B)
					{
						p = 0.4f;
					}
					else if (spectr == ESpectrType.O)
					{
						p = 1.6f;
					}
				}
				else if (type == EStarType.GiantStar)
				{
					p = 2.5f;
				}
				else if (type == EStarType.WhiteDwarf)
				{
					p = 3.5f;
					array[9]++;
					array[9]++;
					int num2 = 1;
					while (num2 < 12 && dotNet35Random.NextDouble() < 0.44999998807907104)
					{
						array[9]++;
						num2++;
					}
					array2[9] = 0.7f;
					array3[9] = 1f;
					array[10]++;
					array[10]++;
					int num3 = 1;
					while (num3 < 12 && dotNet35Random.NextDouble() < 0.44999998807907104)
					{
						array[10]++;
						num3++;
					}
					array2[10] = 0.7f;
					array3[10] = 1f;
					array[12]++;
					int num4 = 1;
					while (num4 < 12 && dotNet35Random.NextDouble() < 0.5)
					{
						array[12]++;
						num4++;
					}
					array2[12] = 0.7f;
					array3[12] = 0.3f;
				}
				else if (type == EStarType.NeutronStar)
				{
					p = 4.5f;
					array[14]++;
					int num5 = 1;
					while (num5 < 12 && dotNet35Random.NextDouble() < 0.6499999761581421)
					{
						array[14]++;
						num5++;
					}
					array2[14] = 0.7f;
					array3[14] = 0.3f;
				}
				else if (type == EStarType.BlackHole)
				{
					p = 5f;
					array[14]++;
					int num6 = 1;
					while (num6 < 12 && dotNet35Random.NextDouble() < 0.6499999761581421)
					{
						array[14]++;
						num6++;
					}
					array2[14] = 0.7f;
					array3[14] = 0.3f;
				}
				for (int i = 0; i < themeProto.RareVeins.Length; i++)
				{
					int num7 = themeProto.RareVeins[i];
					float num8 = (this.planet.star.index == 0) ? themeProto.RareSettings[i * 4] : themeProto.RareSettings[i * 4 + 1];
					float num9 = themeProto.RareSettings[i * 4 + 2];
					float num10 = themeProto.RareSettings[i * 4 + 3];
					float num11 = num10;
					num8 = 1f - Mathf.Pow(1f - num8, p);
					num10 = 1f - Mathf.Pow(1f - num10, p);
					num11 = 1f - Mathf.Pow(1f - num11, p);
					if (dotNet35Random.NextDouble() < (double)num8)
					{
						array[num7]++;
						array2[num7] = num10;
						array3[num7] = num10;
						int num12 = 1;
						while (num12 < 12 && dotNet35Random.NextDouble() < (double)num9)
						{
							array[num7]++;
							num12++;
						}
					}
				}
				bool flag2 = this.planet.galaxy.birthPlanetId == this.planet.id;
				if (flag2)
				{
					this.planet.GenBirthPoints(data, birthSeed);
				}
				float num13 = this.planet.star.resourceCoef;
				bool isInfiniteResource = GameMain.data.gameDesc.isInfiniteResource;
				bool isRareResource = GameMain.data.gameDesc.isRareResource;
				if (flag2)
				{
					num13 *= 0.6666667f;
				}
				else if (isRareResource)
				{
					if (num13 > 1f)
					{
						num13 = Mathf.Pow(num13, 0.8f);
					}
					num13 *= 0.7f;
				}
				float num14 = 1f;
				num14 *= 1.1f;
				Array.Clear(this.veinVectors, 0, this.veinVectors.Length);
				Array.Clear(this.veinVectorTypes, 0, this.veinVectorTypes.Length);
				this.veinVectorCount = 0;
				Vector3 vector;
				if (flag2)
				{
					vector = this.planet.birthPoint;
					vector.Normalize();
					vector *= 0.75f;
				}
				else
				{
					vector.x = (float)dotNet35Random2.NextDouble() * 2f - 1f;
					vector.y = (float)dotNet35Random2.NextDouble() - 0.5f;
					vector.z = (float)dotNet35Random2.NextDouble() * 2f - 1f;
					vector.Normalize();
					vector *= (float)(dotNet35Random2.NextDouble() * 0.4 + 0.2);
				}
				this.planet.veinBiasVector = vector;
				if (flag2)
				{
					this.veinVectorTypes[0] = EVeinType.Iron;
					this.veinVectors[0] = this.planet.birthResourcePoint0;
					this.veinVectorTypes[1] = EVeinType.Copper;
					this.veinVectors[1] = this.planet.birthResourcePoint1;
					this.veinVectorCount = 2;
				}
				int num15 = 1;
				while (num15 < 15 && this.veinVectorCount < this.veinVectors.Length)
				{
					EVeinType eveinType = (EVeinType)num15;
					int num16 = array[num15];
					if (num16 > 1)
					{
						num16 += dotNet35Random2.Next(-1, 2);
					}
					for (int j = 0; j < num16; j++)
					{
						int num17 = 0;
						Vector3 vector2 = Vector3.zero;
						bool flag3 = false;
						while (num17++ < 200)
						{
							vector2.x = (float)dotNet35Random2.NextDouble() * 2f - 1f;
							vector2.y = (float)dotNet35Random2.NextDouble() * 2f - 1f;
							vector2.z = (float)dotNet35Random2.NextDouble() * 2f - 1f;
							if (eveinType != EVeinType.Oil)
							{
								vector2 += vector;
							}
							vector2.Normalize();
							float num18 = data.QueryHeight(vector2);
							if (num18 >= this.planet.radius && (eveinType != EVeinType.Oil || num18 >= this.planet.radius + 0.5f))
							{
								bool flag4 = false;
								float num19 = (eveinType == EVeinType.Oil) ? 100f : 196f;
								for (int k = 0; k < this.veinVectorCount; k++)
								{
									if ((this.veinVectors[k] - vector2).sqrMagnitude < num * num * num19)
									{
										flag4 = true;
										break;
									}
								}
								if (!flag4)
								{
									flag3 = true;
									break;
								}
							}
						}
						if (flag3)
						{
							this.veinVectors[this.veinVectorCount] = vector2;
							this.veinVectorTypes[this.veinVectorCount] = eveinType;
							this.veinVectorCount++;
							if (this.veinVectorCount == this.veinVectors.Length)
							{
								break;
							}
						}
					}
					num15++;
				}
				data.veinCursor = 1;
				this.tmp_vecs.Clear();
				VeinData veinData = default(VeinData);
				for (int l = 0; l < this.veinVectorCount; l++)
				{
					this.tmp_vecs.Clear();
					Vector3 normalized = this.veinVectors[l].normalized;
					EVeinType eveinType2 = this.veinVectorTypes[l];
					int num20 = (int)eveinType2;
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normalized);
					Vector3 a = rotation * Vector3.right;
					Vector3 a2 = rotation * Vector3.forward;
					this.tmp_vecs.Add(Vector2.zero);
					int num21 = Mathf.RoundToInt(array2[num20] * (float)dotNet35Random2.Next(20, 25));
					if (eveinType2 == EVeinType.Oil)
					{
						num21 = 1;
					}
					float num22 = array3[num20];
					if (flag2 && l < 2)
					{
						num21 = 6;
						num22 = 0.2f;
					}
					int num23 = 0;
					while (num23++ < 20)
					{
						int count = this.tmp_vecs.Count;
						int num24 = 0;
						while (num24 < count && this.tmp_vecs.Count < num21)
						{
							if (this.tmp_vecs[num24].sqrMagnitude <= 36f)
							{
								double num25 = dotNet35Random2.NextDouble() * 3.141592653589793 * 2.0;
								Vector2 vector3 = new Vector2((float)Math.Cos(num25), (float)Math.Sin(num25));
								vector3 += this.tmp_vecs[num24] * 0.2f;
								vector3.Normalize();
								Vector2 vector4 = this.tmp_vecs[num24] + vector3;
								bool flag5 = false;
								for (int m = 0; m < this.tmp_vecs.Count; m++)
								{
									if ((this.tmp_vecs[m] - vector4).sqrMagnitude < 0.85f)
									{
										flag5 = true;
										break;
									}
								}
								if (!flag5)
								{
									this.tmp_vecs.Add(vector4);
								}
							}
							num24++;
						}
						if (this.tmp_vecs.Count >= num21)
						{
							break;
						}
					}
					float num26 = num13;
					if (eveinType2 == EVeinType.Oil)
					{
						num26 = Mathf.Pow(num13, 0.5f);
					}
					int num27 = Mathf.RoundToInt(num22 * 100000f * num26);
					if (num27 < 20)
					{
						num27 = 20;
					}
					int num28 = (num27 < 16000) ? Mathf.FloorToInt((float)num27 * 0.9375f) : 15000;
					int minValue = num27 - num28;
					int maxValue = num27 + num28 + 1;
					for (int n = 0; n < this.tmp_vecs.Count; n++)
					{
						Vector3 b = (this.tmp_vecs[n].x * a + this.tmp_vecs[n].y * a2) * num;
						veinData.type = eveinType2;
						veinData.groupIndex = (short)(l + 1);
						veinData.modelIndex = (short)dotNet35Random2.Next(veinModelIndexs[num20], veinModelIndexs[num20] + veinModelCounts[num20]);
						veinData.amount = Mathf.RoundToInt((float)dotNet35Random2.Next(minValue, maxValue) * num14);
						if (eveinType2 != EVeinType.Oil)
						{
							veinData.amount = Mathf.RoundToInt((float)veinData.amount * DSPGame.GameDesc.resourceMultiplier);
						}
						else
						{
							veinData.amount = Mathf.RoundToInt((float)veinData.amount * DSPGame.GameDesc.oilAmountMultiplier);
						}
						if (veinData.amount < 1)
						{
							veinData.amount = 1;
						}
						if (isInfiniteResource && veinData.type != EVeinType.Oil)
						{
							veinData.amount = 1000000000;
						}
						veinData.productId = veinProducts[num20];
						veinData.pos = normalized + b;
						if (veinData.type == EVeinType.Oil)
						{
							veinData.pos = this.planet.aux.RawSnap(veinData.pos);
						}
						veinData.minerCount = 0;
						float num29 = data.QueryHeight(veinData.pos);
						data.EraseVegetableAtPoint(veinData.pos);
						veinData.pos = veinData.pos.normalized * num29;
						if (this.planet.waterItemId == 0 || num29 >= this.planet.radius)
						{
							data.AddVeinData(veinData);
						}
					}
				}
				this.tmp_vecs.Clear();
			}
		}
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x0030E188 File Offset: 0x0030C388
	public static void CalcLandPercent(PlanetData _planet)
	{
		if (_planet == null)
		{
			return;
		}
		PlanetRawData data = _planet.data;
		if (data == null)
		{
			return;
		}
		int stride = data.stride;
		int num = stride / 2;
		int dataLength = data.dataLength;
		ushort[] heightData = data.heightData;
		if (heightData == null)
		{
			return;
		}
		float num2 = _planet.radius * 100f - 20f;
		if (_planet.type == EPlanetType.Gas)
		{
			_planet.landPercent = 0f;
			return;
		}
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < dataLength; i++)
		{
			int num5 = i % stride;
			int num6 = i / stride;
			if (num5 > num)
			{
				num5--;
			}
			if (num6 > num)
			{
				num6--;
			}
			if ((num5 & 1) == 1 && (num6 & 1) == 1)
			{
				if ((float)heightData[i] >= num2)
				{
					num4++;
				}
				else if (data.GetModLevel(i) == 3)
				{
					num4++;
				}
				num3++;
			}
		}
		_planet.landPercent = ((num3 > 0) ? ((float)num4 / (float)num3) : 0f);
	}

	// Token: 0x04004BD7 RID: 19415
	protected int seed;

	// Token: 0x04004BD8 RID: 19416
	protected PlanetData planet;

	// Token: 0x04004BD9 RID: 19417
	private Vector3[] veinVectors = new Vector3[512];

	// Token: 0x04004BDA RID: 19418
	private EVeinType[] veinVectorTypes = new EVeinType[512];

	// Token: 0x04004BDB RID: 19419
	private int veinVectorCount;

	// Token: 0x04004BDC RID: 19420
	private List<Vector2> tmp_vecs = new List<Vector2>(100);
}
