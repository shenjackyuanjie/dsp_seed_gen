use std::cell::RefCell;
use std::rc::Rc;

use dotnet35_rand_rs::DotNet35Random;
use lerp::Lerp;

// use crate::data_struct::consts::{GRAVITY, PI};
use crate::data_struct::enums::EPlanetSingularity;
use crate::data_struct::enums::EStarType;
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::planet_data::PlanetData;
use crate::data_struct::star_data::StarData;
use crate::data_struct::vectors::{LocalQuaternion, Quaternion, VectorF3};
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

    let star_borrow = &(*star.borrow());
    let planet_round_borrow = &(*planet_data.orbit_around_planet.borrow());
    planet_data.id = star_borrow.astro_id() + index + 1;

    if orbit_around > 0 {
        let mut j = 0;
        while j < star_borrow.planet_count {
            if orbit_around == star_borrow.planets[j as usize].number
                && star_borrow.planets[j as usize].orbit_around == 0
            {
                let data = star_borrow.planets[j as usize].clone();
                planet_data.orbit_around_planet.replace(Some(data));
                if orbit_index > 1 {
                    planet_round_borrow.clone().unwrap().singularity |= EPlanetSingularity::MULTIPLE_SATELLITES;
                }
                break;
            } else {
                j += 1;
            }
        }
        // Assert.NotNull(planetData.orbitAroundPlanet);
        let planet_round_borrow = (*planet_data.orbit_around_planet.borrow()).clone();
        assert!(planet_round_borrow.is_some());
    }

    if star_borrow.planet_count <= 20 {
        planet_data.name = format!("{} {} 号星", star.borrow().name.clone(), name_gen::ROMAN[index as usize + 1]);
    } else {
        planet_data.name = format!("{} {} 号星", star.borrow().name.clone(), (index + 1));
    }

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
            + planet_round_borrow.clone().unwrap().read_radius())
            / 40000.0;
    }

    planet_data.orbit_radius = num16;
    planet_data.orbit_inclination = (num4 * 16.0 - 8.0) as f32;
    if orbit_around > 0 {
        planet_data.orbit_inclination *= 2.2;
    }
    planet_data.orbit_longitude = (num5 * 360.0) as f32;
    if star.borrow().star_type > EStarType::NeutronStar {
        if planet_data.orbit_inclination > 0.0 {
            planet_data.orbit_inclination += 3.0;
        } else {
            planet_data.orbit_inclination -= 3.0;
        }
    }
    if planet_round_borrow.is_none() {
        planet_data.orbital_period = ((39.47841760435743 * (num16 as f64).powi(3))
            / (1.3538551990520382e-06 * star.borrow().mass as f64))
            .sqrt();
    } else {
        planet_data.orbital_period = ((39.47841760435743 * (num16 as f64).powi(3)) / 1.0830842106853677e-08).sqrt();
    }
    planet_data.orbit_phase = (num6 * 360.0) as f32;
    if num14 < 0.03999999910593033 {
        planet_data.obliquity = (num7 * (num8 - 0.5) * 39.9) as f32;
        if planet_data.obliquity < 0.0 {
            planet_data.obliquity -= 70.0;
        } else {
            planet_data.obliquity += 70.0;
        }
        planet_data.singularity |= EPlanetSingularity::LAY_SIDE;
    } else if num14 < 0.10000000149011612 {
        planet_data.obliquity = (num7 * (num8 - 0.5) * 80.0) as f32;
        if planet_data.obliquity < 0.0 {
            planet_data.obliquity -= 30.0;
        } else {
            planet_data.obliquity += 30.0;
        }
    } else {
        planet_data.obliquity = (num7 * (num8 - 0.5) * 60.0) as f32;
    }
    planet_data.rotation_period = (num9 * num10 * 1000.0 + 400.0)
        * if orbit_around == 0 {
            num16.powf(0.25) as f64
        } else {
            1.0_f64
        }
        * if gas_giant { 0.2_f64 } else { 1.0_f64 };

    if !gas_giant {
        match star.borrow().star_type {
            EStarType::WhiteDwarf => {
                planet_data.rotation_period *= 0.5;
            }
            EStarType::NeutronStar => {
                planet_data.rotation_period *= 0.20000000298023224;
            }
            EStarType::BlackHole => {
                planet_data.rotation_period *= 0.15000000596046448;
            }
            _ => {}
        }
    }
    planet_data.rotation_phase = (num11 * 360.0) as f32;
    planet_data.sun_distance = if orbit_around == 0 {
        planet_data.orbit_radius
    } else {
        planet_round_borrow.clone().unwrap().orbit_radius
    };
    planet_data.scale = 1.0;
    let num18 = if orbit_around == 0 {
        planet_data.orbital_period
    } else {
        planet_round_borrow.clone().unwrap().orbital_period
    };
    planet_data.rotation_period = 1.0 / (1.0 / num18 + 1.0 / planet_data.rotation_period);
    if orbit_around == 0 && orbit_index <= 4 && !gas_giant {
        if num14 > 0.9599999785423279 {
            planet_data.obliquity *= 0.01;
            planet_data.rotation_period = planet_data.orbital_period;
            planet_data.singularity |= EPlanetSingularity::TIDAL_LOCKED;
        } else if num14 > 0.9300000071525574 {
            planet_data.obliquity *= 0.1;
            planet_data.rotation_period = planet_data.orbital_period * 0.5;
            planet_data.singularity |= EPlanetSingularity::TIDAL_LOCKED2;
        } else if num14 > 0.8999999761581421 {
            planet_data.obliquity *= 0.2;
            planet_data.rotation_period = planet_data.orbital_period * 0.25;
            planet_data.singularity |= EPlanetSingularity::TIDAL_LOCKED4;
        }
    }
    if num14 > 0.85 && num14 <= 0.9 {
        planet_data.rotation_period = -planet_data.rotation_period;
        planet_data.singularity |= EPlanetSingularity::CLOCKWISE_ROTATE;
    }
    // planet_data.runtime_orbit_rotation =
    let a_qua: Quaternion = Quaternion::angle_axis(planet_data.orbit_longitude, &VectorF3::new(0.0, 1.0, 0.0));
    let b_qua: Quaternion = Quaternion::angle_axis(planet_data.orbit_inclination, &VectorF3::new(0.0, 0.0, 1.0));
    planet_data.runtime_orbit_rotation = a_qua * b_qua;
    if planet_round_borrow.is_some() {
        planet_data.runtime_orbit_rotation =
            planet_round_borrow.clone().unwrap().runtime_orbit_rotation * planet_data.runtime_orbit_rotation;
    }
    planet_data.runtime_system_rotation = planet_data.runtime_orbit_rotation
        * Quaternion::angle_axis(planet_data.obliquity, &VectorF3::new(0.0, 0.0, 1.0));
    let habitable_radius = star.borrow().habitable_radius;
    todo!()
}
