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

impl VeinData {
    pub fn new(id: i32, vein_type: EVeinType) -> Self { 
        VeinData {
            id,
            vein_type,
            model_index: 0,
            group_index: 0,
            amount: 0,
            product_id: 0,
            pos: VectorLF3::zero(),
            combat_stat_id: 0,
            miner_count: 0,
            miner_id0: 0,
            miner_id1: 0,
            miner_id2: 0,
            miner_id3: 0,
            hash_address: 0,
            model_id: 0,
            collider_id: 0,
            miner_base_model_id: 0,
            miner_circle_model_id0: 0,
            miner_circle_model_id1: 0,
            miner_circle_model_id2: 0,
            miner_circle_model_id3: 0,
        }
    }

    pub fn remove_miner(&mut self, miner_id: i32) {
        if self.miner_id0 == miner_id {
            self.miner_id0 = 0;
            self.miner_count -= 1;
            self.miner_id0 = self.miner_id1;
            self.miner_id1 = self.miner_id2;
            self.miner_id2 = self.miner_id3;
            self.miner_id3 = 0;
            return;
        }
        if self.miner_id1 == miner_id {
            self.miner_id1 = 0;
            self.miner_count -= 1;
            self.miner_id1 = self.miner_id2;
            self.miner_id2 = self.miner_id3;
            self.miner_id3 = 0;
            return;
        }
        if self.miner_id2 == miner_id {
            self.miner_id2 = 0;
            self.miner_count -= 1;
            self.miner_id2 = self.miner_id3;
            self.miner_id3 = 0;
            return;
        }
        if self.miner_id3 == miner_id {
            self.miner_id3 = 0;
            self.miner_count -= 1;
        }
    }

    pub fn add_miner(&mut self, miner_id: i32) {
        if miner_id == self.miner_id0 || miner_id == self.miner_id1 || miner_id == self.miner_id2 || miner_id == self.miner_id3 {
            return;
        }
        if self.miner_id0 == 0 {
            self.miner_id0 = miner_id;
            self.miner_count += 1;
            return;
        }
        if self.miner_id1 == 0 {
            self.miner_id1 = miner_id;
            self.miner_count += 1;
            return;
        }
        if self.miner_id2 == 0 {
            self.miner_id2 = miner_id;
            self.miner_count += 1;
            return;
        }
        if self.miner_id3 == 0 {
            self.miner_id3 = miner_id;
            self.miner_count += 1;
        }
    }
}
