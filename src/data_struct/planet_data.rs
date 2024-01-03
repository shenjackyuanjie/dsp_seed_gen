use std::cell::RefCell;
use std::rc::Rc;

use crate::data_struct::enums::{EPlanetSingularity, EPlanetType};
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::star_data::StarData;
use crate::data_struct::vectors::{Quaternion, VectorLF3};

pub const K_ENTER_ALTITUDE: f32 = 1000.0;
pub const K_BIRTH_HEIGHT_SHIFT: f32 = 1.45;

pub struct PlanetData {
    /// 星系数据
    pub galaxy: Rc<RefCell<GalaxyData>>,
    /// 星星数据
    pub star: StarData,
    /// 种子
    pub seed: i32,
    /// 信息种子
    pub info_seed: i32,
    /// ID
    pub id: i32,
    /// 索引
    pub index: i32,
    /// 绕行
    pub orbit_around: i32,
    /// 数量
    pub number: i32,
    /// 轨道索引
    pub orbit_index: i32,
    /// 名称
    pub name: String,
    /// 覆盖名称
    pub override_name: String,
    /// 轨道半径
    pub orbit_radius: f32,
    /// 轨道倾角
    pub orbit_inclination: f32,
    /// 轨道经度
    pub orbit_longitude: f32,
    /// 轨道周期
    pub orbital_period: f64,
    /// 轨道相位
    pub orbit_phase: f32,
    /// 倾斜角
    pub obliquity: f32,
    /// 自转周期
    pub rotation_period: f64,
    /// 自转相位
    pub rotation_phase: f32,
    /// 半径
    pub radius: f32,
    /// 规模
    pub scale: f32,
    /// 太阳距离
    pub sun_distance: f32,
    /// 适宜生物偏好
    pub habitable_bias: f32,
    /// 温度偏好
    pub temperature_bias: f32,
    /// 离子高度
    pub ion_height: f32,
    /// 风力
    pub wind_strength: f32,
    /// 亮度
    pub luminosity: f32,
    /// 陆地百分比
    pub land_percent: f32,
    /// 模型X
    pub mod_x: f64,
    /// 模型Y
    pub mod_y: f64,
    /// 水高度
    pub water_height: f32,
    /// 水物品ID
    pub water_item_id: i32,
    /// 是否已平均化
    pub levelized: bool,
    /// 冰旗
    pub ice_flag: i32,
    /// 类型
    pub r#type: EPlanetType,
    /// 奇点
    pub singularity: EPlanetSingularity,
    /// 主题
    pub theme: i32,
    /// 算法ID
    pub algo_id: i32,
    /// 样式
    pub style: i32,
    /// 绕行行星
    pub orbit_around_planet: Option<Rc<PlanetData>>,
    /// 运行时位置
    pub runtime_position: VectorLF3,
    /// 下一个运行时位置
    pub runtime_position_next: VectorLF3,
    /// 运行时旋转
    pub runtime_rotation: Quaternion,
    /// 下一个运行时旋转
    pub runtime_rotation_next: Quaternion,
    /// 运行时系统旋转
    pub runtime_system_rotation: Quaternion,
    /// 运行时轨道旋转
    pub runtime_orbit_rotation: Quaternion,
    /// 运行时轨道相位
    pub runtime_orbit_phase: f32,
    /// 运行时旋转相位
    pub runtime_rotation_phase: f32,
    /// U位置
    pub u_position: VectorLF3,
    /// 下一个U位置
    pub u_position_next: VectorLF3,
    /// 运行时本地太阳方向
    pub runtime_local_sun_direction: VectorLF3,
    /// 模型数据
    // pub mod_data: Vec<u8>,
    /// 精度
    pub precision: i32,
    /// 段
    pub segment: i32,
    /// 数据
    // pub data: PlanetRawData,
    /// 矿物组锁
    // pub vein_groups_lock: Mutex,
    /// 矿物组
    // pub vein_groups: Vec<VeinGroup>,
    /// 矿物偏向向量
    pub vein_bias_vector: VectorLF3,
    // ... 其他字段
}

impl PlanetData {
    pub fn new(galaxy_data: Rc<RefCell<GalaxyData>>, seed: i32, info_seed: i32) -> Self {
        Self {
            galaxy: galaxy_data.clone(),
            star: StarData::new(galaxy_data.clone(), seed),
            seed,
            info_seed,
            id: 0,
            index: 0,
            orbit_around: 0,
            number: 0,
            orbit_index: 0,
            name: "".to_string(),
            override_name: "".to_string(),
            orbit_radius: 1.0,
            orbit_inclination: 0.0,
            orbit_longitude: 0.0,
            orbital_period: 3600.0,
            orbit_phase: 0.0,
            obliquity: 0.0,
            rotation_period: 480.0,
            rotation_phase: 0.0,
            radius: 200.0,
            scale: 1.0,
            sun_distance: 0.0,
            habitable_bias: 0.0,
            temperature_bias: 0.0,
            ion_height: 0.0,
            wind_strength: 0.0,
            luminosity: 0.0,
            land_percent: 0.0,
            mod_x: 0.0,
            mod_y: 0.0,
            water_height: 0.0,
            water_item_id: 0,
            levelized: false,
            ice_flag: 0,
            r#type: EPlanetType::None,
            singularity: EPlanetSingularity::NONE,
            theme: 0,
            algo_id: 0,
            style: 0,
            orbit_around_planet: None,
            runtime_position: VectorLF3::zero(),
            runtime_position_next: VectorLF3::zero(),
            runtime_rotation: Quaternion::zero(),
            runtime_rotation_next: Quaternion::zero(),
            runtime_system_rotation: Quaternion::zero(),
            runtime_orbit_rotation: Quaternion::zero(),
            runtime_orbit_phase: 0.0,
            runtime_rotation_phase: 0.0,
            u_position: VectorLF3::zero(),
            u_position_next: VectorLF3::zero(),
            runtime_local_sun_direction: VectorLF3::zero(),
            // mod_data: vec![],
            precision: 160,
            segment: 5,
            vein_bias_vector: VectorLF3::zero(),
        }
    }
}
