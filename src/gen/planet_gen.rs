use std::cell::RefCell;
use std::rc::Rc;

use dotnet35_rand_rs::DotNet35Random;
use lerp::Lerp;

// use crate::data_struct::consts::{GRAVITY, PI};
use crate::data_struct::enums::EPlanetSingularity;
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::planet_data::PlanetData;
use crate::data_struct::star_data::StarData;
use crate::gen::name_gen;
use crate::gen::star_gen;

pub const K_GRAVITATIONAL_CONST: f64 = 346586930.95732176;
pub const K_PLANET_MASS: f32 = 0.006;
pub const K_GIANT_MASS_COEF: f32 = 3.33333;
pub const K_GIANT_MASS: f32 = 0.019999979;
pub static mut GAS_COEF: f32 = 1.0;

pub fn create_planet(
    galaxy: Rc<RefCell<GalaxyData>>,
    star: Rc<RefCell<StarData>>,
    theme_ids: Vec<i32>,
    index: i32,
    orbit_around: i32,
    orbit_index: i32,
    number: i32,
    gas_giant: bool,
    info_seed: i32,
    gen_seed: i32,
) -> PlanetData {
    let mut planet_data = PlanetData::new(galaxy.clone(), star.clone(), gen_seed, info_seed);
    planet_data.index = index;
    planet_data.orbit_around = orbit_around;
    planet_data.orbit_index = orbit_index;
    planet_data.number = number;
    let star_borrow = (*star.borrow()).clone();
    let planet_round_borrow = (*planet_data.orbit_around_planet.borrow()).clone();
    planet_data.id = star_borrow.astro_id() + index + 1;
    if orbit_around > 0 {
        let mut j = 0;
        while j < star_borrow.planet_count {
            if (orbit_around == star.borrow().planets[j as usize].number)
                & (star.borrow().planets[j as usize].orbit_around == 0)
            {
                let data = star_borrow.planets[j as usize].clone();
                planet_data.orbit_around_planet.replace(Some(data));
                if orbit_index > 1 {
                    planet_round_borrow.unwrap().singularity |= EPlanetSingularity::MULTIPLE_SATELLITES;
                    // let oap: Rc<RefCell<PlanetData>> = planet_data.orbit_around_planet.);
                    // let mut oap = oap.get_mut();
                    // oap.singularity = oap.singularity | EPlanetSingularity::MULTIPLE_SATELLITES;
                    // // oap |= EPlanetSingularity::MULTIPLE_SATELLITES;
                    // planet_data.orbit_around_planet = Some(Rc::new(RefCell::new(oap)));
                }
                break;
            } else {
                j += 1;
            }
        }
        // Assert.NotNull(planetData.orbitAroundPlanet);
        assert!(planet_round_borrow.is_some());
    }
    let name: String;
    if star.borrow().planet_count <= 20 {
        name = name_gen::ROMAN[index as usize + 1].to_string()
    } else {
        name = (index + 1).to_string()
    }
    planet_data.name = format!("{} {} 号星", star.borrow().name.clone(), name);
    // part 2, 真正开始生成星球
    let mut rand_gen = DotNet35Random::new(info_seed);
    let num2 = rand_gen.next_double();
    let num3 = rand_gen.next_double();
    let num4 = rand_gen.next_double();
    let num5 = rand_gen.next_double();
    let num6 = rand_gen.next_double();
    let num7 = rand_gen.next_double();
    let num8 = rand_gen.next_double();
    let num9 = rand_gen.next_double();
    let num10 = rand_gen.next_double();
    let num11 = rand_gen.next_double();
    let num12 = rand_gen.next_double();
    let num13 = rand_gen.next_double();
    let rand = rand_gen.next_double();
    let num14 = rand_gen.next_double();
    let rand2 = rand_gen.next_double();
    let rand3 = rand_gen.next_double();
    let rand4 = rand_gen.next_double();
    let theme_seed = rand_gen.next();
    let num15 = 1.2_f32.powf(num2 as f32 * (num3 as f32 - 0.5) * 0.5);
    let mut num16: f32;
    if orbit_around == 0 {
        num16 = star_gen::ORBIT_RADIUS[orbit_index as usize] * star.borrow().orbit_scaler;
        num16 *= (num15 - 1.0) / num16.max(1.0) + 1.0;
    } else {
        num16 = ((1600.0 * orbit_index as f32 + 200.0) * star.borrow().orbit_scaler.powf(0.3) * num15.lerp(1.0, 0.5)
            + planet_data.orbit_around_planet.unwrap().borrow().read_radius() as f32)
            / 40000.0;
    }
    todo!()
}
