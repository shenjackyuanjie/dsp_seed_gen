using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class PlanetAlgorithm13 : PlanetAlgorithm
{
	// Token: 0x06003AA6 RID: 15014 RVA: 0x00313290 File Offset: 0x00311490
	public override void GenerateTerrain(double modX, double modY)
	{
		double num = 0.007 * modX;
		double num2 = 0.007 * modX;
		double num3 = 0.007 * modX;
		SimplexNoise simplexNoise = new SimplexNoise(new DotNet35Random(this.planet.seed).Next());
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num4 = (double)(data.vertices[i].x * this.planet.radius);
			double num5 = (double)(data.vertices[i].y * this.planet.radius);
			double num6 = (double)(data.vertices[i].z * this.planet.radius);
			double num7 = this.Remap(-1.0, 1.0, 0.0, 1.0, simplexNoise.Noise3DFBM(num4 * num, num5 * num2, num6 * num3, 6, 0.5, 2.0));
			num7 = Math.Pow(num7, modY) * 3.0625;
			num7 = this.Remap(0.0, 2.0, 0.0, 4.0, num7);
			if (num7 < 1.0)
			{
				num7 = Math.Pow(num7, 2.0);
			}
			num7 -= 0.2;
			double num8 = Math.Min(num7, 4.0);
			Math.Max(1.0 - Math.Abs(1.0 - num8), 0.0);
			if (num8 > 2.0)
			{
				if (num8 > 3.0)
				{
					if (num8 > 3.5)
					{
						num8 = 1.0 + 2.0 * (num8 - 3.5);
					}
					else
					{
						num8 = 1.0;
					}
				}
				else
				{
					num8 = 2.0 - 1.0 * (num8 - 2.0);
				}
			}
			double num9 = num8;
			data.heightData[i] = (ushort)(((double)this.planet.radius + num9 + 0.1) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num8 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x0030FA86 File Offset: 0x0030DC86
	private double Remap(double sourceMin, double sourceMax, double targetMin, double targetMax, double x)
	{
		return (x - sourceMin) / (sourceMax - sourceMin) * (targetMax - targetMin) + targetMin;
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x0030EFCF File Offset: 0x0030D1CF
	private static float diff(float a, float b)
	{
		if (a <= b)
		{
			return b - a;
		}
		return a - b;
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x00313550 File Offset: 0x00311750
	public override void GenerateVegetables()
	{
		ThemeProto themeProto = LDB.themes.Select(this.planet.theme);
		if (themeProto == null)
		{
			return;
		}
		int[] vegetables = themeProto.Vegetables0;
		int[] vegetables2 = themeProto.Vegetables1;
		int[] vegetables3 = themeProto.Vegetables2;
		int[] vegetables4 = themeProto.Vegetables3;
		int[] vegetables5 = themeProto.Vegetables4;
		int[] vegetables6 = themeProto.Vegetables5;
		double num = 0.005;
		double num2 = 0.02;
		double num3 = 0.005;
		float num4 = 0.18f;
		float num5 = -0.34f;
		float num6 = 2.5f;
		float num7 = 2f;
		float num8 = -0.2f;
		float num9 = 1.4f;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
		SimplexNoise simplexNoise = new SimplexNoise(dotNet35Random2.Next());
		SimplexNoise simplexNoise2 = new SimplexNoise(dotNet35Random2.Next());
		PlanetRawData data = this.planet.data;
		int stride = data.stride;
		int num10 = stride / 2;
		float num11 = this.planet.radius * 3.14159f * 2f / ((float)data.precision * 4f);
		VegeData vegeData = default(VegeData);
		VegeProto[] vegeProtos = PlanetModelingManager.vegeProtos;
		Vector4[] vegeScaleRanges = PlanetModelingManager.vegeScaleRanges;
		short[] vegeHps = PlanetModelingManager.vegeHps;
		this.ComputeRawBiomo();
		for (int i = 0; i < data.dataLength; i++)
		{
			int num12 = i % stride;
			int num13 = i / stride;
			if (num12 > num10)
			{
				num12--;
			}
			if (num13 > num10)
			{
				num13--;
			}
			if (num12 % 2 == 1 && num13 % 2 == 1)
			{
				Vector3 vector = data.vertices[i];
				double num14 = (double)(data.vertices[i].x * this.planet.radius);
				double num15 = (double)(data.vertices[i].y * this.planet.radius);
				double num16 = (double)(data.vertices[i].z * this.planet.radius);
				float num17 = (float)data.heightData[i] * 0.01f;
				float b = (float)data.heightData[i + 1 + stride] * 0.01f;
				float b2 = (float)data.heightData[i - 1 + stride] * 0.01f;
				float b3 = (float)data.heightData[i + 1 - stride] * 0.01f;
				float b4 = (float)data.heightData[i - 1 - stride] * 0.01f;
				ushort num18 = data.heightData[i + 1];
				ushort num19 = data.heightData[i - 1];
				ushort num20 = data.heightData[i + stride];
				ushort num21 = data.heightData[i - stride];
				float num22 = (float)data.biomoData[i] * 0.01f;
				float num23 = this.notClampedBiomo[i];
				float num24 = this.planet.radius + 0.15f;
				bool flag = false;
				bool flag2 = true;
				if (PlanetAlgorithm13.diff(num17, b) > 0.2f)
				{
					flag2 = false;
				}
				if (PlanetAlgorithm13.diff(num17, b2) > 0.2f)
				{
					flag2 = false;
				}
				if (PlanetAlgorithm13.diff(num17, b3) > 0.2f)
				{
					flag2 = false;
				}
				if (PlanetAlgorithm13.diff(num17, b4) > 0.2f)
				{
					flag2 = false;
				}
				double num25 = dotNet35Random2.NextDouble();
				num25 *= num25;
				double num26 = dotNet35Random2.NextDouble();
				float d = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num27 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float angle = (float)dotNet35Random2.NextDouble() * 360f;
				float num28 = (float)dotNet35Random2.NextDouble();
				float num29 = (float)dotNet35Random2.NextDouble();
				float num30 = 1f;
				float num31 = 0.5f;
				float num32 = 1f;
				int[] array;
				if (!flag)
				{
					if (num23 < 0.3f)
					{
						array = vegetables;
						num30 = num4;
						num31 = num5;
						num32 = num6;
						num = 0.005;
						num2 = 0.02;
						num3 = 0.005;
					}
					else if (num23 >= 2.5f)
					{
						array = vegetables4;
						num30 = num4;
						num31 = num5 + 0.1f;
						num32 = num6;
						num = 0.005;
						num2 = 0.005;
						num3 = 0.005;
					}
					else if (num23 < 0.7f)
					{
						array = vegetables5;
						num30 = num4;
						num31 = num5 + 0.05f;
						num32 = num6;
						num = 0.005;
						num2 = 0.005;
						num3 = 0.005;
					}
					else
					{
						array = vegetables2;
						num30 = num4;
						num31 = num5 + 0.05f;
						num32 = num6;
						num = 0.005;
						num2 = 0.005;
						num3 = 0.005;
					}
				}
				else
				{
					array = null;
				}
				double num33 = simplexNoise.Noise(num14 * num, num15 * num2, num16 * num3) * (double)num30 + (double)num31 + 0.5;
				double num34 = simplexNoise2.Noise(num14 * 0.4, num15 * 0.4, num16 * 0.4) * (double)num7 + (double)num8 + 0.5;
				double num35 = num34 - 0.55;
				double num36 = num34 - 1.1;
				int[] array2;
				double num37;
				int num38;
				if (!flag)
				{
					if (num22 > 1f)
					{
						array2 = vegetables3;
						num37 = num34;
						num38 = ((vegetables6 == null || vegetables6.Length == 0) ? 4 : 2);
					}
					else if (num22 > 0.5f)
					{
						array2 = null;
						num37 = num35;
						num38 = 1;
					}
					else if (num22 > 0f)
					{
						array2 = null;
						num37 = num35;
						num38 = 1;
					}
					else
					{
						array2 = null;
						num37 = num34;
						num38 = 1;
					}
				}
				else
				{
					if (num17 >= num24 - 1f || num17 <= num24 - 2.2f)
					{
						goto IL_9B6;
					}
					array2 = vegetables6;
					num37 = num36;
					num38 = 1;
				}
				if (flag2 && num26 < num33 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num25 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					Vector3 a3 = vector * num17;
					Vector3 normalized = (a2 * d + a * d2).normalized;
					float num39 = num27 * num32;
					Vector3 b5 = normalized * (num39 * num11);
					float num40 = num29 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num41 = (num28 * (vector2.z + vector2.w) + (1f - vector2.z)) * num40;
					vegeData.pos = (a3 + b5).normalized;
					num17 = data.QueryHeight(vegeData.pos);
					vegeData.pos *= num17;
					vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
					vegeData.scl = new Vector3(num41, num40, num41);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num42 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num42;
				}
				if (num26 < num37 && array2 != null && array2.Length != 0)
				{
					vegeData.protoId = (short)array2[(int)(num25 * (double)array2.Length)];
					Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a4 = rotation2 * Vector3.forward;
					Vector3 a5 = rotation2 * Vector3.right;
					Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
					for (int j = 0; j < num38; j++)
					{
						float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float num43 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
						float angle2 = (float)dotNet35Random2.NextDouble() * 360f;
						float num44 = (float)dotNet35Random2.NextDouble();
						float num45 = (float)dotNet35Random2.NextDouble();
						Vector3 a6 = vector * num17;
						Vector3 normalized2 = (a5 * d3 + a4 * d4).normalized;
						float num46 = num43 * num9;
						Vector3 b6 = normalized2 * (num46 * num11);
						float num47 = num45 * (vector3.x + vector3.y) + (1f - vector3.x);
						float num48 = (num44 * (vector3.z + vector3.w) + (1f - vector3.z)) * num47;
						vegeData.pos = (a6 + b6).normalized;
						num17 = (flag ? num24 : data.QueryHeight(vegeData.pos));
						vegeData.pos *= num17;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle2, Vector3.up);
						vegeData.scl = new Vector3(num48, num47, num48);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num49 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num49;
					}
				}
			}
			IL_9B6:;
		}
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x00313F28 File Offset: 0x00312128
	public override void GenerateVeins()
	{
		PlanetData planet = this.planet;
		lock (planet)
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
							if (num18 >= this.planet.radius && (eveinType != EVeinType.Oil || num18 >= this.planet.radius + 0.5f) && (eveinType > EVeinType.Titanium || num18 <= this.planet.radius + 0.7f))
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

	// Token: 0x06003AAB RID: 15019 RVA: 0x00314C4C File Offset: 0x00312E4C
	public void ComputeRawBiomo()
	{
		double num = 0.007 * this.planet.mod_x;
		double num2 = 0.007 * this.planet.mod_x;
		double num3 = 0.007 * this.planet.mod_x;
		SimplexNoise simplexNoise = new SimplexNoise(new DotNet35Random(this.planet.seed).Next());
		this.notClampedBiomo = new float[this.planet.data.dataLength];
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num4 = (double)(data.vertices[i].x * this.planet.radius);
			double num5 = (double)(data.vertices[i].y * this.planet.radius);
			double num6 = (double)(data.vertices[i].z * this.planet.radius);
			double num7 = this.Remap(-1.0, 1.0, 0.0, 1.0, simplexNoise.Noise3DFBM(num4 * num, num5 * num2, num6 * num3, 6, 0.5, 2.0));
			num7 = Math.Pow(num7, this.planet.mod_y) * 3.0625;
			num7 = this.Remap(0.0, 2.0, 0.0, 4.0, num7);
			if (num7 < 1.0)
			{
				num7 = Math.Pow(num7, 2.0);
			}
			num7 -= 0.2;
			double num8 = Math.Min(num7, 4.0);
			this.notClampedBiomo[i] = (float)num8;
		}
	}

	// Token: 0x04004BE9 RID: 19433
	private Vector3[] veinVectors = new Vector3[512];

	// Token: 0x04004BEA RID: 19434
	private EVeinType[] veinVectorTypes = new EVeinType[512];

	// Token: 0x04004BEB RID: 19435
	private int veinVectorCount;

	// Token: 0x04004BEC RID: 19436
	private List<Vector2> tmp_vecs = new List<Vector2>(100);

	// Token: 0x04004BED RID: 19437
	private float[] notClampedBiomo;
}
