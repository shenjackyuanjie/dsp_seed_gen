using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000580 RID: 1408
public static class UniverseGen
{
	// Token: 0x06003AEF RID: 15087 RVA: 0x00323BD6 File Offset: 0x00321DD6
	public static void Start()
	{
		PlanetModelingManager.Start();
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x00323BDD File Offset: 0x00321DDD
	public static void End()
	{
		PlanetModelingManager.End();
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x00323BE4 File Offset: 0x00321DE4
	public static void Update()
	{
		PlanetModelingManager.Update();
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x00323BEC File Offset: 0x00321DEC
	public static GalaxyData CreateGalaxy(GameDesc gameDesc)
	{
		int galaxyAlgo = gameDesc.galaxyAlgo;
		int galaxySeed = gameDesc.galaxySeed;
		int num = gameDesc.starCount;
		if (galaxyAlgo < 20200101 || galaxyAlgo > 20591231)
		{
			throw new Exception("Wrong version of unigen algorithm!");
		}
		PlanetGen.gasCoef = (gameDesc.isRareResource ? 0.8f : 1f);
		DotNet35Random dotNet35Random = new DotNet35Random(galaxySeed);
		num = UniverseGen.GenerateTempPoses(dotNet35Random.Next(), num, 4, 2.0, 2.3, 3.5, 0.18);
		GalaxyData galaxyData = new GalaxyData();
		galaxyData.seed = galaxySeed;
		galaxyData.starCount = num;
		galaxyData.stars = new StarData[num];
		Assert.Positive(num);
		if (num <= 0)
		{
			return galaxyData;
		}
		float num2 = (float)dotNet35Random.NextDouble();
		float num3 = (float)dotNet35Random.NextDouble();
		float num4 = (float)dotNet35Random.NextDouble();
		float num5 = (float)dotNet35Random.NextDouble();
		int num6 = Mathf.CeilToInt(0.01f * (float)num + num2 * 0.3f);
		int num7 = Mathf.CeilToInt(0.01f * (float)num + num3 * 0.3f);
		int num8 = Mathf.CeilToInt(0.016f * (float)num + num4 * 0.4f);
		int num9 = Mathf.CeilToInt(0.013f * (float)num + num5 * 1.4f);
		int num10 = num - num6;
		int num11 = num10 - num7;
		int num12 = num11 - num8;
		int num13 = (num12 - 1) / num9;
		int num14 = num13 / 2;
		for (int i = 0; i < num; i++)
		{
			int seed = dotNet35Random.Next();
			if (i == 0)
			{
				galaxyData.stars[i] = StarGen.CreateBirthStar(galaxyData, gameDesc, seed);
			}
			else
			{
				ESpectrType needSpectr = ESpectrType.X;
				if (i == 3)
				{
					needSpectr = ESpectrType.M;
				}
				else if (i == num12 - 1)
				{
					needSpectr = ESpectrType.O;
				}
				EStarType needtype = EStarType.MainSeqStar;
				if (i % num13 == num14)
				{
					needtype = EStarType.GiantStar;
				}
				if (i >= num10)
				{
					needtype = EStarType.BlackHole;
				}
				else if (i >= num11)
				{
					needtype = EStarType.NeutronStar;
				}
				else if (i >= num12)
				{
					needtype = EStarType.WhiteDwarf;
				}
				galaxyData.stars[i] = StarGen.CreateStar(galaxyData, UniverseGen.tmp_poses[i], gameDesc, i + 1, seed, needtype, needSpectr);
			}
		}
		AstroData[] astrosData = galaxyData.astrosData;
		StarData[] stars = galaxyData.stars;
		for (int j = 0; j < galaxyData.astrosData.Length; j++)
		{
			astrosData[j].uRot.w = 1f;
			astrosData[j].uRotNext.w = 1f;
		}
		for (int k = 0; k < num; k++)
		{
			StarGen.CreateStarPlanets(galaxyData, stars[k], gameDesc);
			int astroId = stars[k].astroId;
			astrosData[astroId].id = astroId;
			astrosData[astroId].type = EAstroType.Star;
			astrosData[astroId].uPos = (astrosData[astroId].uPosNext = stars[k].uPosition);
			astrosData[astroId].uRot = (astrosData[astroId].uRotNext = Quaternion.identity);
			astrosData[astroId].uRadius = stars[k].physicsRadius;
		}
		galaxyData.UpdatePoses(0.0);
		galaxyData.birthPlanetId = 0;
		if (num > 0)
		{
			StarData starData = stars[0];
			for (int l = 0; l < starData.planetCount; l++)
			{
				PlanetData planetData = starData.planets[l];
				ThemeProto themeProto = LDB.themes.Select(planetData.theme);
				if (themeProto != null && themeProto.Distribute == EThemeDistribute.Birth)
				{
					galaxyData.birthPlanetId = planetData.id;
					galaxyData.birthStarId = starData.id;
					break;
				}
			}
		}
		Assert.Positive(galaxyData.birthPlanetId);
		UniverseGen.CreateGalaxyStarGraph(galaxyData);
		PlanetGen.gasCoef = 1f;
		return galaxyData;
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x00323FAC File Offset: 0x003221AC
	private static int GenerateTempPoses(int seed, int targetCount, int iterCount, double minDist, double minStepLen, double maxStepLen, double flatten)
	{
		if (UniverseGen.tmp_poses == null)
		{
			UniverseGen.tmp_poses = new List<VectorLF3>();
			UniverseGen.tmp_drunk = new List<VectorLF3>();
		}
		else
		{
			UniverseGen.tmp_poses.Clear();
			UniverseGen.tmp_drunk.Clear();
		}
		if (iterCount < 1)
		{
			iterCount = 1;
		}
		else if (iterCount > 16)
		{
			iterCount = 16;
		}
		UniverseGen.RandomPoses(seed, targetCount * iterCount, minDist, minStepLen, maxStepLen, flatten);
		for (int i = UniverseGen.tmp_poses.Count - 1; i >= 0; i--)
		{
			if (i % iterCount != 0)
			{
				UniverseGen.tmp_poses.RemoveAt(i);
			}
			if (UniverseGen.tmp_poses.Count <= targetCount)
			{
				break;
			}
		}
		return UniverseGen.tmp_poses.Count;
	}

	// Token: 0x06003AF4 RID: 15092 RVA: 0x0032404C File Offset: 0x0032224C
	private static void RandomPoses(int seed, int maxCount, double minDist, double minStepLen, double maxStepLen, double flatten)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		double num = dotNet35Random.NextDouble();
		UniverseGen.tmp_poses.Add(VectorLF3.zero);
		int num2 = 6;
		int num3 = 8;
		if (num2 < 1)
		{
			num2 = 1;
		}
		if (num3 < 1)
		{
			num3 = 1;
		}
		int num4 = (int)(num * (double)(num3 - num2) + (double)num2);
		for (int i = 0; i < num4; i++)
		{
			int num5 = 0;
			while (num5++ < 256)
			{
				double num6 = dotNet35Random.NextDouble() * 2.0 - 1.0;
				double num7 = (dotNet35Random.NextDouble() * 2.0 - 1.0) * flatten;
				double num8 = dotNet35Random.NextDouble() * 2.0 - 1.0;
				double num9 = dotNet35Random.NextDouble();
				double num10 = num6 * num6 + num7 * num7 + num8 * num8;
				if (num10 <= 1.0 && num10 >= 1E-08)
				{
					double num11 = Math.Sqrt(num10);
					num9 = (num9 * (maxStepLen - minStepLen) + minDist) / num11;
					VectorLF3 vectorLF = new VectorLF3(num6 * num9, num7 * num9, num8 * num9);
					if (!UniverseGen.CheckCollision(UniverseGen.tmp_poses, vectorLF, minDist))
					{
						UniverseGen.tmp_drunk.Add(vectorLF);
						UniverseGen.tmp_poses.Add(vectorLF);
						if (UniverseGen.tmp_poses.Count >= maxCount)
						{
							return;
						}
						break;
					}
				}
			}
		}
		int num12 = 0;
		while (num12++ < 256)
		{
			for (int j = 0; j < UniverseGen.tmp_drunk.Count; j++)
			{
				if (dotNet35Random.NextDouble() <= 0.7)
				{
					int num13 = 0;
					while (num13++ < 256)
					{
						double num14 = dotNet35Random.NextDouble() * 2.0 - 1.0;
						double num15 = (dotNet35Random.NextDouble() * 2.0 - 1.0) * flatten;
						double num16 = dotNet35Random.NextDouble() * 2.0 - 1.0;
						double num17 = dotNet35Random.NextDouble();
						double num18 = num14 * num14 + num15 * num15 + num16 * num16;
						if (num18 <= 1.0 && num18 >= 1E-08)
						{
							double num19 = Math.Sqrt(num18);
							num17 = (num17 * (maxStepLen - minStepLen) + minDist) / num19;
							VectorLF3 vectorLF2 = new VectorLF3(UniverseGen.tmp_drunk[j].x + num14 * num17, UniverseGen.tmp_drunk[j].y + num15 * num17, UniverseGen.tmp_drunk[j].z + num16 * num17);
							if (!UniverseGen.CheckCollision(UniverseGen.tmp_poses, vectorLF2, minDist))
							{
								UniverseGen.tmp_drunk[j] = vectorLF2;
								UniverseGen.tmp_poses.Add(vectorLF2);
								if (UniverseGen.tmp_poses.Count >= maxCount)
								{
									return;
								}
								break;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003AF5 RID: 15093 RVA: 0x00324340 File Offset: 0x00322540
	private static bool CheckCollision(List<VectorLF3> pts, VectorLF3 pt, double min_dist)
	{
		double num = min_dist * min_dist;
		foreach (VectorLF3 vectorLF in pts)
		{
			double num2 = pt.x - vectorLF.x;
			double num3 = pt.y - vectorLF.y;
			double num4 = pt.z - vectorLF.z;
			if (num2 * num2 + num3 * num3 + num4 * num4 < num)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003AF6 RID: 15094 RVA: 0x003243D0 File Offset: 0x003225D0
	public static void CreateGalaxyStarGraph(GalaxyData galaxy)
	{
		galaxy.graphNodes = new StarGraphNode[galaxy.starCount];
		for (int i = 0; i < galaxy.starCount; i++)
		{
			galaxy.graphNodes[i] = new StarGraphNode(galaxy.stars[i]);
			StarGraphNode starGraphNode = galaxy.graphNodes[i];
			for (int j = 0; j < i; j++)
			{
				StarGraphNode starGraphNode2 = galaxy.graphNodes[j];
				VectorLF3 pos = starGraphNode.pos;
				VectorLF3 pos2 = starGraphNode2.pos;
				if ((pos - pos2).sqrMagnitude < 64.0)
				{
					UniverseGen.list_sorted_add(starGraphNode.conns, starGraphNode2);
					UniverseGen.list_sorted_add(starGraphNode2.conns, starGraphNode);
				}
			}
			UniverseGen.line_arragement_for_add_node(starGraphNode);
		}
	}

	// Token: 0x06003AF7 RID: 15095 RVA: 0x0032447C File Offset: 0x0032267C
	private static void list_sorted_add(List<StarGraphNode> l, StarGraphNode n)
	{
		int count = l.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			if (l[i].index == n.index)
			{
				flag = true;
				break;
			}
			if (l[i].index > n.index)
			{
				l.Insert(i, n);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			l.Add(n);
		}
	}

	// Token: 0x06003AF8 RID: 15096 RVA: 0x003244E0 File Offset: 0x003226E0
	private static void line_arragement_for_add_node(StarGraphNode node)
	{
		if (UniverseGen.tmp_state == null)
		{
			UniverseGen.tmp_state = new int[128];
		}
		Array.Clear(UniverseGen.tmp_state, 0, UniverseGen.tmp_state.Length);
		Vector3 vector = node.pos;
		for (int i = 0; i < node.conns.Count; i++)
		{
			StarGraphNode starGraphNode = node.conns[i];
			Vector3 vector2 = starGraphNode.pos;
			for (int j = i + 1; j < node.conns.Count; j++)
			{
				StarGraphNode starGraphNode2 = node.conns[j];
				Vector3 vector3 = starGraphNode2.pos;
				bool flag = false;
				for (int k = 0; k < starGraphNode.conns.Count; k++)
				{
					if (starGraphNode.conns[k] == starGraphNode2)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					float num = (vector2.x - vector.x) * (vector2.x - vector.x) + (vector2.y - vector.y) * (vector2.y - vector.y) + (vector2.z - vector.z) * (vector2.z - vector.z);
					float num2 = (vector3.x - vector.x) * (vector3.x - vector.x) + (vector3.y - vector.y) * (vector3.y - vector.y) + (vector3.z - vector.z) * (vector3.z - vector.z);
					float num3 = (vector2.x - vector3.x) * (vector2.x - vector3.x) + (vector2.y - vector3.y) * (vector2.y - vector3.y) + (vector2.z - vector3.z) * (vector2.z - vector3.z);
					float num4 = (num > num2) ? ((num > num3) ? num : num3) : ((num2 > num3) ? num2 : num3);
					float num5 = (num < num2) ? ((num < num3) ? num : num3) : ((num2 < num3) ? num2 : num3);
					float num6 = (num + num2 + num3 - num4 - num5) * 1.001f;
					num5 *= 1.01f;
					if (num <= num6 || num <= num5)
					{
						if (UniverseGen.tmp_state[i] == 0)
						{
							UniverseGen.list_sorted_add(node.lines, starGraphNode);
							UniverseGen.list_sorted_add(starGraphNode.lines, node);
							UniverseGen.tmp_state[i] = 1;
						}
					}
					else
					{
						UniverseGen.tmp_state[i] = -1;
						node.lines.Remove(starGraphNode);
						starGraphNode.lines.Remove(node);
					}
					if (num2 <= num6 || num2 <= num5)
					{
						if (UniverseGen.tmp_state[j] == 0)
						{
							UniverseGen.list_sorted_add(node.lines, starGraphNode2);
							UniverseGen.list_sorted_add(starGraphNode2.lines, node);
							UniverseGen.tmp_state[j] = 1;
						}
					}
					else
					{
						UniverseGen.tmp_state[j] = -1;
						node.lines.Remove(starGraphNode2);
						starGraphNode2.lines.Remove(node);
					}
					if (num3 > num6 && num3 > num5)
					{
						starGraphNode.lines.Remove(starGraphNode2);
						starGraphNode2.lines.Remove(starGraphNode);
					}
				}
			}
			if (UniverseGen.tmp_state[i] == 0)
			{
				UniverseGen.list_sorted_add(node.lines, starGraphNode);
				UniverseGen.list_sorted_add(starGraphNode.lines, node);
				UniverseGen.tmp_state[i] = 1;
			}
		}
		Array.Clear(UniverseGen.tmp_state, 0, UniverseGen.tmp_state.Length);
	}

	// Token: 0x04004C3C RID: 19516
	public static int algoVersion = 20200403;

	// Token: 0x04004C3D RID: 19517
	private static List<VectorLF3> tmp_poses;

	// Token: 0x04004C3E RID: 19518
	private static List<VectorLF3> tmp_drunk;

	// Token: 0x04004C3F RID: 19519
	private static int[] tmp_state = null;
}
