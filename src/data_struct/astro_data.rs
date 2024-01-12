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
    pub fn position_u_l(&self, upos: &mut VectorLF3) {
        let num = 2.0 * upos.x;
        let num2 = 2.0 * upos.y;
        let num3 = 2.0 * upos.z;
        let num4 = (self.u_rot.w * self.u_rot.w) as f64 - 0.5;
        let u_rot = self.u_rot.cast::<f64>();
        let num5 = u_rot.x * num + u_rot.y * num2 + u_rot.z * num3;
        upos.x = num * num4 + (u_rot.y * num3 - u_rot.z * num2) * u_rot.w + u_rot.x * num5 + self.u_pos.x;
        upos.y = num2 * num4 + (u_rot.z * num - u_rot.x * num3) * u_rot.w + u_rot.y * num5 + self.u_pos.y;
        upos.z = num3 * num4 + (u_rot.x * num2 - u_rot.y * num) * u_rot.w + u_rot.z * num5 + self.u_pos.z;
    }
    pub fn position_u_f(&self, upos: &mut VectorLF3) {
        let num = 2.0 * upos.x as f32;
        let num2 = 2.0 * upos.y as f32;
        let num3 = 2.0 * upos.z as f32;
        let num4 = ((self.u_rot.w * self.u_rot.w) as f64 - 0.5) as f32;
        let num5 = self.u_rot.x * num + self.u_rot.y * num2 + self.u_rot.z * num3;
        let u_rot = self.u_rot.cast::<f64>();
        upos.x = (num * num4
            + (self.u_rot.y * num3 - self.u_rot.z * num2) * self.u_rot.w
            + self.u_rot.x * num5
            + self.u_pos.x as f32) as f64;
        upos.y = (num2 * num4
            + (self.u_rot.z * num - self.u_rot.x * num3) * self.u_rot.w
            + self.u_rot.y * num5
            + self.u_pos.y as f32) as f64;
        upos.z = (num3 * num4
            + (self.u_rot.x * num2 - self.u_rot.y * num) * self.u_rot.w
            + self.u_rot.z * num5
            + self.u_pos.z as f32) as f64;
    }
}
