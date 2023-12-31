use dotnet35_rand_rs::DotNet35Random;

use crate::data_struct::planet_data::PlanetData;

pub trait PlanetAlgorithm {
    fn reset(&mut self, seed: i32, planet: PlanetData);
}
