using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class PlanetAlgorithm7 : PlanetAlgorithm
{
	// Token: 0x06003AC2 RID: 15042 RVA: 0x0031A4AC File Offset: 0x003186AC
	public override void GenerateTerrain(double modX, double modY)
	{
		double num = 0.008;
		double num2 = 0.01;
		double num3 = 0.01;
		double num4 = 3.0;
		double num5 = -2.4;
		double num6 = 0.9;
		double num7 = 0.5;
		double num8 = 2.5;
		double num9 = 0.3;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		int seed = dotNet35Random.Next();
		int seed2 = dotNet35Random.Next();
		SimplexNoise simplexNoise = new SimplexNoise(seed);
		SimplexNoise simplexNoise2 = new SimplexNoise(seed2);
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num10 = (double)(data.vertices[i].x * this.planet.radius);
			double num11 = (double)(data.vertices[i].y * this.planet.radius);
			double num12 = (double)(data.vertices[i].z * this.planet.radius);
			double num13 = simplexNoise.Noise3DFBM(num10 * num, num11 * num2, num12 * num3, 6, 0.5, 2.0) * num4 + num5;
			double num14 = simplexNoise2.Noise3DFBM(num10 * 0.0025, num11 * 0.0025, num12 * 0.0025, 3, 0.5, 2.0) * num4 * num6 + num7;
			double num15 = (num14 > 0.0) ? (num14 * 0.5) : num14;
			double num16 = num13 + num15;
			double num17 = (num16 > 0.0) ? (num16 * 0.5) : (num16 * 1.6);
			double num18 = (num17 > 0.0) ? Maths.Levelize3(num17, 0.7, 0.0) : Maths.Levelize2(num17, 0.5, 0.0);
			double num19 = simplexNoise2.Noise3DFBM(num10 * num * 2.5, num11 * num2 * 8.0, num12 * num3 * 2.5, 2, 0.5, 2.0) * 0.6 - 0.3;
			double num20 = num17 * num8 + num19 + num9;
			double num21 = (num20 < 1.0) ? num20 : ((num20 - 1.0) * 0.8 + 1.0);
			double num22 = num18;
			double num23 = num21;
			data.heightData[i] = (ushort)(((double)this.planet.radius + num22) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num23 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x0031A7D8 File Offset: 0x003189D8
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
		float num = 1.3f;
		float num2 = -0.5f;
		float num3 = 2.5f;
		float num4 = 4f;
		float num5 = 0.5f;
		float num6 = 1f;
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
				float num18 = (float)data.biomoData[i] * 0.01f;
				bool flag = true;
				if (PlanetAlgorithm7.diff(num17, b) > 0.2f)
				{
					flag = false;
				}
				if (PlanetAlgorithm7.diff(num17, b2) > 0.2f)
				{
					flag = false;
				}
				if (PlanetAlgorithm7.diff(num17, b3) > 0.2f)
				{
					flag = false;
				}
				if (PlanetAlgorithm7.diff(num17, b4) > 0.2f)
				{
					flag = false;
				}
				double num19 = dotNet35Random2.NextDouble();
				num19 *= num19;
				double num20 = dotNet35Random2.NextDouble();
				float d = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num21 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float angle = (float)dotNet35Random2.NextDouble() * 360f;
				float num22 = (float)dotNet35Random2.NextDouble();
				float num23 = (float)dotNet35Random2.NextDouble();
				int[] array;
				float num24;
				float num25;
				float num26;
				if (num18 < 0.8f)
				{
					array = vegetables;
					num24 = num;
					num25 = num2;
					num26 = num3;
				}
				else if (num18 < 2f)
				{
					array = vegetables2;
					num24 = num4;
					num25 = num5;
					num26 = num6;
				}
				else
				{
					array = vegetables6;
					num24 = num4;
					num25 = num5;
					num26 = num6;
				}
				double num27 = simplexNoise.Noise(num14 * 0.07, num15 * 0.07, num16 * 0.07) * (double)num24 + (double)num25 + 0.5;
				double num28 = simplexNoise2.Noise(num14 * 0.4, num15 * 0.4, num16 * 0.4) * (double)num7 + (double)num8 + 0.5;
				double num29 = num28 - 0.55;
				int[] array2;
				double num30;
				int num31;
				if (num18 > 1f)
				{
					array2 = vegetables3;
					num30 = num28;
					num31 = 4;
				}
				else if (num18 > 0.5f)
				{
					array2 = vegetables4;
					num30 = num29;
					num31 = 1;
				}
				else if (num18 > 0f)
				{
					array2 = vegetables5;
					num30 = num29;
					num31 = 1;
				}
				else
				{
					array2 = null;
					num30 = num28;
					num31 = 1;
				}
				if (flag && num20 < num27 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num19 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					Vector3 a3 = vector * num17;
					Vector3 normalized = (a2 * d + a * d2).normalized;
					float num32 = num21 * num26;
					Vector3 b5 = normalized * (num32 * num11);
					float num33 = num23 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num34 = (num22 * (vector2.z + vector2.w) + (1f - vector2.z)) * num33;
					vegeData.pos = (a3 + b5).normalized;
					num17 = data.QueryHeight(vegeData.pos);
					vegeData.pos *= num17;
					vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
					vegeData.scl = new Vector3(num34, num33, num34);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num35 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num35;
				}
				if (num20 < num30 && array2 != null && array2.Length != 0)
				{
					vegeData.protoId = (short)array2[(int)(num19 * (double)array2.Length)];
					Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a4 = rotation2 * Vector3.forward;
					Vector3 a5 = rotation2 * Vector3.right;
					Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
					for (int j = 0; j < num31; j++)
					{
						float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float num36 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
						float angle2 = (float)dotNet35Random2.NextDouble() * 360f;
						float num37 = (float)dotNet35Random2.NextDouble();
						float num38 = (float)dotNet35Random2.NextDouble();
						Vector3 a6 = vector * num17;
						Vector3 normalized2 = (a5 * d3 + a4 * d4).normalized;
						float num39 = num36 * num9;
						Vector3 b6 = normalized2 * (num39 * num11);
						float num40 = num38 * (vector3.x + vector3.y) + (1f - vector3.x);
						float num41 = (num37 * (vector3.z + vector3.w) + (1f - vector3.z)) * num40;
						vegeData.pos = (a6 + b6).normalized;
						num17 = data.QueryHeight(vegeData.pos);
						vegeData.pos *= num17;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle2, Vector3.up);
						vegeData.scl = new Vector3(num41, num40, num41);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num42 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num42;
					}
				}
			}
		}
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x0031B04C File Offset: 0x0031924C
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
				dotNet35Random.Next();
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
				if (this.planet.galaxy.birthPlanetId == this.planet.id)
				{
					Pose pose = this.planet.PredictPose(120.0);
					vector = Maths.QInvRotateLF(pose.rotation, this.planet.star.uPosition - pose.position * 40000.0);
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
							if (eveinType != EVeinType.Bamboo || data.QueryHeight(vector2) <= this.planet.realRadius - 4f)
							{
								bool flag4 = false;
								float num18 = (eveinType == EVeinType.Oil) ? 100f : 196f;
								for (int k = 0; k < this.veinVectorCount; k++)
								{
									if ((this.veinVectors[k] - vector2).sqrMagnitude < num * num * num18)
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
					int num19 = (int)eveinType2;
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normalized);
					Vector3 a = rotation * Vector3.right;
					Vector3 a2 = rotation * Vector3.forward;
					this.tmp_vecs.Add(Vector2.zero);
					int num20 = Mathf.RoundToInt(array2[num19] * (float)dotNet35Random2.Next(20, 25));
					if (eveinType2 == EVeinType.Oil)
					{
						num20 = 1;
					}
					int num21 = 0;
					while (num21++ < 20)
					{
						int count = this.tmp_vecs.Count;
						int num22 = 0;
						while (num22 < count && this.tmp_vecs.Count < num20)
						{
							if (this.tmp_vecs[num22].sqrMagnitude <= 36f)
							{
								double num23 = dotNet35Random2.NextDouble() * 3.141592653589793 * 2.0;
								Vector2 vector3 = new Vector2((float)Math.Cos(num23), (float)Math.Sin(num23));
								vector3 += this.tmp_vecs[num22] * 0.2f;
								vector3.Normalize();
								Vector2 vector4 = this.tmp_vecs[num22] + vector3;
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
							num22++;
						}
						if (this.tmp_vecs.Count >= num20)
						{
							break;
						}
					}
					float num24 = num13;
					if (eveinType2 == EVeinType.Oil)
					{
						num24 = Mathf.Pow(num13, 0.5f);
					}
					int num25 = Mathf.RoundToInt(array3[num19] * 100000f * num24);
					if (num25 < 20)
					{
						num25 = 20;
					}
					int num26 = (num25 < 16000) ? Mathf.FloorToInt((float)num25 * 0.9375f) : 15000;
					int minValue = num25 - num26;
					int maxValue = num25 + num26 + 1;
					for (int n = 0; n < this.tmp_vecs.Count; n++)
					{
						Vector3 b = (this.tmp_vecs[n].x * a + this.tmp_vecs[n].y * a2) * num;
						veinData.type = eveinType2;
						veinData.groupIndex = (short)(l + 1);
						veinData.modelIndex = (short)dotNet35Random2.Next(veinModelIndexs[num19], veinModelIndexs[num19] + veinModelCounts[num19]);
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
						veinData.productId = veinProducts[num19];
						veinData.pos = normalized + b;
						if (veinData.type == EVeinType.Oil)
						{
							veinData.pos = this.planet.aux.RawSnap(veinData.pos);
						}
						veinData.minerCount = 0;
						float d = data.QueryHeight(veinData.pos);
						data.EraseVegetableAtPoint(veinData.pos);
						veinData.pos = veinData.pos.normalized * d;
						data.AddVeinData(veinData);
					}
				}
				this.tmp_vecs.Clear();
			}
		}
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x0030EFCF File Offset: 0x0030D1CF
	private static float diff(float a, float b)
	{
		if (a <= b)
		{
			return b - a;
		}
		return a - b;
	}

	// Token: 0x04004BF2 RID: 19442
	private Vector3[] veinVectors = new Vector3[512];

	// Token: 0x04004BF3 RID: 19443
	private EVeinType[] veinVectorTypes = new EVeinType[512];

	// Token: 0x04004BF4 RID: 19444
	private int veinVectorCount;

	// Token: 0x04004BF5 RID: 19445
	private List<Vector2> tmp_vecs = new List<Vector2>(100);
}
