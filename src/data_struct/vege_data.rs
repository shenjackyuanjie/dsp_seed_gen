use crate::data_struct::simple_hash::SimpleHash;
use crate::data_struct::vectors::{Quaternion, VectorF3};

pub struct VegeData {
    pub id: i32,
    pub proto_id: i16,
    pub model_index: i16,
    pub hash: SimpleHash,
    pub hash_address: i32,
    pub combat_stat_id: i32,
    pub pos: VectorF3,
    pub rot: Quaternion,
    pub scl: VectorF3,
    pub model_id: i32,
    pub collider_id: i32,
}

impl VegeData {
    pub fn new(id: i32, proto_id: i16) -> Self {
        VegeData {
            id,
            proto_id,
            model_index: 0,
            hash: SimpleHash::new(),
            hash_address: 0,
            combat_stat_id: 0,
            pos: VectorF3::zero(),
            rot: Quaternion::zero(),
            scl: VectorF3::zero(),
            model_id: 0,
            collider_id: 0,
        }
    }
}
