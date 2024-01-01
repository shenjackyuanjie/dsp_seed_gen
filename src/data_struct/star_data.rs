use std::cell::RefCell;
use std::rc::Rc;

use crate::data_struct::enums::{SpectrTypeEnum, StarTypeEnum};
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::planet_data::PlanetData;
use crate::data_struct::vectors::VectorLF3;

pub const K_ENTER_DISTANCE: f64 = 3600000.0;
pub const K_PHYSICS_RADIUS_RATIO: f32 = 1200.0;
pub const K_VIEW_RADIUS_RATIO: f32 = 800.0;
pub const K_MAX_DFHIVE_ORBIT: i32 = 8;

pub struct StarData {
    pub galaxy: Rc<RefCell<GalaxyData>>,
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

impl StarData {
    pub fn new(galaxy: Rc<RefCell<GalaxyData>>, seed: i32) -> Self {
        StarData {
            galaxy,
            seed,
            index: 0,
            id: 0,
            name: String::new(),
            override_name: String::new(),
            position: VectorLF3::zero(),
            u_position: VectorLF3::zero(),
            mass: 0.0,
            lifetime: 0.0,
            age: 0.0,
            star_type: StarTypeEnum::MainSeqStar,
            temperature: 0.0,
            spectr: SpectrTypeEnum::M,
            class_factor: 0.0,
            color: 0.0,
            luminosity: 0.0,
            radius: 0.0,
            acdisk_radius: 0.0,
            habitable_radius: 0.0,
            light_balance_radius: 0.0,
            dyson_radius: 0.0,
            orbit_scaler: 0.0,
            aster_belt1_orbit_index: 0.0,
            aster_belt2_orbit_index: 0.0,
            aster_belt1_radius: 0.0,
            aster_belt2_radius: 0.0,
            planet_count: 0,
            level: 0.0,
            resource_coef: 0.0,
            planets: Vec::new(),
            safety_factor: 0.0,
            hive_pattern_level: 0,
            initial_hive_count: 0,
            max_hive_count: 0,
            // hive_astro_orbits: Vec::new(),
        }
    }

    pub fn physics_radius(&self) -> f32 {
        self.radius * K_PHYSICS_RADIUS_RATIO
    }

    pub fn view_radius(&self) -> f32 {
        self.radius * K_VIEW_RADIUS_RATIO
    }

    pub fn astro_id(&self) -> i32 {
        self.id * 100
    }

    pub fn dyson_lumino(&self) -> f32 {
        let raw_value = self.luminosity.powf(0.33000001311302185);
        (raw_value * 1000.0).round() / 1000.0
    }

    pub fn display_name(&self) -> String {
        if self.override_name.len() > 0 {
            self.override_name.clone()
        } else {
            self.name.clone()
        }
    }
}
