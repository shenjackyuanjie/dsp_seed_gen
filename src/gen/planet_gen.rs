use std::cell::RefCell;
use std::rc::Rc;

use dotnet35_rand_rs::DotNet35Random;

use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::planet_data::PlanetData;
use crate::data_struct::star_data::StarData;

pub const GRAVITY: f64 = 1.3538551990520382E-06;
pub const PI: f64 = 3.141592653589793;
pub const K_GRAVITATIONAL_CONST: f64 = 346586930.95732176;
pub const K_PLANET_MASS: f32 = 0.006;
pub const K_GIANT_MASS_COEF: f32 = 3.33333;
pub const K_GIANT_MASS: f32 = 0.019999979;
pub static mut GAS_COEF: f32 = 1.0;

pub fn create_planet(
    galaxy: Rc<RefCell<GalaxyData>>,
    star: &StarData,
    theme_ids: Vec<i32>,
    index: i32,
    orbit_around: i32,
    orbit_index: i32,
    number: i32,
    gas_giant: bool,
    info_seed: i32,
    gen_seed: i32,
) -> PlanetData {
    // let mut planet_data = PlanetData::new()
    todo!()
}
