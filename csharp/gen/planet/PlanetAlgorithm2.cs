using System;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class PlanetAlgorithm2 : PlanetAlgorithm
{
	// Token: 0x06003AB0 RID: 15024 RVA: 0x00315FB0 File Offset: 0x003141B0
	public override void GenerateTerrain(double modX, double modY)
	{
		modX = (3.0 - modX - modX) * modX * modX;
		double num = 0.0035;
		double num2 = 0.025 * modX + 0.0035 * (1.0 - modX);
		double num3 = 0.0035;
		double num4 = 3.0;
		double num5 = 1.0 + 1.3 * modY;
		num *= num5;
		num2 *= num5;
		num3 *= num5;
		DotNet35Random dotNet35Random = new DotNet35Random(this.planet.seed);
		int seed = dotNet35Random.Next();
		int seed2 = dotNet35Random.Next();
		SimplexNoise simplexNoise = new SimplexNoise(seed);
		SimplexNoise simplexNoise2 = new SimplexNoise(seed2);
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			double num6 = (double)(data.vertices[i].x * this.planet.radius);
			double num7 = (double)(data.vertices[i].y * this.planet.radius);
			double num8 = (double)(data.vertices[i].z * this.planet.radius);
			double num9 = (double)data.vertices[i].y;
			double num10 = simplexNoise.Noise3DFBM(num6 * num, num7 * num2, num8 * num3, 6, 0.45, 1.8);
			double num11 = simplexNoise2.Noise3DFBM(num6 * num * 2.0, num7 * num2 * 2.0, num8 * num3 * 2.0, 3, 0.5, 2.0);
			double value = num10 * num4 + num4 * 0.4;
			double num12 = 0.6 / (Math.Abs(value) + 0.6) - 0.25;
			double num13 = (num12 < 0.0) ? (num12 * 0.3) : num12;
			double num14 = Math.Pow(Math.Abs(num9 * 1.01), 3.0) * 1.0;
			double num15 = (num11 < 0.0) ? 0.0 : num11;
			double num16 = (num14 > 1.0) ? 1.0 : num14;
			double num18;
			double num17 = (num18 = num13) * 1.5 + num15 * 1.0 + num16;
			data.heightData[i] = (ushort)(((double)this.planet.radius + num18 + 0.1) * 100.0);
			data.biomoData[i] = (byte)Mathf.Clamp((float)(num17 * 100.0), 0f, 200f);
		}
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x003162B8 File Offset: 0x003144B8
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
		float num = 0.25f;
		float num2 = -0.48f;
		float num3 = 2.5f;
		float num4 = 0.25f;
		float num5 = -0.48f;
		float num6 = 1f;
		float num7 = 1.5f;
		float num8 = -0.7f;
		float num9 = 3f;
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
				float d = (float)data.heightData[i] * 0.01f;
				double num17 = (double)((float)data.biomoData[i] * 0.01f);
				double num18 = dotNet35Random2.NextDouble();
				num18 *= num18;
				double num19 = dotNet35Random2.NextDouble();
				float d2 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float d3 = (float)dotNet35Random2.NextDouble() - 0.5f;
				float num20 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
				float num21 = (float)dotNet35Random2.NextDouble() * 360f;
				float num22 = (float)dotNet35Random2.NextDouble();
				float num23 = (float)dotNet35Random2.NextDouble();
				int[] array;
				float num24;
				float num25;
				float num26;
				if (num17 < 0.800000011920929 - this.planet.mod_y * 0.30000001192092896)
				{
					array = vegetables;
					num24 = num;
					num25 = num2;
					num26 = num3;
				}
				else
				{
					array = vegetables2;
					num24 = num4;
					num25 = num5;
					num26 = num6;
				}
				double num27 = simplexNoise.Noise3DFBM(num14 * 0.02, num15 * 0.02, num16 * 0.02, 2, 0.5, 2.0) * (double)num24 + (double)num25 + 0.5 + this.planet.mod_y * 0.019999999552965164;
				double num28 = simplexNoise2.Noise(num14 * 0.06, num15 * 0.06, num16 * 0.06) * (double)num7 + (double)num8 + 0.13 + this.planet.mod_x * 0.37;
				double num29 = (double)Math.Abs(data.vertices[i].y);
				num29 = 1.0 - Math.Pow(num29, 3.0) * this.planet.mod_x;
				num27 *= num29;
				int[] array2 = vegetables3;
				double num30 = num28;
				int num31 = 1;
				if (num19 < num27 && array != null && array.Length != 0)
				{
					vegeData.protoId = (short)array[(int)(num18 * (double)array.Length)];
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a = rotation * Vector3.forward;
					Vector3 a2 = rotation * Vector3.right;
					Vector3 a3 = vector * d;
					Vector3 normalized = (a2 * d2 + a * d3).normalized;
					float num32 = num20 * num26;
					Vector3 b = normalized * (num32 * num11);
					Vector4 vector2 = vegeScaleRanges[(int)vegeData.protoId];
					float num33 = num23 * (vector2.x + vector2.y) + (1f - vector2.x);
					float num34 = (num22 * (vector2.z + vector2.w) + (1f - vector2.z)) * num33;
					float num35 = 1f;
					if (vegeProtos[(int)vegeData.protoId].Type != EVegeType.Detail)
					{
						num35 = 1f - (float)this.planet.mod_x;
					}
					vegeData.pos = (a3 + b).normalized;
					d = data.QueryHeight(vegeData.pos);
					vegeData.pos *= d;
					vegeData.rot = Maths.SphericalRotation(vegeData.pos, num21 * num35);
					vegeData.scl = new Vector3(num34, num33, num34);
					vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
					vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
					int num36 = data.AddVegeData(vegeData);
					data.vegeIds[i] = (ushort)num36;
				}
				if (num19 < num30 && array2 != null && array2.Length != 0)
				{
					vegeData.protoId = (short)array2[(int)(num18 * (double)array2.Length)];
					if (vegeProtos[(int)vegeData.protoId].Type == EVegeType.VFX)
					{
						float num37 = Mathf.Max(0f, vector.x * vector.x + vector.z * vector.z - 0.1f);
						if (num19 > num30 * (double)num37)
						{
							goto IL_837;
						}
					}
					Quaternion rotation2 = Quaternion.FromToRotation(Vector3.up, vector);
					Vector3 a4 = rotation2 * Vector3.forward;
					Vector3 a5 = rotation2 * Vector3.right;
					Vector4 vector3 = vegeScaleRanges[(int)vegeData.protoId];
					for (int j = 0; j < num31; j++)
					{
						float d4 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float d5 = (float)dotNet35Random2.NextDouble() - 0.5f;
						float num38 = (float)Math.Sqrt(dotNet35Random2.NextDouble());
						float num39 = (float)dotNet35Random2.NextDouble() * 360f;
						float num40 = (float)dotNet35Random2.NextDouble();
						float num41 = (float)dotNet35Random2.NextDouble();
						Vector3 a6 = vector * d;
						Vector3 normalized2 = (a5 * d4 + a4 * d5).normalized;
						float num42 = num38 * num9;
						Vector3 b2 = normalized2 * (num42 * num11);
						float num43 = num41 * (vector3.x + vector3.y) + (1f - vector3.x);
						float num44 = (num40 * (vector3.z + vector3.w) + (1f - vector3.z)) * num43;
						float num45 = 1f;
						if (vegeProtos[(int)vegeData.protoId].Type != EVegeType.Detail)
						{
							num45 = 1f - (float)this.planet.mod_x;
						}
						vegeData.pos = (a6 + b2).normalized;
						d = data.QueryHeight(vegeData.pos);
						vegeData.pos *= d;
						vegeData.rot = Maths.SphericalRotation(vegeData.pos, num39 * num45);
						vegeData.scl = new Vector3(num44, num43, num44);
						vegeData.modelIndex = (short)vegeProtos[(int)vegeData.protoId].ModelIndex;
						vegeData.hash.InitHashBits(vegeData.pos.x, vegeData.pos.y, vegeData.pos.z);
						int num46 = data.AddVegeData(vegeData);
						data.vegeIds[i] = (ushort)num46;
					}
				}
			}
			IL_837:;
		}
	}
}
