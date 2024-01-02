using System;
using UnityEngine;

// Token: 0x0200057B RID: 1403
public class PlanetAlgorithm8 : PlanetAlgorithm
{
	// Token: 0x06003AC7 RID: 15047 RVA: 0x0031BD48 File Offset: 0x00319F48
	public override void GenerateTerrain(double modX, double modY)
	{
		double num = 0.002 * modX;
		double num2 = 0.002 * modX * modX * 6.66667;
		double num3 = 0.002 * modX;
		SimplexNoise simplexNoise = new SimplexNoise(new DotNet35Random(this.planet.seed).Next());
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num4 = (double)(data.vertices[i].x * this.planet.radius);
			double num5 = (double)(data.vertices[i].y * this.planet.radius);
			double num6 = (double)(data.vertices[i].z * this.planet.radius);
			float num7 = Mathf.Clamp((float)simplexNoise.Noise3DFBM(num4 * num, num5 * num2, num6 * num3, 6, 0.45, 1.8) + 1f + (float)modY * 0.01f, 0f, 2f);
			float num9;
			if ((double)num7 < 1.0)
			{
				float num8 = Mathf.Cos(num7 * 3.1415927f) * 1.1f;
				num8 = Mathf.Sign(num8) * Mathf.Pow(num8, 4f);
				num8 = Mathf.Clamp(num8, -1f, 1f);
				num9 = 1f - (num8 + 1f) * 0.5f;
			}
			else
			{
				float num10 = Mathf.Cos((num7 - 1f) * 3.1415927f) * 1.1f;
				num10 = Mathf.Sign(num10) * Mathf.Pow(num10, 4f);
				num10 = Mathf.Clamp(num10, -1f, 1f);
				num9 = 2f - (num10 + 1f) * 0.5f;
			}
			double num11 = (double)num9;
			double num12 = (double)num9;
			num12 = ((num12 < 1.0) ? (Math.Max(num12 - 0.2, 0.0) * 1.25) : num12);
			num12 = ((num12 > 1.0) ? Math.Min(num12 * num12, 2.0) : num12);
			num12 = Maths.Levelize2(num12, 1.0, 0.0);
			data.heightData[i] = (ushort)(((double)this.planet.radius + num11 + 0.1) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num12 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x0031C028 File Offset: 0x0031A228
	public override void GenerateVegetables()
	{
		ThemeProto themeProto = LDB.themes.Select(this.planet.theme);
		if (themeProto == null)
		{
			return;
		}
		int[] vegetables = themeProto.Vegetables0;
		int[] vegetables2 = themeProto.Vegetables1;
		double num = 0.02;
		double num2 = 0.035;
		double num3 = 0.02;
		float num4 = 0.4f;
		float num5 = -0.4f;
		float num6 = 2.5f;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
		SimplexNoise simplexNoise = new SimplexNoise(dotNet35Random2.Next());
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
				byte b = data.biomoData[i];
				double num14 = dotNet35Random2.NextDouble();
				num14 *= num14;
				double num15 = dotNet35Random2.NextDouble();
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num16 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float angle = (float)dotNet35Random2.NextDouble() * 360f;
				float num17 = (float)dotNet35Random2.NextDouble();
				float num18 = (float)dotNet35Random2.NextDouble();
				int[] array = vegetables;
				float num19 = num4;
				float num20 = num5;
				float num21 = num6;
				double num22 = simplexNoise.Noise3DFBM(num11 * num, num12 * num2, num13 * num3, 2, 0.5, 2.0) * (double)num19 + (double)num20 + 0.5;
				if (num15 <= num22 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num14 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector3 a3 = vector * d;
					Vector3 normalized = (a2 * d2 + a * d3).normalized;
					float num23 = num16 * num21;
					Vector3 b2 = normalized * (num23 * num8);
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					float num24 = num18 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num25 = (num17 * (vector2.z + vector2.w) + (1f - vector2.z)) * num24;
					vegeData.pos = (a3 + b2).normalized;
					d = data.QueryHeight(vegeData.pos);
					vegeData.pos *= d;
					vegeData.rot = Quaternion.FromToRotation(Vector3.up, vegeData.pos.normalized) * Quaternion.AngleAxis(angle, Vector3.up);
					vegeData.scl = new Vector3(num25, num24, num25);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num26 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num26;
				}
			}
		}
	}
}
