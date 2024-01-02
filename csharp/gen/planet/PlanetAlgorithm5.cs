using System;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public class PlanetAlgorithm5 : PlanetAlgorithm
{
	// Token: 0x06003ABA RID: 15034 RVA: 0x003182C8 File Offset: 0x003164C8
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
			double num12 = simplexNoise.Noise3DFBM(num3 * 0.06, num2 * 0.06, num * 0.06, 2, 0.5, 2.0);
			num4 -= num9 * 1.2 * num10;
			if (num4 >= 0.0)
			{
				num4 += num8 * 0.25 + num11 * 0.6;
			}
			num4 -= 0.1;
			double num13 = num8 * 2.1;
			if (num13 < 0.0)
			{
				num13 *= 5.0;
			}
			num13 = ((num13 > -1.0) ? ((num13 > 2.0) ? 2.0 : num13) : -1.0);
			num13 += num12 * 0.6 * num13;
			double num14 = -0.3 - num4;
			if (num14 > 0.0)
			{
				double num15 = simplexNoise2.Noise(num * 0.16, num2 * 0.16, num3 * 0.16) - 1.0;
				num14 = ((num14 > 1.0) ? 1.0 : num14);
				num14 = (3.0 - num14 - num14) * num14 * num14;
				num4 = -0.3 - num14 * 3.700000047683716 + num14 * num14 * num14 * num14 * num15 * 0.5;
			}
			data.heightData[i] = (ushort)(((double)this.planet.radius + num4 + 0.2) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num13 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x003187F4 File Offset: 0x003169F4
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
		float num2 = -0.2f;
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
		ushort num12 = (ushort)((this.planet.radius + this.planet.waterHeight - 0.25f) * 100f);
		float d = this.planet.radius + this.planet.waterHeight + 0.5f;
		for (int i = 0; i < data.dataLength; i++)
		{
			int num13 = i % stride;
			int num14 = i / stride;
			if (num13 > num10)
			{
				num13--;
			}
			if (num14 > num10)
			{
				num14--;
			}
			if (num13 % 3 == 1 && num14 % 3 == 1 && num13 > 3 && num14 > 3 && (num13 < num10 - 4 || num13 > num10 + 4) && (num14 < num10 - 4 || num14 > num10 + 4) && num13 < stride - 4 && num14 < stride - 4)
			{
				Vector3 vector = data.vertices[i];
				double num15 = (double)(data.vertices[i].x * this.planet.radius);
				double num16 = (double)(data.vertices[i].y * this.planet.radius);
				double num17 = (double)(data.vertices[i].z * this.planet.radius);
				float num18 = (float)data.heightData[i] * 0.01f;
				float num19 = (float)data.heightData[i + 1 + stride] * 0.01f;
				float num20 = (float)data.heightData[i - 1 + stride] * 0.01f;
				float num21 = (float)data.heightData[i + 1 - stride] * 0.01f;
				float num22 = (float)data.heightData[i - 1 - stride] * 0.01f;
				float num23 = (float)data.heightData[i + 1] * 0.01f;
				float num24 = (float)data.heightData[i - 1] * 0.01f;
				float num25 = (float)data.heightData[i + stride] * 0.01f;
				float num26 = (float)data.heightData[i - stride] * 0.01f;
				float num27 = (float)data.biomoData[i] * 0.01f;
				bool flag = false;
				if (num18 < this.planet.radius)
				{
					flag = true;
				}
				if (num19 < this.planet.radius)
				{
					flag = true;
				}
				if (num20 < this.planet.radius)
				{
					flag = true;
				}
				if (num21 < this.planet.radius)
				{
					flag = true;
				}
				if (num22 < this.planet.radius)
				{
					flag = true;
				}
				if (num23 < this.planet.radius)
				{
					flag = true;
				}
				if (num24 < this.planet.radius)
				{
					flag = true;
				}
				if (num25 < this.planet.radius)
				{
					flag = true;
				}
				if (num26 < this.planet.radius)
				{
					flag = true;
				}
				bool flag2 = false;
				ushort num28 = data.heightData[i];
				ushort num29 = data.heightData[i + 2 + stride * 2];
				ushort num30 = data.heightData[i - 2 + stride * 2];
				ushort num31 = data.heightData[i + 2 - stride * 2];
				ushort num32 = data.heightData[i - 2 - stride * 2];
				ushort num33 = data.heightData[i + 2];
				ushort num34 = data.heightData[i - 2];
				ushort num35 = data.heightData[i + stride * 2];
				ushort num36 = data.heightData[i - stride * 2];
				if (num28 < num12 && num29 < num12 && num30 < num12 && num31 < num12 && num32 < num12 && num33 < num12 && num34 < num12 && num35 < num12 && num36 < num12)
				{
					flag2 = true;
				}
				bool flag3 = true;
				if (PlanetAlgorithm5.diff((float)num28, (float)num29) > 40f)
				{
					flag3 = false;
				}
				if (PlanetAlgorithm5.diff((float)num28, (float)num30) > 40f)
				{
					flag3 = false;
				}
				if (PlanetAlgorithm5.diff((float)num28, (float)num31) > 40f)
				{
					flag3 = false;
				}
				if (PlanetAlgorithm5.diff((float)num28, (float)num32) > 40f)
				{
					flag3 = false;
				}
				double num37 = dotNet35Random2.NextDouble();
				num37 *= num37;
				double num38 = dotNet35Random2.NextDouble();
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num39 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float angle = (float)dotNet35Random2.NextDouble() * 360f;
				float num40 = (float)dotNet35Random2.NextDouble();
				float num41 = (float)dotNet35Random2.NextDouble();
				int[] array;
				float num42;
				float num43;
				float num44;
				if (num27 < 0.8f)
				{
					array = (flag ? vegetables5 : vegetables);
					num42 = num;
					num43 = num2;
					num44 = (flag ? num3 : (num3 * 1.5f));
				}
				else
				{
					array = vegetables2;
					num42 = num4;
					num43 = num5;
					num44 = num6;
				}
				double num45 = simplexNoise.Noise(num15 * 0.07, num16 * 0.07, num17 * 0.07) * (double)num42 + (double)num43 + 0.5;
				double num46 = simplexNoise2.Noise(num15 * 0.4, num16 * 0.4, num17 * 0.4) * (double)num7 + (double)num8 + 0.5;
				double num47 = num46 - 0.2;
				int[] array2;
				double num48;
				int num49;
				if (num27 > 1f)
				{
					array2 = (flag ? vegetables6 : vegetables3);
					num48 = num46;
					num49 = 4;
				}
				else
				{
					array2 = vegetables4;
					num48 = num47;
					num49 = 1;
				}
				if (!flag && flag3 && num38 < num45 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num37 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					Vector3 a3 = vector * num18;
					Vector3 normalized = (a2 * d2 + a * d3).normalized;
					float num50 = num39 * num44;
					Vector3 b = normalized * (num50 * num11);
					float num51 = num41 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num52 = (num40 * (vector2.z + vector2.w) + (1f - vector2.z)) * num51;
					vegeData.pos = (a3 + b).normalized;
					num18 = data.QueryHeight(vegeData.pos);
					vegeData.pos *= num18;
					vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
					vegeData.scl = new Vector3(num52, num51, num52);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num53 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num53;
				}
				if (flag && flag2 && num38 < num45 + 0.55 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num37 * (double)array.Length)];
					Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a4 = rotation2 * Vector3.forward;
					Vector3 a5 = rotation2 * Vector3.right;
					Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
					Vector3 a6 = vector * num18;
					Vector3 normalized2 = (a5 * d2 + a4 * d3).normalized;
					float num54 = num39 * num44;
					Vector3 b2 = normalized2 * (num54 * num11);
					float num55 = num41 * (vector3.x + vector3.y) + (1f - vector3.x);
					float num56 = (num40 * (vector3.z + vector3.w) + (1f - vector3.z)) * num55;
					vegeData.pos = (a6 + b2).normalized;
					vegeData.pos *= d;
					vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
					vegeData.scl = new Vector3(num56, num55, num56);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num57 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num57;
				}
				if (!flag && num38 < num48 && array2 != null && array2.Length != 0)
				{
					vegeData.protoId = (short)array2[(int)(num37 * (double)array2.Length)];
					Quaternion rotation3 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a7 = rotation3 * Vector3.forward;
					Vector3 a8 = rotation3 * Vector3.right;
					Vector4 vector4 = vegeScaleRanges[(int)vegeData.protoId];
					for (int j = 0; j < num49; j++)
					{
						float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float d5 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float num58 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
						float angle2 = (float)dotNet35Random2.NextDouble() * 360f;
						float num59 = (float)dotNet35Random2.NextDouble();
						float num60 = (float)dotNet35Random2.NextDouble();
						Vector3 a9 = vector * num18;
						Vector3 normalized3 = (a8 * d4 + a7 * d5).normalized;
						float num61 = num58 * num9;
						Vector3 b3 = normalized3 * (num61 * num11);
						float num62 = num60 * (vector4.x + vector4.y) + (1f - vector4.x);
						float num63 = (num59 * (vector4.z + vector4.w) + (1f - vector4.z)) * num62;
						vegeData.pos = (a9 + b3).normalized;
						num18 = data.QueryHeight(vegeData.pos);
						vegeData.pos *= num18;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle2, Vector3.up);
						vegeData.scl = new Vector3(num63, num62, num63);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num64 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num64;
					}
				}
				if (flag && flag2 && num38 < num48 && array2 != null && array2.Length != 0)
				{
					vegeData.protoId = (short)array2[(int)(num37 * (double)array2.Length)];
					Quaternion rotation4 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a10 = rotation4 * Vector3.forward;
					Vector3 a11 = rotation4 * Vector3.right;
					Vector4 vector5 = vegeScaleRanges[(int)vegeData.protoId];
					for (int k = 0; k < num49; k++)
					{
						float d6 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float d7 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float num65 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
						float angle3 = (float)dotNet35Random2.NextDouble() * 360f;
						float num66 = (float)dotNet35Random2.NextDouble();
						float num67 = (float)dotNet35Random2.NextDouble();
						Vector3 a12 = vector * num18;
						Vector3 normalized4 = (a11 * d6 + a10 * d7).normalized;
						float num68 = num65 * num9;
						Vector3 b4 = normalized4 * (num68 * num11);
						float num69 = num67 * (vector5.x + vector5.y) + (1f - vector5.x);
						float num70 = (num66 * (vector5.z + vector5.w) + (1f - vector5.z)) * num69;
						vegeData.pos = (a12 + b4).normalized;
						vegeData.pos *= d;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle3, Vector3.up);
						vegeData.scl = new Vector3(num70, num69, num70);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num71 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num71;
					}
				}
			}
		}
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x0030EFCF File Offset: 0x0030D1CF
	private static float diff(float a, float b)
	{
		if (a <= b)
		{
			return b - a;
		}
		return a - b;
	}
}
