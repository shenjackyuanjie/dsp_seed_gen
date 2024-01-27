using System;

// Token: 0x02000272 RID: 626
[Serializable]
public abstract class Proto
{
	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06001B76 RID: 7030 RVA: 0x001DD014 File Offset: 0x001DB214
	// (set) Token: 0x06001B77 RID: 7031 RVA: 0x001DD01C File Offset: 0x001DB21C
	public string name { get; set; }

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06001B78 RID: 7032 RVA: 0x001DD025 File Offset: 0x001DB225
	// (set) Token: 0x06001B79 RID: 7033 RVA: 0x001DD02D File Offset: 0x001DB22D
	public string sid { get; set; }

	// Token: 0x040022EA RID: 8938
	public string Name;

	// Token: 0x040022EB RID: 8939
	public int ID;

	// Token: 0x040022EC RID: 8940
	public string SID;
}
