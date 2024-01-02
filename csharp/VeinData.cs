using System;
using System.IO;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public struct VeinData
{
	// Token: 0x0600165B RID: 5723 RVA: 0x00182CF0 File Offset: 0x00180EF0
	public void SetNull()
	{
		this.id = 0;
		this.type = EVeinType.None;
		this.modelIndex = 0;
		this.groupIndex = 0;
		this.amount = 0;
		this.productId = 0;
		this.pos.x = 0f;
		this.pos.y = 0f;
		this.pos.z = 0f;
		this.combatStatId = 0;
		this.minerCount = 0;
		this.minerId0 = 0;
		this.minerId1 = 0;
		this.minerId2 = 0;
		this.minerId3 = 0;
		this.hashAddress = 0;
		this.modelId = 0;
		this.colliderId = 0;
		this.minerBaseModelId = 0;
		this.minerCircleModelId0 = 0;
		this.minerCircleModelId1 = 0;
		this.minerCircleModelId2 = 0;
		this.minerCircleModelId3 = 0;
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x00182DBC File Offset: 0x00180FBC
	public void Export(BinaryWriter w)
	{
		w.Write(1);
		w.Write(this.id);
		w.Write((short)this.type);
		w.Write(this.modelIndex);
		w.Write(this.groupIndex);
		w.Write(this.amount);
		w.Write(this.productId);
		w.Write(this.pos.x);
		w.Write(this.pos.y);
		w.Write(this.pos.z);
		w.Write(this.combatStatId);
		w.Write(this.minerCount);
		w.Write(this.minerId0);
		w.Write(this.minerId1);
		w.Write(this.minerId2);
		w.Write(this.minerId3);
		w.Write(this.hashAddress);
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x00182EA0 File Offset: 0x001810A0
	public void Import(BinaryReader r)
	{
		byte b = r.ReadByte();
		this.id = r.ReadInt32();
		this.type = (EVeinType)r.ReadInt16();
		this.modelIndex = r.ReadInt16();
		this.groupIndex = r.ReadInt16();
		this.amount = r.ReadInt32();
		this.productId = r.ReadInt32();
		this.pos.x = r.ReadSingle();
		this.pos.y = r.ReadSingle();
		this.pos.z = r.ReadSingle();
		if (b >= 1)
		{
			this.combatStatId = r.ReadInt32();
		}
		else
		{
			this.combatStatId = 0;
		}
		this.minerCount = r.ReadInt32();
		this.minerId0 = r.ReadInt32();
		this.minerId1 = r.ReadInt32();
		this.minerId2 = r.ReadInt32();
		this.minerId3 = r.ReadInt32();
		if (b >= 1)
		{
			this.hashAddress = r.ReadInt32();
			return;
		}
		this.hashAddress = 0;
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x00182F9C File Offset: 0x0018119C
	public void AddMiner(int minerId)
	{
		if (this.minerId0 == minerId)
		{
			return;
		}
		if (this.minerId1 == minerId)
		{
			return;
		}
		if (this.minerId2 == minerId)
		{
			return;
		}
		if (this.minerId3 == minerId)
		{
			return;
		}
		if (this.minerId0 == 0)
		{
			this.minerId0 = minerId;
			this.minerCount++;
			return;
		}
		if (this.minerId1 == 0)
		{
			this.minerId1 = minerId;
			this.minerCount++;
			return;
		}
		if (this.minerId2 == 0)
		{
			this.minerId2 = minerId;
			this.minerCount++;
			return;
		}
		if (this.minerId3 == 0)
		{
			this.minerId3 = minerId;
			this.minerCount++;
		}
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x00183048 File Offset: 0x00181248
	public void RemoveMiner(int minerId)
	{
		if (this.minerId0 == minerId)
		{
			this.minerId0 = 0;
			this.minerCount--;
			this.minerId0 = this.minerId1;
			this.minerId1 = this.minerId2;
			this.minerId2 = this.minerId3;
			this.minerId3 = 0;
			return;
		}
		if (this.minerId1 == minerId)
		{
			this.minerId1 = 0;
			this.minerCount--;
			this.minerId1 = this.minerId2;
			this.minerId2 = this.minerId3;
			this.minerId3 = 0;
			return;
		}
		if (this.minerId2 == minerId)
		{
			this.minerId2 = 0;
			this.minerCount--;
			this.minerId2 = this.minerId3;
			this.minerId3 = 0;
			return;
		}
		if (this.minerId3 == minerId)
		{
			this.minerId3 = 0;
			this.minerCount--;
		}
	}

	// Token: 0x04001CC5 RID: 7365
	public int id;

	// Token: 0x04001CC6 RID: 7366
	public EVeinType type;

	// Token: 0x04001CC7 RID: 7367
	public short modelIndex;

	// Token: 0x04001CC8 RID: 7368
	public short groupIndex;

	// Token: 0x04001CC9 RID: 7369
	public int amount;

	// Token: 0x04001CCA RID: 7370
	public int productId;

	// Token: 0x04001CCB RID: 7371
	public Vector3 pos;

	// Token: 0x04001CCC RID: 7372
	public int combatStatId;

	// Token: 0x04001CCD RID: 7373
	public int minerCount;

	// Token: 0x04001CCE RID: 7374
	public int minerId0;

	// Token: 0x04001CCF RID: 7375
	public int minerId1;

	// Token: 0x04001CD0 RID: 7376
	public int minerId2;

	// Token: 0x04001CD1 RID: 7377
	public int minerId3;

	// Token: 0x04001CD2 RID: 7378
	public int hashAddress;

	// Token: 0x04001CD3 RID: 7379
	public int modelId;

	// Token: 0x04001CD4 RID: 7380
	public int colliderId;

	// Token: 0x04001CD5 RID: 7381
	public int minerBaseModelId;

	// Token: 0x04001CD6 RID: 7382
	public int minerCircleModelId0;

	// Token: 0x04001CD7 RID: 7383
	public int minerCircleModelId1;

	// Token: 0x04001CD8 RID: 7384
	public int minerCircleModelId2;

	// Token: 0x04001CD9 RID: 7385
	public int minerCircleModelId3;

	// Token: 0x04001CDA RID: 7386
	public static float oilSpeedMultiplier = 4E-05f;
}
