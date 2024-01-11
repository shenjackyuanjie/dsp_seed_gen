use std::fmt::Display;

use crate::data_struct::enums::EVeinType;
use crate::data_struct::vectors::VectorF3;

pub struct VeinGroup {
    /// 矿物类型
    pub vein_type: EVeinType,
    /// 位置
    pub pos: VectorF3,
    /// long count
    pub count: i32,
    /// long amount
    pub amount: i64,
}

impl Display for VeinGroup {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "[{:?}] {} | {}   @ {}", self.vein_type, self.amount, self.count, self.pos)
    }
}

impl VeinGroup {
    pub fn new(vein_type: EVeinType) -> Self {
        Self {
            vein_type,
            pos: VectorF3::zeros(),
            count: 0,
            amount: 0,
        }
    }
}
