using System;
using System.IO;

// Token: 0x020001B6 RID: 438
public struct CombatSettings
{
	// Token: 0x06001365 RID: 4965 RVA: 0x0014B08C File Offset: 0x0014928C
	public void SetDefault()
	{
		this.aggressiveness = 1f;
		this.initialLevel = 0f;
		this.initialGrowth = 1f;
		this.initialColonize = 1f;
		this.maxDensity = 1f;
		this.growthSpeedFactor = 1f;
		this.powerThreatFactor = 1f;
		this.battleThreatFactor = 1f;
		this.battleExpFactor = 1f;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x0014B0FC File Offset: 0x001492FC
	public void Export(BinaryWriter w)
	{
		w.Write(0);
		w.Write(this.aggressiveness);
		w.Write(this.initialLevel);
		w.Write(this.initialGrowth);
		w.Write(this.initialColonize);
		w.Write(this.maxDensity);
		w.Write(this.growthSpeedFactor);
		w.Write(this.powerThreatFactor);
		w.Write(this.battleThreatFactor);
		w.Write(this.battleExpFactor);
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0014B17C File Offset: 0x0014937C
	public void Import(BinaryReader r)
	{
		r.ReadInt32();
		this.aggressiveness = r.ReadSingle();
		this.initialLevel = r.ReadSingle();
		this.initialGrowth = r.ReadSingle();
		this.initialColonize = r.ReadSingle();
		this.maxDensity = r.ReadSingle();
		this.growthSpeedFactor = r.ReadSingle();
		this.powerThreatFactor = r.ReadSingle();
		this.battleThreatFactor = r.ReadSingle();
		this.battleExpFactor = r.ReadSingle();
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06001368 RID: 4968 RVA: 0x0014B1FC File Offset: 0x001493FC
	public EAggressiveLevel aggressiveLevel
	{
		get
		{
			return (EAggressiveLevel)((this.aggressiveness + 1f) * 10f + 0.5f);
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06001369 RID: 4969 RVA: 0x0014B218 File Offset: 0x00149418
	public float aggressiveHatredCoef
	{
		get
		{
			EAggressiveLevel eaggressiveLevel = (EAggressiveLevel)((this.aggressiveness + 1f) * 10f + 0.5f);
			if (eaggressiveLevel <= EAggressiveLevel.Torpid)
			{
				if (eaggressiveLevel == EAggressiveLevel.Passive || eaggressiveLevel == EAggressiveLevel.Torpid)
				{
					return 0.6f;
				}
			}
			else
			{
				if (eaggressiveLevel == EAggressiveLevel.Normal)
				{
					return 1f;
				}
				if (eaggressiveLevel == EAggressiveLevel.Sharp)
				{
					return 4f;
				}
				if (eaggressiveLevel == EAggressiveLevel.Rampage)
				{
					return 6f;
				}
			}
			return 0f;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x0600136A RID: 4970 RVA: 0x0014B27E File Offset: 0x0014947E
	public bool isEnemyPassive
	{
		get
		{
			return this.aggressiveness <= 0f;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600136B RID: 4971 RVA: 0x0014B290 File Offset: 0x00149490
	public bool isEnemyHostile
	{
		get
		{
			return this.aggressiveness >= 0f;
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x0600136C RID: 4972 RVA: 0x0014B2A4 File Offset: 0x001494A4
	public float difficulty
	{
		get
		{
			float num;
			if (this.aggressiveness < -0.1f)
			{
				num = -0.2f;
			}
			else if (this.aggressiveness < 0.25f)
			{
				num = 0f;
			}
			else if (this.aggressiveness < 0.75f)
			{
				num = 0.5f;
			}
			else if (this.aggressiveness < 1.5f)
			{
				num = 0.75f;
			}
			else if (this.aggressiveness < 2.5f)
			{
				num = 0.875f;
			}
			else
			{
				num = 1.125f;
			}
			float num2 = this.initialLevel * 0.8f;
			float num3;
			if (this.initialGrowth < 0.15f)
			{
				num3 = 0f;
			}
			else if (this.initialGrowth < 0.3f)
			{
				num3 = 0.25f;
			}
			else if (this.initialGrowth < 0.65f)
			{
				num3 = 0.5f;
			}
			else if (this.initialGrowth < 0.8f)
			{
				num3 = 0.75f;
			}
			else if (this.initialGrowth < 1.15f)
			{
				num3 = 1f;
			}
			else if (this.initialGrowth < 1.65f)
			{
				num3 = 1.25f;
			}
			else
			{
				num3 = 1.5f;
			}
			float num4;
			if (this.initialColonize < 0.15f)
			{
				num4 = 0f;
			}
			else if (this.initialColonize < 0.3f)
			{
				num4 = 0.25f;
			}
			else if (this.initialColonize < 0.65f)
			{
				num4 = 0.5f;
			}
			else if (this.initialColonize < 0.8f)
			{
				num4 = 0.75f;
			}
			else if (this.initialColonize < 1.15f)
			{
				num4 = 1f;
			}
			else if (this.initialColonize < 1.65f)
			{
				num4 = 1.25f;
			}
			else
			{
				num4 = 1.5f;
			}
			float num5 = this.maxDensity - 1f;
			float num6;
			if (this.growthSpeedFactor < 0.35f)
			{
				num6 = 0.3f;
			}
			else if (this.growthSpeedFactor < 0.75f)
			{
				num6 = 0.7f;
			}
			else if (this.growthSpeedFactor < 1.5f)
			{
				num6 = 1f;
			}
			else if (this.growthSpeedFactor < 2.5f)
			{
				num6 = 1.2f;
			}
			else
			{
				num6 = 1.5f;
			}
			float num7;
			if (this.powerThreatFactor < 0.05f)
			{
				num7 = 0.125f;
			}
			else if (this.powerThreatFactor < 0.15f)
			{
				num7 = 0.3f;
			}
			else if (this.powerThreatFactor < 0.25f)
			{
				num7 = 0.6f;
			}
			else if (this.powerThreatFactor < 0.55f)
			{
				num7 = 0.8f;
			}
			else if (this.powerThreatFactor < 1.15f)
			{
				num7 = 1f;
			}
			else if (this.powerThreatFactor < 2.15f)
			{
				num7 = 1.2f;
			}
			else if (this.powerThreatFactor < 5.15f)
			{
				num7 = 1.5f;
			}
			else if (this.powerThreatFactor < 8.15f)
			{
				num7 = 1.8f;
			}
			else
			{
				num7 = 2f;
			}
			float num8;
			if (this.battleThreatFactor < 0.05f)
			{
				num8 = 0.125f;
			}
			else if (this.battleThreatFactor < 0.15f)
			{
				num8 = 0.3f;
			}
			else if (this.battleThreatFactor < 0.25f)
			{
				num8 = 0.6f;
			}
			else if (this.battleThreatFactor < 0.55f)
			{
				num8 = 0.8f;
			}
			else if (this.battleThreatFactor < 1.15f)
			{
				num8 = 1f;
			}
			else if (this.battleThreatFactor < 2.15f)
			{
				num8 = 1.2f;
			}
			else if (this.battleThreatFactor < 5.15f)
			{
				num8 = 1.5f;
			}
			else if (this.battleThreatFactor < 8.15f)
			{
				num8 = 1.8f;
			}
			else
			{
				num8 = 2f;
			}
			float num9;
			if (this.battleExpFactor < 0.05f)
			{
				num9 = 0f;
			}
			else if (this.battleExpFactor < 0.15f)
			{
				num9 = 1f;
			}
			else if (this.battleExpFactor < 0.25f)
			{
				num9 = 3f;
			}
			else if (this.battleExpFactor < 0.55f)
			{
				num9 = 6f;
			}
			else if (this.battleExpFactor < 1.15f)
			{
				num9 = 10f;
			}
			else if (this.battleExpFactor < 2.15f)
			{
				num9 = 12f;
			}
			else if (this.battleExpFactor < 5.15f)
			{
				num9 = 14f;
			}
			else if (this.battleExpFactor < 8.15f)
			{
				num9 = 16f;
			}
			else
			{
				num9 = 18f;
			}
			float num10 = (num < 0f) ? 0f : (0.25f + num * (num7 * 0.5f + num8 * 0.5f));
			float num11 = 0.375f + 0.625f * ((num2 + num9) / 10f);
			float num12 = 0.375f + 0.625f * ((num4 * 0.6f + num3 * 0.4f * (num4 * 0.75f + 0.25f)) * 0.6f + num6 * 0.4f * (num4 * 0.8f + 0.2f) + num5 * 0.29f * (num4 * 0.5f + 0.5f));
			return (float)((int)(num10 * num11 * num12 * 10000f + 0.5f)) / 10000f;
		}
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x0014B7D0 File Offset: 0x001499D0
	public static bool operator ==(CombatSettings lhs, CombatSettings rhs)
	{
		return lhs.aggressiveness == rhs.aggressiveness && lhs.initialLevel == rhs.initialLevel && lhs.initialGrowth == rhs.initialGrowth && lhs.initialColonize == rhs.initialColonize && lhs.maxDensity == rhs.maxDensity && lhs.growthSpeedFactor == rhs.growthSpeedFactor && lhs.powerThreatFactor == rhs.powerThreatFactor && lhs.battleThreatFactor == rhs.battleThreatFactor && lhs.battleExpFactor == rhs.battleExpFactor;
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x0014B860 File Offset: 0x00149A60
	public static bool operator !=(CombatSettings lhs, CombatSettings rhs)
	{
		return lhs.aggressiveness != rhs.aggressiveness || lhs.initialLevel != rhs.initialLevel || lhs.initialGrowth != rhs.initialGrowth || lhs.initialColonize != rhs.initialColonize || lhs.maxDensity != rhs.maxDensity || lhs.growthSpeedFactor != rhs.growthSpeedFactor || lhs.powerThreatFactor != rhs.powerThreatFactor || lhs.battleThreatFactor != rhs.battleThreatFactor || lhs.battleExpFactor != rhs.battleExpFactor;
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x0014B8F0 File Offset: 0x00149AF0
	public override bool Equals(object obj)
	{
		return obj is CombatSettings && this == (CombatSettings)obj;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0014B90D File Offset: 0x00149B0D
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x040017D3 RID: 6099
	public float aggressiveness;

	// Token: 0x040017D4 RID: 6100
	public float initialLevel;

	// Token: 0x040017D5 RID: 6101
	public float initialGrowth;

	// Token: 0x040017D6 RID: 6102
	public float initialColonize;

	// Token: 0x040017D7 RID: 6103
	public float maxDensity;

	// Token: 0x040017D8 RID: 6104
	public float growthSpeedFactor;

	// Token: 0x040017D9 RID: 6105
	public float powerThreatFactor;

	// Token: 0x040017DA RID: 6106
	public float battleThreatFactor;

	// Token: 0x040017DB RID: 6107
	public float battleExpFactor;

	// Token: 0x040017DC RID: 6108
	public const float AGG_VALUE_0_DUMMY = -1f;

	// Token: 0x040017DD RID: 6109
	public const float AGG_VALUE_1_PASSIVE = 0f;

	// Token: 0x040017DE RID: 6110
	public const float AGG_VALUE_2_TORPID = 0.5f;

	// Token: 0x040017DF RID: 6111
	public const float AGG_VALUE_3_NORMAL = 1f;

	// Token: 0x040017E0 RID: 6112
	public const float AGG_VALUE_4_SHARP = 2f;

	// Token: 0x040017E1 RID: 6113
	public const float AGG_VALUE_5_RAMPAGE = 3f;

	// Token: 0x040017E2 RID: 6114
	public const float INITIALLEVEL_0 = 0f;

	// Token: 0x040017E3 RID: 6115
	public const float INITIALLEVEL_1 = 1f;

	// Token: 0x040017E4 RID: 6116
	public const float INITIALLEVEL_2 = 2f;

	// Token: 0x040017E5 RID: 6117
	public const float INITIALLEVEL_3 = 3f;

	// Token: 0x040017E6 RID: 6118
	public const float INITIALLEVEL_4 = 4f;

	// Token: 0x040017E7 RID: 6119
	public const float INITIALLEVEL_5 = 5f;

	// Token: 0x040017E8 RID: 6120
	public const float INITIALLEVEL_6 = 6f;

	// Token: 0x040017E9 RID: 6121
	public const float INITIALLEVEL_7 = 7f;

	// Token: 0x040017EA RID: 6122
	public const float INITIALLEVEL_8 = 8f;

	// Token: 0x040017EB RID: 6123
	public const float INITIALLEVEL_9 = 9f;

	// Token: 0x040017EC RID: 6124
	public const float INITIALLEVEL_10 = 10f;

	// Token: 0x040017ED RID: 6125
	public const float INITIAL_GROWTH_LEVEL_0 = 0f;

	// Token: 0x040017EE RID: 6126
	public const float INITIAL_GROWTH_LEVEL_1 = 0.25f;

	// Token: 0x040017EF RID: 6127
	public const float INITIAL_GROWTH_LEVEL_2 = 0.5f;

	// Token: 0x040017F0 RID: 6128
	public const float INITIAL_GROWTH_LEVEL_3 = 0.75f;

	// Token: 0x040017F1 RID: 6129
	public const float INITIAL_GROWTH_LEVEL_4 = 1f;

	// Token: 0x040017F2 RID: 6130
	public const float INITIAL_GROWTH_LEVEL_5 = 1.5f;

	// Token: 0x040017F3 RID: 6131
	public const float INITIAL_GROWTH_LEVEL_6 = 2f;

	// Token: 0x040017F4 RID: 6132
	public const float INITIAL_EXPAND_LEVEL_0 = 0.01f;

	// Token: 0x040017F5 RID: 6133
	public const float INITIAL_EXPAND_LEVEL_1 = 0.25f;

	// Token: 0x040017F6 RID: 6134
	public const float INITIAL_EXPAND_LEVEL_2 = 0.5f;

	// Token: 0x040017F7 RID: 6135
	public const float INITIAL_EXPAND_LEVEL_3 = 0.75f;

	// Token: 0x040017F8 RID: 6136
	public const float INITIAL_EXPAND_LEVEL_4 = 1f;

	// Token: 0x040017F9 RID: 6137
	public const float INITIAL_EXPAND_LEVEL_5 = 1.5f;

	// Token: 0x040017FA RID: 6138
	public const float INITIAL_EXPAND_LEVEL_6 = 2f;

	// Token: 0x040017FB RID: 6139
	public const float MAX_DENSITY_LEVEL_0 = 1f;

	// Token: 0x040017FC RID: 6140
	public const float MAX_DENSITY_LEVEL_1 = 1.5f;

	// Token: 0x040017FD RID: 6141
	public const float MAX_DENSITY_LEVEL_2 = 2f;

	// Token: 0x040017FE RID: 6142
	public const float MAX_DENSITY_LEVEL_3 = 2.5f;

	// Token: 0x040017FF RID: 6143
	public const float MAX_DENSITY_LEVEL_4 = 3f;

	// Token: 0x04001800 RID: 6144
	public const float GROWTH_SPEED_0 = 0.25f;

	// Token: 0x04001801 RID: 6145
	public const float GROWTH_SPEED_1 = 0.5f;

	// Token: 0x04001802 RID: 6146
	public const float GROWTH_SPEED_2 = 1f;

	// Token: 0x04001803 RID: 6147
	public const float GROWTH_SPEED_3 = 2f;

	// Token: 0x04001804 RID: 6148
	public const float GROWTH_SPEED_4 = 3f;

	// Token: 0x04001805 RID: 6149
	public const float POWER_THREAT_LEVEL_0 = 0.01f;

	// Token: 0x04001806 RID: 6150
	public const float POWER_THREAT_LEVEL_1 = 0.1f;

	// Token: 0x04001807 RID: 6151
	public const float POWER_THREAT_LEVEL_2 = 0.2f;

	// Token: 0x04001808 RID: 6152
	public const float POWER_THREAT_LEVEL_3 = 0.5f;

	// Token: 0x04001809 RID: 6153
	public const float POWER_THREAT_LEVEL_4 = 1f;

	// Token: 0x0400180A RID: 6154
	public const float POWER_THREAT_LEVEL_5 = 2f;

	// Token: 0x0400180B RID: 6155
	public const float POWER_THREAT_LEVEL_6 = 5f;

	// Token: 0x0400180C RID: 6156
	public const float POWER_THREAT_LEVEL_7 = 8f;

	// Token: 0x0400180D RID: 6157
	public const float POWER_THREAT_LEVEL_8 = 10f;

	// Token: 0x0400180E RID: 6158
	public const float COMBAT_THREAT_LEVEL_0 = 0.01f;

	// Token: 0x0400180F RID: 6159
	public const float COMBAT_THREAT_LEVEL_1 = 0.1f;

	// Token: 0x04001810 RID: 6160
	public const float COMBAT_THREAT_LEVEL_2 = 0.2f;

	// Token: 0x04001811 RID: 6161
	public const float COMBAT_THREAT_LEVEL_3 = 0.5f;

	// Token: 0x04001812 RID: 6162
	public const float COMBAT_THREAT_LEVEL_4 = 1f;

	// Token: 0x04001813 RID: 6163
	public const float COMBAT_THREAT_LEVEL_5 = 2f;

	// Token: 0x04001814 RID: 6164
	public const float COMBAT_THREAT_LEVEL_6 = 5f;

	// Token: 0x04001815 RID: 6165
	public const float COMBAT_THREAT_LEVEL_7 = 8f;

	// Token: 0x04001816 RID: 6166
	public const float COMBAT_THREAT_LEVEL_8 = 10f;

	// Token: 0x04001817 RID: 6167
	public const float COMBAT_EXP_MULTIPLIER_0 = 0.01f;

	// Token: 0x04001818 RID: 6168
	public const float COMBAT_EXP_MULTIPLIER_1 = 0.1f;

	// Token: 0x04001819 RID: 6169
	public const float COMBAT_EXP_MULTIPLIER_2 = 0.2f;

	// Token: 0x0400181A RID: 6170
	public const float COMBAT_EXP_MULTIPLIER_3 = 0.5f;

	// Token: 0x0400181B RID: 6171
	public const float COMBAT_EXP_MULTIPLIER_4 = 1f;

	// Token: 0x0400181C RID: 6172
	public const float COMBAT_EXP_MULTIPLIER_5 = 2f;

	// Token: 0x0400181D RID: 6173
	public const float COMBAT_EXP_MULTIPLIER_6 = 5f;

	// Token: 0x0400181E RID: 6174
	public const float COMBAT_EXP_MULTIPLIER_7 = 8f;

	// Token: 0x0400181F RID: 6175
	public const float COMBAT_EXP_MULTIPLIER_8 = 10f;
}
