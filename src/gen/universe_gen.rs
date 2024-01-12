use std::cell::RefCell;
use std::rc::Rc;

use dotnet35_rand_rs::DotNet35Random;

use crate::data_struct::enums::{ESpectrType, EStarType};
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::game_desc::GameDesc;
use crate::data_struct::vectors::{LocalVectors, VectorLF3};
use crate::gen::star_gen;

pub static ALGO_VERSION: i32 = 20200403;
pub const PLANET_ID_MAX: i32 = 256;

pub type TempPos = (Vec<VectorLF3>, Vec<VectorLF3>);

pub fn create_galaxy(game: GameDesc) -> GalaxyData {
    // 检查算法版本
    if game.galaxy_algo < 20200101 || game.galaxy_algo > 20591231 {
        panic!("Invalid galaxy algorithm version: {}", game.galaxy_algo);
    }
    // TODO: PlanetGen.gasCoef = (gameDesc.isRareResource ? 0.8f : 1f);
    let _gas_conf = if game.is_rare_resource() { 0.8f32 } else { 1f32 };
    let mut rand = DotNet35Random::new(game.galaxy_seed);
    let (tmp_poses, tmp_drunk) = generate_temp_poses(rand.next(), game.star_count, 4, 2.0, 2.3, 3.5, 0.18);
    let num = tmp_poses.len() as i32;
    let galaxy_data = GalaxyData::new(game.galaxy_seed, game.star_count);
    if tmp_poses.is_empty() {
        return galaxy_data;
    }
    let galaxy_data = Rc::new(RefCell::new(galaxy_data));
    let num2 = rand.next_double() as f32;
    let num3 = rand.next_double() as f32;
    let num4 = rand.next_double() as f32;
    let num5 = rand.next_double() as f32;
    let num6 = (0.01 * num as f32 + num2 * 0.3).ceil() as i32;
    let num7 = (0.01 * num as f32 + num3 * 0.3).ceil() as i32;
    let num8 = (0.016 * num as f32 + num4 * 0.4).ceil() as i32;
    let num9 = (0.013 * num as f32 + num5 * 1.4).ceil() as i32;
    let num10 = num - num6;
    let num11 = num10 - num7;
    let num12 = num11 - num8;
    let num13 = (num12 - 1) / num9;
    let num14 = num13 / 2;
    for i in 0..num {
        let seed = rand.next();
        if i == 0 {
            galaxy_data.borrow_mut().stars[i as usize] = star_gen::create_birth_star(galaxy_data.clone(), &game, seed);
        } else {
            let mut need_spectr = ESpectrType::X;
            if i == 3 {
                need_spectr = ESpectrType::M;
            } else if i == num12 - 1 {
                need_spectr = ESpectrType::O;
            }
            let mut need_type = EStarType::MainSeqStar;
            if i % num13 == num14 {
                need_type = EStarType::GiantStar;
            }
            if i >= num10 {
                need_type = EStarType::BlackHole;
            } else if i >= num11 {
                need_type = EStarType::NeutronStar;
            } else if i >= num12 {
                need_type = EStarType::WhiteDwarf;
            }
            // galaxy_data.stars[i] = star_gen::create_star(&galaxy_data, &tmp_poses[i], &game, i + 1, seed, need_type, need_spectr);
        }
    }
    // TODO: 生成行星
    todo!()
}

pub fn check_collision(pts: &Vec<VectorLF3>, &pt: &VectorLF3, min_dist: f64) -> bool {
    let min_dist_squared = min_dist.powi(2);
    for &vector_lf in pts {
        let dist = pt - vector_lf;
        if dist.sqr_magnitude() < min_dist_squared {
            return true;
        }
    }
    false
}

pub fn generate_temp_poses(
    seed: i32,
    target_count: i32,
    iter_count: i32,
    min_dist: f64,
    min_step_len: f64,
    max_step_len: f64,
    flatten: f64,
) -> TempPos {
    let iter_count = if iter_count < 1 {
        1
    } else if iter_count > 16 {
        16
    } else {
        iter_count
    };
    let (mut tmp_poses, tmp_drunk) =
        random_poses(seed, target_count * iter_count, min_dist, min_step_len, max_step_len, flatten);
    for i in (0..tmp_poses.len()).rev() {
        if i % (iter_count as usize) != 0 {
            tmp_poses.remove(i);
        }
        if tmp_poses.len() <= target_count as usize {
            break;
        }
    }
    (tmp_poses, tmp_drunk)
}

pub fn random_poses(
    seed: i32,
    max_count: i32,
    min_dist: f64,
    min_step_len: f64,
    max_step_len: f64,
    flatten: f64,
) -> TempPos {
    let mut tmp_poses = Vec::new();
    let mut tmp_drunk = Vec::new();
    tmp_poses.push(VectorLF3::zeros());
    let mut rand = DotNet35Random::new(seed);
    let random_num = rand.next_double();
    let min_loop_count = 6;
    let max_loop_count = 8;
    let num4 = (random_num * (max_loop_count - min_loop_count) as f64 + min_loop_count as f64) as i32;
    // 大的 Part 1
    for _ in 0..num4 {
        for _ in 0..PLANET_ID_MAX {
            let random_x = rand.next_double() * 2f64 - 1f64;
            let random_y = rand.next_double() * 2f64 - 1f64 * flatten;
            let random_z = rand.next_double() * 2f64 - 1f64;
            let mut rand_vec = VectorLF3::new(random_x, random_y, random_z);
            let length_square = rand_vec.sqr_magnitude();
            let mut random_step_size = rand.next_double();
            if length_square <= 1.0 && length_square >= 1E-08 {
                let distance = length_square.sqrt();
                random_step_size = (random_step_size * (max_step_len - min_step_len) + min_dist) / distance;
                rand_vec *= random_step_size;
                if !check_collision(&tmp_poses, &rand_vec, min_dist) {
                    tmp_drunk.push(rand_vec);
                    tmp_poses.push(rand_vec);
                    if tmp_poses.len() >= max_count as usize {
                        return (tmp_poses, tmp_drunk);
                    }
                    break;
                }
            }
        }
    }
    // 大的 Part 2
    for index in 0..tmp_drunk.len() {
        // 有 0.7 的概率
        if rand.next_double() <= 0.7 {
            for _ in 0..PLANET_ID_MAX {
                let random_x = rand.next_double() * 2f64 - 1f64;
                let random_y = rand.next_double() * 2f64 - 1f64 * flatten;
                let random_z = rand.next_double() * 2f64 - 1f64;
                let mut rand_vec = VectorLF3::new(random_x, random_y, random_z);
                let length_square = rand_vec.sqr_magnitude();
                let mut random_step_size = rand.next_double();
                if length_square <= 1.0 && length_square >= 1E-08 {
                    let distance = length_square.sqrt();
                    random_step_size = (random_step_size * (max_step_len - min_step_len) + min_dist) / distance;
                    // let vector_lf2 = rand_vec * random_step_size;
                    rand_vec *= random_step_size;
                    if !check_collision(&tmp_poses, &rand_vec, min_dist) {
                        tmp_drunk[index] = rand_vec;
                        tmp_poses.push(rand_vec);
                        if tmp_poses.len() >= max_count as usize {
                            return (tmp_poses, tmp_drunk);
                        }
                        break;
                    }
                }
            }
        }
    }
    (tmp_poses, tmp_drunk)
}
