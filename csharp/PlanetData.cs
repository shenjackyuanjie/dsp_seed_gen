using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class PlanetData
{
	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001484 RID: 5252 RVA: 0x0015AC6B File Offset: 0x00158E6B
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

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001485 RID: 5253 RVA: 0x0015AC87 File Offset: 0x00158E87
	public float realRadius
	{
		get
		{
			return this.radius * this.scale;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06001486 RID: 5254 RVA: 0x0015AC96 File Offset: 0x00158E96
	public VeinGroup[] runtimeVeinGroups
	{
		get
		{
			if (this.factory == null)
			{
				return this.veinGroups;
			}
			return this.factory.veinGroups;
		}
	}

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06001487 RID: 5255 RVA: 0x0015ACB4 File Offset: 0x00158EB4
	// (remove) Token: 0x06001488 RID: 5256 RVA: 0x0015ACEC File Offset: 0x00158EEC
	public event Action<PlanetData> onLoaded;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06001489 RID: 5257 RVA: 0x0015AD24 File Offset: 0x00158F24
	// (remove) Token: 0x0600148A RID: 5258 RVA: 0x0015AD5C File Offset: 0x00158F5C
	public event Action<PlanetData> onFactoryLoaded;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x0600148B RID: 5259 RVA: 0x0015AD94 File Offset: 0x00158F94
	// (remove) Token: 0x0600148C RID: 5260 RVA: 0x0015ADCC File Offset: 0x00158FCC
	public event Action<PlanetData> onDisplayNameChange;

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600148D RID: 5261 RVA: 0x0015AE01 File Offset: 0x00159001
	public int astroId
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0015AE0C File Offset: 0x0015900C
	public bool CalcVeinAmounts(ref long[] veinAmounts, HashSet<int> hashes, int filterType)
	{
		if (veinAmounts == null)
		{
			veinAmounts = new long[64];
		}
		Array.Clear(veinAmounts, 0, veinAmounts.Length);
		hashes.Clear();
		VeinGroup[] runtimeVeinGroups = this.runtimeVeinGroups;
		if (filterType == 1)
		{
			if (this.factory != null)
			{
				MinerComponent[] minerPool = this.factory.factorySystem.minerPool;
				int minerCursor = this.factory.factorySystem.minerCursor;
				PowerConsumerComponent[] consumerPool = this.factory.powerSystem.consumerPool;
				VeinData[] veinPool = this.factory.veinPool;
				hashes.Clear();
				for (int i = 0; i < minerCursor; i++)
				{
					ref MinerComponent ptr = ref minerPool[i];
					if (ptr.id == i && ptr.type != EMinerType.Water && consumerPool[ptr.pcId].networkId > 0)
					{
						for (int j = 0; j < ptr.veinCount; j++)
						{
							int num = ptr.veins[j];
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
			if (runtimeVeinGroups != null)
			{
				Mutex obj = this.veinGroupsLock;
				lock (obj)
				{
					for (int k = 1; k < runtimeVeinGroups.Length; k++)
					{
						veinAmounts[(int)runtimeVeinGroups[k].type] += runtimeVeinGroups[k].amount;
					}
				}
			}
			if (this.factory != null)
			{
				MinerComponent[] minerPool2 = this.factory.factorySystem.minerPool;
				int minerCursor2 = this.factory.factorySystem.minerCursor;
				PowerConsumerComponent[] consumerPool2 = this.factory.powerSystem.consumerPool;
				VeinData[] veinPool2 = this.factory.veinPool;
				hashes.Clear();
				for (int l = 0; l < minerCursor2; l++)
				{
					ref MinerComponent ptr2 = ref minerPool2[l];
					if (ptr2.id == l && ptr2.type != EMinerType.Water && consumerPool2[ptr2.pcId].networkId > 0)
					{
						for (int m = 0; m < ptr2.veinCount; m++)
						{
							int num2 = ptr2.veins[m];
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
		else if (runtimeVeinGroups != null)
		{
			Mutex obj = this.veinGroupsLock;
			lock (obj)
			{
				for (int n = 1; n < runtimeVeinGroups.Length; n++)
				{
					veinAmounts[(int)runtimeVeinGroups[n].type] += runtimeVeinGroups[n].amount;
				}
			}
		}
		veinAmounts[0] = 0L;
		return runtimeVeinGroups != null;
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x0015B164 File Offset: 0x00159364
	public bool CalcVeinCounts(ref int[] veinCounts, HashSet<int> hashes, int filterType)
	{
		if (veinCounts == null)
		{
			veinCounts = new int[64];
		}
		Array.Clear(veinCounts, 0, veinCounts.Length);
		hashes.Clear();
		VeinGroup[] runtimeVeinGroups = this.runtimeVeinGroups;
		if (filterType == 1)
		{
			if (this.factory != null)
			{
				MinerComponent[] minerPool = this.factory.factorySystem.minerPool;
				int minerCursor = this.factory.factorySystem.minerCursor;
				PowerConsumerComponent[] consumerPool = this.factory.powerSystem.consumerPool;
				VeinData[] veinPool = this.factory.veinPool;
				hashes.Clear();
				for (int i = 0; i < minerCursor; i++)
				{
					ref MinerComponent ptr = ref minerPool[i];
					if (ptr.id == i && ptr.type != EMinerType.Water && consumerPool[ptr.pcId].networkId > 0)
					{
						for (int j = 0; j < ptr.veinCount; j++)
						{
							int num = ptr.veins[j];
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
			if (runtimeVeinGroups != null)
			{
				Mutex obj = this.veinGroupsLock;
				lock (obj)
				{
					for (int k = 1; k < runtimeVeinGroups.Length; k++)
					{
						veinCounts[(int)runtimeVeinGroups[k].type] += runtimeVeinGroups[k].count;
					}
				}
			}
			if (this.factory != null)
			{
				MinerComponent[] minerPool2 = this.factory.factorySystem.minerPool;
				int minerCursor2 = this.factory.factorySystem.minerCursor;
				PowerConsumerComponent[] consumerPool2 = this.factory.powerSystem.consumerPool;
				VeinData[] veinPool2 = this.factory.veinPool;
				hashes.Clear();
				for (int l = 0; l < minerCursor2; l++)
				{
					ref MinerComponent ptr2 = ref minerPool2[l];
					if (ptr2.id == l && ptr2.type != EMinerType.Water && consumerPool2[ptr2.pcId].networkId > 0)
					{
						for (int m = 0; m < ptr2.veinCount; m++)
						{
							int num2 = ptr2.veins[m];
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
		else if (runtimeVeinGroups != null)
		{
			Mutex obj = this.veinGroupsLock;
			lock (obj)
			{
				for (int n = 1; n < runtimeVeinGroups.Length; n++)
				{
					veinCounts[(int)runtimeVeinGroups[n].type] += runtimeVeinGroups[n].count;
				}
			}
		}
		veinCounts[0] = 0;
		return runtimeVeinGroups != null;
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001490 RID: 5264 RVA: 0x0015B498 File Offset: 0x00159698
	public string typeString
	{
		get
		{
			string result = "未知".Translate();
			ThemeProto themeProto = LDB.themes.Select(this.theme);
			if (themeProto != null)
			{
				result = themeProto.displayName;
			}
			return result;
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001491 RID: 5265 RVA: 0x0015B4CC File Offset: 0x001596CC
	public string briefString
	{
		get
		{
			string result = "";
			ThemeProto themeProto = LDB.themes.Select(this.theme);
			if (themeProto != null)
			{
				result = themeProto.BriefIntroduction;
			}
			return result;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001492 RID: 5266 RVA: 0x0015B4FC File Offset: 0x001596FC
	public string singularityString
	{
		get
		{
			string text = "";
			if (this.orbitAround > 0)
			{
				text += "卫星".Translate();
			}
			if ((this.singularity & EPlanetSingularity.TidalLocked) != EPlanetSingularity.None)
			{
				text += "潮汐锁定永昼永夜".Translate();
			}
			if ((this.singularity & EPlanetSingularity.TidalLocked2) != EPlanetSingularity.None)
			{
				text += "潮汐锁定1:2".Translate();
			}
			if ((this.singularity & EPlanetSingularity.TidalLocked4) != EPlanetSingularity.None)
			{
				text += "潮汐锁定1:4".Translate();
			}
			if ((this.singularity & EPlanetSingularity.LaySide) != EPlanetSingularity.None)
			{
				text += "横躺自转".Translate();
			}
			if ((this.singularity & EPlanetSingularity.ClockwiseRotate) != EPlanetSingularity.None)
			{
				text += "反向自转".Translate();
			}
			if ((this.singularity & EPlanetSingularity.MultipleSatellites) != EPlanetSingularity.None)
			{
				text += "多卫星".Translate();
			}
			return text;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001493 RID: 5267 RVA: 0x0015B5CE File Offset: 0x001597CE
	public float atmosphereHeight
	{
		get
		{
			if (!(this.atmosMaterial == null))
			{
				return this.atmosMaterial.GetVector("_PlanetRadius").z - this.realRadius;
			}
			return 0f;
		}
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0015B600 File Offset: 0x00159800
	public void Load()
	{
		PlanetModelingManager.RequestLoadPlanet(this);
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x0015B608 File Offset: 0x00159808
	public void LoadFactory()
	{
		this.bodyObject.SetActive(true);
		PlanetModelingManager.RequestLoadPlanetFactory(this);
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x0015B61C File Offset: 0x0015981C
	public void Unload()
	{
		if (!this.loading)
		{
			this.loaded = false;
			this.factoryLoaded = false;
			this.wanted = false;
			this.loading = false;
			this.factoryLoading = false;
			this.UnloadFactory();
			this.UnloadData();
			this.UnloadMeshes();
		}
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0015B65B File Offset: 0x0015985B
	public void RunCalculateThread()
	{
		PlanetModelingManager.RequestCalcPlanet(this);
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0015B664 File Offset: 0x00159864
	public void UnloadFactory()
	{
		this.factoryLoading = false;
		this.factoryLoaded = false;
		if (this.physics != null)
		{
			this.physics.Free();
			this.physics = null;
		}
		if (this.audio != null)
		{
			this.audio.Free();
			this.audio = null;
		}
		if (this.factoryModel != null)
		{
			this.factoryModel.Free();
			Object.Destroy(this.factoryModel.gameObject);
			this.factoryModel = null;
		}
		if (this.factoryAudio != null)
		{
			this.factoryAudio.Free();
			Object.Destroy(this.factoryAudio.gameObject);
			this.factoryAudio = null;
		}
		if (this.factory != null)
		{
			this.factory.UnloadDisplay();
		}
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0015B726 File Offset: 0x00159926
	public void Free()
	{
		this.loaded = false;
		this.factoryLoaded = false;
		this.wanted = false;
		this.loading = false;
		this.factoryLoading = false;
		this.UnloadFactory();
		this.UnloadData();
		this.UnloadMeshes();
		this.veinGroups = null;
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x0015B764 File Offset: 0x00159964
	public void NotifyLoaded()
	{
		this.loaded = true;
		this.loading = false;
		this.wanted = true;
		if (this.onLoaded != null)
		{
			this.onLoaded(this);
		}
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0015B78F File Offset: 0x0015998F
	public void NotifyFactoryLoaded()
	{
		this.factoryLoaded = true;
		this.factoryLoading = false;
		this.factingCompletedStage = -1;
		this.wanted = true;
		if (this.onFactoryLoaded != null)
		{
			this.onFactoryLoaded(this);
		}
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x0015B7C1 File Offset: 0x001599C1
	public void NotifyFactingStageComplete(int stage)
	{
		this.factingCompletedStage = stage;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x0015B7CA File Offset: 0x001599CA
	public void NotifyCalculated()
	{
		this.calculated = true;
		this.calculating = false;
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x0015B7DA File Offset: 0x001599DA
	public void NotifyOnDisplayNameChange()
	{
		if (this.onDisplayNameChange != null)
		{
			this.onDisplayNameChange(this);
		}
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x0015B7F0 File Offset: 0x001599F0
	public void RegenerateName(bool notifychange)
	{
		string str;
		if (this.star.planetCount <= 20)
		{
			str = NameGen.roman[this.index + 1];
		}
		else
		{
			str = (this.index + 1).ToString();
		}
		this.name = this.star.displayName + " " + str + "号星".Translate();
		if (notifychange && string.IsNullOrEmpty(this.overrideName))
		{
			this.NotifyOnDisplayNameChange();
		}
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x0015B86C File Offset: 0x00159A6C
	private void UnloadData()
	{
		if (this.data != null)
		{
			this.data.Free();
			this.data = null;
		}
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x0015B888 File Offset: 0x00159A88
	private void UnloadMeshes()
	{
		for (int i = 0; i < this.meshes.Length; i++)
		{
			if (this.meshes[i] != null)
			{
				Object.Destroy(this.meshes[i]);
				this.meshes[i] = null;
			}
		}
		if (this.gameObject != null)
		{
			Object.Destroy(this.gameObject);
			this.gameObject = null;
		}
		if (this.terrainMaterial != null)
		{
			Object.Destroy(this.terrainMaterial);
			this.terrainMaterial = null;
		}
		if (this.oceanMaterial != null)
		{
			Object.Destroy(this.oceanMaterial);
			this.oceanMaterial = null;
		}
		if (this.atmosMaterial != null)
		{
			Object.Destroy(this.atmosMaterial);
			this.atmosMaterial = null;
		}
		if (this.atmosMaterialLate != null)
		{
			Object.Destroy(this.atmosMaterialLate);
			this.atmosMaterialLate = null;
		}
		if (this.nephogramMaterial != null)
		{
			Object.Destroy(this.nephogramMaterial);
			this.nephogramMaterial = null;
		}
		if (this.cloudMaterial != null)
		{
			Object.Destroy(this.cloudMaterial);
			this.cloudMaterial = null;
		}
		if (this.minimapMaterial != null)
		{
			Object.Destroy(this.minimapMaterial);
			this.minimapMaterial = null;
		}
		if (this.reformMaterial0 != null)
		{
			Object.Destroy(this.reformMaterial0);
			this.reformMaterial0 = null;
		}
		if (this.reformMaterial1 != null)
		{
			Object.Destroy(this.reformMaterial1);
			this.reformMaterial1 = null;
		}
		if (this.heightmap != null)
		{
			this.heightmap.Release();
			Object.Destroy(this.heightmap);
			this.heightmap = null;
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x0015BA3C File Offset: 0x00159C3C
	public void CalculateVeinGroups()
	{
		if (this.data != null)
		{
			VeinData[] veinPool = this.data.veinPool;
			int veinCursor = this.data.veinCursor;
			int num = 0;
			for (int i = 1; i < veinCursor; i++)
			{
				int groupIndex = (int)veinPool[i].groupIndex;
				if (groupIndex > num)
				{
					num = groupIndex;
				}
			}
			Mutex obj = this.veinGroupsLock;
			lock (obj)
			{
				if (this.veinGroups == null || this.veinGroups.Length != num + 1)
				{
					this.veinGroups = new VeinGroup[num + 1];
				}
				Array.Clear(this.veinGroups, 0, this.veinGroups.Length);
				this.veinGroups[0].SetNull();
				for (int j = 1; j < veinCursor; j++)
				{
					if (veinPool[j].id == j)
					{
						int groupIndex2 = (int)veinPool[j].groupIndex;
						this.veinGroups[groupIndex2].type = veinPool[j].type;
						VeinGroup[] array = this.veinGroups;
						int num2 = groupIndex2;
						array[num2].pos = array[num2].pos + veinPool[j].pos;
						VeinGroup[] array2 = this.veinGroups;
						int num3 = groupIndex2;
						array2[num3].count = array2[num3].count + 1;
						VeinGroup[] array3 = this.veinGroups;
						int num4 = groupIndex2;
						array3[num4].amount = array3[num4].amount + (long)veinPool[j].amount;
					}
				}
				this.veinGroups[0].type = EVeinType.None;
				for (int k = 0; k < this.veinGroups.Length; k++)
				{
					this.veinGroups[k].pos.Normalize();
				}
			}
		}
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x0015BC18 File Offset: 0x00159E18
	public void GenVeinBiasVector()
	{
		if ((double)this.veinBiasVector.sqrMagnitude < 0.1)
		{
			DotNet35Random dotNet35Random = new DotNet35Random(this.seed);
			dotNet35Random.Next();
			dotNet35Random.Next();
			dotNet35Random.Next();
			dotNet35Random.Next();
			dotNet35Random.Next();
			if (this.galaxy != null && this.galaxy.birthPlanetId == this.id)
			{
				bool flag = !this.loaded && !this.loading;
				if (flag)
				{
					this.RegenerateRawDataImmediately();
				}
				this.veinBiasVector = this.birthPoint;
				this.veinBiasVector.Normalize();
				this.veinBiasVector *= 0.75f;
				if (flag)
				{
					this.UnloadData();
					return;
				}
			}
			else
			{
				DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random.Next());
				Vector3 a;
				a.x = (float)dotNet35Random2.NextDouble() * 2f - 1f;
				a.y = (float)dotNet35Random2.NextDouble() - 0.5f;
				a.z = (float)dotNet35Random2.NextDouble() * 2f - 1f;
				a.Normalize();
				a *= (float)(dotNet35Random2.NextDouble() * 0.4 + 0.2);
				this.veinBiasVector = a;
			}
		}
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0015BD64 File Offset: 0x00159F64
	public void GenBirthPoints()
	{
		DotNet35Random dotNet35Random = new DotNet35Random(this.seed);
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		dotNet35Random.Next();
		int birthSeed = dotNet35Random.Next();
		this.GenBirthPoints(this.data, birthSeed);
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x0015BDAC File Offset: 0x00159FAC
	public void GenBirthPoints(PlanetRawData rawData, int _birthSeed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(_birthSeed);
		Pose pose = this.PredictPose(85.0);
		Vector3 vector = Maths.QInvRotateLF(pose.rotation, this.star.uPosition - pose.position * 40000.0);
		vector.Normalize();
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, vector).normalized;
		int i = 0;
		int num = 256;
		while (i < num)
		{
			float d = (float)(dotNet35Random.NextDouble() * 2.0 - 1.0) * 0.5f;
			float d2 = (float)(dotNet35Random.NextDouble() * 2.0 - 1.0) * 0.5f;
			Vector3 vector2 = vector + d * normalized + d2 * normalized2;
			vector2.Normalize();
			this.birthPoint = vector2 * (this.realRadius + 0.2f + 1.45f);
			normalized = Vector3.Cross(vector2, Vector3.up).normalized;
			normalized2 = Vector3.Cross(normalized, vector2).normalized;
			bool flag = false;
			for (int j = 0; j < 10; j++)
			{
				float x = (float)(dotNet35Random.NextDouble() * 2.0 - 1.0);
				float y = (float)(dotNet35Random.NextDouble() * 2.0 - 1.0);
				Vector2 vector3 = new Vector2(x, y).normalized * 0.1f;
				Vector2 vector4 = -vector3;
				float num2 = (float)(dotNet35Random.NextDouble() * 2.0 - 1.0) * 0.06f;
				float num3 = (float)(dotNet35Random.NextDouble() * 2.0 - 1.0) * 0.06f;
				vector4.x += num2;
				vector4.y += num3;
				Vector3 normalized3 = (vector2 + vector3.x * normalized + vector3.y * normalized2).normalized;
				Vector3 normalized4 = (vector2 + vector4.x * normalized + vector4.y * normalized2).normalized;
				this.birthResourcePoint0 = normalized3.normalized;
				this.birthResourcePoint1 = normalized4.normalized;
				float num4 = this.realRadius + 0.2f;
				if (rawData.QueryHeight(vector2) > num4 && rawData.QueryHeight(normalized3) > num4 && rawData.QueryHeight(normalized4) > num4)
				{
					Vector3 vpos = normalized3 + normalized * 0.03f;
					Vector3 vpos2 = normalized3 - normalized * 0.03f;
					Vector3 vpos3 = normalized3 + normalized2 * 0.03f;
					Vector3 vpos4 = normalized3 - normalized2 * 0.03f;
					Vector3 vpos5 = normalized4 + normalized * 0.03f;
					Vector3 vpos6 = normalized4 - normalized * 0.03f;
					Vector3 vpos7 = normalized4 + normalized2 * 0.03f;
					Vector3 vpos8 = normalized4 - normalized2 * 0.03f;
					if (rawData.QueryHeight(vpos) > num4 && rawData.QueryHeight(vpos2) > num4 && rawData.QueryHeight(vpos3) > num4 && rawData.QueryHeight(vpos4) > num4 && rawData.QueryHeight(vpos5) > num4 && rawData.QueryHeight(vpos6) > num4 && rawData.QueryHeight(vpos7) > num4 && rawData.QueryHeight(vpos8) > num4)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
			i++;
		}
		if (i >= num)
		{
			this.birthPoint = new Vector3(0f, this.realRadius + 5f, 0f);
		}
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x0015C1D4 File Offset: 0x0015A3D4
	public void UpdateRuntimePose(double time)
	{
		double num = time / this.orbitalPeriod + (double)this.orbitPhase / 360.0;
		int num2 = (int)(num + 0.1);
		num -= (double)num2;
		this.runtimeOrbitPhase = (float)num * 360f;
		num *= 6.283185307179586;
		double num3 = time / this.rotationPeriod + (double)this.rotationPhase / 360.0;
		int num4 = (int)(num3 + 0.1);
		num3 = (num3 - (double)num4) * 360.0;
		this.runtimeRotationPhase = (float)num3;
		VectorLF3 vectorLF = Maths.QRotateLF(this.runtimeOrbitRotation, new VectorLF3((float)Math.Cos(num) * this.orbitRadius, 0f, (float)Math.Sin(num) * this.orbitRadius));
		if (this.orbitAroundPlanet != null)
		{
			vectorLF.x += this.orbitAroundPlanet.runtimePosition.x;
			vectorLF.y += this.orbitAroundPlanet.runtimePosition.y;
			vectorLF.z += this.orbitAroundPlanet.runtimePosition.z;
		}
		this.runtimePosition = vectorLF;
		this.runtimeRotation = this.runtimeSystemRotation * Quaternion.AngleAxis((float)num3, Vector3.down);
		this.uPosition.x = this.star.uPosition.x + vectorLF.x * 40000.0;
		this.uPosition.y = this.star.uPosition.y + vectorLF.y * 40000.0;
		this.uPosition.z = this.star.uPosition.z + vectorLF.z * 40000.0;
		this.runtimeLocalSunDirection = Maths.QInvRotate(this.runtimeRotation, -vectorLF);
		double num5 = time + 0.016666666666666666;
		double num6 = num5 / this.orbitalPeriod + (double)this.orbitPhase / 360.0;
		int num7 = (int)(num6 + 0.1);
		num6 -= (double)num7;
		num6 *= 6.283185307179586;
		double num8 = num5 / this.rotationPeriod + (double)this.rotationPhase / 360.0;
		int num9 = (int)(num8 + 0.1);
		num8 = (num8 - (double)num9) * 360.0;
		VectorLF3 vectorLF2 = Maths.QRotateLF(this.runtimeOrbitRotation, new VectorLF3((float)Math.Cos(num6) * this.orbitRadius, 0f, (float)Math.Sin(num6) * this.orbitRadius));
		if (this.orbitAroundPlanet != null)
		{
			vectorLF2.x += this.orbitAroundPlanet.runtimePositionNext.x;
			vectorLF2.y += this.orbitAroundPlanet.runtimePositionNext.y;
			vectorLF2.z += this.orbitAroundPlanet.runtimePositionNext.z;
		}
		this.runtimePositionNext = vectorLF2;
		this.runtimeRotationNext = this.runtimeSystemRotation * Quaternion.AngleAxis((float)num8, Vector3.down);
		this.uPositionNext.x = this.star.uPosition.x + vectorLF2.x * 40000.0;
		this.uPositionNext.y = this.star.uPosition.y + vectorLF2.y * 40000.0;
		this.uPositionNext.z = this.star.uPosition.z + vectorLF2.z * 40000.0;
		this.galaxy.astrosData[this.id].uPos = this.uPosition;
		this.galaxy.astrosData[this.id].uRot = this.runtimeRotation;
		this.galaxy.astrosData[this.id].uPosNext = this.uPositionNext;
		this.galaxy.astrosData[this.id].uRotNext = this.runtimeRotationNext;
		this.galaxy.astrosFactory[this.id] = this.factory;
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x0015C634 File Offset: 0x0015A834
	public Pose PredictPose(double time)
	{
		double num = time / this.orbitalPeriod + (double)this.orbitPhase / 360.0;
		int num2 = (int)(num + 0.1);
		num -= (double)num2;
		num *= 6.283185307179586;
		double num3 = time / this.rotationPeriod + (double)this.rotationPhase / 360.0;
		int num4 = (int)(num3 + 0.1);
		num3 = (num3 - (double)num4) * 360.0;
		Vector3 vector = new Vector3((float)Math.Cos(num) * this.orbitRadius, 0f, (float)Math.Sin(num) * this.orbitRadius);
		vector = Maths.QRotate(this.runtimeOrbitRotation, vector);
		if (this.orbitAroundPlanet != null)
		{
			Pose pose = this.orbitAroundPlanet.PredictPose(time);
			vector.x += pose.position.x;
			vector.y += pose.position.y;
			vector.z += pose.position.z;
		}
		return new Pose(vector, this.runtimeSystemRotation * Quaternion.AngleAxis((float)num3, Vector3.down));
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x0015C770 File Offset: 0x0015A970
	public void PredictUPose(double time, out VectorLF3 uPos, out Quaternion uRot)
	{
		Pose pose = this.PredictPose(time);
		uPos.x = (double)pose.position.x * 40000.0 + this.star.uPosition.x;
		uPos.y = (double)pose.position.y * 40000.0 + this.star.uPosition.y;
		uPos.z = (double)pose.position.z * 40000.0 + this.star.uPosition.z;
		uRot = pose.rotation;
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x0015C818 File Offset: 0x0015AA18
	public VectorLF3 GetUniversalVelocityAtLocalPoint(double time, Vector3 lpoint)
	{
		double time2 = time + 0.016666666666666666;
		VectorLF3 lhs;
		Quaternion rotation;
		this.PredictUPose(time, out lhs, out rotation);
		VectorLF3 lhs2;
		Quaternion rotation2;
		this.PredictUPose(time2, out lhs2, out rotation2);
		VectorLF3 rhs = lhs + rotation * lpoint;
		return (lhs2 + rotation2 * lpoint - rhs) / 0.016666666666666666;
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0015C888 File Offset: 0x0015AA88
	private void PredictLocalGeography(Vector3 local, double time, out float sunMaxAngle, out float sunMinAngle, out float sunAngle)
	{
		Pose pose = this.PredictPose(time);
		float num = 90f - Vector3.Angle(pose.rotation * Vector3.up, -pose.position);
		float num2 = 90f - Vector3.Angle(Vector3.up, local);
		sunMaxAngle = 90f - Mathf.Abs(num - num2);
		sunMinAngle = 90f - Mathf.Abs(num - (180f - num2));
		if (sunMinAngle < -180f)
		{
			sunMinAngle += 360f;
		}
		if (sunMinAngle > 90f)
		{
			sunMinAngle = 180f - sunMinAngle;
		}
		if (sunMinAngle < -90f)
		{
			sunMinAngle = -180f - sunMinAngle;
		}
		sunAngle = 90f - Vector3.Angle(pose.rotation * local, -pose.position);
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x0015C964 File Offset: 0x0015AB64
	public void GetLocalGeography(Vector3 local, double time, out int summerWinter, out bool tropical, out bool polar, out float sunMaxAngle, out float sunMinAngle, out int dayNight, out float remainTime)
	{
		float num = 90f - Vector3.Angle(this.runtimeRotation * Vector3.up, -this.runtimePosition);
		float num2 = 90f - Vector3.Angle(Vector3.up, local);
		sunMaxAngle = 90f - Mathf.Abs(num - num2);
		sunMinAngle = 90f - Mathf.Abs(num - (180f - num2));
		if (sunMinAngle < -180f)
		{
			sunMinAngle += 360f;
		}
		if (sunMinAngle > 90f)
		{
			sunMinAngle = 180f - sunMinAngle;
		}
		if (sunMinAngle < -90f)
		{
			sunMinAngle = -180f - sunMinAngle;
		}
		float num3 = 90f - Vector3.Angle(this.runtimeRotation * local, -this.runtimePosition);
		dayNight = ((num3 >= 0f) ? 1 : -1);
		remainTime = 10000000f;
		float num4 = Vector3.Angle(this.runtimeRotation * Vector3.up, Vector3.up);
		summerWinter = ((num * num2 >= 0f) ? 1 : -1);
		tropical = ((Mathf.Abs(num2) < num4 && sunMaxAngle > 70f) || sunMaxAngle > 85f);
		polar = (sunMaxAngle < 0f || sunMinAngle > 0f);
		bool flag = false;
		int num5 = 0;
		double num6 = (this.orbitAround == 0) ? this.orbitalPeriod : this.orbitAroundPlanet.orbitalPeriod;
		double num7 = this.rotationPeriod;
		bool flag2 = Math.Abs(this.rotationPeriod - num6) < 1.0;
		if (!flag2)
		{
			num7 = 1.0 / (1.0 / this.rotationPeriod - 1.0 / num6);
		}
		num7 = Math.Abs(num7);
		if (dayNight >= 0)
		{
			float num8 = sunMaxAngle;
			float num9 = sunMinAngle;
			float num10 = num3;
			double num11 = time;
			double num12 = time + num7 * 0.8;
			double num13 = time + num7 * 0.4;
			double num15;
			do
			{
				if (num9 > 0f)
				{
					double num14 = (this.orbitAroundPlanet != null) ? this.orbitAroundPlanet.orbitalPeriod : this.orbitalPeriod;
					num11 = time;
					num12 = time + num14 * 0.8;
					for (;;)
					{
						num13 = (num11 + num12) * 0.5;
						this.PredictLocalGeography(local, num13, out num8, out num9, out num10);
						if (num9 > 0f)
						{
							num11 = num13;
						}
						else
						{
							num12 = num13;
						}
						if (num12 - num11 < 1.0)
						{
							break;
						}
						if (num5++ >= 100)
						{
							goto IL_2A5;
						}
					}
					num11 = num13;
					num12 = num13 + num7 * 0.8;
				}
				for (;;)
				{
					IL_2A5:
					num13 = (num11 + num12) * 0.5;
					this.PredictLocalGeography(local, num13, out num8, out num9, out num10);
					if (num10 > 0f)
					{
						num11 = num13;
					}
					else
					{
						num12 = num13;
					}
					if (num12 - num11 < 0.10000000149011612)
					{
						break;
					}
					if (num5++ >= 100)
					{
						goto IL_49A;
					}
				}
				num15 = num11 - time;
				if (num15 <= num7 * 0.8 - 0.10999999940395355 || flag)
				{
					break;
				}
				flag = true;
			}
			while (num5++ < 100);
			remainTime = (float)num15;
		}
		else
		{
			float num16 = sunMaxAngle;
			float num17 = sunMinAngle;
			float num18 = num3;
			double num19 = time;
			double num20 = time + num7 * 0.8;
			double num21 = time + num7 * 0.4;
			double num23;
			do
			{
				if (num16 < 0f)
				{
					double num22 = (this.orbitAroundPlanet != null) ? this.orbitAroundPlanet.orbitalPeriod : this.orbitalPeriod;
					num19 = time;
					num20 = time + num22 * 0.8;
					for (;;)
					{
						num21 = (num19 + num20) * 0.5;
						this.PredictLocalGeography(local, num21, out num16, out num17, out num18);
						if (num16 < 0f)
						{
							num19 = num21;
						}
						else
						{
							num20 = num21;
						}
						if (num20 - num19 < 1.0)
						{
							break;
						}
						if (num5++ >= 100)
						{
							goto IL_40C;
						}
					}
					num19 = num21;
					num20 = num21 + num7 * 0.8;
				}
				for (;;)
				{
					IL_40C:
					num21 = (num19 + num20) * 0.5;
					this.PredictLocalGeography(local, num21, out num16, out num17, out num18);
					if (num18 < 0f)
					{
						num19 = num21;
					}
					else
					{
						num20 = num21;
					}
					if (num20 - num19 < 0.10000000149011612)
					{
						break;
					}
					if (num5++ >= 100)
					{
						goto IL_49A;
					}
				}
				num23 = num19 - time;
				if (num23 <= num7 * 0.8 - 0.10999999940395355 || flag)
				{
					break;
				}
				flag = true;
			}
			while (num5++ < 100);
			remainTime = (float)num23;
		}
		IL_49A:
		if (flag2)
		{
			remainTime = 10000000f;
		}
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0015CE18 File Offset: 0x0015B018
	public void AddHeightMapModLevel(int index, int level)
	{
		if (this.data.AddModLevel(index, level))
		{
			int num = this.precision / this.segment;
			int num2 = index % this.data.stride;
			int num3 = index / this.data.stride;
			int num4 = ((num2 < this.data.substride) ? 0 : 1) + ((num3 < this.data.substride) ? 0 : 2);
			int num5 = num2 % this.data.substride;
			int num6 = num3 % this.data.substride;
			int num7 = (num5 - 1) / num;
			int num8 = (num6 - 1) / num;
			int num9 = num5 / num;
			int num10 = num6 / num;
			if (num9 >= this.segment)
			{
				num9 = this.segment - 1;
			}
			if (num10 >= this.segment)
			{
				num10 = this.segment - 1;
			}
			int num11 = num4 * this.segment * this.segment;
			int num12 = num7 + num8 * this.segment + num11;
			int num13 = num9 + num8 * this.segment + num11;
			int num14 = num7 + num10 * this.segment + num11;
			int num15 = num9 + num10 * this.segment + num11;
			this.dirtyFlags[num12] = true;
			this.dirtyFlags[num13] = true;
			this.dirtyFlags[num14] = true;
			this.dirtyFlags[num15] = true;
		}
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0015CF5C File Offset: 0x0015B15C
	public bool UpdateDirtyMeshes()
	{
		bool result = false;
		for (int i = 0; i < this.dirtyFlags.Length; i++)
		{
			if (this.UpdateDirtyMesh(i))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0015CF8C File Offset: 0x0015B18C
	public bool UpdateDirtyMesh(int dirtyIdx)
	{
		if (this.dirtyFlags[dirtyIdx])
		{
			this.dirtyFlags[dirtyIdx] = false;
			int num = this.precision / this.segment;
			int num2 = this.segment * this.segment;
			int num3 = dirtyIdx / num2;
			int num4 = num3 % 2;
			int num5 = num3 / 2;
			int num6 = dirtyIdx % num2;
			int num7 = num6 % this.segment * num + num4 * this.data.substride;
			int num8 = num6 / this.segment * num + num5 * this.data.substride;
			int stride = this.data.stride;
			float num9 = this.radius * this.scale + 0.2f;
			Mesh mesh = this.meshes[dirtyIdx];
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			int num10 = 0;
			for (int i = num8; i <= num8 + num; i++)
			{
				for (int j = num7; j <= num7 + num; j++)
				{
					int num11 = j + i * stride;
					float num12 = (float)this.data.heightData[num11] * 0.01f * this.scale;
					float num13 = (float)this.data.GetModLevel(num11) * 0.3333333f;
					float num14 = num9;
					if (num13 > 0f)
					{
						num14 = (float)this.data.GetModPlane(num11) * 0.01f * this.scale;
					}
					float num15 = num12 * (1f - num13) + num14 * num13;
					vertices[num10].x = this.data.vertices[num11].x * num15;
					vertices[num10].y = this.data.vertices[num11].y * num15;
					vertices[num10].z = this.data.vertices[num11].z * num15;
					normals[num10].x = this.data.normals[num11].x * (1f - num13) + this.data.vertices[num11].x * num13;
					normals[num10].y = this.data.normals[num11].y * (1f - num13) + this.data.vertices[num11].y * num13;
					normals[num10].z = this.data.normals[num11].z * (1f - num13) + this.data.vertices[num11].z * num13;
					normals[num10].Normalize();
					num10++;
				}
			}
			mesh.vertices = vertices;
			mesh.normals = normals;
			this.meshColliders[dirtyIdx].sharedMesh = null;
			this.meshColliders[dirtyIdx].sharedMesh = mesh;
			return true;
		}
		return false;
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x0015D290 File Offset: 0x0015B490
	public void RegenerateRawDataTerrainOnlyImmediately()
	{
		PlanetAlgorithm planetAlgorithm = PlanetModelingManager.Algorithm(this);
		this.data = new PlanetRawData(this.precision);
		this.modData = this.data.InitModData(this.modData);
		this.data.CalcVerts();
		this.aux = new PlanetAuxData(this);
		planetAlgorithm.GenerateTerrain(this.mod_x, this.mod_y);
		this.GenBirthPoints();
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x0015D2FC File Offset: 0x0015B4FC
	public void RegenerateRawDataImmediately()
	{
		PlanetAlgorithm planetAlgorithm = PlanetModelingManager.Algorithm(this);
		this.data = new PlanetRawData(this.precision);
		this.modData = this.data.InitModData(this.modData);
		this.data.CalcVerts();
		this.aux = new PlanetAuxData(this);
		planetAlgorithm.GenerateTerrain(this.mod_x, this.mod_y);
		planetAlgorithm.CalcWaterPercent();
		this.data.vegeCursor = 1;
		if (this.type != EPlanetType.Gas)
		{
			planetAlgorithm.GenerateVegetables();
		}
		this.data.veinCursor = 1;
		if (this.type != EPlanetType.Gas)
		{
			planetAlgorithm.GenerateVeins();
		}
		this.CalculateVeinGroups();
		this.GenBirthPoints();
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x0015D3A9 File Offset: 0x0015B5A9
	public void RegenerateVegetations()
	{
		this.data.vegeCursor = 1;
		PlanetModelingManager.Algorithm(this).GenerateVegetables();
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x0015D3C2 File Offset: 0x0015B5C2
	public void ExportRuntime(BinaryWriter w)
	{
		w.Write(this.modData.Length);
		w.Write(this.modData);
		w.Write(0);
		w.Write(0);
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x0015D3EC File Offset: 0x0015B5EC
	public void ImportRuntime(BinaryReader r)
	{
		int count = r.ReadInt32();
		this.modData = r.ReadBytes(count);
		int num = r.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			r.ReadInt64();
		}
		int num2 = r.ReadInt32();
		for (int j = 0; j < num2; j++)
		{
			r.ReadInt32();
			r.ReadSingle();
			r.ReadSingle();
			r.ReadSingle();
			r.ReadInt32();
			r.ReadInt64();
		}
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x0015D468 File Offset: 0x0015B668
	public override string ToString()
	{
		return "Planet " + this.displayName;
	}

	// Token: 0x0400195D RID: 6493
	public GalaxyData galaxy;

	// Token: 0x0400195E RID: 6494
	public StarData star;

	// Token: 0x0400195F RID: 6495
	public int seed;

	// Token: 0x04001960 RID: 6496
	public int infoSeed;

	// Token: 0x04001961 RID: 6497
	public int id;

	// Token: 0x04001962 RID: 6498
	public int index;

	// Token: 0x04001963 RID: 6499
	public int orbitAround;

	// Token: 0x04001964 RID: 6500
	public int number;

	// Token: 0x04001965 RID: 6501
	public int orbitIndex;

	// Token: 0x04001966 RID: 6502
	public string name = "";

	// Token: 0x04001967 RID: 6503
	public string overrideName = "";

	// Token: 0x04001968 RID: 6504
	public float orbitRadius = 1f;

	// Token: 0x04001969 RID: 6505
	public float orbitInclination;

	// Token: 0x0400196A RID: 6506
	public float orbitLongitude;

	// Token: 0x0400196B RID: 6507
	public double orbitalPeriod = 3600.0;

	// Token: 0x0400196C RID: 6508
	public float orbitPhase;

	// Token: 0x0400196D RID: 6509
	public float obliquity;

	// Token: 0x0400196E RID: 6510
	public double rotationPeriod = 480.0;

	// Token: 0x0400196F RID: 6511
	public float rotationPhase;

	// Token: 0x04001970 RID: 6512
	public float radius = 200f;

	// Token: 0x04001971 RID: 6513
	public float scale = 1f;

	// Token: 0x04001972 RID: 6514
	public float sunDistance;

	// Token: 0x04001973 RID: 6515
	public float habitableBias;

	// Token: 0x04001974 RID: 6516
	public float temperatureBias;

	// Token: 0x04001975 RID: 6517
	public float ionHeight;

	// Token: 0x04001976 RID: 6518
	public float windStrength;

	// Token: 0x04001977 RID: 6519
	public float luminosity;

	// Token: 0x04001978 RID: 6520
	public float landPercent;

	// Token: 0x04001979 RID: 6521
	public double mod_x;

	// Token: 0x0400197A RID: 6522
	public double mod_y;

	// Token: 0x0400197B RID: 6523
	public float waterHeight;

	// Token: 0x0400197C RID: 6524
	public int waterItemId;

	// Token: 0x0400197D RID: 6525
	public bool levelized;

	// Token: 0x0400197E RID: 6526
	public int iceFlag;

	// Token: 0x0400197F RID: 6527
	public EPlanetType type;

	// Token: 0x04001980 RID: 6528
	public EPlanetSingularity singularity;

	// Token: 0x04001981 RID: 6529
	public int theme;

	// Token: 0x04001982 RID: 6530
	public int algoId;

	// Token: 0x04001983 RID: 6531
	public int style;

	// Token: 0x04001984 RID: 6532
	public PlanetData orbitAroundPlanet;

	// Token: 0x04001985 RID: 6533
	public VectorLF3 runtimePosition;

	// Token: 0x04001986 RID: 6534
	public VectorLF3 runtimePositionNext;

	// Token: 0x04001987 RID: 6535
	public Quaternion runtimeRotation;

	// Token: 0x04001988 RID: 6536
	public Quaternion runtimeRotationNext;

	// Token: 0x04001989 RID: 6537
	public Quaternion runtimeSystemRotation;

	// Token: 0x0400198A RID: 6538
	public Quaternion runtimeOrbitRotation;

	// Token: 0x0400198B RID: 6539
	public float runtimeOrbitPhase;

	// Token: 0x0400198C RID: 6540
	public float runtimeRotationPhase;

	// Token: 0x0400198D RID: 6541
	public VectorLF3 uPosition;

	// Token: 0x0400198E RID: 6542
	public VectorLF3 uPositionNext;

	// Token: 0x0400198F RID: 6543
	public Vector3 runtimeLocalSunDirection;

	// Token: 0x04001990 RID: 6544
	public byte[] modData;

	// Token: 0x04001991 RID: 6545
	public int precision = 160;

	// Token: 0x04001992 RID: 6546
	public int segment = 5;

	// Token: 0x04001993 RID: 6547
	public PlanetRawData data;

	// Token: 0x04001994 RID: 6548
	public Mutex veinGroupsLock = new Mutex();

	// Token: 0x04001995 RID: 6549
	public VeinGroup[] veinGroups;

	// Token: 0x04001996 RID: 6550
	public Vector3 veinBiasVector;

	// Token: 0x04001997 RID: 6551
	public const int kMaxMeshCnt = 100;

	// Token: 0x04001998 RID: 6552
	public GameObject gameObject;

	// Token: 0x04001999 RID: 6553
	public GameObject bodyObject;

	// Token: 0x0400199A RID: 6554
	public Material terrainMaterial;

	// Token: 0x0400199B RID: 6555
	public Material oceanMaterial;

	// Token: 0x0400199C RID: 6556
	public Material atmosMaterial;

	// Token: 0x0400199D RID: 6557
	public Material atmosMaterialLate;

	// Token: 0x0400199E RID: 6558
	public Material nephogramMaterial;

	// Token: 0x0400199F RID: 6559
	public Material cloudMaterial;

	// Token: 0x040019A0 RID: 6560
	public Material minimapMaterial;

	// Token: 0x040019A1 RID: 6561
	public Material reformMaterial0;

	// Token: 0x040019A2 RID: 6562
	public Material reformMaterial1;

	// Token: 0x040019A3 RID: 6563
	public RenderTexture heightmap;

	// Token: 0x040019A4 RID: 6564
	public AmbientDesc ambientDesc;

	// Token: 0x040019A5 RID: 6565
	public Color groundScreenColor;

	// Token: 0x040019A6 RID: 6566
	public AudioClip ambientSfx;

	// Token: 0x040019A7 RID: 6567
	public float ambientSfxVolume;

	// Token: 0x040019A8 RID: 6568
	public Mesh[] meshes = new Mesh[100];

	// Token: 0x040019A9 RID: 6569
	public MeshRenderer[] meshRenderers = new MeshRenderer[100];

	// Token: 0x040019AA RID: 6570
	public MeshCollider[] meshColliders = new MeshCollider[100];

	// Token: 0x040019AB RID: 6571
	public bool[] dirtyFlags = new bool[100];

	// Token: 0x040019AC RID: 6572
	public bool landPercentDirty;

	// Token: 0x040019AD RID: 6573
	public int factoryIndex = -1;

	// Token: 0x040019AE RID: 6574
	public PlanetFactory factory;

	// Token: 0x040019AF RID: 6575
	public PlanetPhysics physics;

	// Token: 0x040019B0 RID: 6576
	public PlanetAudio audio;

	// Token: 0x040019B1 RID: 6577
	public FactoryModel factoryModel;

	// Token: 0x040019B2 RID: 6578
	public FactoryAudio factoryAudio;

	// Token: 0x040019B3 RID: 6579
	public PlanetAuxData aux;

	// Token: 0x040019B4 RID: 6580
	public int[] gasItems;

	// Token: 0x040019B5 RID: 6581
	public float[] gasSpeeds;

	// Token: 0x040019B6 RID: 6582
	public float[] gasHeatValues;

	// Token: 0x040019B7 RID: 6583
	public double gasTotalHeat;

	// Token: 0x040019B8 RID: 6584
	public Vector3 birthPoint;

	// Token: 0x040019B9 RID: 6585
	public Vector3 birthResourcePoint0;

	// Token: 0x040019BA RID: 6586
	public Vector3 birthResourcePoint1;

	// Token: 0x040019BB RID: 6587
	public bool loaded;

	// Token: 0x040019BC RID: 6588
	public bool wanted;

	// Token: 0x040019BD RID: 6589
	public bool loading;

	// Token: 0x040019BE RID: 6590
	public bool calculating;

	// Token: 0x040019BF RID: 6591
	public bool calculated;

	// Token: 0x040019C0 RID: 6592
	public bool factoryLoaded;

	// Token: 0x040019C1 RID: 6593
	public bool factoryLoading;

	// Token: 0x040019C2 RID: 6594
	public int factingCompletedStage = -1;

	// Token: 0x040019C6 RID: 6598
	public const float kEnterAltitude = 1000f;

	// Token: 0x040019C7 RID: 6599
	public const float kBirthHeightShift = 1.45f;
}