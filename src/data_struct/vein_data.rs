use crate::data_struct::enums::EVeinType;
use crate::data_struct::vectors::VectorLF3;

pub static OIL_SPEED_MULTIPLIER: f32 = 4E-05;

pub struct VeinData {
    pub id: i32,
    pub vein_type: EVeinType,
    pub model_index: i16,
    pub group_index: i16,
    pub amount: i32,
    pub product_id: i32,
    pub pos: VectorLF3,
    pub combat_stat_id: i32,
    pub miner_count: i32,
    pub miner_id0: i32,
    pub miner_id1: i32,
    pub miner_id2: i32,
    pub miner_id3: i32,
    pub hash_address: i32,
    pub model_id: i32,
    pub collider_id: i32,
    pub miner_base_model_id: i32,
    pub miner_circle_model_id0: i32,
    pub miner_circle_model_id1: i32,
    pub miner_circle_model_id2: i32,
    pub miner_circle_model_id3: i32,
}
