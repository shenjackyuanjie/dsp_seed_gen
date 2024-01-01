use crate::data_struct::star_data::StarData;

pub const AU: f64 = 40000_f64;
pub const LY: f64 = 2400000_f64;

pub struct GalaxyData {
    /// 种子
    pub seed: i32,
    /// 星系中星星的数量
    pub star_count: i32,
    /// 每一个恒星的数据
    pub stars: Vec<StarData>,
    /// 初始星球的id
    pub birth_planet_id: i32,
    /// 初始恒星的id
    pub birth_star_id: i32,
    pub habitable_count: i32,
    // public StarGraphNode[] graphNodes;
    // public AstroData[25700] astrosData;
    // public PlanetFactory[25700] astrosFactory;
}

impl GalaxyData {
    pub fn new(seed: i32, star_count: i32) -> GalaxyData {
        GalaxyData {
            seed,
            star_count,
            stars: Vec::new(),
            birth_planet_id: 0,
            birth_star_id: 0,
            habitable_count: 0,
        }
    }
}
