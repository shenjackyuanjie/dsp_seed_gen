use crate::data_struct::enums::EAstroType;
use crate::data_struct::vectors::{Quaternion, VectorLF3};

pub struct AstroData {
    pub id: i32,
    pub astro_type: EAstroType,
    pub parent_id: i32,
    pub u_radius: f32,
    pub u_rot: Quaternion,
    pub u_rot_next: Quaternion,
    pub u_pos: VectorLF3,
    pub u_pos_next: VectorLF3,
}

impl AstroData {
    pub fn new() -> Self {
        Self {
            id: 0,
            astro_type: EAstroType::None,
            parent_id: 0,
            u_radius: 0.0,
            u_rot: Quaternion::zeros(),
            u_rot_next: Quaternion::zeros(),
            u_pos: VectorLF3::zeros(),
            u_pos_next: VectorLF3::zeros(),
        }
    }
    pub fn position_u(&self, lpos: &mut VectorLF3) -> () {
        let num = 2.0 * lpos.x;
        let num2 = 2.0 * lpos.y;
        let num3 = 2.0 * lpos.z;
        let num4 = (self.u_rot.w * self.u_rot.w) as f64 - 0.5;
        let num5 =
            self.u_rot.x as f64 * num + self.u_rot.y as f64 * num2 + self.u_rot.z as f64 * num3;
        lpos.x = num * num4
            + (self.u_rot.y as f64 * num3 - self.u_rot.z as f64 * num2) * self.u_rot.w as f64
            + self.u_rot.x as f64 * num5
            + self.u_pos.x as f64;
        lpos.y = num2 * num4
            + (self.u_rot.z as f64 * num - self.u_rot.x as f64 * num3) * self.u_rot.w as f64
            + self.u_rot.y as f64 * num5
            + self.u_pos.y as f64;
        lpos.z = num3 * num4
            + (self.u_rot.x as f64 * num2 - self.u_rot.y as f64 * num) * self.u_rot.w as f64
            + self.u_rot.z as f64 * num5
            + self.u_pos.z as f64;
    }
}
