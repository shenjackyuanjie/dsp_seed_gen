use std::rc::Rc;

use crate::data_struct::enums::{SpectrTypeEnum, StarTypeEnum};
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::game_desc::GameDesc;
use crate::data_struct::star_data::StarData;
use crate::data_struct::vectors::VectorLF3;
use crate::gen::name_gen;

use dotnet35_rand_rs::DotNet35Random;

pub const E: f64 = 2.7182817;
pub const GRAVITY: f64 = 1.3538551990520382E-06;
pub const PI: f64 = 3.141592653589793;

pub static HIVE_ORBIT_RADIUS: [f32; 18] = [
    0.4, 0.55, 0.7, 0.83, 1.0, 1.2, 1.4, 1.58, 1.72, 1.9, 2.11, 2.29, 2.5, 2.78, 3.02, 3.3, 3.6,
    3.9,
];
pub static ORBIT_RADIUS: [f32; 17] = [
    0.0, 0.4, 0.7, 1.0, 1.4, 1.9, 2.5, 3.3, 4.3, 5.5, 6.9, 8.4, 10.0, 11.7, 13.5, 15.4, 17.5,
];
pub static PLANET_2_HIVE_ORBIT_TABEL: [i32; 8] = [0, 0, 2, 4, 6, 9, 12, 15];
/// public static float specifyBirthStarMass = 0f;
pub static SPECIFY_BIRTH_STAR_MASS: f32 = 0.0;
/// public static float specifyBirthStarAge = 0f;
pub static SPECIFY_BIRTH_STAR_AGE: f32 = 0.0;

// hiveOrbitCondition
// pub hive_orbit_condition: [bool; 18],
// private static double[] pGas = new double[10];
// pub p_gas: [f64; 10],

pub type StarGenCondition = ([bool; 18], [f64; 10]);

/// 使用Box-Muller变换从均匀分布的随机数生成正态分布的随机数。
///
/// # 参数
/// * `average_value` - 正态分布的平均值
/// * `standard_deviation` - 正态分布的标准差
/// * `r1` - 第一个均匀分布的随机数
/// * `r2` - 第二个均匀分布的随机数
///
/// # 返回
/// 返回一个正态分布的随机数
pub fn rand_normal(average_value: f32, standard_deviation: f32, r1: f64, r2: f64) -> f32 {
    average_value
        + standard_deviation
            * ((-2.0 * (1.0 - r1).ln()).sqrt() * (2.0 * std::f64::consts::PI * r2).sin()) as f32
}

