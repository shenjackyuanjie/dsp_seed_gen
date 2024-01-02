using System;
using UnityEngine;

// Token: 0x02000576 RID: 1398
public class PlanetAlgorithm3 : PlanetAlgorithm
{
	// Token: 0x06003AB3 RID: 15027 RVA: 0x00316B10 File Offset: 0x00314D10
	public override void GenerateTerrain(double modX, double modY)
	{
		double num = 0.007;
		double num2 = 0.007;
		double num3 = 0.007;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		int seed = dotNet35Random.Next();
		int seed2 = dotNet35Random.Next();
		SimplexNoise simplexNoise = new SimplexNoise(seed);
		SimplexNoise simplexNoise2 = new SimplexNoise(seed2);
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num4 = (double)(data.vertices[i].x * this.planet.radius);
			double num5 = (double)(data.vertices[i].y * this.planet.radius);
			double num6 = (double)(data.vertices[i].z * this.planet.radius);
			num4 += Math.Sin(num5 * 0.15) * 3.0;
			num5 += Math.Sin(num6 * 0.15) * 3.0;
			num6 += Math.Sin(num4 * 0.15) * 3.0;
			double num7 = simplexNoise.Noise3DFBM(num4 * num * 1.0, num5 * num2 * 1.1, num6 * num3 * 1.0, 6, 0.5, 1.8);
			double num8 = simplexNoise2.Noise3DFBM(num4 * num * 1.3 + 0.5, num5 * num2 * 2.8 + 0.2, num6 * num3 * 1.3 + 0.7, 3, 0.5, 2.0) * 2.0;
			double num9 = simplexNoise2.Noise3DFBM(num4 * num * 6.0, num5 * num2 * 12.0, num6 * num3 * 6.0, 2, 0.5, 2.0) * 2.0;
			num9 = this.Lerp(num9, num9 * 0.1, modX);
			double num10 = simplexNoise2.Noise3DFBM(num4 * num * 0.8, num5 * num2 * 0.8, num6 * num3 * 0.8, 2, 0.5, 2.0) * 2.0;
			double num11 = num7 * 2.0 + 0.92;
			double num12 = num8 * (double)Mathf.Abs((float)num10 + 0.5f);
			num11 += (double)Mathf.Clamp01((float)(num12 - 0.35) * 1f);
			if (num11 < 0.0)
			{
				num11 *= 2.0;
			}
			double num13 = Maths.Levelize2(num11, 1.0, 0.0);
			if (num13 > 0.0)
			{
				num13 = Maths.Levelize2(num11, 1.0, 0.0);
				num13 = this.Lerp(Maths.Levelize4(num13, 1.0, 0.0), num13, modX);
			}
			double b = (num13 > 0.0) ? ((num13 > 1.0) ? ((num13 > 2.0) ? ((double)Mathf.Lerp(1.2f, 2f, (float)num13 - 2f) + num9 * 0.12) : ((double)Mathf.Lerp(0.3f, 1.2f, (float)num13 - 1f) + num9 * 0.12)) : ((double)Mathf.Lerp(0f, 0.3f, (float)num13) + num9 * 0.1)) : ((double)Mathf.Lerp(-1f, 0f, (float)num13 + 1f));
			double a = (num13 > 0.0) ? ((num13 > 1.0) ? ((num13 > 2.0) ? ((double)Mathf.Lerp(1.4f, 2.7f, (float)num13 - 2f) + num9 * 0.12) : ((double)Mathf.Lerp(0.3f, 1.4f, (float)num13 - 1f) + num9 * 0.12)) : ((double)Mathf.Lerp(0f, 0.3f, (float)num13) + num9 * 0.1)) : ((double)Mathf.Lerp(-4f, 0f, (float)num13 + 1f));
			double num14 = this.Lerp(a, b, modX);
			if (num11 < 0.0)
			{
				num11 *= 2.0;
			}
			if (num11 < 1.0)
			{
				num11 = Maths.Levelize(num11, 1.0, 0.0);
			}
			double num15 = num14;
			double num16 = (double)Mathf.Abs((float)num11);
			num16 = ((num16 > 0.0) ? ((num16 > 2.0) ? 2.0 : num16) : 0.0);
			num16 += ((num16 > 1.8) ? (-num9 * 0.8) : (num9 * 0.2));
			data.heightData[i] = (ushort)(((double)this.planet.radius + num15 + 0.2) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num16 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x00317121 File Offset: 0x00315321
	private double Lerp(double a, double b, double t)
	{
		return a + (b - a) * t;
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x0031712C File Offset: 0x0031532C
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
		float num = 1f;
		float x = themeProto.ModY.x;
		float num2 = 2.5f;
		float num3 = 1f;
		float x2 = themeProto.ModY.x;
		float num4 = 1f;
		float num5 = 0.7f;
		float y = themeProto.ModY.y;
		float num6 = 1.4f;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
		SimplexNoise simplexNoise = new SimplexNoise(dotNet35Random2.Next());
		SimplexNoise simplexNoise2 = new SimplexNoise(dotNet35Random2.Next());
		PlanetRawData data = this.planet.data;
		int stride = data.stride;
		int num7 = stride / 2;
		float num8 = this.planet.radius * 3.14159f * 2f / ((float)data.precision * 4f);
		VegeData vegeData = default(VegeData);
		VegeProto[] vegeProtos = PlanetModelingManager.vegeProtos;
		Vector4[] vegeScaleRanges = PlanetModelingManager.vegeScaleRanges;
		short[] vegeHps = PlanetModelingManager.vegeHps;
		for (int i = 0; i < data.dataLength; i++)
		{
			int num9 = i % stride;
			int num10 = i / stride;
			if (num9 > num7)
			{
				num9--;
			}
			if (num10 > num7)
			{
				num10--;
			}
			if (num9 % 2 == 1 && num10 % 2 == 1)
			{
				Vector3 vector = data.vertices[i];
				double num11 = (double)(data.vertices[i].x * this.planet.radius);
				double num12 = (double)(data.vertices[i].y * this.planet.radius);
				double num13 = (double)(data.vertices[i].z * this.planet.radius);
				float d = (float)data.heightData[i] * 0.01f;
				float num14 = (float)data.biomoData[i] * 0.01f;
				double num15 = dotNet35Random2.NextDouble();
				num15 *= num15;
				double num16 = dotNet35Random2.NextDouble() * 1.5;
				double num17 = dotNet35Random2.NextDouble() * 1.5;
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num18 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float angle = (float)dotNet35Random2.NextDouble() * 360f;
				float num19 = (float)dotNet35Random2.NextDouble();
				float num20 = (float)dotNet35Random2.NextDouble();
				int[] array;
				float num21;
				float num22;
				float num23;
				if (num14 < 0.8f)
				{
					array = vegetables;
					num21 = num;
					num22 = x;
					num23 = num2;
				}
				else
				{
					array = vegetables2;
					num21 = num3;
					num22 = x2;
					num23 = num4;
				}
				double num24 = simplexNoise.Noise3DFBM(num11 * 0.016, num12 * 0.016, num13 * 0.016, 2, 0.5, 2.0) * (double)num21 + (double)num22;
				double num25 = simplexNoise2.Noise(num11 * 0.016, num12 * 0.016, num13 * 0.016) * (double)num5 + (double)y;
				if (num24 < 0.0)
				{
					num24 = 0.0;
				}
				else if (num24 > 1.0)
				{
					num24 = 1.0;
				}
				if (num25 < 0.0)
				{
					num25 = 0.0;
				}
				else if (num25 > 1.0)
				{
					num25 = 1.0;
				}
				num24 *= num24;
				num25 *= num25;
				num24 = Maths.Levelize(num24, 1.0, 0.0);
				num25 = Maths.Levelize(num25, 1.0, 0.0);
				int[] array2;
				int num26;
				if (num14 < 0.8f)
				{
					array2 = vegetables3;
					num26 = 1;
				}
				else
				{
					array2 = vegetables4;
					num26 = 1;
				}
				if (num16 <= num24 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num15 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector3 a3 = vector * d;
					Vector3 normalized = (a2 * d2 + a * d3).normalized;
					float num27 = num18 * num23;
					Vector3 b = normalized * (num27 * num8);
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					float num28 = num20 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num29 = (num19 * (vector2.z + vector2.w) + (1f - vector2.z)) * num28;
					vegeData.pos = (a3 + b).normalized;
					d = data.QueryHeight(vegeData.pos);
					vegeData.pos *= d;
					vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
					vegeData.scl = new Vector3(num29, num28, num29);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num30 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num30;
				}
				if (num17 < num25 && array2 != null && array2.Length != 0)
				{
					vegeData.protoId = (short)array2[(int)(num15 * (double)array2.Length)];
					Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a4 = rotation2 * Vector3.forward;
					Vector3 a5 = rotation2 * Vector3.right;
					Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
					for (int j = 0; j < num26; j++)
					{
						float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float d5 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float num31 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
						float angle2 = (float)dotNet35Random2.NextDouble() * 360f;
						float num32 = (float)dotNet35Random2.NextDouble();
						float num33 = (float)dotNet35Random2.NextDouble();
						Vector3 a6 = vector * d;
						Vector3 normalized2 = (a5 * d4 + a4 * d5).normalized;
						float num34 = num31 * num6;
						Vector3 b2 = normalized2 * (num34 * num8);
						float num35 = num33 * (vector3.x + vector3.y) + (1f - vector3.x);
						float num36 = (num32 * (vector3.z + vector3.w) + (1f - vector3.z)) * num35;
						vegeData.pos = (a6 + b2).normalized;
						d = data.QueryHeight(vegeData.pos);
						vegeData.pos *= d;
						vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle2, Vector3.up);
						vegeData.scl = new Vector3(num36, num35, num36);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num37 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num37;
					}
				}
			}
		}
	}
}
