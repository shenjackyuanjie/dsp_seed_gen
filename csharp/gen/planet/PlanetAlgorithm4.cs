using System;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class PlanetAlgorithm4 : PlanetAlgorithm
{
	// Token: 0x06003AB7 RID: 15031 RVA: 0x00317950 File Offset: 0x00315B50
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
		int num4 = dotNet35Random.Next();
		for (int i = 0; i < 80; i++)
		{
			VectorLF3 vectorLF = RandomTable.SphericNormal(ref num4, 1.0);
			Vector4 vector = new Vector4((float)vectorLF.x, (float)vectorLF.y, (float)vectorLF.z);
			vector.Normalize();
			vector *= this.planet.radius;
			vector.w = (float)vectorLF.magnitude * 8f + 8f;
			vector.w *= vector.w;
			this.circles[i] = vector;
			this.heights[i] = dotNet35Random.NextDouble() * 0.4 + 0.20000000298023224;
		}
		PlanetRawData data = this.planet.data;
		for (int j = 0; j < data.dataLength; j++)
		{
			double num5 = (double)(data.vertices[j].x * this.planet.radius);
			double num6 = (double)(data.vertices[j].y * this.planet.radius);
			double num7 = (double)(data.vertices[j].z * this.planet.radius);
			double num8 = simplexNoise.Noise3DFBM(num5 * num, num6 * num2, num7 * num3, 4, 0.45, 1.8);
			double num9 = simplexNoise2.Noise3DFBM(num5 * num * 5.0, num6 * num2 * 5.0, num7 * num3 * 5.0, 4, 0.5, 2.0);
			double num10 = num8 * 1.5;
			double num11 = num9 * 0.2;
			double num12 = num10 * 0.08 + num11 * 2.0;
			double num13 = 0.0;
			for (int k = 0; k < 80; k++)
			{
				double num14 = (double)this.circles[k].x - num5;
				double num15 = (double)this.circles[k].y - num6;
				double num16 = (double)this.circles[k].z - num7;
				double num17 = num14 * num14 + num15 * num15 + num16 * num16;
				if (num17 <= (double)this.circles[k].w)
				{
					double num18 = num17 / (double)this.circles[k].w + num11 * 1.2;
					if (num18 < 0.0)
					{
						num18 = 0.0;
					}
					double num19 = num18 * num18;
					double num20 = num19 * num18;
					double num21 = -15.0 * num20 + 21.833333333334 * num19 - 7.533333333333 * num18 + 0.7 + num11;
					if (num21 < 0.0)
					{
						num21 = 0.0;
					}
					num21 *= num21;
					num21 *= this.heights[k];
					num13 = ((num13 > num21) ? num13 : num21);
				}
			}
			double num22 = num13 + num12 + 0.2;
			double num23 = num10 * 2.0 + 0.8;
			num23 = ((num23 > 2.0) ? 2.0 : ((num23 < 0.0) ? 0.0 : num23));
			num23 += ((num23 > 1.5) ? (-num13) : num13) * 0.5;
			num23 += num9 * 0.63;
			data.heightData[j] = (ushort)(((double)this.planet.radius + num22 + 0.1) * 100.0);
			data.biomoData[j] = (byte)Mathf.Clamp((float)(num23 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x00317DF0 File Offset: 0x00315FF0
	public override void GenerateVegetables()
	{
		ThemeProto themeProto = LDB.themes.Select(this.planet.theme);
		if (themeProto == null)
		{
			return;
		}
		int[] vegetables = themeProto.Vegetables0;
		int[] vegetables2 = themeProto.Vegetables1;
		double num = 0.005;
		double num2 = 0.02;
		double num3 = 0.005;
		float num4 = 0.18f;
		float num5 = -0.45f;
		float num6 = 2.5f;
		float num7 = 0.25f;
		float num8 = -0.45f;
		float num9 = 1f;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
		SimplexNoise simplexNoise = new SimplexNoise(dotNet35Random2.Next());
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
				float d = (float)data.heightData[i] * 0.01f;
				float num17 = (float)data.biomoData[i] * 0.01f;
				double num18 = dotNet35Random2.NextDouble();
				num18 *= num18;
				double num19 = dotNet35Random2.NextDouble();
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num20 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float angle = (float)dotNet35Random2.NextDouble() * 360f;
				float num21 = (float)dotNet35Random2.NextDouble();
				float num22 = (float)dotNet35Random2.NextDouble();
				int[] array;
				float num23;
				float num24;
				float num25;
				if (num17 < 0.8f)
				{
					array = vegetables;
					num23 = num4;
					num24 = num5;
					num25 = num6;
				}
				else
				{
					array = vegetables2;
					num23 = num7;
					num24 = num8;
					num25 = num9;
				}
				double num26 = simplexNoise.Noise3DFBM(num14 * num, num15 * num2, num16 * num3, 2, 0.5, 2.0) * (double)num23 + (double)num24 + 0.5;
				if (num19 <= num26 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num18 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector3 a3 = vector * d;
					Vector3 normalized = (a2 * d2 + a * d3).normalized;
					float num27 = num20 * num25;
					Vector3 b = normalized * (num27 * num11);
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					float num28 = num22 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num29 = (num21 * (vector2.z + vector2.w) + (1f - vector2.z)) * num28;
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
			}
		}
	}

	// Token: 0x04004BEF RID: 19439
	private const int kCircleCount = 80;

	// Token: 0x04004BF0 RID: 19440
	private Vector4[] circles = new Vector4[80];

	// Token: 0x04004BF1 RID: 19441
	private double[] heights = new double[80];
}
