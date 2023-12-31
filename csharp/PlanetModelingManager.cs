using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200057D RID: 1405
public static class PlanetModelingManager
{
	// Token: 0x06003ACE RID: 15054 RVA: 0x0031D401 File Offset: 0x0031B601
	public static void RequestLoadPlanetFactory(PlanetData planet)
	{
		if (planet.factoryLoaded)
		{
			return;
		}
		if (planet.factoryLoading)
		{
			return;
		}
		PlanetModelingManager.fctPlanetReqList.Enqueue(planet);
	}

	// Token: 0x06003ACF RID: 15055 RVA: 0x0031D420 File Offset: 0x0031B620
	public static void LoadingPlanetFactoryCoroutine()
	{
		if (PlanetModelingManager.currentFactingPlanet == null)
		{
			PlanetData planetData = null;
			Queue<PlanetData> obj = PlanetModelingManager.fctPlanetReqList;
			lock (obj)
			{
				if (PlanetModelingManager.fctPlanetReqList.Count > 0)
				{
					planetData = PlanetModelingManager.fctPlanetReqList.Dequeue();
				}
			}
			if (planetData != null)
			{
				PlanetModelingManager.currentFactingPlanet = planetData;
				PlanetModelingManager.currentFactingStage = 0;
			}
		}
		if (PlanetModelingManager.currentFactingPlanet != null)
		{
			try
			{
				PlanetModelingManager.LoadingPlanetFactoryMain(PlanetModelingManager.currentFactingPlanet);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				PlanetModelingManager.currentFactingPlanet.UnloadFactory();
				PlanetModelingManager.currentFactingPlanet.factoryLoaded = false;
				PlanetModelingManager.currentFactingPlanet = null;
				PlanetModelingManager.currentFactingStage = 0;
			}
		}
	}

