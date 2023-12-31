using System;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class PlanetAlgorithm10 : PlanetAlgorithm
{
	// Token: 0x06003A95 RID: 14997 RVA: 0x0030EFDC File Offset: 0x0030D1DC
	public override void GenerateTerrain(double modX, double modY)
	{
		double num = 0.007;
		double num2 = 0.007;
		double num3 = 0.007;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		int seed = dotNet35Random.Next();
		int seed2 = dotNet35Random.Next();
		int seed3 = dotNet35Random.Next();
		int seed4 = dotNet35Random.Next();
		SimplexNoise simplexNoise = new SimplexNoise(seed);
		SimplexNoise simplexNoise2 = new SimplexNoise(seed2);
		SimplexNoise simplexNoise3 = new SimplexNoise(seed3);
		SimplexNoise simplexNoise4 = new SimplexNoise(seed4);
		int num4 = dotNet35Random.Next();
		for (int i = 0; i < 10; i++)
		{
			VectorLF3 vectorLF = RandomTable.SphericNormal(ref num4, 1.0);
			Vector4 vector = new Vector4((float)vectorLF.x, (float)vectorLF.y, (float)vectorLF.z);
			vector.Normalize();
			vector *= this.planet.radius;
			vector.w = (float)(dotNet35Random.NextDouble() * 10.0 + 40.0);
			this.ellipses[i] = vector;
			if (dotNet35Random.NextDouble() > 0.5)
			{
				this.eccentricities[i] = this.Remap(0.0, 1.0, 3.0, 5.0, dotNet35Random.NextDouble());
			}
			else
			{
				this.eccentricities[i] = this.Remap(0.0, 1.0, 0.2, 0.3333333333333333, dotNet35Random.NextDouble());
			}
			this.heights[i] = this.Remap(0.0, 1.0, 1.0, 2.0, dotNet35Random.NextDouble());
		}
		PlanetRawData data = this.planet.data;
		for (int j = 0; j < data.dataLength; j++)
		{
			double num5 = (double)(data.vertices[j].x * this.planet.radius);
			double num6 = (double)(data.vertices[j].y * this.planet.radius);
			double num7 = (double)(data.vertices[j].z * this.planet.radius);
			double num8 = Maths.Levelize(num5 * 0.007, 1.0, 0.0);
			double num9 = Maths.Levelize(num6 * 0.007, 1.0, 0.0);
			double num10 = Maths.Levelize(num7 * 0.007, 1.0, 0.0);
			num8 += simplexNoise3.Noise(num5 * 0.05, num6 * 0.05, num7 * 0.05) * 0.04;
			num9 += simplexNoise3.Noise(num6 * 0.05, num7 * 0.05, num5 * 0.05) * 0.04;
			num10 += simplexNoise3.Noise(num7 * 0.05, num5 * 0.05, num6 * 0.05) * 0.04;
			double num11 = Math.Abs(simplexNoise4.Noise(num8, num9, num10));
			double num12 = (0.16 - num11) * 10.0;
			num12 = ((num12 > 0.0) ? ((num12 > 1.0) ? 1.0 : num12) : 0.0);
			num12 *= num12;
			double num13 = (simplexNoise3.Noise3DFBM(num6 * 0.005, num7 * 0.005, num5 * 0.005, 4, 0.5, 2.0) + 0.22) * 5.0;
			num13 = ((num13 > 0.0) ? ((num13 > 1.0) ? 1.0 : num13) : 0.0);
			double num14 = Math.Abs(simplexNoise4.Noise3DFBM(num8 * 1.5, num9 * 1.5, num10 * 1.5, 2, 0.5, 2.0));
			double num15 = simplexNoise2.Noise3DFBM(num5 * num * 5.0, num6 * num2 * 5.0, num7 * num3 * 5.0, 4, 0.5, 2.0);
			double num16 = num15 * 0.2;
			double num17 = 0.0;
			for (int k = 0; k < 10; k++)
			{
				double num18 = (double)this.ellipses[k].x - num5;
				double num19 = (double)this.ellipses[k].y - num6;
				double num20 = (double)this.ellipses[k].z - num7;
				double num21 = this.eccentricities[k] * num18 * num18 + num19 * num19 + num20 * num20;
				num21 = this.Remap(-1.0, 1.0, 0.2, 5.0, num15) * num21;
				if (num21 < (double)(this.ellipses[k].w * this.ellipses[k].w))
				{
					double num22 = (double)(1f - Mathf.Sqrt((float)(num21 / (double)(this.ellipses[k].w * this.ellipses[k].w))));
					double num23 = 1.0 - num22;
					double num24 = 1.0 - num23 * num23 * num23 * num23 + num16 * 2.0;
					if (num24 < 0.0)
					{
						num24 = 0.0;
					}
					num17 = this.Max(num17, this.heights[k] * num24);
				}
			}
			num5 += Math.Sin(num6 * 0.15) * 2.0;
			num6 += Math.Sin(num7 * 0.15) * 2.0;
			num7 += Math.Sin(num5 * 0.15) * 2.0;
			num5 *= num;
			num6 *= num2;
			num7 *= num3;
			double num25 = (double)Mathf.Pow((float)((simplexNoise.Noise3DFBM(num5 * 0.6, num6 * 0.6, num7 * 0.6, 4, 0.5, 1.8) + 1.0) * 0.5), 1.3f);
			double num26 = simplexNoise2.Noise3DFBM(num5 * 6.0, num6 * 6.0, num7 * 6.0, 5, 0.5, 2.0);
			num26 = this.Remap(-1.0, 1.0, -0.1, 0.15, num26);
			double num27 = simplexNoise2.Noise3DFBM(num5 * 5.0 * 3.0, num6 * 5.0, num7 * 5.0, 1, 0.5, 2.0);
			double num28 = simplexNoise2.Noise3DFBM(num5 * 5.0 * 3.0 + num27 * 0.3, num6 * 5.0 + num27 * 0.3, num7 * 5.0 + num27 * 0.3, 5, 0.5, 2.0) * 0.1;
			num25 = (double)((float)Maths.Levelize(Maths.Levelize4(num25, 1.0, 0.0), 1.0, 0.0));
			num25 = Math.Min(1.0, num25);
			if (num25 <= 0.8)
			{
				if (num25 > 0.4)
				{
					num25 += num28;
				}
				else
				{
					num25 += num26;
				}
			}
			double a = num25 * 2.5 - num25 * num17;
			double num29 = this.Max(a, num26 * 2.0);
			double num30 = (2.0 - num29) / 2.0;
			num29 -= num12 * 1.2 * num13 * num30;
			if (num29 >= 0.0)
			{
				num29 += (num11 * 0.25 + num14 * 0.6) * num30;
			}
			num29 -= 0.1;
			double num31 = num29;
			num31 = this.Max(num31, -1.0);
			num31 = Math.Abs(num31);
			double num32 = 100.0;
			if (num25 < 0.4)
			{
				num31 += this.Remap(-1.0, 1.0, -0.2, 0.2, simplexNoise.Noise3DFBM(num5 * 2.0 + num32, num6 * 2.0 + num32, num7 * 2.0 + num32, 5, 0.5, 2.0));
			}
			data.heightData[j] = (ushort)(((double)this.planet.radius + num29 + 0.1) * 100.0);
			data.biomoData[j] = (byte)Mathf.Clamp((float)(num31 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x0030FA86 File Offset: 0x0030DC86
	private double Remap(double sourceMin, double sourceMax, double targetMin, double targetMax, double x)
	{
		return (x - sourceMin) / (sourceMax - sourceMin) * (targetMax - targetMin) + targetMin;
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x0030FA97 File Offset: 0x0030DC97
	private double Max(double a, double b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x0030EFCF File Offset: 0x0030D1CF
	private static float diff(float a, float b)
	{
		if (a <= b)
		{
			return b - a;
		}
		return a - b;
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x0030FAA0 File Offset: 0x0030DCA0
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
		float num2 = -1f;
		float num3 = 2.5f;
		float num4 = 0.25f;
		float num5 = -0.45f;
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
				float num18 = (float)data.heightData[i + 1 + stride] * 0.01f;
				float num19 = (float)data.heightData[i - 1 + stride] * 0.01f;
				float num20 = (float)data.heightData[i + 1 - stride] * 0.01f;
				float num21 = (float)data.heightData[i - 1 - stride] * 0.01f;
				float num22 = (float)data.heightData[i + 1] * 0.01f;
				float num23 = (float)data.heightData[i - 1] * 0.01f;
				float num24 = (float)data.heightData[i + stride] * 0.01f;
				float num25 = (float)data.heightData[i - stride] * 0.01f;
				float num26 = (float)data.biomoData[i] * 0.01f;
				float num27 = this.planet.radius - 0.1f;
				bool flag = false;
				if (num17 < num27)
				{
					flag = true;
				}
				else if (num18 < num27)
				{
					flag = true;
				}
				else if (num19 < num27)
				{
					flag = true;
				}
				else if (num20 < num27)
				{
					flag = true;
				}
				else if (num21 < num27)
				{
					flag = true;
				}
				else if (num22 < num27)
				{
					flag = true;
				}
				else if (num23 < num27)
				{
					flag = true;
				}
				else if (num24 < num27)
				{
					flag = true;
				}
				else if (num25 < num27)
				{
					flag = true;
				}
				if (!flag || (vegetables6 != null && vegetables6.Length != 0))
				{
					bool flag2 = true;
					if (PlanetAlgorithm10.diff(num17, num18) > 0.2f)
					{
						flag2 = false;
					}
					if (PlanetAlgorithm10.diff(num17, num19) > 0.2f)
					{
						flag2 = false;
					}
					if (PlanetAlgorithm10.diff(num17, num20) > 0.2f)
					{
						flag2 = false;
					}
					if (PlanetAlgorithm10.diff(num17, num21) > 0.2f)
					{
						flag2 = false;
					}
					double num28 = dotNet35Random2.NextDouble();
					num28 *= num28;
					double num29 = dotNet35Random2.NextDouble();
					float d = (float)dotNet35Random2.NextDouble() - 0.5f;
					float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
					float num30 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
					float angle = (float)dotNet35Random2.NextDouble() * 360f;
					float num31 = (float)dotNet35Random2.NextDouble();
					float num32 = (float)dotNet35Random2.NextDouble();
					float num33 = 1f;
					float num34 = 0.5f;
					float num35 = 1f;
					int[] array;
					if (!flag)
					{
						if (num26 < 0.8f)
						{
							array = vegetables;
							num33 = num;
							num34 = num2;
							num35 = num3;
						}
						else
						{
							array = vegetables2;
							num33 = num4;
							num34 = num5;
							num35 = num6;
						}
					}
					else
					{
						array = null;
					}
					double num36 = simplexNoise.Noise(num14 * 0.07, num15 * 0.07, num16 * 0.07) * (double)num33 + (double)num34 + 0.5;
					double num37 = simplexNoise2.Noise(num14 * 0.4, num15 * 0.4, num16 * 0.4) * (double)num7 + (double)num8 + 0.5;
					double num38 = num37 - 1.1;
					int[] array2;
					double num39;
					int num40;
					if (!flag)
					{
						if (num26 > 1f)
						{
							array2 = null;
							num39 = num37;
							num40 = ((vegetables6 == null || vegetables6.Length == 0) ? 4 : 2);
						}
						else if (num26 > 0.6f)
						{
							array2 = vegetables3;
							num39 = num37 - 0.55;
							num40 = 1;
						}
						else if (num26 > 0f)
						{
							array2 = vegetables4;
							num39 = num37;
							num40 = 2;
						}
						else
						{
							array2 = null;
							num39 = num37;
							num40 = 1;
						}
					}
					else
					{
						if (num17 >= num27 - 1f || num17 <= num27 - 2.2f)
						{
							goto IL_96C;
						}
						array2 = vegetables6;
						num39 = num38;
						num40 = 1;
					}
					if (flag2 && num29 < num36 && array != null && array.Length != 0)
					{
						vegeData.protoId = (short)array[(int)(num28 * (double)array.Length)];
						Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
						Vector3 a = rotation * Vector3.forward;
						Vector3 a2 = rotation * Vector3.right;
						Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
						Vector3 a3 = vector * num17;
						Vector3 normalized = (a2 * d + a * d2).normalized;
						float num41 = num30 * num35;
						Vector3 b = normalized * (num41 * num11);
						float num42 = num32 * (vector2.x + vector2.y) + (1f - vector2.x);
						float num43 = (num31 * (vector2.z + vector2.w) + (1f - vector2.z)) * num42;
						vegeData.pos = (a3 + b).normalized;
						num17 = data.QueryHeight(vegeData.pos);
						vegeData.pos *= num17;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
						vegeData.scl = new Vector3(num43, num42, num43);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num44 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num44;
					}
					if (num29 < num39 && array2 != null && array2.Length != 0)
					{
						vegeData.protoId = (short)array2[(int)(num28 * (double)array2.Length)];
						Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
						Vector3 a4 = rotation2 * Vector3.forward;
						Vector3 a5 = rotation2 * Vector3.right;
						Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
						for (int j = 0; j < num40; j++)
						{
							float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
							float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
							float num45 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
							float angle2 = (float)dotNet35Random2.NextDouble() * 360f;
							float num46 = (float)dotNet35Random2.NextDouble();
							float num47 = (float)dotNet35Random2.NextDouble();
							Vector3 a6 = vector * num17;
							Vector3 normalized2 = (a5 * d3 + a4 * d4).normalized;
							float num48 = num45 * num9;
							Vector3 b2 = normalized2 * (num48 * num11);
							float num49 = num47 * (vector3.x + vector3.y) + (1f - vector3.x);
							float num50 = (num46 * (vector3.z + vector3.w) + (1f - vector3.z)) * num49;
							vegeData.pos = (a6 + b2).normalized;
							num17 = (flag ? num27 : data.QueryHeight(vegeData.pos));
							vegeData.pos *= num17;
							vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle2, Vector3.up);
							vegeData.scl = new Vector3(num50, num49, num50);
							vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
							vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
							int num51 = data.AddVegeData(vegeData);
							data.vegeIds[i] = (ushort)num51;
						}
					}
				}
			}
			IL_96C:;
		}
	}

	// Token: 0x04004BDD RID: 19421
	private const int kCircleCount = 10;

	// Token: 0x04004BDE RID: 19422
	private Vector4[] ellipses = new Vector4[10];

	// Token: 0x04004BDF RID: 19423
	private double[] eccentricities = new double[10];

	// Token: 0x04004BE0 RID: 19424
	private double[] heights = new double[10];
}
