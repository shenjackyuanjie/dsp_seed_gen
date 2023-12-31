using System;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class PlanetAlgorithm6 : PlanetAlgorithm
{
	// Token: 0x06003ABE RID: 15038 RVA: 0x00319698 File Offset: 0x00317898
	public override void GenerateTerrain(double modX, double modY)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		int seed = dotNet35Random.Next();
		int seed2 = dotNet35Random.Next();
		SimplexNoise simplexNoise = new SimplexNoise(seed);
		SimplexNoise simplexNoise2 = new SimplexNoise(seed2);
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num = (double)(data.vertices[i].x * this.planet.radius);
			double num2 = (double)(data.vertices[i].y * this.planet.radius);
			double num3 = (double)(data.vertices[i].z * this.planet.radius);
			double num4 = 0.0;
			double num5 = Maths.Levelize(num * 0.007, 1.0, 0.0);
			double num6 = Maths.Levelize(num2 * 0.007, 1.0, 0.0);
			double num7 = Maths.Levelize(num3 * 0.007, 1.0, 0.0);
			num5 += simplexNoise.Noise(num * 0.05, num2 * 0.05, num3 * 0.05) * 0.04;
			num6 += simplexNoise.Noise(num2 * 0.05, num3 * 0.05, num * 0.05) * 0.04;
			num7 += simplexNoise.Noise(num3 * 0.05, num * 0.05, num2 * 0.05) * 0.04;
			double num8 = Math.Abs(simplexNoise2.Noise(num5, num6, num7));
			double num9 = (0.16 - num8) * 10.0;
			num9 = ((num9 > 0.0) ? ((num9 > 1.0) ? 1.0 : num9) : 0.0);
			num9 *= num9;
			double num10 = (simplexNoise.Noise3DFBM(num2 * 0.005, num3 * 0.005, num * 0.005, 4, 0.5, 2.0) + 0.22) * 5.0;
			num10 = ((num10 > 0.0) ? ((num10 > 1.0) ? 1.0 : num10) : 0.0);
			double num11 = Math.Abs(simplexNoise2.Noise3DFBM(num5 * 1.5, num6 * 1.5, num7 * 1.5, 2, 0.5, 2.0));
			num4 -= num9 * 1.2 * num10;
			if (num4 >= 0.0)
			{
				num4 += num8 * 0.25 + num11 * 0.6;
			}
			num4 -= 0.1;
			double num12 = -0.3 - num4;
			if (num12 > 0.0)
			{
				num12 = ((num12 > 1.0) ? 1.0 : num12);
				num12 = (3.0 - num12 - num12) * num12 * num12;
				num4 = -0.3 - num12 * 3.700000047683716;
			}
			double num13 = (num9 > 0.30000001192092896) ? num9 : 0.30000001192092896;
			num13 = Maths.Levelize(num13, 0.7, 0.0);
			num4 = ((num4 > -0.800000011920929) ? num4 : ((-num13 - num8) * 0.8999999761581421));
			num4 = ((num4 > -1.2000000476837158) ? num4 : -1.2000000476837158);
			double num14 = num4 * num9;
			num14 += num8 * 2.1 + 0.800000011920929;
			if (num14 > 1.7000000476837158 && num14 < 2.0)
			{
				num14 = 2.0;
			}
			data.heightData[i] = (ushort)(((double)this.planet.radius + num4 + 0.2) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num14 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x00319B7C File Offset: 0x00317D7C
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
				float num27 = this.planet.radius + 0.2f;
				if (num17 >= num27 && num18 >= num27 && num19 >= num27 && num20 >= num27 && num21 >= num27 && num22 >= num27 && num23 >= num27 && num24 >= num27 && num25 >= num27)
				{
					bool flag = true;
					if (PlanetAlgorithm6.diff(num17, num18) > 0.2f)
					{
						flag = false;
					}
					if (PlanetAlgorithm6.diff(num17, num19) > 0.2f)
					{
						flag = false;
					}
					if (PlanetAlgorithm6.diff(num17, num20) > 0.2f)
					{
						flag = false;
					}
					if (PlanetAlgorithm6.diff(num17, num21) > 0.2f)
					{
						flag = false;
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
					int[] array;
					float num33;
					float num34;
					float num35;
					if (num26 < 0.8f)
					{
						array = vegetables;
						num33 = num;
						num34 = num2;
						num35 = num3;
					}
					else if (num26 < 2f)
					{
						array = vegetables2;
						num33 = num4;
						num34 = num5;
						num35 = num6;
					}
					else
					{
						array = vegetables6;
						num33 = num4;
						num34 = num5;
						num35 = num6;
					}
					double num36 = simplexNoise.Noise(num14 * 0.07, num15 * 0.07, num16 * 0.07) * (double)num33 + (double)num34 + 0.5;
					double num37 = simplexNoise2.Noise(num14 * 0.4, num15 * 0.4, num16 * 0.4) * (double)num7 + (double)num8 + 0.5;
					double num38 = num37 - 0.55;
					int[] array2;
					double num39;
					int num40;
					if (num26 > 1f)
					{
						array2 = vegetables3;
						num39 = num37;
						num40 = 4;
					}
					else if (num26 > 0.5f)
					{
						array2 = vegetables4;
						num39 = num38;
						num40 = 1;
					}
					else if (num26 > 0f)
					{
						array2 = vegetables5;
						num39 = num38;
						num40 = 1;
					}
					else
					{
						array2 = null;
						num39 = num37;
						num40 = 1;
					}
					if (flag && num29 < num36 && array != null && array.Length != 0)
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
							num17 = data.QueryHeight(vegeData.pos);
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
		}
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x0030EFCF File Offset: 0x0030D1CF
	private static float diff(float a, float b)
	{
		if (a <= b)
		{
			return b - a;
		}
		return a - b;
	}
}