pub fn create_birth_star(galaxy_data: Rc<GalaxyData>, game_desc: &GameDesc, seed: i32) -> StarData {
    let mut star_data = StarData::new(galaxy_data.clone(), seed);
    // starData.galaxy = galaxy;
    // starData.index = 0;
    // starData.level = 0f;
    // starData.id = 1;
    // starData.seed = seed;
    star_data.resource_coef = 0.6;
    let mut rng = DotNet35Random::new(seed);
    let seed2 = rng.next();
    let seed3 = rng.next();
    star_data.name = name_gen::random_name(seed2);
    // star_data.override_name = "".to_string();
    // starData.position = VectorLF3.zero;
    let mut rng2 = DotNet35Random::new(seed3);
    let r = rng2.next_double();
    let r2 = rng2.next_double();
    let age_base = rng2.next_double();
    let rn = rng2.next_double();
    let rt = rng2.next_double();
    let age_factor = rng2.next_double() * 0.2 + 0.9;
    let y = rng2.next_double() * 0.4 - 0.2;
    let radius_base = 2f64.powf(y);
    let mut rng3 = DotNet35Random::new(rng2.next());
    let safety_factor = rng3.next_double();
    let mut mass_factor = rand_normal(0.0, 0.08, r, r2);
    mass_factor = mass_factor.clamp(-0.2, 0.2);
    star_data.mass = 2f32.powf(mass_factor);
    if SPECIFY_BIRTH_STAR_MASS > 0.1 {
        star_data.mass = SPECIFY_BIRTH_STAR_MASS;
    }
    if SPECIFY_BIRTH_STAR_AGE > 0.00001 {
        star_data.age = SPECIFY_BIRTH_STAR_AGE;
    }
    let d = 2.0 + 0.4 * (1.0 - star_data.mass as f64);

    star_data.lifetime = (10000.0
        * (0.1_f64).powf((star_data.mass as f64 * 0.5_f64).log10() / d.log10() + 1.0_f64) as f64
        * age_factor) as f32;

    star_data.age = (age_base * 0.4 + 0.3) as f32;
    if SPECIFY_BIRTH_STAR_AGE > 0.00001 {
        star_data.age = SPECIFY_BIRTH_STAR_AGE;
    }
    let mass_factor = (1.0 - star_data.age.min(1.0).powf(20.0) * 0.5) * star_data.mass;

    star_data.temperature = ((mass_factor as f64)
        .powf(0.56 + 0.14 / ((mass_factor + 4.0) as f64).log10() / 5.0_f64.log10())
        * 4450.0
        + 1300.0) as f32;

    let mut temperature_factor =
        ((star_data.temperature as f64 - 1300.0) / 4500.0).log10() / 2.6_f64.log10() - 0.5;
    if temperature_factor < 0.0 {
        temperature_factor *= 4.0;
    }
    if temperature_factor > 2.0 {
        temperature_factor = 2.0;
    } else if temperature_factor < -4.0 {
        temperature_factor = -4.0;
    }
    star_data.spectr = SpectrTypeEnum::new((temperature_factor + 4.0).round() as i32);
    star_data.color = ((temperature_factor + 3.5) * 0.2).min(1.0) as f32;
    star_data.class_factor = temperature_factor as f32;
    star_data.luminosity = mass_factor.powf(0.7);
    star_data.radius = ((star_data.mass as f64).powf(0.4) * radius_base) as f32;
    star_data.acdisk_radius = 0.0;
    let p = temperature_factor + 2.0;
    star_data.habitable_radius = 1.7_f32.powf(p as f32) + 0.2 * star_data.orbit_scaler.min(1.0);
    star_data.light_balance_radius = 1.7_f32.powf(p as f32);
    star_data.orbit_scaler = 1.35_f32.powf(p as f32);
    if star_data.orbit_scaler < 1.0 {
        star_data.orbit_scaler = star_data.orbit_scaler * 0.4 + 1.0 * 0.6;
    }
    // StarGen.SetStarAge(starData, starData.age, rn, rt);
    // ?????
    // 不是, 他原文先传进去一遍 star age, 然后再传 stardata.age 啥玩意
    let age = star_data.age;
    set_star_age(&mut star_data, age, rn, rt);
    if star_data.dyson_radius * 40000.0 < star_data.physics_radius() * 1.5 {
        star_data.dyson_radius = star_data.physics_radius() * 1.5 / 40000.0;
    }
    star_data.u_position = VectorLF3::zero();
    star_data.name = name_gen::random_star_name(seed2, &star_data, &galaxy_data);
    star_data.override_name = String::from("");
    star_data.hive_pattern_level = 0;
    star_data.safety_factor = 0.847 + safety_factor as f32 * 0.026;
    let hive_count_adjustment_factor = rng3.next_with_max(1000);
    star_data.max_hive_count =
        ((game_desc.combat_settings.max_density * 1000.0 + hive_count_adjustment_factor as f32 + 0.5) / 1000.0) as i32;
    let initial_colonize = game_desc.combat_settings.initial_colonize;
    let colonize_check_result = if (initial_colonize * star_data.max_hive_count as f32) < 0.7 {
        0
    } else {
        1
    };
    if initial_colonize < 0.015 {
        star_data.initial_hive_count = 0;
    } else {
        let mut base_initial_hive_count = 0.6 * initial_colonize * star_data.max_hive_count as f32;
        let mut standard_deviation = 0.5;
        if base_initial_hive_count < 1.0 {
            standard_deviation = base_initial_hive_count.sqrt() * 0.29 + 0.21;
        } else if base_initial_hive_count > star_data.max_hive_count as f32 {
            base_initial_hive_count = star_data.max_hive_count as f32;
        }
        let mut i = 16;
        loop {
            let r3 = rng3.next_double();
            let r4 = rng3.next_double();
            star_data.initial_hive_count =
                (rand_normal(base_initial_hive_count, standard_deviation, r3, r4) + 0.5) as i32;
            i -= 1;
            if i <= 0
                || (star_data.initial_hive_count >= 0
                    && star_data.initial_hive_count <= star_data.max_hive_count)
            {
                break;
            }
        }
        if star_data.initial_hive_count < colonize_check_result {
            star_data.initial_hive_count = colonize_check_result;
        } else if star_data.initial_hive_count > star_data.max_hive_count {
            star_data.initial_hive_count = star_data.max_hive_count;
        }
    }
    star_data
}

