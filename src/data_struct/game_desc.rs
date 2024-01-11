use crate::data_struct::combat_setting::CombatSettings;

pub struct GameDesc {
    pub galaxy_algo: i32,
    pub galaxy_seed: i32,
    /// 星球数量
    /// 1~64
    pub star_count: i32,
    pub player_proto: i32,
    /// 资源倍率
    pub resource_multiplier: f32,
    /// 战斗模式设置
    pub combat_settings: CombatSettings,
    /// RARE_RESOURCE_MULTIPLIER = 0.1f
    /// 稀有资源倍率
    pub rare_resource_multiplier: f32,
    /// INFINITE_RESOURCE_MULTIPLIER = 100f
    /// 无限资源倍率
    pub infinite_resource_multiplier: f32,
}

impl GameDesc {
    pub fn new(
        algo: i32,
        seed: i32,
        star_count: i32,
        player_proto: i32,
        resource_multiplier: f32,
        rare_resource_multiplier: Option<f32>,
        infinite_resource_multiplier: Option<f32>,
    ) -> Self {
        GameDesc {
            galaxy_algo: algo,
            galaxy_seed: seed,
            star_count,
            player_proto,
            resource_multiplier,
            combat_settings: CombatSettings::new(),
            rare_resource_multiplier: rare_resource_multiplier.unwrap_or(0.1f32),
            infinite_resource_multiplier: infinite_resource_multiplier.unwrap_or(100f32),
        }
    }

    /// 元数据产出倍率
    /// 顺手做了
    ///
    pub fn property_multplier(&self) -> f32 {
        // todo, 战斗模式倍率
        let combat_multiplier = 1.0f32;

        let mut resource_multiplier = match self.resource_multiplier {
            // mult <= 0.15f
            mult if mult <= 0.15f32 => 4_f32,
            // mult <= 0.65f
            mult if mult <= 0.65f32 => 3_f32,
            // 0.65 ~ 0.9
            mult if mult <= 0.9f32 => 2_f32,
            // 0.9 ~ 1.25
            mult if mult <= 1.25f32 => 1_f32,
            // 1.25 ~ 1.75
            mult if mult <= 1.75f32 => 0.8,
            // 1.75 ~ 2.5
            mult if mult <= 2.5f32 => 0.6,
            // 2.5 ~ 4
            mult if mult <= 4f32 => 0.5,
            // 4 ~ 6.5
            mult if mult <= 6.5f32 => 0.4,
            // 6.5 ~ 8.5
            mult if mult <= 8.5f32 => 0.3,
            _ => 0.4,
        };
        resource_multiplier += combat_multiplier * (resource_multiplier * 0.5 + 0.5);
        if combat_multiplier >= 9.999 {
            resource_multiplier = 1.0;
        }
        // return (float)((int)((double)(resource_multiplier * 100f) + 0.5)) / 100f;
        (resource_multiplier * 100f32 + 0.5) / 100f32
    }

    /// 星区种子
    pub fn seed_key(&self) -> i64 {
        let galaxy_seed = self.galaxy_seed * 100000000;
        let mut star_count = self.star_count;
        let mut resource_multiplier = (self.resource_multiplier * 10.0 + 0.5) as i64;
        if star_count > 999 {
            star_count = 999;
        } else if star_count < 1 {
            star_count = 1;
        }
        if resource_multiplier > 99 {
            resource_multiplier = 99;
        } else if resource_multiplier < 1 {
            resource_multiplier = 1;
        }
        let combat_multiplier: i64 = 0;
        // TODO: combat
        // if self.
        // return galaxySeed * 100000000L + (long)num2 * 100000L + (long)num3 * 1000L + (long)num4;
        galaxy_seed as i64 + star_count as i64 * 100000 + resource_multiplier * 1000 + combat_multiplier
    }

    /// 是否为困难模式
    pub fn is_rare_resource(&self) -> bool {
        self.resource_multiplier <= 0.1001f32
    }
}
