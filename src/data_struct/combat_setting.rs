pub struct CombatSettings {
    pub aggressiveness: f32,
    pub initial_level: f32,
    pub initial_growth: f32,
    pub initial_colonize: f32,
    pub max_density: f32,
    pub growth_speed_factor: f32,
    pub power_threat_factor: f32,
    pub battle_threat_factor: f32,
    pub battle_exp_factor: f32,
}

pub const AGG_VALUE_0_DUMMY: f32 = -1.0;
pub const AGG_VALUE_1_PASSIVE: f32 = 0.0;
pub const AGG_VALUE_2_TORPID: f32 = 0.5;
pub const AGG_VALUE_3_NORMAL: f32 = 1.0;
pub const AGG_VALUE_4_SHARP: f32 = 2.0;
pub const AGG_VALUE_5_RAMPAGE: f32 = 3.0;
pub const INITIALLEVEL_0: f32 = 0.0;
pub const INITIALLEVEL_1: f32 = 1.0;
pub const INITIALLEVEL_2: f32 = 2.0;
pub const INITIALLEVEL_3: f32 = 3.0;
pub const INITIALLEVEL_4: f32 = 4.0;
pub const INITIALLEVEL_5: f32 = 5.0;
pub const INITIALLEVEL_6: f32 = 6.0;
pub const INITIALLEVEL_7: f32 = 7.0;
pub const INITIALLEVEL_8: f32 = 8.0;
pub const INITIALLEVEL_9: f32 = 9.0;
pub const INITIALLEVEL_10: f32 = 10.0;
pub const INITIAL_GROWTH_LEVEL_0: f32 = 0.0;
pub const INITIAL_GROWTH_LEVEL_1: f32 = 0.25;
pub const INITIAL_GROWTH_LEVEL_2: f32 = 0.5;
pub const INITIAL_GROWTH_LEVEL_3: f32 = 0.75;
pub const INITIAL_GROWTH_LEVEL_4: f32 = 1.0;
pub const INITIAL_GROWTH_LEVEL_5: f32 = 1.5;
pub const INITIAL_GROWTH_LEVEL_6: f32 = 2.0;
pub const INITIAL_EXPAND_LEVEL_0: f32 = 0.01;
pub const INITIAL_EXPAND_LEVEL_1: f32 = 0.25;
pub const INITIAL_EXPAND_LEVEL_2: f32 = 0.5;
pub const INITIAL_EXPAND_LEVEL_3: f32 = 0.75;
pub const INITIAL_EXPAND_LEVEL_4: f32 = 1.0;
pub const INITIAL_EXPAND_LEVEL_5: f32 = 1.5;
pub const INITIAL_EXPAND_LEVEL_6: f32 = 2.0;
pub const MAX_DENSITY_LEVEL_0: f32 = 1.0;
pub const MAX_DENSITY_LEVEL_1: f32 = 1.5;
pub const MAX_DENSITY_LEVEL_2: f32 = 2.0;
pub const MAX_DENSITY_LEVEL_3: f32 = 2.5;
pub const MAX_DENSITY_LEVEL_4: f32 = 3.0;
pub const GROWTH_SPEED_0: f32 = 0.25;
pub const GROWTH_SPEED_1: f32 = 0.5;
pub const GROWTH_SPEED_2: f32 = 1.0;
pub const GROWTH_SPEED_3: f32 = 2.0;
pub const GROWTH_SPEED_4: f32 = 3.0;
pub const POWER_THREAT_LEVEL_0: f32 = 0.01;
pub const POWER_THREAT_LEVEL_1: f32 = 0.1;
pub const POWER_THREAT_LEVEL_2: f32 = 0.2;
pub const POWER_THREAT_LEVEL_3: f32 = 0.5;
pub const POWER_THREAT_LEVEL_4: f32 = 1.0;
pub const POWER_THREAT_LEVEL_5: f32 = 2.0;
pub const POWER_THREAT_LEVEL_6: f32 = 5.0;
pub const POWER_THREAT_LEVEL_7: f32 = 8.0;
pub const POWER_THREAT_LEVEL_8: f32 = 10.0;
pub const COMBAT_THREAT_LEVEL_0: f32 = 0.01;
pub const COMBAT_THREAT_LEVEL_1: f32 = 0.1;
pub const COMBAT_THREAT_LEVEL_2: f32 = 0.2;
pub const COMBAT_THREAT_LEVEL_3: f32 = 0.5;
pub const COMBAT_THREAT_LEVEL_4: f32 = 1.0;
pub const COMBAT_THREAT_LEVEL_5: f32 = 2.0;
pub const COMBAT_THREAT_LEVEL_6: f32 = 5.0;
pub const COMBAT_THREAT_LEVEL_7: f32 = 8.0;
pub const COMBAT_THREAT_LEVEL_8: f32 = 10.0;
pub const COMBAT_EXP_MULTIPLIER_0: f32 = 0.01;
pub const COMBAT_EXP_MULTIPLIER_1: f32 = 0.1;
pub const COMBAT_EXP_MULTIPLIER_2: f32 = 0.2;
pub const COMBAT_EXP_MULTIPLIER_3: f32 = 0.5;
pub const COMBAT_EXP_MULTIPLIER_4: f32 = 1.0;
pub const COMBAT_EXP_MULTIPLIER_5: f32 = 2.0;
pub const COMBAT_EXP_MULTIPLIER_6: f32 = 5.0;
pub const COMBAT_EXP_MULTIPLIER_7: f32 = 8.0;
pub const COMBAT_EXP_MULTIPLIER_8: f32 = 10.0;

impl CombatSettings {
    pub fn new() -> Self {
        // this.aggressiveness = 1f;
        // this.initialLevel = 0f;
        // this.initialGrowth = 1f;
        // this.initialColonize = 1f;
        // this.maxDensity = 1f;
        // this.growthSpeedFactor = 1f;
        // this.powerThreatFactor = 1f;
        // this.battleThreatFactor = 1f;
        // this.battleExpFactor = 1f;
        Self {
            aggressiveness: MAX_DENSITY_LEVEL_0,
            initial_level: INITIALLEVEL_1,
            initial_growth: GROWTH_SPEED_0,
            initial_colonize: MAX_DENSITY_LEVEL_0,
            max_density: MAX_DENSITY_LEVEL_0,
            growth_speed_factor: GROWTH_SPEED_0,
            power_threat_factor: POWER_THREAT_LEVEL_0,
            battle_threat_factor: COMBAT_THREAT_LEVEL_0,
            battle_exp_factor: COMBAT_EXP_MULTIPLIER_0,
        }
    }
}

impl Default for CombatSettings {
    fn default() -> Self {
        Self::new()
    }
}
