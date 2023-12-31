using System;
using System.IO;

// Token: 0x020001B4 RID: 436
public class GameDesc
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001354 RID: 4948 RVA: 0x0014A604 File Offset: 0x00148804
	public float oilAmountMultiplier
	{
		get
		{
			if (this.resourceMultiplier > 0.1001f)
			{
				return 1f;
			}
			return 0.5f;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06001355 RID: 4949 RVA: 0x0014A61E File Offset: 0x0014881E
	public bool isInfiniteResource
	{
		get
		{
			return this.resourceMultiplier >= 99.5f;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001356 RID: 4950 RVA: 0x0014A630 File Offset: 0x00148830
	public bool isRareResource
	{
		get
		{
			return this.resourceMultiplier <= 0.1001f;
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001357 RID: 4951 RVA: 0x0014A644 File Offset: 0x00148844
	public float enemyDropMultiplier
	{
		get
		{
			float num;
			if (this.resourceMultiplier > 1f)
			{
				num = 1f + this.resourceMultiplier * 0.1f;
				if (num > 2f)
				{
					num = 2f;
				}
			}
			else
			{
				num = (float)(Math.Round(Math.Sqrt((double)this.resourceMultiplier) * 10.0) / 10.0);
			}
			return num;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001359 RID: 4953 RVA: 0x0014A6B0 File Offset: 0x001488B0
	public string clusterString
	{
		get
		{
			string text = (this.resourceMultiplier > 9.95f) ? "99" : (this.resourceMultiplier * 10f).ToString("00");
			string text2 = "-A";
			if (this.isSandboxMode)
			{
				text2 = "-S";
				return string.Concat(new string[]
				{
					this.galaxySeed.ToString("00000000"),
					"-",
					this.starCount.ToString(),
					text2,
					text
				});
			}
			if (this.isCombatMode)
			{
				text2 = "-Z";
				int num = 1;
				int combatModeDifficultyNumber = this.combatModeDifficultyNumber;
				string text3 = ((num * 100 + combatModeDifficultyNumber) % 100).ToString("00");
				return string.Concat(new string[]
				{
					this.galaxySeed.ToString("00000000"),
					"-",
					this.starCount.ToString(),
					text2,
					text,
					"-",
					text3
				});
			}
			return string.Concat(new string[]
			{
				this.galaxySeed.ToString("00000000"),
				"-",
				this.starCount.ToString(),
				text2,
				text
			});
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x0600135A RID: 4954 RVA: 0x0014A7F0 File Offset: 0x001489F0
	public string clusterStringLong
	{
		get
		{
			string text = (this.resourceMultiplier > 9.95f) ? "99" : (this.resourceMultiplier * 10f).ToString("00");
			string text2 = " - A";
			if (this.isSandboxMode)
			{
				text2 = " - S";
				return string.Concat(new string[]
				{
					this.galaxySeed.ToString("0000 0000"),
					" - ",
					this.starCount.ToString(),
					text2,
					text
				});
			}
			if (this.isCombatMode)
			{
				text2 = " - Z";
				int num = 1;
				int combatModeDifficultyNumber = this.combatModeDifficultyNumber;
				string text3 = ((num * 100 + combatModeDifficultyNumber) % 100).ToString("00");
				return string.Concat(new string[]
				{
					this.galaxySeed.ToString("0000 0000"),
					" - ",
					this.starCount.ToString(),
					text2,
					text,
					" - ",
					text3
				});
			}
			return string.Concat(new string[]
			{
				this.galaxySeed.ToString("0000 0000"),
				" - ",
				this.starCount.ToString(),
				text2,
				text
			});
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x0600135B RID: 4955 RVA: 0x0014A930 File Offset: 0x00148B30
	public long seedKey64
	{
		get
		{
			long num = (long)this.galaxySeed;
			int num2 = this.starCount;
			int num3 = (int)((double)(this.resourceMultiplier * 10f) + 0.5);
			if (num2 > 999)
			{
				num2 = 999;
			}
			else if (num2 < 1)
			{
				num2 = 1;
			}
			if (num3 > 99)
			{
				num3 = 99;
			}
			else if (num3 < 1)
			{
				num3 = 1;
			}
			int num4 = 0;
			if (this.isSandboxMode)
			{
				num4 = 999;
			}
			else if (this.isCombatMode)
			{
				int num5 = 1;
				int combatModeDifficultyNumber = this.combatModeDifficultyNumber;
				num4 = num5 * 100 + combatModeDifficultyNumber;
			}
			return num * 100000000L + (long)num2 * 100000L + (long)num3 * 1000L + (long)num4;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x0600135C RID: 4956 RVA: 0x0014A9D0 File Offset: 0x00148BD0
	public bool isCombatMode
	{
		get
		{
			return !this.isPeaceMode;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x0600135D RID: 4957 RVA: 0x0014A9DC File Offset: 0x00148BDC
	public float propertyMultiplier
	{
		get
		{
			if (this.isSandboxMode)
			{
				return 0f;
			}
			float num = this.isCombatMode ? this.combatSettings.difficulty : 0f;
			float num2;
			if (this.resourceMultiplier <= 0.15f)
			{
				num2 = 4f;
			}
			else if (this.resourceMultiplier <= 0.65f)
			{
				num2 = 2f;
			}
			else if (this.resourceMultiplier > 0.65f && this.resourceMultiplier <= 0.9f)
			{
				num2 = 1.5f;
			}
			else if (this.resourceMultiplier > 0.9f && this.resourceMultiplier <= 1.25f)
			{
				num2 = 1f;
			}
			else if (this.resourceMultiplier > 1.25f && this.resourceMultiplier <= 1.75f)
			{
				num2 = 0.9f;
			}
			else if (this.resourceMultiplier > 1.75f && this.resourceMultiplier <= 2.5f)
			{
				num2 = 0.8f;
			}
			else if (this.resourceMultiplier > 2.5f && this.resourceMultiplier <= 4f)
			{
				num2 = 0.7f;
			}
			else if (this.resourceMultiplier > 4f && this.resourceMultiplier <= 6.5f)
			{
				num2 = 0.6f;
			}
			else if (this.resourceMultiplier > 6.5f && this.resourceMultiplier <= 8.5f)
			{
				num2 = 0.5f;
			}
			else
			{
				num2 = 0.4f;
			}
			num2 += num * (num2 * 0.5f + 0.5f);
			if (num >= 9.999f)
			{
				num2 += 1f;
			}
			return (float)((int)((double)(num2 * 100f) + 0.5)) / 100f;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x0600135E RID: 4958 RVA: 0x0014AB84 File Offset: 0x00148D84
	public int combatModeDifficultyNumber
	{
		get
		{
			float difficulty = this.combatSettings.difficulty;
			int num = (int)(difficulty * 10f + 0.001f);
			if (num >= 100)
			{
				num = 99;
			}
			if (num == 0 && difficulty > 0.001f)
			{
				num = 1;
			}
			return num;
		}
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0014ABC4 File Offset: 0x00148DC4
	public void SetForNewGame(int _galaxyAlgo, int _galaxySeed, int _starCount, int _playerProto, float _resourceMultiplier)
	{
		this.creationTime = DateTime.UtcNow;
		this.creationVersion = GameConfig.gameVersion;
		this.galaxyAlgo = _galaxyAlgo;
		this.galaxySeed = _galaxySeed;
		this.starCount = _starCount;
		this.playerProto = _playerProto;
		this.resourceMultiplier = _resourceMultiplier;
		ThemeProtoSet themes = LDB.themes;
		int length = themes.Length;
		this.savedThemeIds = new int[length];
		for (int i = 0; i < length; i++)
		{
			this.savedThemeIds[i] = themes.dataArray[i].ID;
		}
		DateTime t = new DateTime(2021, 9, 29, 0, 0, 0);
		this.achievementEnable = (DateTime.Compare(this.creationTime, t) > 0);
		this.isPeaceMode = true;
		this.isSandboxMode = false;
		this.combatSettings.SetDefault();
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x0014AC88 File Offset: 0x00148E88
	public void CopyTo(GameDesc other)
	{
		if (other == null)
		{
			return;
		}
		other.creationTime = this.creationTime;
		other.creationVersion = this.creationVersion;
		other.galaxyAlgo = this.galaxyAlgo;
		other.starCount = this.starCount;
		other.playerProto = this.playerProto;
		other.resourceMultiplier = this.resourceMultiplier;
		if (other.savedThemeIds == null || other.savedThemeIds.Length != this.savedThemeIds.Length)
		{
			other.savedThemeIds = new int[this.savedThemeIds.Length];
		}
		Array.Copy(this.savedThemeIds, other.savedThemeIds, this.savedThemeIds.Length);
		other.achievementEnable = this.achievementEnable;
		other.isPeaceMode = this.isPeaceMode;
		other.isSandboxMode = this.isSandboxMode;
		other.combatSettings = this.combatSettings;
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x0014AD58 File Offset: 0x00148F58
	public void Export(BinaryWriter w)
	{
		w.Write(8);
		w.Write(this.creationTime.Ticks);
		w.Write(this.creationVersion.Major);
		w.Write(this.creationVersion.Minor);
		w.Write(this.creationVersion.Release);
		w.Write(this.creationVersion.Build);
		w.Write(this.galaxyAlgo);
		w.Write(this.galaxySeed);
		w.Write(this.starCount);
		w.Write(this.playerProto);
		w.Write(this.resourceMultiplier);
		int num = this.savedThemeIds.Length;
		w.Write(num);
		for (int i = 0; i < num; i++)
		{
			w.Write(this.savedThemeIds[i]);
		}
		w.Write(this.achievementEnable);
		w.Write(this.isPeaceMode);
		w.Write(this.isSandboxMode);
		this.combatSettings.Export(w);
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x0014AE58 File Offset: 0x00149058
	public void Import(BinaryReader r)
	{
		int num = r.ReadInt32();
		if (num >= 3)
		{
			this.creationTime = new DateTime(r.ReadInt64());
		}
		else
		{
			this.creationTime = new DateTime(2021, 1, 21, 6, 58, 30);
		}
		this.creationTime = DateTime.SpecifyKind(this.creationTime, DateTimeKind.Utc);
		this.creationVersion = new Version(0, 0, 0);
		if (num >= 5)
		{
			if (num >= 6)
			{
				this.creationVersion.Major = r.ReadInt32();
				this.creationVersion.Minor = r.ReadInt32();
				this.creationVersion.Release = r.ReadInt32();
				this.creationVersion.Build = r.ReadInt32();
			}
			else
			{
				this.creationVersion.Build = r.ReadInt32();
			}
		}
		this.galaxyAlgo = r.ReadInt32();
		this.galaxySeed = r.ReadInt32();
		this.starCount = r.ReadInt32();
		this.playerProto = r.ReadInt32();
		if (num >= 2)
		{
			this.resourceMultiplier = r.ReadSingle();
		}
		else
		{
			this.resourceMultiplier = 1f;
		}
		if (num >= 1)
		{
			int num2 = r.ReadInt32();
			this.savedThemeIds = new int[num2];
			for (int i = 0; i < num2; i++)
			{
				this.savedThemeIds[i] = r.ReadInt32();
			}
		}
		else
		{
			ThemeProtoSet themes = LDB.themes;
			int length = themes.Length;
			this.savedThemeIds = new int[length];
			for (int j = 0; j < length; j++)
			{
				this.savedThemeIds[j] = themes.dataArray[j].ID;
			}
		}
		if (num >= 4)
		{
			this.achievementEnable = r.ReadBoolean();
		}
		else
		{
			DateTime t = new DateTime(2021, 9, 29, 0, 0, 0);
			this.achievementEnable = (DateTime.Compare(this.creationTime, t) > 0);
		}
		if (num >= 7)
		{
			this.isPeaceMode = r.ReadBoolean();
			this.isSandboxMode = r.ReadBoolean();
		}
		else
		{
			this.isPeaceMode = true;
			this.isSandboxMode = false;
		}
		if (num >= 8)
		{
			this.combatSettings.Import(r);
			return;
		}
		this.combatSettings.SetDefault();
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x0014B060 File Offset: 0x00149260
	public static bool IsCombatModeSeedKey(long seedKey)
	{
		long num = seedKey % 1000L;
		return num >= 100L && num <= 199L;
	}

	// Token: 0x040017BE RID: 6078
	public DateTime creationTime;

	// Token: 0x040017BF RID: 6079
	public Version creationVersion;

	// Token: 0x040017C0 RID: 6080
	public int galaxyAlgo;

	// Token: 0x040017C1 RID: 6081
	public int galaxySeed;

	// Token: 0x040017C2 RID: 6082
	public int starCount;

	// Token: 0x040017C3 RID: 6083
	public int playerProto;

	// Token: 0x040017C4 RID: 6084
	public float resourceMultiplier;

	// Token: 0x040017C5 RID: 6085
	public int[] savedThemeIds;

	// Token: 0x040017C6 RID: 6086
	public bool achievementEnable;

	// Token: 0x040017C7 RID: 6087
	public bool isPeaceMode;

	// Token: 0x040017C8 RID: 6088
	public bool isSandboxMode;

	// Token: 0x040017C9 RID: 6089
	public CombatSettings combatSettings;

	// Token: 0x040017CA RID: 6090
	public const float RARE_RESOURCE_MULTIPLIER = 0.1f;

	// Token: 0x040017CB RID: 6091
	public const float INFINITE_RESOURCE_MULTIPLIER = 100f;
}
