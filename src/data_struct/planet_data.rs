use std::rc::Rc;

use crate::data_struct::enums::{PlanetSingularityEnum, PlanetTypeEnum};
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::star_data::StarData;
use crate::data_struct::vectors::{VectorLF3, VectorLF4};

pub struct PlanetData {
    /// 星系数据
    pub galaxy: Rc<GalaxyData>,
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
    pub r#type: PlanetTypeEnum,
    /// 奇点
    pub singularity: PlanetSingularityEnum,
    /// 主题
    pub theme: i32,
    /// 算法ID
    pub algo_id: i32,
    /// 样式
    pub style: i32,
    /// 绕行行星
    pub orbit_around_planet: Rc<PlanetData>,
    /// 运行时位置
    pub runtime_position: VectorLF3,
    /// 下一个运行时位置
    pub runtime_position_next: VectorLF3,
    /// 运行时旋转（转换为VectorLF4）
    pub runtime_rotation: VectorLF4,
    /// 下一个运行时旋转（转换为VectorLF4）
    pub runtime_rotation_next: VectorLF4,
    /// 运行时系统旋转（转换为VectorLF4）
    pub runtime_system_rotation: VectorLF4,
    /// 运行时轨道旋转（转换为VectorLF4）
    pub runtime_orbit_rotation: VectorLF4,
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
    pub mod_data: Vec<u8>,
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