	// Token: 0x06003AD0 RID: 15056 RVA: 0x0031D4D0 File Offset: 0x0031B6D0
	private static void LoadingPlanetFactoryMain(PlanetData planet)
	{
		planet.factoryLoading = true;
		if (PlanetModelingManager.currentFactingStage > PlanetModelingManager.Debug_LoadingPlanetFactoryMain_MaxStage)
		{
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 0)
		{
			Debug.Log("Loading [" + planet.displayName + "] factory");
			if (planet.factory == null)
			{
				GameMain.data.GetOrCreateFactory(planet);
			}
			if (planet.physics == null)
			{
				planet.physics = new PlanetPhysics(planet);
				planet.physics.Init();
			}
			if (planet.audio == null)
			{
				planet.audio = new PlanetAudio(planet);
				planet.audio.Init();
			}
			if (planet.factoryModel == null)
			{
				GameObject gameObject = new GameObject("Factory Model");
				gameObject.transform.SetParent(planet.gameObject.transform, false);
				planet.factoryModel = gameObject.AddComponent<FactoryModel>();
				planet.factoryModel.planet = planet;
				planet.factoryModel.Init();
			}
			if (planet.factoryAudio == null)
			{
				GameObject gameObject2 = new GameObject("Factory Audio");
				gameObject2.transform.SetParent(planet.gameObject.transform, false);
				planet.factoryAudio = gameObject2.AddComponent<FactoryAudio>();
				planet.factoryAudio.planet = planet;
				planet.factoryAudio.Init();
			}
			Debug.Log("step 0 complete");
			planet.NotifyFactingStageComplete(0);
			PlanetModelingManager.currentFactingStage = 1;
			return;
		}
		PlanetFactory factory = planet.factory;
		PlanetPhysics physics = planet.physics;
		FactoryModel factoryModel = planet.factoryModel;
		ItemProtoSet items = LDB.items;
		if (PlanetModelingManager.currentFactingStage == 1)
		{
			VegeData[] vegePool = factory.vegePool;
			Debug.Log("vege count = " + (factory.vegeCursor - 1).ToString());
			for (int i = 1; i < factory.vegeCursor; i++)
			{
				if (vegePool[i].id == i)
				{
					VegeProto vegeProto = PlanetModelingManager.vegeProtos[(int)vegePool[i].protoId];
					if (vegeProto != null)
					{
						vegePool[i].modelId = 0;
						vegePool[i].colliderId = 0;
						vegePool[i].modelId = factoryModel.gpuiManager.AddModel((int)vegePool[i].modelIndex, i, vegePool[i].pos, vegePool[i].rot, false);
						ColliderData[] colliders = vegeProto.prefabDesc.colliders;
						int num = 0;
						while (colliders != null && num < colliders.Length)
						{
							vegePool[i].colliderId = physics.AddColliderData(colliders[num].BindToObject(i, vegePool[i].colliderId, EObjectType.Vegetable, vegePool[i].pos, vegePool[i].rot, vegePool[i].scl));
							num++;
						}
					}
				}
			}
			Debug.Log("step 1 complete : vegetation model & physics");
			planet.NotifyFactingStageComplete(1);
			PlanetModelingManager.currentFactingStage = 2;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 2)
		{
			VeinData[] veinPool = factory.veinPool;
			AnimData[] veinAnimPool = factory.veinAnimPool;
			Debug.Log("vein count = " + (factory.veinCursor - 1).ToString());
			for (int j = 1; j < factory.veinCursor; j++)
			{
				if (veinPool[j].id == j)
				{
					if (veinPool[j].amount <= 0)
					{
						factory.RemoveVeinData(j);
					}
					else
					{
						VeinProto veinProto = PlanetModelingManager.veinProtos[(int)veinPool[j].type];
						if (veinProto != null)
						{
							veinPool[j].modelId = 0;
							veinPool[j].colliderId = 0;
							veinPool[j].minerBaseModelId = 0;
							veinPool[j].minerCircleModelId0 = 0;
							veinPool[j].minerCircleModelId1 = 0;
							veinPool[j].minerCircleModelId2 = 0;
							veinPool[j].minerCircleModelId3 = 0;
							veinAnimPool[j].time = ((veinPool[j].amount >= 20000) ? 0f : (1f - (float)veinPool[j].amount * 5E-05f));
							veinAnimPool[j].prepare_length = 0f;
							veinAnimPool[j].working_length = 1f;
							veinAnimPool[j].state = (uint)veinPool[j].type;
							veinAnimPool[j].power = 0f;
							veinPool[j].modelId = factoryModel.gpuiManager.AddModel((int)veinPool[j].modelIndex, j, veinPool[j].pos, Maths.SphericalRotation(veinPool[j].pos, Random.value * 360f), false);
							ColliderData[] colliders2 = veinProto.prefabDesc.colliders;
							int num2 = 0;
							while (colliders2 != null && num2 < colliders2.Length)
							{
								veinPool[j].colliderId = physics.AddColliderData(colliders2[num2].BindToObject(j, veinPool[j].colliderId, EObjectType.Vein, veinPool[j].pos, Quaternion.FromToRotation(Vector3.up, veinPool[j].pos.normalized)));
								num2++;
							}
							factory.RefreshVeinMiningDisplay(j, 0, 0);
						}
					}
				}
			}
			Debug.Log("step 2 complete : vein model & physics");
			planet.NotifyFactingStageComplete(2);
			PlanetModelingManager.currentFactingStage = 3;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 3)
		{
			PrebuildData[] prebuildPool = factory.prebuildPool;
			for (int k = 1; k < factory.prebuildCursor; k++)
			{
				if (prebuildPool[k].id == k)
				{
					ItemProto itemProto = items.Select((int)prebuildPool[k].protoId);
					if (itemProto != null)
					{
						prebuildPool[k].modelId = 0;
						prebuildPool[k].colliderId = 0;
						prebuildPool[k].modelId = factoryModel.gpuiManager.AddPrebuildModel((int)prebuildPool[k].modelIndex, k, prebuildPool[k].pos, prebuildPool[k].rot, false);
						ColliderData[] colliders3 = itemProto.prefabDesc.colliders;
						int num3 = 0;
						while (colliders3 != null && num3 < colliders3.Length)
						{
							prebuildPool[k].colliderId = physics.AddColliderData(colliders3[num3].BindToObject(k, prebuildPool[k].colliderId, EObjectType.Prebuild, prebuildPool[k].pos, prebuildPool[k].rot));
							num3++;
						}
						factory.PostRefreshPrebuildDisplay(k);
					}
				}
			}
			Debug.Log("step 3 complete : prebuild model & physics");
			planet.NotifyFactingStageComplete(3);
			PlanetModelingManager.currentFactingStage = 4;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 4)
		{
			EntityData[] entityPool = factory.entityPool;
			for (int l = 1; l < factory.entityCursor; l++)
			{
				if (entityPool[l].id == l)
				{
					entityPool[l].modelId = 0;
					entityPool[l].mmblockId = 0;
					entityPool[l].colliderId = 0;
					entityPool[l].audioId = 0;
					factory.CreateEntityDisplayComponents(l);
				}
			}
			Debug.Log("step 4 complete : entity model & physics");
			planet.NotifyFactingStageComplete(4);
			PlanetModelingManager.currentFactingStage = 5;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 5)
		{
			CraftData[] craftPool = factory.craftPool;
			for (int m = 1; m < factory.craftCursor; m++)
			{
				if (craftPool[m].id == m)
				{
					craftPool[m].modelId = 0;
					craftPool[m].mmblockId = 0;
					craftPool[m].colliderId = 0;
					craftPool[m].audioId = 0;
					factory.CreateCraftDisplayComponents(m);
				}
			}
			Debug.Log("step 5 complete : craft model & physics");
			planet.NotifyFactingStageComplete(5);
			PlanetModelingManager.currentFactingStage = 6;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 6)
		{
			EnemyData[] enemyPool = factory.enemyPool;
			for (int n = 1; n < factory.enemyCursor; n++)
			{
				if (enemyPool[n].id == n)
				{
					enemyPool[n].modelId = 0;
					enemyPool[n].mmblockId = 0;
					enemyPool[n].colliderId = 0;
					enemyPool[n].audioId = 0;
					factory.CreateEnemyDisplayComponents(n);
				}
			}
			Debug.Log("step 6 complete : enemy model & physics");
			planet.NotifyFactingStageComplete(6);
			PlanetModelingManager.currentFactingStage = 7;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 7)
		{
			RuinData[] ruinPool = factory.ruinPool;
			for (int num4 = 1; num4 < factory.ruinCursor; num4++)
			{
				if (ruinPool[num4].id == num4)
				{
					ruinPool[num4].modelId = 0;
					ruinPool[num4].mmblockId = 0;
					ruinPool[num4].colliderId = 0;
					factory.CreateRuinDisplayComponent(num4);
				}
			}
			Debug.Log("step 7 complete : ruin model & physics");
			planet.NotifyFactingStageComplete(7);
			PlanetModelingManager.currentFactingStage = 8;
			return;
		}
		if (PlanetModelingManager.currentFactingStage == 8)
		{
			HighStopwatch highStopwatch = new HighStopwatch();
			highStopwatch.Begin();
			factory.cargoTraffic.CreateRenderingBatches();
			factoryModel.gpuiManager.SyncAllGPUBuffer();
			factoryModel.RefreshPowerNodes();
			factoryModel.RefreshTurrets();
			Debug.Log("step 8 complete : create rendering batch and sync buffers, time cost: " + (highStopwatch.duration * 1000.0).ToString("0.000") + " ms");
			planet.NotifyFactingStageComplete(8);
		}
		factory.LocalizeEntities();
		planet.NotifyFactoryLoaded();
		PlanetModelingManager.currentFactingStage = 0;
		PlanetModelingManager.currentFactingPlanet = null;
	}

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0031DE8B File Offset: 0x0031C08B
	public static void Start()
	{
		PlanetModelingManager.PrepareWorks();
		PlanetModelingManager.StartPlanetComputeThread();
		PlanetModelingManager.StartPlanetCalculateThread();
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x0031DE9C File Offset: 0x0031C09C
	public static void End()
	{
		PlanetModelingManager.EndPlanetComputeThread();
		PlanetModelingManager.EndPlanetCalculateThread();
	}

	// Token: 0x06003AD3 RID: 15059 RVA: 0x0031DEA8 File Offset: 0x0031C0A8
	public static void Update()
	{
		PlanetModelingManager.ModelingPlanetCoroutine();
		PlanetModelingManager.LoadingPlanetFactoryCoroutine();
		string obj = PlanetModelingManager.planetComputeThreadError;
		lock (obj)
		{
			if (!string.IsNullOrEmpty(PlanetModelingManager.planetComputeThreadError))
			{
				Debug.LogError("Thread Error: \r\n" + PlanetModelingManager.planetComputeThreadError);
				PlanetModelingManager.planetComputeThreadError = "";
			}
		}
		if (PlanetModelingManager.planetComputeThreadLogs != null)
		{
			List<string> obj2 = PlanetModelingManager.planetComputeThreadLogs;
			lock (obj2)
			{
				foreach (string text in PlanetModelingManager.planetComputeThreadLogs)
				{
					if (!string.IsNullOrEmpty(text))
					{
						Debug.Log("Thread Log: \r\n" + text);
					}
				}
				PlanetModelingManager.planetComputeThreadLogs.Clear();
			}
		}
		obj = PlanetModelingManager.planetCalculateThreadError;
		lock (obj)
		{
			if (!string.IsNullOrEmpty(PlanetModelingManager.planetCalculateThreadError))
			{
				Debug.LogError("Vein Thread Error: \r\n" + PlanetModelingManager.planetCalculateThreadError);
				PlanetModelingManager.planetCalculateThreadError = "";
			}
		}
		if (PlanetModelingManager.planetCalculateThreadLogs != null)
		{
			List<string> obj2 = PlanetModelingManager.planetCalculateThreadLogs;
			lock (obj2)
			{
				foreach (string text2 in PlanetModelingManager.planetCalculateThreadLogs)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						Debug.Log("Vein Thread Log: \r\n" + text2);
					}
				}
				PlanetModelingManager.planetCalculateThreadLogs.Clear();
			}
		}
	}

	// Token: 0x06003AD4 RID: 15060 RVA: 0x0031E084 File Offset: 0x0031C284
	public static void RequestLoadStar(StarData star)
	{
		Queue<PlanetData> obj = PlanetModelingManager.genPlanetReqList;
		lock (obj)
		{
			for (int i = 0; i < star.planetCount; i++)
			{
				star.planets[i].wanted = true;
				if (!star.planets[i].loaded && !star.planets[i].loading)
				{
					star.planets[i].loading = true;
					PlanetModelingManager.genPlanetReqList.Enqueue(star.planets[i]);
				}
			}
		}
	}

	// Token: 0x06003AD5 RID: 15061 RVA: 0x0031E11C File Offset: 0x0031C31C
	public static void RequestLoadPlanet(PlanetData planet)
	{
		Queue<PlanetData> obj = PlanetModelingManager.genPlanetReqList;
		lock (obj)
		{
			planet.wanted = true;
			if (!planet.loaded && !planet.loading)
			{
				planet.loading = true;
				PlanetModelingManager.genPlanetReqList.Enqueue(planet);
			}
		}
	}

	// Token: 0x06003AD6 RID: 15062 RVA: 0x0031E180 File Offset: 0x0031C380
	public static void RequestCalcStar(StarData star)
	{
		Queue<PlanetData> obj = PlanetModelingManager.calPlanetReqList;
		lock (obj)
		{
			for (int i = 0; i < star.planetCount; i++)
			{
				if (!star.planets[i].calculated && !star.planets[i].calculating && star.planets[i].data == null && !star.planets[i].loaded && !star.planets[i].loading)
				{
					star.planets[i].calculating = true;
					PlanetModelingManager.calPlanetReqList.Enqueue(star.planets[i]);
				}
			}
		}
	}

	// Token: 0x06003AD7 RID: 15063 RVA: 0x0031E238 File Offset: 0x0031C438
	public static void RequestCalcPlanet(PlanetData planet)
	{
		Queue<PlanetData> obj = PlanetModelingManager.calPlanetReqList;
		lock (obj)
		{
			if (!planet.calculated && !planet.calculating && planet.data == null)
			{
				if (!planet.loaded && !planet.loading)
				{
					planet.calculating = true;
					PlanetModelingManager.calPlanetReqList.Enqueue(planet);
				}
			}
		}
	}

	// Token: 0x06003AD8 RID: 15064 RVA: 0x0031E2B0 File Offset: 0x0031C4B0
	private static void PrepareWorks()
	{
		int num = 0;
		VegeProto[] dataArray = LDB.veges.dataArray;
		for (int i = 0; i < dataArray.Length; i++)
		{
			num = dataArray[i].ID + 1;
		}
		PlanetModelingManager.vegeHps = new short[num];
		PlanetModelingManager.vegeScaleRanges = new Vector4[num];
		PlanetModelingManager.vegeProtos = new VegeProto[num];
		for (int j = 0; j < dataArray.Length; j++)
		{
			PlanetModelingManager.vegeHps[dataArray[j].ID] = (short)dataArray[j].HpMax;
			PlanetModelingManager.vegeScaleRanges[dataArray[j].ID] = dataArray[j].ScaleRange;
			PlanetModelingManager.vegeProtos[dataArray[j].ID] = dataArray[j];
		}
		VeinProto[] dataArray2 = LDB.veins.dataArray;
		for (int k = 0; k < dataArray2.Length; k++)
		{
			num = dataArray2[k].ID + 1;
		}
		PlanetModelingManager.veinProducts = new int[num];
		PlanetModelingManager.veinModelIndexs = new int[num];
		PlanetModelingManager.veinModelCounts = new int[num];
		PlanetModelingManager.veinProtos = new VeinProto[num];
		for (int l = 0; l < dataArray2.Length; l++)
		{
			PlanetModelingManager.veinProducts[dataArray2[l].ID] = dataArray2[l].MiningItem;
			PlanetModelingManager.veinModelIndexs[dataArray2[l].ID] = dataArray2[l].ModelIndex;
			PlanetModelingManager.veinModelCounts[dataArray2[l].ID] = dataArray2[l].ModelCount;
			PlanetModelingManager.veinProtos[dataArray2[l].ID] = dataArray2[l];
		}
	}

	// Token: 0x06003AD9 RID: 15065 RVA: 0x0031E420 File Offset: 0x0031C620
	private static void StartPlanetComputeThread()
	{
		if (PlanetModelingManager.genPlanetReqList == null)
		{
			PlanetModelingManager.genPlanetReqList = new Queue<PlanetData>(50);
		}
		if (PlanetModelingManager.modPlanetReqList == null)
		{
			PlanetModelingManager.modPlanetReqList = new Queue<PlanetData>(50);
		}
		if (PlanetModelingManager.fctPlanetReqList == null)
		{
			PlanetModelingManager.fctPlanetReqList = new Queue<PlanetData>(50);
		}
		PlanetModelingManager.ThreadFlagLock obj = PlanetModelingManager.planetComputeThreadFlagLock;
		lock (obj)
		{
			if (PlanetModelingManager.planetComputeThreadFlag == PlanetModelingManager.ThreadFlag.Ended)
			{
				PlanetModelingManager.planetComputeThread = new Thread(new ThreadStart(PlanetModelingManager.PlanetComputeThreadMain));
				PlanetModelingManager.planetComputeThread.Start();
				PlanetModelingManager.planetComputeThreadFlag = PlanetModelingManager.ThreadFlag.Running;
			}
			else
			{
				PlanetModelingManager.planetComputeThreadFlag = PlanetModelingManager.ThreadFlag.Running;
			}
		}
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x0031E4C8 File Offset: 0x0031C6C8
	private static void EndPlanetComputeThread()
	{
		PlanetModelingManager.ThreadFlagLock obj = PlanetModelingManager.planetComputeThreadFlagLock;
		lock (obj)
		{
			PlanetModelingManager.planetComputeThreadFlag = PlanetModelingManager.ThreadFlag.Ending;
		}
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x0031E508 File Offset: 0x0031C708
	private static void StartPlanetCalculateThread()
	{
		if (PlanetModelingManager.calPlanetReqList == null)
		{
			PlanetModelingManager.calPlanetReqList = new Queue<PlanetData>(50);
		}
		PlanetModelingManager.ThreadFlagLock obj = PlanetModelingManager.planetCalculateThreadFlagLock;
		lock (obj)
		{
			if (PlanetModelingManager.planetCalculateThreadFlag == PlanetModelingManager.ThreadFlag.Ended)
			{
				PlanetModelingManager.planetCalculateThread = new Thread(new ThreadStart(PlanetModelingManager.PlanetCalculateThreadMain));
				PlanetModelingManager.planetCalculateThread.Start();
				PlanetModelingManager.planetCalculateThreadFlag = PlanetModelingManager.ThreadFlag.Running;
			}
			else
			{
				PlanetModelingManager.planetCalculateThreadFlag = PlanetModelingManager.ThreadFlag.Running;
			}
		}
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x0031E58C File Offset: 0x0031C78C
	private static void EndPlanetCalculateThread()
	{
		PlanetModelingManager.ThreadFlagLock obj = PlanetModelingManager.planetCalculateThreadFlagLock;
		lock (obj)
		{
			PlanetModelingManager.planetCalculateThreadFlag = PlanetModelingManager.ThreadFlag.Ending;
		}
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x0031E5CC File Offset: 0x0031C7CC
	public static PlanetAlgorithm Algorithm(PlanetData planet)
	{
		PlanetAlgorithm planetAlgorithm;
		switch (planet.algoId)
		{
		case 1:
			planetAlgorithm = new PlanetAlgorithm1();
			break;
		case 2:
			planetAlgorithm = new PlanetAlgorithm2();
			break;
		case 3:
			planetAlgorithm = new PlanetAlgorithm3();
			break;
		case 4:
			planetAlgorithm = new PlanetAlgorithm4();
			break;
		case 5:
			planetAlgorithm = new PlanetAlgorithm5();
			break;
		case 6:
			planetAlgorithm = new PlanetAlgorithm6();
			break;
		case 7:
			planetAlgorithm = new PlanetAlgorithm7();
			break;
		case 8:
			planetAlgorithm = new PlanetAlgorithm8();
			break;
		case 9:
			planetAlgorithm = new PlanetAlgorithm9();
			break;
		case 10:
			planetAlgorithm = new PlanetAlgorithm10();
			break;
		case 11:
			planetAlgorithm = new PlanetAlgorithm11();
			break;
		case 12:
			planetAlgorithm = new PlanetAlgorithm12();
			break;
		case 13:
			planetAlgorithm = new PlanetAlgorithm13();
			break;
		case 14:
			planetAlgorithm = new PlanetAlgorithm14();
			break;
		default:
			planetAlgorithm = new PlanetAlgorithm0();
			break;
		}
		if (planetAlgorithm != null)
		{
			planetAlgorithm.Reset(planet.seed, planet);
		}
		return planetAlgorithm;
	}

	// Token: 0x06003ADE RID: 15070 RVA: 0x0031E6AC File Offset: 0x0031C8AC
	private static void PlanetComputeThreadMain()
	{
		object obj = null;
		PlanetModelingManager.ThreadFlagLock obj2 = PlanetModelingManager.planetComputeThreadFlagLock;
		lock (obj2)
		{
			obj = PlanetModelingManager.planetComputeThread;
		}
		for (;;)
		{
			int num = 0;
			obj2 = PlanetModelingManager.planetComputeThreadFlagLock;
			lock (obj2)
			{
				if (PlanetModelingManager.planetComputeThreadFlag != PlanetModelingManager.ThreadFlag.Running)
				{
					PlanetModelingManager.planetComputeThreadFlag = PlanetModelingManager.ThreadFlag.Ended;
					break;
				}
				if (obj != PlanetModelingManager.planetComputeThread)
				{
					break;
				}
			}
			PlanetData planetData = null;
			Queue<PlanetData> obj3 = PlanetModelingManager.genPlanetReqList;
			lock (obj3)
			{
				if (PlanetModelingManager.genPlanetReqList.Count > 0)
				{
					planetData = PlanetModelingManager.genPlanetReqList.Dequeue();
				}
			}
			if (planetData != null)
			{
				while (planetData.calculating)
				{
					Thread.Sleep(2);
				}
				try
				{
					PlanetAlgorithm planetAlgorithm = PlanetModelingManager.Algorithm(planetData);
					if (planetAlgorithm != null)
					{
						HighStopwatch highStopwatch = new HighStopwatch();
						double num2 = 0.0;
						double num3 = 0.0;
						double num4 = 0.0;
						if (planetData.data == null)
						{
							highStopwatch.Begin();
							planetData.data = new PlanetRawData(planetData.precision);
							planetData.modData = planetData.data.InitModData(planetData.modData);
							planetData.data.CalcVerts();
							planetData.aux = new PlanetAuxData(planetData);
							planetAlgorithm.GenerateTerrain(planetData.mod_x, planetData.mod_y);
							planetAlgorithm.CalcWaterPercent();
							num2 = highStopwatch.duration;
						}
						if (planetData.factory == null)
						{
							highStopwatch.Begin();
							planetData.data.vegeCursor = 1;
							if (planetData.type != EPlanetType.Gas)
							{
								planetAlgorithm.GenerateVegetables();
							}
							num3 = highStopwatch.duration;
							highStopwatch.Begin();
							planetData.data.veinCursor = 1;
							if (planetData.type != EPlanetType.Gas)
							{
								planetAlgorithm.GenerateVeins();
							}
							planetData.CalculateVeinGroups();
							num4 = highStopwatch.duration;
						}
						else if (planetData.galaxy.birthPlanetId == planetData.id)
						{
							planetData.GenBirthPoints();
						}
						if (PlanetModelingManager.planetComputeThreadLogs != null)
						{
							List<string> obj4 = PlanetModelingManager.planetComputeThreadLogs;
							lock (obj4)
							{
								PlanetModelingManager.planetComputeThreadLogs.Add(string.Format("{0}\r\nGenerate Terrain {1:F5} s\r\nGenerate Vegetables {2:F5} s\r\nGenerate Veins {3:F5} s\r\n", new object[]
								{
									planetData.displayName,
									num2,
									num3,
									num4
								}));
							}
						}
						planetData.NotifyCalculated();
					}
				}
				catch (Exception ex)
				{
					string obj5 = PlanetModelingManager.planetComputeThreadError;
					lock (obj5)
					{
						if (string.IsNullOrEmpty(PlanetModelingManager.planetComputeThreadError))
						{
							PlanetModelingManager.planetComputeThreadError = ex.ToString();
						}
					}
				}
				obj3 = PlanetModelingManager.modPlanetReqList;
				lock (obj3)
				{
					PlanetModelingManager.modPlanetReqList.Enqueue(planetData);
				}
			}
			if (planetData == null)
			{
				Thread.Sleep(50);
			}
			else if (num % 20 == 0)
			{
				Thread.Sleep(2);
			}
		}
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x0031EA54 File Offset: 0x0031CC54
	private static void PlanetCalculateThreadMain()
	{
		object obj = null;
		PlanetModelingManager.ThreadFlagLock obj2 = PlanetModelingManager.planetCalculateThreadFlagLock;
		lock (obj2)
		{
			obj = PlanetModelingManager.planetCalculateThread;
		}
		for (;;)
		{
			int num = 0;
			obj2 = PlanetModelingManager.planetCalculateThreadFlagLock;
			lock (obj2)
			{
				if (PlanetModelingManager.planetCalculateThreadFlag != PlanetModelingManager.ThreadFlag.Running)
				{
					PlanetModelingManager.planetCalculateThreadFlag = PlanetModelingManager.ThreadFlag.Ended;
					break;
				}
				if (obj != PlanetModelingManager.planetCalculateThread)
				{
					break;
				}
			}
			PlanetData planetData = null;
			Queue<PlanetData> obj3 = PlanetModelingManager.calPlanetReqList;
			lock (obj3)
			{
				if (PlanetModelingManager.calPlanetReqList.Count > 0)
				{
					planetData = PlanetModelingManager.calPlanetReqList.Dequeue();
				}
			}
			if (planetData != null)
			{
				try
				{
					PlanetAlgorithm planetAlgorithm = PlanetModelingManager.Algorithm(planetData);
					if (planetAlgorithm != null)
					{
						HighStopwatch highStopwatch = new HighStopwatch();
						highStopwatch.Begin();
						planetData.data = new PlanetRawData(planetData.precision);
						planetData.modData = planetData.data.InitModData(planetData.modData);
						planetData.data.CalcVerts();
						planetData.aux = new PlanetAuxData(planetData);
						planetAlgorithm.GenerateTerrain(planetData.mod_x, planetData.mod_y);
						planetAlgorithm.CalcWaterPercent();
						double duration = highStopwatch.duration;
						highStopwatch.Begin();
						planetData.data.vegeCursor = 1;
						if (planetData.type != EPlanetType.Gas)
						{
							planetAlgorithm.GenerateVegetables();
						}
						double duration2 = highStopwatch.duration;
						highStopwatch.Begin();
						planetData.data.veinCursor = 1;
						if (planetData.type != EPlanetType.Gas)
						{
							planetAlgorithm.GenerateVeins();
						}
						planetData.CalculateVeinGroups();
						planetData.GenBirthPoints();
						double duration3 = highStopwatch.duration;
						if (PlanetModelingManager.planetCalculateThreadLogs != null)
						{
							List<string> obj4 = PlanetModelingManager.planetCalculateThreadLogs;
							lock (obj4)
							{
								PlanetModelingManager.planetCalculateThreadLogs.Add(string.Format("{0}\r\nGenerate Terrain {1:F5} s\r\nGenerate Vegetables {2:F5} s\r\nGenerate Veins {3:F5} s\r\n", new object[]
								{
									planetData.displayName,
									duration,
									duration2,
									duration3
								}));
							}
						}
						planetData.NotifyCalculated();
					}
				}
				catch (Exception ex)
				{
					string obj5 = PlanetModelingManager.planetCalculateThreadError;
					lock (obj5)
					{
						if (string.IsNullOrEmpty(PlanetModelingManager.planetCalculateThreadError))
						{
							PlanetModelingManager.planetCalculateThreadError = ex.ToString();
						}
					}
					planetData.calculated = false;
				}
				planetData.calculating = false;
			}
			if (planetData == null)
			{
				Thread.Sleep(50);
			}
			else if (num % 20 == 0)
			{
				Thread.Sleep(2);
			}
		}
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0031ED88 File Offset: 0x0031CF88
	private static void ModelingPlanetCoroutine()
	{
		if (PlanetModelingManager.currentModelingPlanet == null)
		{
			PlanetData planetData = null;
			Queue<PlanetData> obj = PlanetModelingManager.modPlanetReqList;
			lock (obj)
			{
				if (PlanetModelingManager.modPlanetReqList.Count > 0)
				{
					planetData = PlanetModelingManager.modPlanetReqList.Dequeue();
				}
			}
			if (planetData != null)
			{
				PlanetModelingManager.currentModelingPlanet = planetData;
				PlanetModelingManager.currentModelingStage = 0;
				PlanetModelingManager.currentModelingSeamNormal = 0;
			}
		}
		if (PlanetModelingManager.currentModelingPlanet != null)
		{
			try
			{
				PlanetModelingManager.ModelingPlanetMain(PlanetModelingManager.currentModelingPlanet);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				PlanetModelingManager.currentModelingPlanet.Unload();
				PlanetModelingManager.currentModelingPlanet.factoryLoaded = false;
				PlanetModelingManager.currentModelingPlanet = null;
				PlanetModelingManager.currentModelingStage = 0;
				PlanetModelingManager.currentModelingSeamNormal = 0;
			}
		}
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x0031EE44 File Offset: 0x0031D044
	private static void ModelingPlanetMain(PlanetData planet)
	{
		ThemeProto themeProto = LDB.themes.Select(planet.theme);
		if (PlanetModelingManager.currentModelingStage == 0)
		{
			if (!planet.wanted)
			{
				PlanetModelingManager.currentModelingStage = 4;
				return;
			}
			if (PlanetModelingManager.tmpMeshList == null)
			{
				PlanetModelingManager.tmpMeshList = new List<Mesh>(100);
				PlanetModelingManager.tmpMeshRendererList = new List<MeshRenderer>(100);
				PlanetModelingManager.tmpMeshColliderList = new List<MeshCollider>(100);
				PlanetModelingManager.tmpOceanCollider = null;
				PlanetModelingManager.tmpVerts = new List<Vector3>(1700);
				PlanetModelingManager.tmpNorms = new List<Vector3>(1700);
				PlanetModelingManager.tmpTgnts = new List<Vector4>(1700);
				PlanetModelingManager.tmpUvs = new List<Vector2>(1700);
				PlanetModelingManager.tmpUv2s = new List<Vector4>(1700);
				PlanetModelingManager.tmpTris = new List<int>(10000);
			}
			if (planet.heightmap == null)
			{
				planet.heightmap = new RenderTexture(new RenderTextureDescriptor(512, 512, RenderTextureFormat.RGHalf, 0)
				{
					dimension = TextureDimension.Cube,
					useMipMap = false,
					autoGenerateMips = false
				});
			}
			if (PlanetModelingManager.heightmapCamera == null)
			{
				GameObject gameObject = new GameObject("Heightmap Camera");
				PlanetModelingManager.heightmapCamera = gameObject.AddComponent<Camera>();
				PlanetModelingManager.heightmapCamera.cullingMask = 1073741824;
				PlanetModelingManager.heightmapCamera.enabled = false;
				PlanetModelingManager.heightmapCamera.farClipPlane = 900f;
				PlanetModelingManager.heightmapCamera.nearClipPlane = 10f;
				PlanetModelingManager.heightmapCamera.renderingPath = RenderingPath.Forward;
				PlanetModelingManager.heightmapCamera.allowDynamicResolution = false;
				PlanetModelingManager.heightmapCamera.allowMSAA = false;
				PlanetModelingManager.heightmapCamera.allowHDR = true;
				PlanetModelingManager.heightmapCamera.depthTextureMode = DepthTextureMode.None;
				PlanetModelingManager.heightmapCamera.clearFlags = CameraClearFlags.Color;
				PlanetModelingManager.heightmapCamera.backgroundColor = Color.black;
				PlanetModelingManager.heightmapCamera.depth = 0f;
				PlanetModelingManager.heightmapCamera.SetReplacementShader(Configs.builtin.heightmapShader, "ReplaceTag");
				gameObject.SetActive(false);
			}
			if (planet.terrainMaterial == null)
			{
				if (themeProto != null && themeProto.terrainMat != null)
				{
					planet.terrainMaterial = Object.Instantiate<Material>(themeProto.terrainMat[planet.style % themeProto.terrainMat.Length]);
					planet.terrainMaterial.name = planet.displayName + " Terrain";
					planet.terrainMaterial.SetFloat("_Radius", planet.realRadius);
					if (planet.terrainMaterial.HasProperty("_LightColorScreen"))
					{
						planet.groundScreenColor = planet.terrainMaterial.GetColor("_LightColorScreen");
					}
					else
					{
						planet.groundScreenColor = Color.black;
					}
				}
				else
				{
					planet.terrainMaterial = Object.Instantiate<Material>(Configs.builtin.planetSurfaceMatProto);
					planet.groundScreenColor = Color.black;
				}
			}
			if (planet.oceanMaterial == null)
			{
				if (themeProto != null && themeProto.oceanMat != null)
				{
					planet.oceanMaterial = Object.Instantiate<Material>(themeProto.oceanMat[planet.style % themeProto.oceanMat.Length]);
					planet.oceanMaterial.name = planet.displayName + " Ocean";
					planet.oceanMaterial.SetFloat("_Radius", planet.realRadius);
				}
				else
				{
					planet.oceanMaterial = null;
				}
			}
			if (planet.atmosMaterial == null)
			{
				if (themeProto != null && themeProto.atmosMat != null)
				{
					planet.atmosMaterial = Object.Instantiate<Material>(themeProto.atmosMat[planet.style % themeProto.atmosMat.Length]);
					planet.atmosMaterial.name = planet.displayName + " Atmos";
					planet.atmosMaterialLate = Object.Instantiate<Material>(themeProto.atmosMat[planet.style % themeProto.atmosMat.Length]);
					planet.atmosMaterialLate.name = planet.displayName + " Atmos Late";
				}
				else
				{
					planet.atmosMaterial = null;
					planet.atmosMaterialLate = null;
				}
			}
			if (planet.nephogramMaterial == null)
			{
				if (themeProto != null && themeProto.nephogramMat != null)
				{
					planet.nephogramMaterial = Object.Instantiate<Material>(themeProto.nephogramMat[planet.style % themeProto.nephogramMat.Length]);
					planet.nephogramMaterial.name = planet.displayName + " Nephogram";
				}
				else
				{
					planet.nephogramMaterial = null;
				}
			}
			if (planet.cloudMaterial == null)
			{
				if (themeProto != null && themeProto.cloudMat != null)
				{
					planet.cloudMaterial = Object.Instantiate<Material>(themeProto.cloudMat[planet.style % themeProto.cloudMat.Length]);
					planet.cloudMaterial.name = planet.displayName + " Cloud";
				}
				else
				{
					planet.cloudMaterial = null;
				}
			}
			if (planet.reformMaterial0 == null)
			{
				planet.reformMaterial0 = Object.Instantiate<Material>(Configs.builtin.planetReformMatProto0);
			}
			if (planet.reformMaterial1 == null)
			{
				planet.reformMaterial1 = Object.Instantiate<Material>(Configs.builtin.planetReformMatProto1);
			}
			if (planet.ambientDesc == null)
			{
				if (themeProto != null && themeProto.ambientDesc != null)
				{
					planet.ambientDesc = themeProto.ambientDesc[planet.style % themeProto.ambientDesc.Length];
				}
				else
				{
					planet.ambientDesc = null;
				}
			}
			if (planet.ambientSfx == null && themeProto != null && themeProto.ambientSfx != null)
			{
				planet.ambientSfx = themeProto.ambientSfx[planet.style % themeProto.ambientSfx.Length];
				planet.ambientSfxVolume = themeProto.SFXVolume;
			}
			if (planet.minimapMaterial == null)
			{
				if (themeProto != null && themeProto.minimapMat != null)
				{
					planet.minimapMaterial = Object.Instantiate<Material>(themeProto.minimapMat[planet.style % themeProto.minimapMat.Length]);
				}
				else
				{
					planet.minimapMaterial = Object.Instantiate<Material>(Configs.builtin.planetMinimapDefault);
				}
				planet.minimapMaterial.name = planet.displayName + " Minimap";
				planet.minimapMaterial.SetTexture("_HeightMap", planet.heightmap);
			}
			PlanetModelingManager.tmpMeshList.Clear();
			PlanetModelingManager.tmpMeshRendererList.Clear();
			PlanetModelingManager.tmpMeshColliderList.Clear();
			PlanetModelingManager.tmpOceanCollider = null;
			PlanetModelingManager.tmpTris.Clear();
			PlanetModelingManager.currentModelingStage = 1;
			return;
		}
		else if (PlanetModelingManager.currentModelingStage == 1)
		{
			if (!planet.wanted)
			{
				PlanetModelingManager.currentModelingStage = 4;
				return;
			}
			PlanetModelingManager.tmpPlanetGameObject = new GameObject(planet.displayName);
			PlanetModelingManager.tmpPlanetGameObject.layer = 31;
			PlanetSimulator sim = PlanetModelingManager.tmpPlanetGameObject.AddComponent<PlanetSimulator>();
			GameMain.universeSimulator.SetPlanetSimulator(sim, planet);
			PlanetModelingManager.tmpPlanetGameObject.transform.localPosition = Vector3.zero;
			PlanetModelingManager.tmpPlanetBodyGameObject = new GameObject("Planet Body");
			PlanetModelingManager.tmpPlanetBodyGameObject.transform.SetParent(PlanetModelingManager.tmpPlanetGameObject.transform, false);
			PlanetModelingManager.tmpPlanetBodyGameObject.layer = 31;
			PlanetModelingManager.tmpPlanetReformGameObject = new GameObject("Terrain Reform");
			PlanetModelingManager.tmpPlanetReformGameObject.transform.SetParent(PlanetModelingManager.tmpPlanetBodyGameObject.transform, false);
			PlanetModelingManager.tmpPlanetReformGameObject.layer = 14;
			MeshFilter meshFilter = PlanetModelingManager.tmpPlanetReformGameObject.AddComponent<MeshFilter>();
			PlanetModelingManager.tmpPlanetReformRenderer = PlanetModelingManager.tmpPlanetReformGameObject.AddComponent<MeshRenderer>();
			meshFilter.sharedMesh = Configs.builtin.planetReformMesh;
			Material[] sharedMaterials = new Material[]
			{
				planet.reformMaterial0,
				planet.reformMaterial1
			};
			PlanetModelingManager.tmpPlanetReformRenderer.sharedMaterials = sharedMaterials;
			PlanetModelingManager.tmpPlanetReformRenderer.receiveShadows = false;
			PlanetModelingManager.tmpPlanetReformRenderer.lightProbeUsage = LightProbeUsage.Off;
			PlanetModelingManager.tmpPlanetReformRenderer.shadowCastingMode = ShadowCastingMode.Off;
			float num = (planet.realRadius + 0.2f + 0.025f) * 2f;
			PlanetModelingManager.tmpPlanetReformRenderer.transform.localScale = new Vector3(num, num, num);
			PlanetModelingManager.tmpPlanetReformRenderer.transform.rotation = Quaternion.identity;
			if (planet.waterItemId != 0)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(Configs.builtin.oceanSphere, PlanetModelingManager.tmpPlanetBodyGameObject.transform);
				gameObject2.name = "Ocean Sphere";
				gameObject2.layer = 31;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localScale = Vector3.one * ((planet.realRadius + planet.waterHeight) * 2f);
				Renderer component = gameObject2.GetComponent<Renderer>();
				PlanetModelingManager.tmpOceanCollider = gameObject2.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = (planet.oceanMaterial != null);
					component.shadowCastingMode = ShadowCastingMode.Off;
					component.receiveShadows = false;
					component.lightProbeUsage = LightProbeUsage.Off;
					component.sharedMaterial = planet.oceanMaterial;
				}
			}
			int num2 = planet.precision / planet.segment;
			int num3 = num2 + 1;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					PlanetModelingManager.tmpTris.Add(i + 1 + (j + 1) * num3);
					PlanetModelingManager.tmpTris.Add(i + (j + 1) * num3);
					PlanetModelingManager.tmpTris.Add(i + j * num3);
					PlanetModelingManager.tmpTris.Add(i + j * num3);
					PlanetModelingManager.tmpTris.Add(i + 1 + j * num3);
					PlanetModelingManager.tmpTris.Add(i + 1 + (j + 1) * num3);
				}
			}
			PlanetModelingManager.currentModelingStage = 2;
			return;
		}
		else if (PlanetModelingManager.currentModelingStage == 2)
		{
			if (!planet.wanted)
			{
				PlanetModelingManager.currentModelingStage = 4;
				return;
			}
			int precision = planet.precision;
			int num4 = precision / planet.segment;
			PlanetRawData data = planet.data;
			float scale = planet.scale;
			float num5 = planet.radius * scale + 0.2f;
			int stride = data.stride;
			int num6 = 0;
			int num7 = GameMain.isLoading ? 3 : 2;
			int num8 = 0;
			for (int k = 0; k < 4; k++)
			{
				int num9 = k % 2 * (precision + 1);
				int num10 = k / 2 * (precision + 1);
				for (int l = 0; l < precision; l += num4)
				{
					for (int m = 0; m < precision; m += num4)
					{
						if (num8 == 0 && num6 < PlanetModelingManager.tmpMeshList.Count)
						{
							num6++;
						}
						else
						{
							Mesh mesh = new Mesh();
							PlanetModelingManager.tmpMeshList.Add(mesh);
							PlanetModelingManager.tmpVerts.Clear();
							PlanetModelingManager.tmpNorms.Clear();
							PlanetModelingManager.tmpTgnts.Clear();
							PlanetModelingManager.tmpUvs.Clear();
							PlanetModelingManager.tmpUv2s.Clear();
							GameObject gameObject3 = new GameObject("Surface");
							gameObject3.layer = 30;
							gameObject3.transform.SetParent(PlanetModelingManager.tmpPlanetBodyGameObject.transform, false);
							int num11 = l;
							while (num11 <= l + num4 && num11 <= precision)
							{
								int num12 = m;
								while (num12 <= m + num4 && num12 <= precision)
								{
									int num13 = num9 + num12;
									int num14 = num10 + num11;
									int num15 = num13 + num14 * stride;
									int num16 = num15;
									if (num11 == 0)
									{
										int num17 = (k + 3) % 4;
										int num18 = num17 % 2 * (precision + 1);
										int num19 = num17 / 2 * (precision + 1);
										int num20 = precision;
										int num21 = precision - num12;
										int num22 = num18 + num20;
										int num23 = num19 + num21;
										num16 = num22 + num23 * stride;
									}
									else if (num12 == 0)
									{
										int num24 = (k + 3) % 4;
										int num25 = num24 % 2 * (precision + 1);
										int num26 = num24 / 2 * (precision + 1);
										int num27 = precision - num11;
										int num28 = precision;
										int num29 = num25 + num27;
										int num30 = num26 + num28;
										num16 = num29 + num30 * stride;
									}
									if (num11 == precision)
									{
										int num31 = (k + 1) % 4;
										int num32 = num31 % 2 * (precision + 1);
										int num33 = num31 / 2 * (precision + 1);
										int num34 = 0;
										int num35 = precision - num12;
										int num36 = num32 + num34;
										int num37 = num33 + num35;
										num16 = num36 + num37 * stride;
									}
									else if (num12 == precision)
									{
										int num38 = (k + 1) % 4;
										int num39 = num38 % 2 * (precision + 1);
										int num40 = num38 / 2 * (precision + 1);
										int num41 = precision - num11;
										int num42 = 0;
										int num43 = num39 + num41;
										int num44 = num40 + num42;
										num16 = num43 + num44 * stride;
									}
									float num45 = (float)data.heightData[num15] * 0.01f * scale;
									float num46 = (float)data.GetModLevel(num15) * 0.3333333f;
									float num47 = num5;
									if (num46 > 0f)
									{
										num47 = (float)data.GetModPlane(num15) * 0.01f * scale;
									}
									float d = num45 * (1f - num46) + num47 * num46;
									Vector3 item = data.vertices[num15] * d;
									PlanetModelingManager.tmpVerts.Add(item);
									PlanetModelingManager.tmpNorms.Add(data.vertices[num15]);
									Vector3 vector = Vector3.Cross(data.vertices[num15], Vector3.up).normalized;
									if (vector.sqrMagnitude == 0f)
									{
										vector = Vector3.right;
									}
									PlanetModelingManager.tmpTgnts.Add(new Vector4(vector.x, vector.y, vector.z, 1f));
									PlanetModelingManager.tmpUvs.Add(new Vector2(((float)num13 + 0.5f) / (float)stride, ((float)num14 + 0.5f) / (float)stride));
									PlanetModelingManager.tmpUv2s.Add(new Vector4((float)data.biomoData[num15] * 0.01f, (float)data.temprData[num15] * 0.01f, (float)num15 + 0.3f, (float)num16 + 0.3f));
									num12++;
								}
								num11++;
							}
							mesh.indexFormat = IndexFormat.UInt16;
							mesh.SetVertices(PlanetModelingManager.tmpVerts);
							mesh.SetNormals(PlanetModelingManager.tmpNorms);
							mesh.SetTangents(PlanetModelingManager.tmpTgnts);
							mesh.SetUVs(0, PlanetModelingManager.tmpUvs);
							mesh.SetUVs(1, PlanetModelingManager.tmpUv2s);
							mesh.SetTriangles(PlanetModelingManager.tmpTris, 0, true, 0);
							mesh.RecalculateNormals();
							mesh.GetNormals(PlanetModelingManager.tmpNorms);
							for (int n = 0; n < PlanetModelingManager.tmpNorms.Count; n++)
							{
								int num48 = (int)PlanetModelingManager.tmpUv2s[n].z;
								int num49 = (int)PlanetModelingManager.tmpUv2s[n].w;
								data.normals[num48] = data.normals[num48] + PlanetModelingManager.tmpNorms[n];
								data.normals[num49] = data.normals[num49] + PlanetModelingManager.tmpNorms[n];
							}
							MeshFilter meshFilter2 = gameObject3.AddComponent<MeshFilter>();
							MeshRenderer meshRenderer = gameObject3.AddComponent<MeshRenderer>();
							MeshCollider meshCollider = gameObject3.AddComponent<MeshCollider>();
							meshFilter2.sharedMesh = mesh;
							meshRenderer.sharedMaterial = planet.terrainMaterial;
							meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
							meshRenderer.receiveShadows = false;
							meshRenderer.lightProbeUsage = LightProbeUsage.Off;
							meshCollider.sharedMesh = mesh;
							PlanetModelingManager.tmpMeshRendererList.Add(meshRenderer);
							PlanetModelingManager.tmpMeshColliderList.Add(meshCollider);
							num8++;
							if (num8 == num7)
							{
								return;
							}
						}
					}
				}
			}
			int num50 = GameMain.isLoading ? 15 : 5;
			for (int num51 = 0; num51 < PlanetModelingManager.tmpMeshList.Count; num51++)
			{
				int num52 = num51 / num50;
				if (num52 >= PlanetModelingManager.currentModelingSeamNormal)
				{
					if (num52 > PlanetModelingManager.currentModelingSeamNormal)
					{
						PlanetModelingManager.currentModelingSeamNormal++;
						return;
					}
					Mesh mesh2 = PlanetModelingManager.tmpMeshList[num51];
					PlanetModelingManager.tmpNorms.Clear();
					PlanetModelingManager.tmpUv2s.Clear();
					int vertexCount = mesh2.vertexCount;
					mesh2.GetUVs(1, PlanetModelingManager.tmpUv2s);
					for (int num53 = 0; num53 < vertexCount; num53++)
					{
						int num54 = (int)PlanetModelingManager.tmpUv2s[num53].z;
						PlanetModelingManager.tmpNorms.Add(data.normals[num54].normalized);
					}
					mesh2.SetNormals(PlanetModelingManager.tmpNorms);
				}
			}
			PlanetModelingManager.currentModelingStage = 3;
			return;
		}
		else
		{
			if (PlanetModelingManager.currentModelingStage != 3)
			{
				if (PlanetModelingManager.currentModelingStage == 4)
				{
					if (planet.wanted)
					{
						planet.gameObject = PlanetModelingManager.tmpPlanetGameObject;
						planet.bodyObject = PlanetModelingManager.tmpPlanetBodyGameObject;
						PlanetSimulator component2 = PlanetModelingManager.tmpPlanetGameObject.GetComponent<PlanetSimulator>();
						component2.surfaceRenderer = new Renderer[PlanetModelingManager.tmpMeshRendererList.Count];
						component2.surfaceCollider = new Collider[PlanetModelingManager.tmpMeshColliderList.Count];
						for (int num55 = 0; num55 < PlanetModelingManager.tmpMeshList.Count; num55++)
						{
							planet.meshes[num55] = PlanetModelingManager.tmpMeshList[num55];
							planet.meshRenderers[num55] = PlanetModelingManager.tmpMeshRendererList[num55];
							planet.meshColliders[num55] = PlanetModelingManager.tmpMeshColliderList[num55];
						}
						for (int num56 = 0; num56 < PlanetModelingManager.tmpMeshRendererList.Count; num56++)
						{
							PlanetModelingManager.tmpMeshRendererList[num56].gameObject.layer = 31;
							PlanetModelingManager.tmpMeshRendererList[num56].sharedMaterial = planet.terrainMaterial;
							PlanetModelingManager.tmpMeshRendererList[num56].receiveShadows = false;
							PlanetModelingManager.tmpMeshRendererList[num56].shadowCastingMode = ShadowCastingMode.Off;
							component2.surfaceRenderer[num56] = PlanetModelingManager.tmpMeshRendererList[num56];
							component2.surfaceCollider[num56] = PlanetModelingManager.tmpMeshColliderList[num56];
						}
						component2.oceanCollider = PlanetModelingManager.tmpOceanCollider;
						component2.sphereCollider = PlanetModelingManager.tmpPlanetBodyGameObject.AddComponent<SphereCollider>();
						if (component2.sphereCollider != null)
						{
							component2.sphereCollider.enabled = false;
						}
						component2.sphereCollider.radius = planet.realRadius;
						component2.reformRenderer = PlanetModelingManager.tmpPlanetReformRenderer;
						component2.reformMat0 = planet.reformMaterial0;
						component2.reformMat1 = planet.reformMaterial1;
						Material sharedMaterial = component2.surfaceRenderer[0].sharedMaterial;
						if (planet.type != EPlanetType.Gas)
						{
							component2.reformMat0.SetColor("_AmbientColor0", sharedMaterial.GetColor("_AmbientColor0"));
							component2.reformMat0.SetColor("_AmbientColor1", sharedMaterial.GetColor("_AmbientColor1"));
							component2.reformMat0.SetColor("_AmbientColor2", sharedMaterial.GetColor("_AmbientColor2"));
							component2.reformMat0.SetColor("_LightColorScreen", sharedMaterial.GetColor("_LightColorScreen"));
							component2.reformMat0.SetFloat("_Multiplier", sharedMaterial.GetFloat("_Multiplier"));
							component2.reformMat0.SetFloat("_AmbientInc", sharedMaterial.GetFloat("_AmbientInc"));
							component2.reformMat1.SetColor("_AmbientColor0", sharedMaterial.GetColor("_AmbientColor0"));
							component2.reformMat1.SetColor("_AmbientColor1", sharedMaterial.GetColor("_AmbientColor1"));
							component2.reformMat1.SetColor("_AmbientColor2", sharedMaterial.GetColor("_AmbientColor2"));
							component2.reformMat1.SetColor("_LightColorScreen", sharedMaterial.GetColor("_LightColorScreen"));
							component2.reformMat1.SetFloat("_Multiplier", sharedMaterial.GetFloat("_Multiplier"));
							component2.reformMat1.SetFloat("_AmbientInc", sharedMaterial.GetFloat("_AmbientInc"));
						}
						if (planet.nephogramMaterial != null && planet.cloudMaterial != null)
						{
							GameObject gameObject4 = Object.Instantiate<GameObject>(Configs.builtin.nephogramSphere, PlanetModelingManager.tmpPlanetBodyGameObject.transform);
							gameObject4.name = "Nephogram";
							gameObject4.layer = 31;
							gameObject4.transform.localPosition = Vector3.zero;
							component2.cloudSimulator = gameObject4.GetComponent<CloudSimulator>();
							component2.cloudSimulator.planet = planet;
							component2.cloudSimulator.planetSimulator = component2;
							gameObject4.SetActive(true);
						}
						PlanetModelingManager.tmpPlanetGameObject.transform.localPosition = Vector3.zero;
						PlanetModelingManager.heightmapCamera.transform.localPosition = Vector3.zero;
						PlanetModelingManager.tmpPlanetBodyGameObject.SetActive(true);
						PlanetModelingManager.tmpPlanetReformGameObject.SetActive(true);
						PlanetModelingManager.tmpPlanetGameObject = null;
						PlanetModelingManager.tmpPlanetBodyGameObject = null;
						PlanetModelingManager.tmpPlanetReformGameObject = null;
						PlanetModelingManager.tmpPlanetReformRenderer = null;
						PlanetModelingManager.tmpMeshList.Clear();
						PlanetModelingManager.tmpMeshRendererList.Clear();
						PlanetModelingManager.tmpMeshColliderList.Clear();
						PlanetModelingManager.tmpOceanCollider = null;
						PlanetModelingManager.tmpTris.Clear();
						PlanetModelingManager.tmpVerts.Clear();
						PlanetModelingManager.tmpNorms.Clear();
						PlanetModelingManager.tmpTgnts.Clear();
						PlanetModelingManager.tmpUvs.Clear();
						PlanetModelingManager.tmpUv2s.Clear();
						PlanetModelingManager.currentModelingPlanet = null;
						PlanetModelingManager.currentModelingStage = 0;
						PlanetModelingManager.currentModelingSeamNormal = 0;
						planet.NotifyLoaded();
						if (planet.star.loaded)
						{
							planet.star.NotifyLoaded();
							return;
						}
					}
					else
					{
						for (int num57 = 0; num57 < PlanetModelingManager.tmpMeshList.Count; num57++)
						{
							Object.Destroy(PlanetModelingManager.tmpMeshList[num57]);
						}
						Object.Destroy(PlanetModelingManager.tmpPlanetGameObject);
						PlanetModelingManager.tmpPlanetGameObject = null;
						PlanetModelingManager.tmpPlanetBodyGameObject = null;
						PlanetModelingManager.tmpPlanetReformGameObject = null;
						PlanetModelingManager.tmpPlanetReformRenderer = null;
						PlanetModelingManager.tmpMeshList.Clear();
						PlanetModelingManager.tmpTris.Clear();
						PlanetModelingManager.tmpVerts.Clear();
						PlanetModelingManager.tmpNorms.Clear();
						PlanetModelingManager.tmpTgnts.Clear();
						PlanetModelingManager.tmpUvs.Clear();
						PlanetModelingManager.tmpUv2s.Clear();
						PlanetModelingManager.currentModelingPlanet = null;
						PlanetModelingManager.currentModelingStage = 0;
						PlanetModelingManager.currentModelingSeamNormal = 0;
					}
				}
				return;
			}
			if (!planet.wanted)
			{
				PlanetModelingManager.currentModelingStage = 4;
				return;
			}
			PlanetModelingManager.tmpPlanetBodyGameObject.SetActive(true);
			PlanetModelingManager.tmpPlanetReformGameObject.SetActive(true);
			PlanetModelingManager.heightmapCamera.transform.localPosition = PlanetModelingManager.tmpPlanetGameObject.transform.localPosition;
			PlanetModelingManager.heightmapCamera.RenderToCubemap(planet.heightmap, 63);
			PlanetModelingManager.currentModelingStage = 4;
			return;
		}
	}

	// Token: 0x04004BF6 RID: 19446
	public static int Debug_LoadingPlanetFactoryMain_MaxStage = 100;

	// Token: 0x04004BF7 RID: 19447
	public const int NONE = -1;

	// Token: 0x04004BF8 RID: 19448
	public const int FACTORY_MODEL = 0;

	// Token: 0x04004BF9 RID: 19449
	public const int VEGETATION = 1;

	// Token: 0x04004BFA RID: 19450
	public const int VEIN = 2;

	// Token: 0x04004BFB RID: 19451
	public const int PREBUILD = 3;

	// Token: 0x04004BFC RID: 19452
	public const int ENTITY = 4;

	// Token: 0x04004BFD RID: 19453
	public const int CRAFT = 5;

	// Token: 0x04004BFE RID: 19454
	public const int ENEMY = 6;

	// Token: 0x04004BFF RID: 19455
	public const int RUIN = 7;

	// Token: 0x04004C00 RID: 19456
	public const int RENDERING_BATCH = 8;

	// Token: 0x04004C01 RID: 19457
	public static Queue<PlanetData> genPlanetReqList = null;

	// Token: 0x04004C02 RID: 19458
	public static Queue<PlanetData> modPlanetReqList = null;

	// Token: 0x04004C03 RID: 19459
	public static Queue<PlanetData> fctPlanetReqList = null;

	// Token: 0x04004C04 RID: 19460
	public static Queue<PlanetData> calPlanetReqList = null;

	// Token: 0x04004C05 RID: 19461
	private static Thread planetComputeThread;

	// Token: 0x04004C06 RID: 19462
	private static PlanetModelingManager.ThreadFlag planetComputeThreadFlag;

	// Token: 0x04004C07 RID: 19463
	private static PlanetModelingManager.ThreadFlagLock planetComputeThreadFlagLock = new PlanetModelingManager.ThreadFlagLock();

	// Token: 0x04004C08 RID: 19464
	public static List<string> planetComputeThreadLogs = new List<string>();

	// Token: 0x04004C09 RID: 19465
	public static string planetComputeThreadError = "";

	// Token: 0x04004C0A RID: 19466
	private static Thread planetCalculateThread;

	// Token: 0x04004C0B RID: 19467
	private static PlanetModelingManager.ThreadFlag planetCalculateThreadFlag;

	// Token: 0x04004C0C RID: 19468
	private static PlanetModelingManager.ThreadFlagLock planetCalculateThreadFlagLock = new PlanetModelingManager.ThreadFlagLock();

	// Token: 0x04004C0D RID: 19469
	public static List<string> planetCalculateThreadLogs = new List<string>();

	// Token: 0x04004C0E RID: 19470
	public static string planetCalculateThreadError = "";

	// Token: 0x04004C0F RID: 19471
	private static PlanetData currentModelingPlanet;

	// Token: 0x04004C10 RID: 19472
	private static int currentModelingStage = 0;

	// Token: 0x04004C11 RID: 19473
	private static int currentModelingSeamNormal = 0;

	// Token: 0x04004C12 RID: 19474
	private static PlanetData currentFactingPlanet;

	// Token: 0x04004C13 RID: 19475
	private static int currentFactingStage = 0;

	// Token: 0x04004C14 RID: 19476
	private static List<Mesh> tmpMeshList = null;

	// Token: 0x04004C15 RID: 19477
	private static List<MeshRenderer> tmpMeshRendererList = null;

	// Token: 0x04004C16 RID: 19478
	private static List<MeshCollider> tmpMeshColliderList = null;

	// Token: 0x04004C17 RID: 19479
	private static Collider tmpOceanCollider;

	// Token: 0x04004C18 RID: 19480
	private static List<Vector3> tmpVerts = null;

	// Token: 0x04004C19 RID: 19481
	private static List<Vector3> tmpNorms = null;

	// Token: 0x04004C1A RID: 19482
	private static List<Vector4> tmpTgnts = null;

	// Token: 0x04004C1B RID: 19483
	private static List<Vector2> tmpUvs = null;

	// Token: 0x04004C1C RID: 19484
	private static List<Vector4> tmpUv2s = null;

	// Token: 0x04004C1D RID: 19485
	private static List<int> tmpTris = null;

	// Token: 0x04004C1E RID: 19486
	private static GameObject tmpPlanetGameObject;

	// Token: 0x04004C1F RID: 19487
	private static GameObject tmpPlanetBodyGameObject;

	// Token: 0x04004C20 RID: 19488
	private static GameObject tmpPlanetReformGameObject;

	// Token: 0x04004C21 RID: 19489
	private static MeshRenderer tmpPlanetReformRenderer;

	// Token: 0x04004C22 RID: 19490
	public static short[] vegeHps;

	// Token: 0x04004C23 RID: 19491
	public static Vector4[] vegeScaleRanges;

	// Token: 0x04004C24 RID: 19492
	public static VegeProto[] vegeProtos;

	// Token: 0x04004C25 RID: 19493
	public static int[] veinProducts;

	// Token: 0x04004C26 RID: 19494
	public static int[] veinModelIndexs;

	// Token: 0x04004C27 RID: 19495
	public static int[] veinModelCounts;

	// Token: 0x04004C28 RID: 19496
	public static VeinProto[] veinProtos;

	// Token: 0x04004C29 RID: 19497
	public static Camera heightmapCamera;

	// Token: 0x02000A67 RID: 2663
	private enum ThreadFlag
	{
		// Token: 0x04006490 RID: 25744
		Ended,
		// Token: 0x04006491 RID: 25745
		Running,
		// Token: 0x04006492 RID: 25746
		Ending
	}

	// Token: 0x02000A68 RID: 2664
	private class ThreadFlagLock
	{
		// Token: 0x04006493 RID: 25747
		private int obj;
	}
}
