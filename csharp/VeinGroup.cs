using System;
using UnityEngine;

// Token: 0x020001EA RID: 490
public struct VeinGroup
{
	// Token: 0x06001664 RID: 5732 RVA: 0x001837FC File Offset: 0x001819FC
	public void SetNull()
	{
		this.type = EVeinType.None;
		this.pos.x = 0f;
		this.pos.y = 0f;
		this.pos.z = 0f;
		this.count = 0;
		this.amount = 0L;
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001665 RID: 5733 RVA: 0x0018384F File Offset: 0x00181A4F
	public bool isNull
	{
		get
		{
			return this.type == EVeinType.None;
		}
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x0018385C File Offset: 0x00181A5C
	public override string ToString()
	{
		return string.Format("[{0}] {1:N0} | {2}   @ {3}", new object[]
		{
			this.type,
			this.amount,
			this.count,
			this.pos
		});
	}

	// Token: 0x04001CDB RID: 7387
	public EVeinType type;

	// Token: 0x04001CDC RID: 7388
	public Vector3 pos;

	// Token: 0x04001CDD RID: 7389
	public int count;

	// Token: 0x04001CDE RID: 7390
	public long amount;
}