pub fn set_star_age(star: &mut StarData, age: f32, rn: f64, rt: f64) {
    let age_factor = (rn * 0.1 + 0.95) as f32;
    let temperature_factor = (rt * 0.4 + 0.8) as f32;
    let neutron_temperature_factor = (rt * 9.0 + 1.0) as f32;
    star.age = age;
    if age < 1.0 {
        if age >= 0.96 {
            let mut radius_factor =
                (5.0f64.powf((star.mass.log10() - 0.7).abs() as f64) * 5.0) as f32;
            if radius_factor > 10.0 {
                radius_factor = (radius_factor * 0.1).log10() + 1.0 * 10.0;
            }
            let mass_factor = 1.0 - age.powf(30.0) * 0.5;
            star.star_type = StarTypeEnum::GiantStar;
            star.mass = mass_factor * star.mass;
            star.radius = radius_factor * temperature_factor;
            star.acdisk_radius = 0.0;
            star.temperature = mass_factor * star.temperature;
            star.luminosity = 1.6 * star.luminosity;
            star.habitable_radius = 9.0 * star.habitable_radius;
            star.light_balance_radius = 3.0 * star.habitable_radius;
            star.orbit_scaler = 3.3 * star.orbit_scaler;
        }
        return;
    }
    if star.mass >= 18.0 {
        star.star_type = StarTypeEnum::BlackHole;
        star.spectr = SpectrTypeEnum::X;
        star.mass *= 2.5 * temperature_factor;
        star.radius *= 1.0;
        star.acdisk_radius = star.radius * 5.0;
        star.temperature = 0.0;
        star.luminosity *= 0.001 * age_factor;
        star.habitable_radius = 0.0;
        star.light_balance_radius *= 0.4 * age_factor;
        star.color = 1.0;
        return;
    }
    if star.mass >= 7.0 {
        star.star_type = StarTypeEnum::NeutronStar;
        star.spectr = SpectrTypeEnum::X;
        star.mass *= 0.2 * age_factor;
        star.radius *= 0.15;
        star.acdisk_radius = star.radius * 9.0;
        star.temperature = neutron_temperature_factor * 10000000.0;
        star.luminosity *= 0.1 * age_factor;
        star.habitable_radius = 0.0;
        star.light_balance_radius *= 3.0 * age_factor;
        star.orbit_scaler *= 1.5 * age_factor;
        star.color = 1.0;
        return;
    }
    star.star_type = StarTypeEnum::WhiteDwarf;
    star.spectr = SpectrTypeEnum::X;
    star.mass *= 0.2 * age_factor;
    star.radius *= 0.2;
    star.acdisk_radius = 0.0;
    star.temperature = temperature_factor * 150000.0;
    star.luminosity *= 0.04 * temperature_factor;
    star.habitable_radius *= 0.15 * temperature_factor;
    star.light_balance_radius *= 0.2 * age_factor;
    star.color = 0.7;
}
