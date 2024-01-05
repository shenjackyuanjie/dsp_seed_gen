use crate::data_struct::consts::combat::{
    COMBAT_EXP_MULTIPLIER_0, COMBAT_THREAT_LEVEL_0, GROWTH_SPEED_0, INITIALLEVEL_1,
    MAX_DENSITY_LEVEL_0, POWER_THREAT_LEVEL_0,
};

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
