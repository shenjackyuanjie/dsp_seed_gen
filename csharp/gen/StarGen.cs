using System;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public static class StarGen
{
	// Token: 0x06003AE6 RID: 15078 RVA: 0x0032139C File Offset: 0x0031F59C
	public static StarData CreateStar(GalaxyData galaxy, VectorLF3 pos, GameDesc gameDesc, int id, int seed, EStarType needtype, ESpectrType needSpectr = ESpectrType.X)
	{
		StarData starData = new StarData();
		starData.galaxy = galaxy;
		starData.index = id - 1;
		if (galaxy.starCount > 1)
		{
			starData.level = (float)starData.index / (float)(galaxy.starCount - 1);
		}
		else
		{
			starData.level = 0f;
		}
		starData.id = id;
		starData.seed = seed;
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int seed2 = dotNet35Random.Next();
		int seed3 = dotNet35Random.Next();
		starData.position = pos;
		float num = (float)pos.magnitude;
		float num2 = num / 32f;
		if (num2 > 1f)
		{
			num2 = Mathf.Log(num2) + 1f;
			num2 = Mathf.Log(num2) + 1f;
			num2 = Mathf.Log(num2) + 1f;
			num2 = Mathf.Log(num2) + 1f;
			num2 = Mathf.Log(num2) + 1f;
		}
		starData.resourceCoef = Mathf.Pow(7f, num2) * 0.6f;
		DotNet35Random dotNet35Random2 = new DotNet35Random(seed3);
		double num3 = dotNet35Random2.NextDouble();
		double num4 = dotNet35Random2.NextDouble();
		double num5 = dotNet35Random2.NextDouble();
		double rn = dotNet35Random2.NextDouble();
		double rt = dotNet35Random2.NextDouble();
		double num6 = (dotNet35Random2.NextDouble() - 0.5) * 0.2;
		double num7 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
		double num8 = dotNet35Random2.NextDouble() * 0.4 - 0.2;
		double num9 = Math.Pow(2.0, num8);
		DotNet35Random dotNet35Random3 = new DotNet35Random(dotNet35Random2.Next());
		double num10 = dotNet35Random3.NextDouble();
		float num11 = Mathf.Lerp(-0.98f, 0.88f, starData.level);
		if (num11 < 0f)
		{
			num11 -= 0.65f;
		}
		else
		{
			num11 += 0.65f;
		}
		float standardDeviation = 0.33f;
		if (needtype == EStarType.GiantStar)
		{
			num11 = ((num8 > -0.08) ? -1.5f : 1.6f);
			standardDeviation = 0.3f;
		}
		float num12 = StarGen.RandNormal(num11, standardDeviation, num3, num4);
		if (needSpectr == ESpectrType.M)
		{
			num12 = -3f;
		}
		else if (needSpectr == ESpectrType.O)
		{
			num12 = 3f;
		}
		if (num12 > 0f)
		{
			num12 *= 2f;
		}
		else
		{
			num12 *= 1f;
		}
		num12 = Mathf.Clamp(num12, -2.4f, 4.65f) + (float)num6 + 1f;
		if (needtype == EStarType.BlackHole)
		{
			starData.mass = 18f + (float)(num3 * num4) * 30f;
		}
		else if (needtype == EStarType.NeutronStar)
		{
			starData.mass = 7f + (float)num3 * 11f;
		}
		else if (needtype == EStarType.WhiteDwarf)
		{
			starData.mass = 1f + (float)num4 * 5f;
		}
		else
		{
			starData.mass = Mathf.Pow(2f, num12);
		}
		double d = 5.0;
		if (starData.mass < 2f)
		{
			d = 2.0 + 0.4 * (1.0 - (double)starData.mass);
		}
		starData.lifetime = (float)(10000.0 * Math.Pow(0.1, Math.Log10((double)starData.mass * 0.5) / Math.Log10(d) + 1.0) * num7);
		if (needtype == EStarType.GiantStar)
		{
			starData.lifetime = (float)(10000.0 * Math.Pow(0.1, Math.Log10((double)starData.mass * 0.58) / Math.Log10(d) + 1.0) * num7);
			starData.age = (float)num5 * 0.04f + 0.96f;
		}
		else if (needtype == EStarType.BlackHole || needtype == EStarType.NeutronStar || needtype == EStarType.WhiteDwarf)
		{
			starData.age = (float)num5 * 0.4f + 1f;
			if (needtype == EStarType.WhiteDwarf)
			{
				starData.lifetime += 10000f;
			}
			else if (needtype == EStarType.NeutronStar)
			{
				starData.lifetime += 1000f;
			}
		}
		else if ((double)starData.mass < 0.5)
		{
			starData.age = (float)num5 * 0.12f + 0.02f;
		}
		else if ((double)starData.mass < 0.8)
		{
			starData.age = (float)num5 * 0.4f + 0.1f;
		}
		else
		{
			starData.age = (float)num5 * 0.7f + 0.2f;
		}
		float num13 = starData.lifetime * starData.age;
		if (num13 > 5000f)
		{
			num13 = (Mathf.Log(num13 / 5000f) + 1f) * 5000f;
		}
		if (num13 > 8000f)
		{
			num13 = (Mathf.Log(Mathf.Log(Mathf.Log(num13 / 8000f) + 1f) + 1f) + 1f) * 8000f;
		}
		starData.lifetime = num13 / starData.age;
		float num14 = (1f - Mathf.Pow(Mathf.Clamp01(starData.age), 20f) * 0.5f) * starData.mass;
		starData.temperature = (float)(Math.Pow((double)num14, 0.56 + 0.14 / (Math.Log10((double)(num14 + 4f)) / Math.Log10(5.0))) * 4450.0 + 1300.0);
		double num15 = Math.Log10(((double)starData.temperature - 1300.0) / 4500.0) / Math.Log10(2.6) - 0.5;
		if (num15 < 0.0)
		{
			num15 *= 4.0;
		}
		if (num15 > 2.0)
		{
			num15 = 2.0;
		}
		else if (num15 < -4.0)
		{
			num15 = -4.0;
		}
		starData.spectr = (ESpectrType)Mathf.RoundToInt((float)num15 + 4f);
		starData.color = Mathf.Clamp01(((float)num15 + 3.5f) * 0.2f);
		starData.classFactor = (float)num15;
		starData.luminosity = Mathf.Pow(num14, 0.7f);
		starData.radius = (float)(Math.Pow((double)starData.mass, 0.4) * num9);
		starData.acdiskRadius = 0f;
		float p = (float)num15 + 2f;
		starData.habitableRadius = Mathf.Pow(1.7f, p) + 0.25f * Mathf.Min(1f, starData.orbitScaler);
		starData.lightBalanceRadius = Mathf.Pow(1.7f, p);
		starData.orbitScaler = Mathf.Pow(1.35f, p);
		if (starData.orbitScaler < 1f)
		{
			starData.orbitScaler = Mathf.Lerp(starData.orbitScaler, 1f, 0.6f);
		}
		StarGen.SetStarAge(starData, starData.age, rn, rt);
		starData.dysonRadius = starData.orbitScaler * 0.28f;
		if ((double)starData.dysonRadius * 40000.0 < (double)(starData.physicsRadius * 1.5f))
		{
			starData.dysonRadius = (float)((double)(starData.physicsRadius * 1.5f) / 40000.0);
		}
		starData.uPosition = starData.position * 2400000.0;
		starData.name = NameGen.RandomStarName(seed2, starData, galaxy);
		starData.overrideName = "";
		float num16 = Mathf.Pow(starData.color, 1.3f);
		float num17 = Mathf.Clamp((num - 2f) / 20f, 0f, 2.5f);
		if (num17 > 1f)
		{
			num17 = Mathf.Log(num17) + 1f;
			num17 = Mathf.Log(num17) + 1f;
		}
		num17 /= 1.4f;
		if (starData.type == EStarType.BlackHole)
		{
			num16 = 5f;
		}
		else if (starData.type == EStarType.NeutronStar)
		{
			num16 = 1.7f;
		}
		else if (starData.type == EStarType.WhiteDwarf)
		{
			num16 = 1.2f;
		}
		else if (starData.type == EStarType.GiantStar)
		{
			num16 = Mathf.Max(0.6f, num16);
		}
		else if (starData.spectr == ESpectrType.O)
		{
			num16 += 0.05f;
		}
		num16 *= 0.9f;
		num16 += 0.07f;
		float num18 = Mathf.Clamp01(1f - Mathf.Pow(num16, 0.73f) * Mathf.Pow(num17, 0.27f) + (float)num10 * 0.08f - 0.04f);
		if (num18 >= 0.7f)
		{
			starData.hivePatternLevel = 0;
		}
		else if (num18 >= 0.3f)
		{
			starData.hivePatternLevel = 1;
		}
		else
		{
			starData.hivePatternLevel = 2;
		}
		starData.safetyFactor = num18;
		int num19 = dotNet35Random3.Next(0, 1000);
		int num20 = starData.epicHive ? 2 : 1;
		starData.maxHiveCount = (int)(gameDesc.combatSettings.maxDensity * (float)num20 * 1000f + (float)num19 + 0.5f) / 1000;
		float initialColonize = gameDesc.combatSettings.initialColonize;
		if (initialColonize < 0.015f)
		{
			starData.initialHiveCount = 0;
		}
		else
		{
			float num21 = Mathf.Pow(Mathf.Clamp01(starData.safetyFactor - 0.2f), 0.86f);
			float num22 = Mathf.Clamp01(1f - num21 - (float)(starData.maxHiveCount - 1) * 0.05f) * (1.1f - (float)starData.maxHiveCount * 0.1f);
			if (initialColonize <= 1f)
			{
				num22 *= initialColonize;
			}
			else
			{
				num22 = Mathf.Lerp(num22, 1f + (initialColonize - 1f) * 0.2f, (initialColonize - 1f) * 0.5f);
			}
			if (starData.type == EStarType.GiantStar)
			{
				num22 *= 1.2f;
			}
			else if (starData.type == EStarType.WhiteDwarf)
			{
				num22 *= 1.4f;
			}
			else if (starData.type == EStarType.NeutronStar)
			{
				num22 *= 1.6f;
			}
			else if (starData.type == EStarType.BlackHole)
			{
				num22 *= 1.8f;
			}
			else if (starData.spectr == ESpectrType.O)
			{
				num22 *= 1.1f;
			}
			float num23 = num22 * (float)starData.maxHiveCount;
			if (num23 > (float)starData.maxHiveCount + 0.75f)
			{
				num23 = (float)starData.maxHiveCount + 0.75f;
			}
			float standardDeviation2 = 0.5f;
			if ((double)num23 <= 0.01)
			{
				standardDeviation2 = 0f;
			}
			else if (num23 < 1f)
			{
				standardDeviation2 = Mathf.Sqrt(num23) * 0.29f + 0.21f;
			}
			else if (num23 > 1f)
			{
				standardDeviation2 = 0.3f + 0.2f * num23;
			}
			int num24 = 64;
			do
			{
				double r = dotNet35Random3.NextDouble();
				double r2 = dotNet35Random3.NextDouble();
				starData.initialHiveCount = (int)((double)StarGen.RandNormal(num23, standardDeviation2, r, r2) + 0.5);
			}
			while (num24-- > 0 && (starData.initialHiveCount < 0 || starData.initialHiveCount > starData.maxHiveCount));
			if (starData.initialHiveCount < 0)
			{
				starData.initialHiveCount = 0;
			}
			else if (starData.initialHiveCount > starData.maxHiveCount)
			{
				starData.initialHiveCount = starData.maxHiveCount;
			}
		}
		if (starData.type == EStarType.BlackHole)
		{
			int num25 = (int)(gameDesc.combatSettings.maxDensity * 1000f + (float)num19 + 0.5f) / 1000;
			if (starData.initialHiveCount < num25)
			{
				starData.initialHiveCount = num25;
			}
			if (starData.initialHiveCount < 1)
			{
				starData.initialHiveCount = 1;
			}
		}
		return starData;
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x00321F10 File Offset: 0x00320110
	public static StarData CreateBirthStar(GalaxyData galaxy, GameDesc gameDesc, int seed)
	{
		StarData starData = new StarData();
		starData.galaxy = galaxy;
		starData.index = 0;
		starData.level = 0f;
		starData.id = 1;
		starData.seed = seed;
		starData.resourceCoef = 0.6f;
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int seed2 = dotNet35Random.Next();
		int seed3 = dotNet35Random.Next();
		starData.name = NameGen.RandomName(seed2);
		starData.overrideName = "";
		starData.position = VectorLF3.zero;
		DotNet35Random dotNet35Random2 = new DotNet35Random(seed3);
		double r = dotNet35Random2.NextDouble();
		double r2 = dotNet35Random2.NextDouble();
		double num = dotNet35Random2.NextDouble();
		double rn = dotNet35Random2.NextDouble();
		double rt = dotNet35Random2.NextDouble();
		double num2 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
		double y = dotNet35Random2.NextDouble() * 0.4 - 0.2;
		double num3 = Math.Pow(2.0, y);
		DotNet35Random dotNet35Random3 = new DotNet35Random(dotNet35Random2.Next());
		double num4 = dotNet35Random3.NextDouble();
		float num5 = StarGen.RandNormal(0f, 0.08f, r, r2);
		num5 = Mathf.Clamp(num5, -0.2f, 0.2f);
		starData.mass = Mathf.Pow(2f, num5);
		if (StarGen.specifyBirthStarMass > 0.1f)
		{
			starData.mass = StarGen.specifyBirthStarMass;
		}
		if (StarGen.specifyBirthStarAge > 1E-05f)
		{
			starData.age = StarGen.specifyBirthStarAge;
		}
		double d = 2.0 + 0.4 * (1.0 - (double)starData.mass);
		starData.lifetime = (float)(10000.0 * Math.Pow(0.1, Math.Log10((double)starData.mass * 0.5) / Math.Log10(d) + 1.0) * num2);
		starData.age = (float)(num * 0.4 + 0.3);
		if (StarGen.specifyBirthStarAge > 1E-05f)
		{
			starData.age = StarGen.specifyBirthStarAge;
		}
		float num6 = (1f - Mathf.Pow(Mathf.Clamp01(starData.age), 20f) * 0.5f) * starData.mass;
        // !!!!!
		starData.temperature = (float)(Math.Pow((double)num6, 0.56 + 0.14 / (Math.Log10((double)(num6 + 4f)) / Math.Log10(5.0))) * 4450.0 + 1300.0);
		double num7 = Math.Log10(((double)starData.temperature - 1300.0) / 4500.0) / Math.Log10(2.6) - 0.5;
		if (num7 < 0.0)
		{
			num7 *= 4.0;
		}
		if (num7 > 2.0)
		{
			num7 = 2.0;
		}
		else if (num7 < -4.0)
		{
			num7 = -4.0;
		}
		starData.spectr = (ESpectrType)Mathf.RoundToInt((float)num7 + 4f);
		starData.color = Mathf.Clamp01(((float)num7 + 3.5f) * 0.2f);
		starData.classFactor = (float)num7;
		starData.luminosity = Mathf.Pow(num6, 0.7f);
		starData.radius = (float)(Math.Pow((double)starData.mass, 0.4) * num3);
		starData.acdiskRadius = 0f;
		float p = (float)num7 + 2f;
		starData.habitableRadius = Mathf.Pow(1.7f, p) + 0.2f * Mathf.Min(1f, starData.orbitScaler);
		starData.lightBalanceRadius = Mathf.Pow(1.7f, p);
		starData.orbitScaler = Mathf.Pow(1.35f, p);
		if (starData.orbitScaler < 1f)
		{
			starData.orbitScaler = Mathf.Lerp(starData.orbitScaler, 1f, 0.6f);
		}
		StarGen.SetStarAge(starData, starData.age, rn, rt);
		starData.dysonRadius = starData.orbitScaler * 0.28f;
		if ((double)starData.dysonRadius * 40000.0 < (double)(starData.physicsRadius * 1.5f))
		{
			starData.dysonRadius = (float)((double)(starData.physicsRadius * 1.5f) / 40000.0);
		}
		starData.uPosition = VectorLF3.zero;
		starData.name = NameGen.RandomStarName(seed2, starData, galaxy);
		starData.overrideName = "";
		starData.hivePatternLevel = 0;
		starData.safetyFactor = 0.847f + (float)num4 * 0.026f;
		int num8 = dotNet35Random3.Next(0, 1000);
		starData.maxHiveCount = (int)(gameDesc.combatSettings.maxDensity * 1000f + (float)num8 + 0.5f) / 1000;
		float initialColonize = gameDesc.combatSettings.initialColonize;
		int num9 = (initialColonize * (float)starData.maxHiveCount < 0.7f) ? 0 : 1;
		if (initialColonize < 0.015f)
		{
			starData.initialHiveCount = 0;
		}
		else
		{
			float num10 = 0.6f * initialColonize * (float)starData.maxHiveCount;
			float standardDeviation = 0.5f;
			if (num10 < 1f)
			{
				standardDeviation = Mathf.Sqrt(num10) * 0.29f + 0.21f;
			}
			else if (num10 > (float)starData.maxHiveCount)
			{
				num10 = (float)starData.maxHiveCount;
			}
			int num11 = 16;
			do
			{
				double r3 = dotNet35Random3.NextDouble();
				double r4 = dotNet35Random3.NextDouble();
				starData.initialHiveCount = (int)((double)StarGen.RandNormal(num10, standardDeviation, r3, r4) + 0.5);
			}
			while (num11-- > 0 && (starData.initialHiveCount < 0 || starData.initialHiveCount > starData.maxHiveCount));
			if (starData.initialHiveCount < num9)
			{
				starData.initialHiveCount = num9;
			}
			else if (starData.initialHiveCount > starData.maxHiveCount)
			{
				starData.initialHiveCount = starData.maxHiveCount;
			}
		}
		return starData;
	}

	// Token: 0x06003AE8 RID: 15080 RVA: 0x00322504 File Offset: 0x00320704
	private static double _signpow(double x, double pow)
	{
		double num = (x > 0.0) ? 1.0 : -1.0;
		return Math.Abs(Math.Pow(x, pow)) * num;
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x00322540 File Offset: 0x00320740
	public static void CreateStarPlanets(GalaxyData galaxy, StarData star, GameDesc gameDesc)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(star.seed);
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
		double num = dotNet35Random2.NextDouble();
		double num2 = dotNet35Random2.NextDouble();
		double num3 = dotNet35Random2.NextDouble();
		double num4 = dotNet35Random2.NextDouble();
		double num5 = dotNet35Random2.NextDouble();
		double num6 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
		double num7 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
		DotNet35Random dotNet35Random3 = new DotNet35Random(dotNet35Random.Next());
		StarGen.SetHiveOrbitsConditionsTrue();
		if (star.type == EStarType.BlackHole)
		{
			star.planetCount = 1;
			star.planets = new PlanetData[star.planetCount];
			int info_seed = dotNet35Random2.Next();
			int gen_seed = dotNet35Random2.Next();
			star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 3, 1, false, info_seed, gen_seed);
		}
		else if (star.type == EStarType.NeutronStar)
		{
			star.planetCount = 1;
			star.planets = new PlanetData[star.planetCount];
			int info_seed2 = dotNet35Random2.Next();
			int gen_seed2 = dotNet35Random2.Next();
			star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 3, 1, false, info_seed2, gen_seed2);
		}
		else if (star.type == EStarType.WhiteDwarf)
		{
			if (num < 0.699999988079071)
			{
				star.planetCount = 1;
				star.planets = new PlanetData[star.planetCount];
				int info_seed3 = dotNet35Random2.Next();
				int gen_seed3 = dotNet35Random2.Next();
				star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 3, 1, false, info_seed3, gen_seed3);
			}
			else
			{
				star.planetCount = 2;
				star.planets = new PlanetData[star.planetCount];
				if (num2 < 0.30000001192092896)
				{
					int info_seed4 = dotNet35Random2.Next();
					int gen_seed4 = dotNet35Random2.Next();
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 3, 1, false, info_seed4, gen_seed4);
					info_seed4 = dotNet35Random2.Next();
					gen_seed4 = dotNet35Random2.Next();
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, 4, 2, false, info_seed4, gen_seed4);
				}
				else
				{
					int info_seed4 = dotNet35Random2.Next();
					int gen_seed4 = dotNet35Random2.Next();
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 4, 1, true, info_seed4, gen_seed4);
					info_seed4 = dotNet35Random2.Next();
					gen_seed4 = dotNet35Random2.Next();
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 1, false, info_seed4, gen_seed4);
				}
			}
		}
		else if (star.type == EStarType.GiantStar)
		{
			if (num < 0.30000001192092896)
			{
				star.planetCount = 1;
				star.planets = new PlanetData[star.planetCount];
				int info_seed5 = dotNet35Random2.Next();
				int gen_seed5 = dotNet35Random2.Next();
				int orbitIndex = (num3 > 0.5) ? 3 : 2;
				star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex, 1, false, info_seed5, gen_seed5);
			}
			else if (num < 0.800000011920929)
			{
				star.planetCount = 2;
				star.planets = new PlanetData[star.planetCount];
				if (num2 < 0.25)
				{
					int info_seed6 = dotNet35Random2.Next();
					int gen_seed6 = dotNet35Random2.Next();
					int orbitIndex2 = (num3 > 0.5) ? 3 : 2;
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex2, 1, false, info_seed6, gen_seed6);
					info_seed6 = dotNet35Random2.Next();
					gen_seed6 = dotNet35Random2.Next();
					orbitIndex2 = ((num3 > 0.5) ? 4 : 3);
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, orbitIndex2, 2, false, info_seed6, gen_seed6);
				}
				else
				{
					int info_seed6 = dotNet35Random2.Next();
					int gen_seed6 = dotNet35Random2.Next();
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 3, 1, true, info_seed6, gen_seed6);
					info_seed6 = dotNet35Random2.Next();
					gen_seed6 = dotNet35Random2.Next();
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 1, false, info_seed6, gen_seed6);
				}
			}
			else
			{
				star.planetCount = 3;
				star.planets = new PlanetData[star.planetCount];
				if (num2 < 0.15000000596046448)
				{
					int info_seed7 = dotNet35Random2.Next();
					int gen_seed7 = dotNet35Random2.Next();
					int orbitIndex3 = (num3 > 0.5) ? 3 : 2;
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex3, 1, false, info_seed7, gen_seed7);
					info_seed7 = dotNet35Random2.Next();
					gen_seed7 = dotNet35Random2.Next();
					orbitIndex3 = ((num3 > 0.5) ? 4 : 3);
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, orbitIndex3, 2, false, info_seed7, gen_seed7);
					info_seed7 = dotNet35Random2.Next();
					gen_seed7 = dotNet35Random2.Next();
					orbitIndex3 = ((num3 > 0.5) ? 5 : 4);
					star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 0, orbitIndex3, 3, false, info_seed7, gen_seed7);
				}
				else if (num2 < 0.75)
				{
					int info_seed7 = dotNet35Random2.Next();
					int gen_seed7 = dotNet35Random2.Next();
					int orbitIndex4 = (num3 > 0.5) ? 3 : 2;
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex4, 1, false, info_seed7, gen_seed7);
					info_seed7 = dotNet35Random2.Next();
					gen_seed7 = dotNet35Random2.Next();
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, 4, 2, true, info_seed7, gen_seed7);
					info_seed7 = dotNet35Random2.Next();
					gen_seed7 = dotNet35Random2.Next();
					star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 2, 1, 1, false, info_seed7, gen_seed7);
				}
				else
				{
					int info_seed7 = dotNet35Random2.Next();
					int gen_seed7 = dotNet35Random2.Next();
					int orbitIndex5 = (num3 > 0.5) ? 4 : 3;
					star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex5, 1, true, info_seed7, gen_seed7);
					info_seed7 = dotNet35Random2.Next();
					gen_seed7 = dotNet35Random2.Next();
					star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 1, false, info_seed7, gen_seed7);
					info_seed7 = dotNet35Random2.Next();
					gen_seed7 = dotNet35Random2.Next();
					star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 1, 2, 2, false, info_seed7, gen_seed7);
				}
			}
		}
		else
		{
			Array.Clear(StarGen.pGas, 0, StarGen.pGas.Length);
			if (star.index == 0)
			{
				star.planetCount = 4;
				StarGen.pGas[0] = 0.0;
				StarGen.pGas[1] = 0.0;
				StarGen.pGas[2] = 0.0;
			}
			else if (star.spectr == ESpectrType.M)
			{
				if (num < 0.1)
				{
					star.planetCount = 1;
				}
				else if (num < 0.3)
				{
					star.planetCount = 2;
				}
				else if (num < 0.8)
				{
					star.planetCount = 3;
				}
				else
				{
					star.planetCount = 4;
				}
				if (star.planetCount <= 3)
				{
					StarGen.pGas[0] = 0.2;
					StarGen.pGas[1] = 0.2;
				}
				else
				{
					StarGen.pGas[0] = 0.0;
					StarGen.pGas[1] = 0.2;
					StarGen.pGas[2] = 0.3;
				}
			}
			else if (star.spectr == ESpectrType.K)
			{
				if (num < 0.1)
				{
					star.planetCount = 1;
				}
				else if (num < 0.2)
				{
					star.planetCount = 2;
				}
				else if (num < 0.7)
				{
					star.planetCount = 3;
				}
				else if (num < 0.95)
				{
					star.planetCount = 4;
				}
				else
				{
					star.planetCount = 5;
				}
				if (star.planetCount <= 3)
				{
					StarGen.pGas[0] = 0.18;
					StarGen.pGas[1] = 0.18;
				}
				else
				{
					StarGen.pGas[0] = 0.0;
					StarGen.pGas[1] = 0.18;
					StarGen.pGas[2] = 0.28;
					StarGen.pGas[3] = 0.28;
				}
			}
			else if (star.spectr == ESpectrType.G)
			{
				if (num < 0.4)
				{
					star.planetCount = 3;
				}
				else if (num < 0.9)
				{
					star.planetCount = 4;
				}
				else
				{
					star.planetCount = 5;
				}
				if (star.planetCount <= 3)
				{
					StarGen.pGas[0] = 0.18;
					StarGen.pGas[1] = 0.18;
				}
				else
				{
					StarGen.pGas[0] = 0.0;
					StarGen.pGas[1] = 0.2;
					StarGen.pGas[2] = 0.3;
					StarGen.pGas[3] = 0.3;
				}
			}
			else if (star.spectr == ESpectrType.F)
			{
				if (num < 0.35)
				{
					star.planetCount = 3;
				}
				else if (num < 0.8)
				{
					star.planetCount = 4;
				}
				else
				{
					star.planetCount = 5;
				}
				if (star.planetCount <= 3)
				{
					StarGen.pGas[0] = 0.2;
					StarGen.pGas[1] = 0.2;
				}
				else
				{
					StarGen.pGas[0] = 0.0;
					StarGen.pGas[1] = 0.22;
					StarGen.pGas[2] = 0.31;
					StarGen.pGas[3] = 0.31;
				}
			}
			else if (star.spectr == ESpectrType.A)
			{
				if (num < 0.3)
				{
					star.planetCount = 3;
				}
				else if (num < 0.75)
				{
					star.planetCount = 4;
				}
				else
				{
					star.planetCount = 5;
				}
				if (star.planetCount <= 3)
				{
					StarGen.pGas[0] = 0.2;
					StarGen.pGas[1] = 0.2;
				}
				else
				{
					StarGen.pGas[0] = 0.1;
					StarGen.pGas[1] = 0.28;
					StarGen.pGas[2] = 0.3;
					StarGen.pGas[3] = 0.35;
				}
			}
			else if (star.spectr == ESpectrType.B)
			{
				if (num < 0.3)
				{
					star.planetCount = 4;
				}
				else if (num < 0.75)
				{
					star.planetCount = 5;
				}
				else
				{
					star.planetCount = 6;
				}
				if (star.planetCount <= 3)
				{
					StarGen.pGas[0] = 0.2;
					StarGen.pGas[1] = 0.2;
				}
				else
				{
					StarGen.pGas[0] = 0.1;
					StarGen.pGas[1] = 0.22;
					StarGen.pGas[2] = 0.28;
					StarGen.pGas[3] = 0.35;
					StarGen.pGas[4] = 0.35;
				}
			}
			else if (star.spectr == ESpectrType.O)
			{
				if (num < 0.5)
				{
					star.planetCount = 5;
				}
				else
				{
					star.planetCount = 6;
				}
				StarGen.pGas[0] = 0.1;
				StarGen.pGas[1] = 0.2;
				StarGen.pGas[2] = 0.25;
				StarGen.pGas[3] = 0.3;
				StarGen.pGas[4] = 0.32;
				StarGen.pGas[5] = 0.35;
			}
			else
			{
				star.planetCount = 1;
			}
			star.planets = new PlanetData[star.planetCount];
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 1;
			for (int i = 0; i < star.planetCount; i++)
			{
				int info_seed8 = dotNet35Random2.Next();
				int gen_seed8 = dotNet35Random2.Next();
				double num12 = dotNet35Random2.NextDouble();
				double num13 = dotNet35Random2.NextDouble();
				bool flag = false;
				if (num10 == 0)
				{
					num8++;
					if (i < star.planetCount - 1 && num12 < StarGen.pGas[i])
					{
						flag = true;
						if (num11 < 3)
						{
							num11 = 3;
						}
					}
					while (star.index != 0 || num11 != 3)
					{
						int num14 = star.planetCount - i;
						int num15 = 9 - num11;
						if (num15 <= num14)
						{
							goto IL_CB5;
						}
						float num16 = (float)num14 / (float)num15;
						if (num11 > 3)
						{
							num16 = Mathf.Lerp(num16, 1f, 0.45f) + 0.01f;
						}
						else
						{
							num16 = Mathf.Lerp(num16, 1f, 0.15f) + 0.01f;
						}
						if (dotNet35Random2.NextDouble() < (double)num16)
						{
							goto IL_CB5;
						}
						num11++;
					}
					flag = true;
				}
				else
				{
					num9++;
					flag = false;
				}
				IL_CB5:
				star.planets[i] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, i, num10, (num10 == 0) ? num11 : num9, (num10 == 0) ? num8 : num9, flag, info_seed8, gen_seed8);
				num11++;
				if (flag)
				{
					num10 = num8;
					num9 = 0;
				}
				if (num9 >= 1 && num13 < 0.8)
				{
					num10 = 0;
					num9 = 0;
				}
			}
		}
		int num17 = 0;
		int num18 = 0;
		int num19 = 0;
		for (int j = 0; j < star.planetCount; j++)
		{
			if (star.planets[j].type == EPlanetType.Gas)
			{
				num17 = star.planets[j].orbitIndex;
				break;
			}
		}
		for (int k = 0; k < star.planetCount; k++)
		{
			if (star.planets[k].orbitAround == 0)
			{
				num18 = star.planets[k].orbitIndex;
			}
		}
		if (num17 > 0)
		{
			int num20 = num17 - 1;
			bool flag2 = true;
			for (int l = 0; l < star.planetCount; l++)
			{
				if (star.planets[l].orbitAround == 0 && star.planets[l].orbitIndex == num17 - 1)
				{
					flag2 = false;
					break;
				}
			}
			if (flag2 && num4 < 0.2 + (double)num20 * 0.2)
			{
				num19 = num20;
			}
		}
		int num21;
		if (num5 < 0.2)
		{
			num21 = num18 + 3;
		}
		else if (num5 < 0.4)
		{
			num21 = num18 + 2;
		}
		else if (num5 < 0.8)
		{
			num21 = num18 + 1;
		}
		else
		{
			num21 = 0;
		}
		if (num21 != 0 && num21 < 5)
		{
			num21 = 5;
		}
		star.asterBelt1OrbitIndex = (float)num19;
		star.asterBelt2OrbitIndex = (float)num21;
		if (num19 > 0)
		{
			star.asterBelt1Radius = StarGen.orbitRadius[num19] * (float)num6 * star.orbitScaler;
		}
		if (num21 > 0)
		{
			star.asterBelt2Radius = StarGen.orbitRadius[num21] * (float)num7 * star.orbitScaler;
		}
		for (int m = 0; m < star.planetCount; m++)
		{
			PlanetData planetData = star.planets[m];
			int orbitIndex6 = planetData.orbitIndex;
			int orbitAroundOrbitIndex = (planetData.orbitAroundPlanet != null) ? planetData.orbitAroundPlanet.orbitIndex : 0;
			StarGen.SetHiveOrbitConditionFalse(orbitIndex6, orbitAroundOrbitIndex, planetData.sunDistance / star.orbitScaler, star.index);
		}
		star.hiveAstroOrbits = new AstroOrbitData[8];
		AstroOrbitData[] hiveAstroOrbits = star.hiveAstroOrbits;
		int num22 = 0;
		for (int n = 0; n < StarGen.hiveOrbitCondition.Length; n++)
		{
			if (StarGen.hiveOrbitCondition[n])
			{
				num22++;
			}
		}
		for (int num23 = 0; num23 < 8; num23++)
		{
			double num24 = dotNet35Random3.NextDouble() * 2.0 - 1.0;
			double num25 = dotNet35Random3.NextDouble();
			double num26 = dotNet35Random3.NextDouble();
			num24 = (double)Math.Sign(num24) * Math.Pow(Math.Abs(num24), 0.7) * 90.0;
			num25 *= 360.0;
			num26 *= 360.0;
			float num27 = 0.3f;
			Assert.Positive(num22);
			if (num22 > 0)
			{
				int num28 = (star.index != 0) ? 5 : 2;
				num28 = ((num22 > num28) ? num28 : num22);
				int num29 = num28 * 100;
				int num30 = num29 * 100;
				int num31 = dotNet35Random3.Next(num29);
				int num32 = num31 * num31 / num30;
				for (int num33 = 0; num33 < StarGen.hiveOrbitCondition.Length; num33++)
				{
					if (StarGen.hiveOrbitCondition[num33])
					{
						if (num32 == 0)
						{
							num27 = StarGen.hiveOrbitRadius[num33];
							StarGen.hiveOrbitCondition[num33] = false;
							num22--;
							break;
						}
						num32--;
					}
				}
			}
			float num34 = num27 * star.orbitScaler;
			hiveAstroOrbits[num23] = new AstroOrbitData();
			hiveAstroOrbits[num23].orbitRadius = num34;
			hiveAstroOrbits[num23].orbitInclination = (float)num24;
			hiveAstroOrbits[num23].orbitLongitude = (float)num25;
			hiveAstroOrbits[num23].orbitPhase = (float)num26;
			if (gameDesc.creationVersion.Build < 20700)
			{
				hiveAstroOrbits[num23].orbitalPeriod = Math.Sqrt(39.47841760435743 * (double)num27 * (double)num27 * (double)num27 / (1.3538551990520382E-06 * (double)star.mass));
			}
			else
			{
				hiveAstroOrbits[num23].orbitalPeriod = Math.Sqrt(39.47841760435743 * (double)num34 * (double)num34 * (double)num34 / (5.415420796208153E-06 * (double)star.mass));
			}
			hiveAstroOrbits[num23].orbitRotation = Quaternion.AngleAxis(hiveAstroOrbits[num23].orbitLongitude, Vector3.up) * Quaternion.AngleAxis(hiveAstroOrbits[num23].orbitInclination, Vector3.forward);
			hiveAstroOrbits[num23].orbitNormal = Maths.QRotateLF(hiveAstroOrbits[num23].orbitRotation, new VectorLF3(0f, 1f, 0f)).normalized;
		}
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x003236E4 File Offset: 0x003218E4
	public static void SetStarAge(StarData star, float age, double rn, double rt)
	{
		float num = (float)(rn * 0.1 + 0.95);
		float num2 = (float)(rt * 0.4 + 0.8);
		float num3 = (float)(rt * 9.0 + 1.0);
		star.age = age;
		if (age < 1f)
		{
			if (age >= 0.96f)
			{
				float num4 = (float)(Math.Pow(5.0, Math.Abs(Math.Log10((double)star.mass) - 0.7)) * 5.0);
				if (num4 > 10f)
				{
					num4 = (Mathf.Log(num4 * 0.1f) + 1f) * 10f;
				}
				float num5 = 1f - Mathf.Pow(star.age, 30f) * 0.5f;
				star.type = EStarType.GiantStar;
				star.mass = num5 * star.mass;
				star.radius = num4 * num2;
				star.acdiskRadius = 0f;
				star.temperature = num5 * star.temperature;
				star.luminosity = 1.6f * star.luminosity;
				star.habitableRadius = 9f * star.habitableRadius;
				star.lightBalanceRadius = 3f * star.habitableRadius;
				star.orbitScaler = 3.3f * star.orbitScaler;
			}
			return;
		}
		if (star.mass >= 18f)
		{
			star.type = EStarType.BlackHole;
			star.spectr = ESpectrType.X;
			star.mass *= 2.5f * num2;
			star.radius *= 1f;
			star.acdiskRadius = star.radius * 5f;
			star.temperature = 0f;
			star.luminosity *= 0.001f * num;
			star.habitableRadius = 0f;
			star.lightBalanceRadius *= 0.4f * num;
			star.color = 1f;
			return;
		}
		if (star.mass >= 7f)
		{
			star.type = EStarType.NeutronStar;
			star.spectr = ESpectrType.X;
			star.mass *= 0.2f * num;
			star.radius *= 0.15f;
			star.acdiskRadius = star.radius * 9f;
			star.temperature = num3 * 10000000f;
			star.luminosity *= 0.1f * num;
			star.habitableRadius = 0f;
			star.lightBalanceRadius *= 3f * num;
			star.orbitScaler *= 1.5f * num;
			star.color = 1f;
			return;
		}
		star.type = EStarType.WhiteDwarf;
		star.spectr = ESpectrType.X;
		star.mass *= 0.2f * num;
		star.radius *= 0.2f;
		star.acdiskRadius = 0f;
		star.temperature = num2 * 150000f;
		star.luminosity *= 0.04f * num2;
		star.habitableRadius *= 0.15f * num2;
		star.lightBalanceRadius *= 0.2f * num;
		star.color = 0.7f;
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x00323A32 File Offset: 0x00321C32
	private static float RandNormal(float averageValue, float standardDeviation, double r1, double r2)
	{
		return averageValue + standardDeviation * (float)(Math.Sqrt(-2.0 * Math.Log(1.0 - r1)) * Math.Sin(6.283185307179586 * r2));
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x00323A6C File Offset: 0x00321C6C
	private static void SetHiveOrbitsConditionsTrue()
	{
		for (int i = 0; i < StarGen.hiveOrbitCondition.Length; i++)
		{
			StarGen.hiveOrbitCondition[i] = true;
		}
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x00323A94 File Offset: 0x00321C94
	private static void SetHiveOrbitConditionFalse(int planetOrbitIndex, int orbitAroundOrbitIndex, float sunDistance, int starIndex)
	{
		int num = (orbitAroundOrbitIndex > 0) ? orbitAroundOrbitIndex : planetOrbitIndex;
		int num2 = (orbitAroundOrbitIndex > 0) ? planetOrbitIndex : 0;
		if (num <= 0)
		{
			return;
		}
		if (num >= StarGen.planet2HiveOrbitTable.Length)
		{
			return;
		}
		int num3 = StarGen.planet2HiveOrbitTable[num];
		StarGen.hiveOrbitCondition[num3] = false;
		if (num2 > 0)
		{
			float num4;
			if (starIndex == 0)
			{
				num4 = 0.041f * (float)num2 + 0.026f + 0.12f;
			}
			else
			{
				num4 = 0.049f * (float)num2 + 0.026f + 0.13f;
			}
			int num5 = num3 - 1;
			int num6 = num3 + 1;
			if (num5 >= 0 && sunDistance - StarGen.hiveOrbitRadius[num5] < num4)
			{
				StarGen.hiveOrbitCondition[num5] = false;
			}
			if (num6 < StarGen.hiveOrbitCondition.Length && StarGen.hiveOrbitRadius[num6] - sunDistance < num4)
			{
				StarGen.hiveOrbitCondition[num6] = false;
			}
		}
	}

	// Token: 0x04004C32 RID: 19506
	public static float[] orbitRadius = new float[]
	{
		0f,
		0.4f,
		0.7f,
		1f,
		1.4f,
		1.9f,
		2.5f,
		3.3f,
		4.3f,
		5.5f,
		6.9f,
		8.4f,
		10f,
		11.7f,
		13.5f,
		15.4f,
		17.5f
	};

	// Token: 0x04004C33 RID: 19507
	public static float[] hiveOrbitRadius = new float[]
	{
		0.4f,
		0.55f,
		0.7f,
		0.83f,
		1f,
		1.2f,
		1.4f,
		1.58f,
		1.72f,
		1.9f,
		2.11f,
		2.29f,
		2.5f,
		2.78f,
		3.02f,
		3.3f,
		3.6f,
		3.9f
	};

	// Token: 0x04004C34 RID: 19508
	public static int[] planet2HiveOrbitTable = new int[]
	{
		0,
		0,
		2,
		4,
		6,
		9,
		12,
		15
	};

	// Token: 0x04004C35 RID: 19509
	public static bool[] hiveOrbitCondition = new bool[StarGen.hiveOrbitRadius.Length];

	// Token: 0x04004C36 RID: 19510
	public const double GRAVITY = 1.3538551990520382E-06;

	// Token: 0x04004C37 RID: 19511
	public const float E = 2.7182817f;

	// Token: 0x04004C38 RID: 19512
	public static float specifyBirthStarMass = 0f;

	// Token: 0x04004C39 RID: 19513
	public static float specifyBirthStarAge = 0f;

	// Token: 0x04004C3A RID: 19514
	private static double[] pGas = new double[10];

	// Token: 0x04004C3B RID: 19515
	private const double PI = 3.141592653589793;
}
