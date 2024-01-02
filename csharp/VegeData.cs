using System;
using System.IO;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public struct VegeData
{
	// Token: 0x06001658 RID: 5720 RVA: 0x0018298C File Offset: 0x00180B8C
	public void SetNull()
	{
		this.id = 0;
		this.protoId = 0;
		this.modelIndex = 0;
		this.hash.SetEmpty();
		this.hashAddress = 0;
		this.combatStatId = 0;
		this.pos.x = 0f;
		this.pos.y = 0f;
		this.pos.z = 0f;
		this.rot.x = 0f;
		this.rot.y = 0f;
		this.rot.z = 0f;
		this.rot.w = 0f;
		this.scl.x = 0f;
		this.scl.y = 0f;
		this.scl.z = 0f;
		this.modelId = 0;
		this.colliderId = 0;
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x00182A78 File Offset: 0x00180C78
	public void Export(BinaryWriter w)
	{
		w.Write(1);
		w.Write(this.id);
		w.Write(this.protoId);
		w.Write(this.modelIndex);
		w.Write(this.hash.bits);
		w.Write(this.hashAddress);
		w.Write(this.combatStatId);
		w.Write(this.pos.x);
		w.Write(this.pos.y);
		w.Write(this.pos.z);
		w.Write(this.rot.x);
		w.Write(this.rot.y);
		w.Write(this.rot.z);
		w.Write(this.rot.w);
		w.Write(this.scl.x);
		w.Write(this.scl.y);
		w.Write(this.scl.z);
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x00182B84 File Offset: 0x00180D84
	public void Import(BinaryReader r)
	{
		byte b = r.ReadByte();
		this.id = r.ReadInt32();
		this.protoId = r.ReadInt16();
		this.modelIndex = r.ReadInt16();
		if (b >= 1)
		{
			this.hash.bits = r.ReadUInt32();
			this.hashAddress = r.ReadInt32();
			this.combatStatId = r.ReadInt32();
		}
		else
		{
			r.ReadInt16();
			this.hash.bits = 0U;
			this.hashAddress = 0;
			this.combatStatId = 0;
		}
		this.pos.x = r.ReadSingle();
		this.pos.y = r.ReadSingle();
		this.pos.z = r.ReadSingle();
		this.rot.x = r.ReadSingle();
		this.rot.y = r.ReadSingle();
		this.rot.z = r.ReadSingle();
		this.rot.w = r.ReadSingle();
		this.scl.x = r.ReadSingle();
		this.scl.y = r.ReadSingle();
		this.scl.z = r.ReadSingle();
		if (this.id > 0 && b < 1)
		{
			this.hash.InitHashBits(this.pos.x, this.pos.y, this.pos.z);
		}
	}

	// Token: 0x04001CA9 RID: 7337
	public int id;

	// Token: 0x04001CAA RID: 7338
	public short protoId;

	// Token: 0x04001CAB RID: 7339
	public short modelIndex;

	// Token: 0x04001CAC RID: 7340
	public SimpleHash hash;

	// Token: 0x04001CAD RID: 7341
	public int hashAddress;

	// Token: 0x04001CAE RID: 7342
	public int combatStatId;

	// Token: 0x04001CAF RID: 7343
	public Vector3 pos;

	// Token: 0x04001CB0 RID: 7344
	public Quaternion rot;

	// Token: 0x04001CB1 RID: 7345
	public Vector3 scl;

	// Token: 0x04001CB2 RID: 7346
	public int modelId;

	// Token: 0x04001CB3 RID: 7347
	public int colliderId;
}
