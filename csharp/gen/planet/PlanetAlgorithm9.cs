using System;
using UnityEngine;

// Token: 0x0200057C RID: 1404
public class PlanetAlgorithm9 : PlanetAlgorithm
{
	// Token: 0x06003ACA RID: 15050 RVA: 0x0031C4A8 File Offset: 0x0031A6A8
	public override void GenerateTerrain(double modX, double modY)
	{
		double num = 0.01;
		double num2 = 0.012;
		double num3 = 0.01;
		double num4 = 3.0;
		double num5 = -0.2;
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
			double num13 = simplexNoise.Noise3DFBM(num10 * num * 0.75, num11 * num2 * 0.5, num12 * num3 * 0.75, 6, 0.5, 2.0) * num4 + num5;
			double num14 = simplexNoise2.Noise3DFBM(num10 * 0.0025, num11 * 0.0025, num12 * 0.0025, 3, 0.5, 2.0) * num4 * num6 + num7;
			double num15 = (num14 > 0.0) ? (num14 * 0.5) : num14;
			double num16 = num13 + num15;
			double num17 = (num16 > 0.0) ? (num16 * 0.5) : (num16 * 1.6);
			double num18 = (num17 > 0.0) ? Maths.Levelize3(num17, 0.7, 0.0) : Maths.Levelize2(num17, 0.5, 0.0);
			num18 += 0.618;
			num18 = ((num18 > -1.0) ? (num18 * 1.5) : (num18 * 4.0));
			double num19 = simplexNoise2.Noise3DFBM(num10 * num * 2.5, num11 * num2 * 8.0, num12 * num3 * 2.5, 2, 0.5, 2.0) * 0.6 - 0.3;
			double num20 = num17 * num8 + num19 + num9;
			double val = Maths.Levelize(num17 + 0.7, 1.0, 0.0);
			double num21 = simplexNoise.Noise3DFBM(num10 * num * modX, num11 * num2 * modX, num12 * num3 * modX, 6, 0.5, 2.0) * num4 + num5;
			double num22 = simplexNoise2.Noise3DFBM(num10 * 0.0025, num11 * 0.0025, num12 * 0.0025, 3, 0.5, 2.0) * num4 * num6 + num7;
			double num23 = (num22 > 0.0) ? (num22 * 0.5) : num22;
			double num24 = (num21 + num23 + 5.0) * 0.13;
			num24 = Math.Pow(num24, 6.0) * 24.0 - 24.0;
			double num25 = (num18 >= -modY) ? 0.0 : Math.Pow(Math.Min(Math.Abs(num18 + modY) / 5.0, 1.0), 1.0);
			double num26 = num18 * (1.0 - num25) + num24 * num25;
			num26 = ((num26 > 0.0) ? (num26 * 0.5) : num26);
			double num27 = simplexNoise2.Noise3DFBM(num10 * num * 1.5, num11 * num2 * 2.0, num12 * num3 * 1.5, 6, 0.5, 2.0) * num4 + num5;
			num27 = Math.Max(num27 + 1.0, -0.99);
			num27 = ((num27 > 0.0) ? (num27 * 0.25) : num27);
			double num28 = Math.Max(val, 0.0);
			double num29 = (double)Mathf.Clamp01((float)(num28 - 1.0));
			num28 = ((num28 > 1.0) ? (num29 * num27 * 1.15 + 1.0) : num28);
			num28 = Math.Min(num28, 2.0);
			data.heightData[i] = (ushort)(((double)this.planet.radius + num26 + 0.2) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num28 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x0031CA64 File Offset: 0x0031AC64
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
				float num18 = (float)data.heightData[i + 1 + stride] * 0.01f;
				float num19 = (float)data.heightData[i - 1 + stride] * 0.01f;
				float num20 = (float)data.heightData[i + 1 - stride] * 0.01f;
				float num21 = (float)data.heightData[i - 1 - stride] * 0.01f;
				float num22 = (float)data.heightData[i + 1] * 0.01f;
				float num23 = (float)data.heightData[i - 1] * 0.01f;
				float num24 = (float)data.heightData[i + stride] * 0.01f;
				float num25 = (float)data.heightData[i - stride] * 0.01f;
				float num26 = (float)data.biomoData[i] * 0.01f;
				float num27 = this.planet.radius + 0.15f;
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
					if (PlanetAlgorithm9.diff(num17, num18) > 0.2f)
					{
						flag2 = false;
					}
					if (PlanetAlgorithm9.diff(num17, num19) > 0.2f)
					{
						flag2 = false;
					}
					if (PlanetAlgorithm9.diff(num17, num20) > 0.2f)
					{
						flag2 = false;
					}
					if (PlanetAlgorithm9.diff(num17, num21) > 0.2f)
					{
						flag2 = false;
					}
					double num28 = dotNet35Random2.NextDouble();
					num28 *= num28;
					double num29 = dotNet35Random2.NextDouble() + 2.0;
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
					double num38 = num37 - 0.55;
					double num39 = num37 - 1.1;
					int[] array2;
					double num40;
					int num41;
					if (!flag)
					{
						if (num26 > 1f)
						{
							array2 = vegetables3;
							num40 = num37;
							num41 = ((vegetables6 == null || vegetables6.Length == 0) ? 4 : 2);
						}
						else if (num26 > 0.5f)
						{
							array2 = vegetables4;
							num40 = num38;
							num41 = 1;
						}
						else if (num26 > 0f)
						{
							array2 = vegetables5;
							num40 = num38;
							num41 = 1;
						}
						else
						{
							array2 = null;
							num40 = num37;
							num41 = 1;
						}
					}
					else
					{
						if (num17 >= num27 - 1f || num17 <= num27 - 2.2f)
						{
							goto IL_97C;
						}
						array2 = vegetables6;
						num40 = num39;
						num41 = 1;
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
						float num42 = num30 * num35;
						Vector3 b = normalized * (num42 * num11);
						float num43 = num32 * (vector2.x + vector2.y) + (1f - vector2.x);
						float num44 = (num31 * (vector2.z + vector2.w) + (1f - vector2.z)) * num43;
						vegeData.pos = (a3 + b).normalized;
						num17 = data.QueryHeight(vegeData.pos);
						vegeData.pos *= num17;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
						vegeData.scl = new Vector3(num44, num43, num44);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num45 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num45;
					}
					if (num29 < num40 && array2 != null && array2.Length != 0)
					{
						vegeData.protoId = (short)array2[(int)(num28 * (double)array2.Length)];
						Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
						Vector3 a4 = rotation2 * Vector3.forward;
						Vector3 a5 = rotation2 * Vector3.right;
						Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
						for (int j = 0; j < num41; j++)
						{
							float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
							float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
							float num46 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
							float angle2 = (float)dotNet35Random2.NextDouble() * 360f;
							float num47 = (float)dotNet35Random2.NextDouble();
							float num48 = (float)dotNet35Random2.NextDouble();
							Vector3 a6 = vector * num17;
							Vector3 normalized2 = (a5 * d3 + a4 * d4).normalized;
							float num49 = num46 * num9;
							Vector3 b2 = normalized2 * (num49 * num11);
							float num50 = num48 * (vector3.x + vector3.y) + (1f - vector3.x);
							float num51 = (num47 * (vector3.z + vector3.w) + (1f - vector3.z)) * num50;
							vegeData.pos = (a6 + b2).normalized;
							num17 = (flag ? num27 : data.QueryHeight(vegeData.pos));
							vegeData.pos *= num17;
							vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle2, Vector3.up);
							vegeData.scl = new Vector3(num51, num50, num51);
							vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
							vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
							int num52 = data.AddVegeData(vegeData);
							data.vegeIds[i] = (ushort)num52;
						}
					}
				}
			}
			IL_97C:;
		}
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x0030EFCF File Offset: 0x0030D1CF
	private static float diff(float a, float b)
	{
		if (a <= b)
		{
			return b - a;
		}
		return a - b;
	}
}
