use crate::data_struct::simple_hash::SimpleHash;
use crate::data_struct::vectors::{VectorLF3, VectorLF4};

pub struct VegeData {
    pub id: i32,
    pub proto_id: i16,
    pub model_index: i16,
    pub hash: SimpleHash,
    pub hash_address: i32,
    pub combat_stat_id: i32,
    pub pos: VectorLF3,
    pub rot: VectorLF4,
    pub scl: VectorLF3,
    pub model_id: i32,
    pub collider_id: i32,
}
