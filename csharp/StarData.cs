using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class StarData
{
	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06001635 RID: 5685 RVA: 0x00181A9D File Offset: 0x0017FC9D
	public string displayName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overrideName))
			{
				return this.overrideName;
			}
			return this.name;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06001636 RID: 5686 RVA: 0x00181AB9 File Offset: 0x0017FCB9
	public float expSharingFactor
	{
		get
		{
			return (1f - this.safetyFactor) * (1f - this.safetyFactor) * 3.5f + 0.5f;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001637 RID: 5687 RVA: 0x00181AE0 File Offset: 0x0017FCE0
	public int astroId
	{
		get
		{
			return this.id * 100;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001638 RID: 5688 RVA: 0x00181AEB File Offset: 0x0017FCEB
	public float dysonLumino
	{
		get
		{
			return Mathf.Round((float)Math.Pow((double)this.luminosity, 0.33000001311302185) * 1000f) / 1000f;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06001639 RID: 5689 RVA: 0x00181B14 File Offset: 0x0017FD14
	public float systemRadius
	{
		get
		{
			float sunDistance = this.dysonRadius;
			if (this.planetCount > 0)
			{
				sunDistance = this.planets[this.planetCount - 1].sunDistance;
			}
			return sunDistance;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x0600163A RID: 5690 RVA: 0x00181B47 File Offset: 0x0017FD47
	public float physicsRadius
	{
		get
		{
			return this.radius * 1200f;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x0600163B RID: 5691 RVA: 0x00181B55 File Offset: 0x0017FD55
	public float viewRadius
	{
		get
		{
			return this.radius * 800f;
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x0600163C RID: 5692 RVA: 0x00181B64 File Offset: 0x0017FD64
	public string typeString
	{
		get
		{
			string text = "";
			if (this.type == EStarType.GiantStar)
			{
				if (this.spectr <= ESpectrType.K)
				{
					text += "红巨星".Translate();
				}
				else if (this.spectr <= ESpectrType.F)
				{
					text += "黄巨星".Translate();
				}
				else if (this.spectr == ESpectrType.A)
				{
					text += "白巨星".Translate();
				}
				else
				{
					text += "蓝巨星".Translate();
				}
			}
			else if (this.type == EStarType.WhiteDwarf)
			{
				text += "白矮星".Translate();
			}
			else if (this.type == EStarType.NeutronStar)
			{
				text += "中子星".Translate();
			}
			else if (this.type == EStarType.BlackHole)
			{
				text += "黑洞".Translate();
			}
			else if (this.type == EStarType.MainSeqStar)
			{
				text = text + this.spectr.ToString() + "型恒星".Translate();
			}
			return text;
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x0600163D RID: 5693 RVA: 0x00181C6F File Offset: 0x0017FE6F
	public bool epicHive
	{
		get
		{
			return this.type == EStarType.NeutronStar || this.type == EStarType.BlackHole;
		}
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00181C88 File Offset: 0x0017FE88
	public bool CalcVeinAmounts(ref long[] veinAmounts, HashSet<int> hashes, int filterType)
	{
		if (veinAmounts == null)
		{
			veinAmounts = new long[64];
		}
		Array.Clear(veinAmounts, 0, veinAmounts.Length);
		hashes.Clear();
		bool result = true;
		for (int i = 0; i < this.planetCount; i++)
		{
			PlanetData planetData = this.planets[i];
			VeinGroup[] runtimeVeinGroups = planetData.runtimeVeinGroups;
			if (runtimeVeinGroups != null)
			{
				if (filterType == 1)
				{
					PlanetFactory factory = planetData.factory;
					if (factory != null)
					{
						MinerComponent[] minerPool = factory.factorySystem.minerPool;
						int minerCursor = factory.factorySystem.minerCursor;
						PowerConsumerComponent[] consumerPool = factory.powerSystem.consumerPool;
						VeinData[] veinPool = factory.veinPool;
						hashes.Clear();
						for (int j = 0; j < minerCursor; j++)
						{
							ref MinerComponent ptr = ref minerPool[j];
							if (ptr.id == j && ptr.type != EMinerType.Water && consumerPool[ptr.pcId].networkId > 0)
							{
								for (int k = 0; k < ptr.veinCount; k++)
								{
									int num = ptr.veins[k];
									if (num > 0 && veinPool[num].id == num && !hashes.Contains(num))
									{
										veinAmounts[(int)veinPool[num].type] += (long)veinPool[num].amount;
										hashes.Add(num);
									}
								}
							}
						}
					}
				}
				else if (filterType == 2)
				{
					for (int l = 1; l < runtimeVeinGroups.Length; l++)
					{
						veinAmounts[(int)runtimeVeinGroups[l].type] += runtimeVeinGroups[l].amount;
					}
					PlanetFactory factory2 = planetData.factory;
					if (factory2 != null)
					{
						MinerComponent[] minerPool2 = factory2.factorySystem.minerPool;
						int minerCursor2 = factory2.factorySystem.minerCursor;
						PowerConsumerComponent[] consumerPool2 = factory2.powerSystem.consumerPool;
						VeinData[] veinPool2 = factory2.veinPool;
						hashes.Clear();
						for (int m = 0; m < minerCursor2; m++)
						{
							ref MinerComponent ptr2 = ref minerPool2[m];
							if (ptr2.id == m && ptr2.type != EMinerType.Water && consumerPool2[ptr2.pcId].networkId > 0)
							{
								for (int n = 0; n < ptr2.veinCount; n++)
								{
									int num2 = ptr2.veins[n];
									if (num2 > 0 && veinPool2[num2].id == num2 && !hashes.Contains(num2))
									{
										veinAmounts[(int)veinPool2[num2].type] -= (long)veinPool2[num2].amount;
										hashes.Add(num2);
									}
								}
							}
						}
					}
				}
				else
				{
					for (int num3 = 1; num3 < runtimeVeinGroups.Length; num3++)
					{
						veinAmounts[(int)runtimeVeinGroups[num3].type] += runtimeVeinGroups[num3].amount;
					}
				}
			}
			else
			{
				result = false;
			}
		}
		veinAmounts[0] = 0L;
		return result;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x00181F90 File Offset: 0x00180190
	public bool CalcVeinCounts(ref int[] veinCounts, HashSet<int> hashes, int filterType)
	{
		if (veinCounts == null)
		{
			veinCounts = new int[64];
		}
		Array.Clear(veinCounts, 0, veinCounts.Length);
		hashes.Clear();
		bool result = true;
		for (int i = 0; i < this.planetCount; i++)
		{
			PlanetData planetData = this.planets[i];
			VeinGroup[] runtimeVeinGroups = planetData.runtimeVeinGroups;
			if (runtimeVeinGroups != null)
			{
				if (filterType == 1)
				{
					PlanetFactory factory = planetData.factory;
					if (factory != null)
					{
						MinerComponent[] minerPool = factory.factorySystem.minerPool;
						int minerCursor = factory.factorySystem.minerCursor;
						PowerConsumerComponent[] consumerPool = factory.powerSystem.consumerPool;
						VeinData[] veinPool = factory.veinPool;
						hashes.Clear();
						for (int j = 0; j < minerCursor; j++)
						{
							ref MinerComponent ptr = ref minerPool[j];
							if (ptr.id == j && ptr.type != EMinerType.Water && consumerPool[ptr.pcId].networkId > 0)
							{
								for (int k = 0; k < ptr.veinCount; k++)
								{
									int num = ptr.veins[k];
									if (num > 0 && veinPool[num].id == num && !hashes.Contains(num))
									{
										veinCounts[(int)veinPool[num].type]++;
										hashes.Add(num);
									}
								}
							}
						}
					}
				}
				else if (filterType == 2)
				{
					for (int l = 1; l < runtimeVeinGroups.Length; l++)
					{
						veinCounts[(int)runtimeVeinGroups[l].type] += runtimeVeinGroups[l].count;
					}
					PlanetFactory factory2 = planetData.factory;
					if (factory2 != null)
					{
						MinerComponent[] minerPool2 = factory2.factorySystem.minerPool;
						int minerCursor2 = factory2.factorySystem.minerCursor;
						PowerConsumerComponent[] consumerPool2 = factory2.powerSystem.consumerPool;
						VeinData[] veinPool2 = factory2.veinPool;
						hashes.Clear();
						for (int m = 0; m < minerCursor2; m++)
						{
							ref MinerComponent ptr2 = ref minerPool2[m];
							if (ptr2.id == m && ptr2.type != EMinerType.Water && consumerPool2[ptr2.pcId].networkId > 0)
							{
								for (int n = 0; n < ptr2.veinCount; n++)
								{
									int num2 = ptr2.veins[n];
									if (num2 > 0 && veinPool2[num2].id == num2 && !hashes.Contains(num2))
									{
										veinCounts[(int)veinPool2[num2].type]--;
										hashes.Add(num2);
									}
								}
							}
						}
					}
				}
				else
				{
					for (int num3 = 1; num3 < runtimeVeinGroups.Length; num3++)
					{
						veinCounts[(int)runtimeVeinGroups[num3].type] += runtimeVeinGroups[num3].count;
					}
				}
			}
			else
			{
				result = false;
			}
		}
		veinCounts[0] = 0;
		return result;
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06001640 RID: 5696 RVA: 0x00182274 File Offset: 0x00180474
	public bool loaded
	{
		get
		{
			if (this.planets == null)
			{
				return false;
			}
			for (int i = 0; i < this.planetCount; i++)
			{
				if (!this.planets[i].loaded)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001641 RID: 5697 RVA: 0x001822B0 File Offset: 0x001804B0
	public bool calculated
	{
		get
		{
			if (this.planets == null)
			{
				return false;
			}
			for (int i = 0; i < this.planetCount; i++)
			{
				if (!this.planets[i].calculated)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001642 RID: 5698 RVA: 0x001822EC File Offset: 0x001804EC
	public bool hasAnyFactory
	{
		get
		{
			if (this.planets == null)
			{
				return false;
			}
			for (int i = 0; i < this.planetCount; i++)
			{
				if (this.planets[i].factory != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06001643 RID: 5699 RVA: 0x00182328 File Offset: 0x00180528
	// (remove) Token: 0x06001644 RID: 5700 RVA: 0x00182360 File Offset: 0x00180560
	public event Action<StarData> onLoaded;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06001645 RID: 5701 RVA: 0x00182398 File Offset: 0x00180598
	// (remove) Token: 0x06001646 RID: 5702 RVA: 0x001823D0 File Offset: 0x001805D0
	public event Action<StarData> onDisplayNameChange;

	// Token: 0x06001647 RID: 5703 RVA: 0x00182405 File Offset: 0x00180605
	public void Load()
	{
		PlanetModelingManager.RequestLoadStar(this);
		if (GameMain.universeSimulator != null)
		{
			GameMain.universeSimulator.SetLocalStar(this);
		}
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x00182428 File Offset: 0x00180628
	public void Unload()
	{
		for (int i = 0; i < this.planetCount; i++)
		{
			if (this.planets == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"planet == null\r\nstar = ",
					this.id,
					" planetCount = ",
					this.planetCount
				}));
			}
			if (this.planets[i] == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"planet[",
					i,
					"] == null\r\nstar = ",
					this.id,
					" planetCount = ",
					this.planetCount
				}));
			}
			this.planets[i].Unload();
		}
		if (GameMain.universeSimulator != null)
		{
			GameMain.universeSimulator.SetLocalStar(null);
		}
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x0018250C File Offset: 0x0018070C
	public void RunCalculateThread()
	{
		PlanetModelingManager.RequestCalcStar(this);
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x00182514 File Offset: 0x00180714
	public void Free()
	{
		for (int i = 0; i < this.planetCount; i++)
		{
			this.planets[i].Free();
		}
		this.planets = null;
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x00182546 File Offset: 0x00180746
	public void NotifyLoaded()
	{
		if (this.onLoaded != null)
		{
			this.onLoaded(this);
		}
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x0018255C File Offset: 0x0018075C
	public void NotifyOnDisplayNameChange()
	{
		this.RegeneratePlanetNames(true);
		if (this.onDisplayNameChange != null)
		{
			this.onDisplayNameChange(this);
		}
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x0018257C File Offset: 0x0018077C
	public void RegeneratePlanetNames(bool notifyChange)
	{
		PlanetData[] array = this.planets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].RegenerateName(notifyChange);
		}
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x001825A8 File Offset: 0x001807A8
	public string OrbitsDescString()
	{
		string text = "";
		for (int i = 1; i <= 12; i++)
		{
			int num = 0;
			for (int j = 0; j < this.planetCount; j++)
			{
				if (this.planets[j].orbitAround == 0 && this.planets[j].orbitIndex == i)
				{
					num = this.planets[j].number;
					break;
				}
			}
			if (this.asterBelt1OrbitIndex == (float)i)
			{
				text += "a";
			}
			else if (this.asterBelt2OrbitIndex == (float)i)
			{
				text += "b";
			}
			else
			{
				text += num.ToString();
			}
		}
		return text;
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x0018264F File Offset: 0x0018084F
	public override string ToString()
	{
		return "Star " + this.displayName;
	}

	// Token: 0x04001C74 RID: 7284
	public GalaxyData galaxy;

	// Token: 0x04001C75 RID: 7285
	public int seed;

	// Token: 0x04001C76 RID: 7286
	public int index;

	// Token: 0x04001C77 RID: 7287
	public int id;

	// Token: 0x04001C78 RID: 7288
	public string name = "";

	// Token: 0x04001C79 RID: 7289
	public string overrideName = "";

	// Token: 0x04001C7A RID: 7290
	public VectorLF3 position = VectorLF3.zero;

	// Token: 0x04001C7B RID: 7291
	public VectorLF3 uPosition;

	// Token: 0x04001C7C RID: 7292
	public float mass = 1f;

	// Token: 0x04001C7D RID: 7293
	public float lifetime = 50f;

	// Token: 0x04001C7E RID: 7294
	public float age;

	// Token: 0x04001C7F RID: 7295
	public EStarType type;

	// Token: 0x04001C80 RID: 7296
	public float temperature = 8500f;

	// Token: 0x04001C81 RID: 7297
	public ESpectrType spectr;

	// Token: 0x04001C82 RID: 7298
	public float classFactor;

	// Token: 0x04001C83 RID: 7299
	public float color;

	// Token: 0x04001C84 RID: 7300
	public float luminosity = 1f;

	// Token: 0x04001C85 RID: 7301
	public float radius = 1f;

	// Token: 0x04001C86 RID: 7302
	public float acdiskRadius;

	// Token: 0x04001C87 RID: 7303
	public float habitableRadius = 1f;

	// Token: 0x04001C88 RID: 7304
	public float lightBalanceRadius = 1f;

	// Token: 0x04001C89 RID: 7305
	public float dysonRadius = 10f;

	// Token: 0x04001C8A RID: 7306
	public float orbitScaler = 1f;

	// Token: 0x04001C8B RID: 7307
	public float asterBelt1OrbitIndex;

	// Token: 0x04001C8C RID: 7308
	public float asterBelt2OrbitIndex;

	// Token: 0x04001C8D RID: 7309
	public float asterBelt1Radius;

	// Token: 0x04001C8E RID: 7310
	public float asterBelt2Radius;

	// Token: 0x04001C8F RID: 7311
	public int planetCount;

	// Token: 0x04001C90 RID: 7312
	public float level;

	// Token: 0x04001C91 RID: 7313
	public float resourceCoef = 1f;

	// Token: 0x04001C92 RID: 7314
	public PlanetData[] planets;

	// Token: 0x04001C93 RID: 7315
	public float safetyFactor = 1f;

	// Token: 0x04001C94 RID: 7316
	public int hivePatternLevel;

	// Token: 0x04001C95 RID: 7317
	public int initialHiveCount;

	// Token: 0x04001C96 RID: 7318
	public int maxHiveCount;

	// Token: 0x04001C97 RID: 7319
	public AstroOrbitData[] hiveAstroOrbits;

	// Token: 0x04001C98 RID: 7320
	public const double kEnterDistance = 3600000.0;

	// Token: 0x04001C99 RID: 7321
	public const float kPhysicsRadiusRatio = 1200f;

	// Token: 0x04001C9A RID: 7322
	public const float kViewRadiusRatio = 800f;

	// Token: 0x04001C9B RID: 7323
	public const int kMaxDFHiveOrbit = 8;
}
