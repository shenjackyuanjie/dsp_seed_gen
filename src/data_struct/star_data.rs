use crate::data_struct::enums::{SpectrTypeEnum, StarTypeEnum};
use crate::data_struct::planet_data::PlanetData;
use crate::data_struct::vectors::VectorLF3;

pub const K_ENTER_DISTANCE: f64 = 3600000.0;
pub const K_PHYSICS_RADIUS_RATIO: f32 = 1200.0;
pub const K_VIEW_RADIUS_RATIO: f32 = 800.0;
pub const K_MAX_DFHIVE_ORBIT: i32 = 8;

pub struct StarData {
    // pub galaxy: GalaxyData,
    pub seed: i32,
    pub index: i32,
    pub id: i32,
    pub name: String,
    pub override_name: String,
    pub position: VectorLF3,
    pub u_position: VectorLF3,
    pub mass: f32,
    pub lifetime: f32,
    pub age: f32,
    pub star_type: StarTypeEnum,
    pub temperature: f32,
    pub spectr: SpectrTypeEnum,
    pub class_factor: f32,
    pub color: f32,
    pub luminosity: f32,
    pub radius: f32,
    pub acdisk_radius: f32,
    pub habitable_radius: f32,
    pub light_balance_radius: f32,
    pub dyson_radius: f32,
    pub orbit_scaler: f32,
    pub aster_belt1_orbit_index: f32,
    pub aster_belt2_orbit_index: f32,
    pub aster_belt1_radius: f32,
    pub aster_belt2_radius: f32,
    pub planet_count: i32,
    pub level: f32,
    pub resource_coef: f32,
    pub planets: Vec<PlanetData>,
    pub safety_factor: f32,
    pub hive_pattern_level: i32,
    pub initial_hive_count: i32,
    pub max_hive_count: i32,
    // pub hive_astro_orbits: Vec<AstroOrbitData>,
}
