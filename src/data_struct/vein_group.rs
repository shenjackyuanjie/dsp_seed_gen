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

impl VeinGroup {
    pub fn new(vein_type: EVeinType) -> Self {
        Self {
            vein_type,
            pos: VectorF3::zero(),
            count: 0,
            amount: 0,
        }
    }
}
