use crate::data_struct::astro_data::AstroData;
use crate::data_struct::consts::PI;
use crate::data_struct::vectors::{Quaternion, VectorLF3, LocalQuaternion};

pub struct AstroOrbitData {
    orbit_radius: f32,
    orbit_inclination: f32,
    orbit_longitude: f32,
    orbital_period: f64,
    orbit_phase: f32,
    runtime_orbit_phase: f32,
    orbit_rotation: Quaternion,
    orbit_normal: VectorLF3,
}

impl AstroOrbitData {
    pub fn new() -> Self {
        Self {
            orbit_radius: 0.0,
            orbit_inclination: 0.0,
            orbit_longitude: 0.0,
            orbital_period: 0.0,
            orbit_phase: 0.0,
            runtime_orbit_phase: 0.0,
            orbit_rotation: Quaternion::zeros(),
            orbit_normal: VectorLF3::zeros(),
        }
    }

    pub fn predict_pose(&mut self, time: i64, center: VectorLF3, astro_data: &mut AstroData) {
        let num = 40000.0 * self.orbit_radius as f64;
        let mut num2: f64 =
            time as f64 / (self.orbital_period as f64 * 60.0) + self.orbit_phase as f64 / 360.0;
        let num3 = (num2 + 0.1).floor();
        num2 -= num3;
        self.runtime_orbit_phase = (num2 * 360.0) as f32;
        num2 *= 2.0 * PI;
        astro_data.u_pos =
            self.orbit_rotation
                .rotation_lf(VectorLF3::new(num2.cos(), 0.0, num2.sin()));
        astro_data.u_rot = Quaternion::look_rotation(self.orbit_normal, astro_data.u_pos);
        astro_data.u_pos = astro_data.u_pos * num + center;
        let mut num4 =
            (time as f64 + 1.0) / (self.orbital_period * 60.0) + self.orbit_phase as f64 / 360.0;
        let num5 = (num4 + 0.1).floor();
        num4 -= num5;
        // *= 6.283185307179586
        num4 *= 2.0 * PI;
        astro_data.u_pos_next =
            self.orbit_rotation
                .rotation_lf(VectorLF3::new(num4.cos(), 0.0, num4.sin()));
        astro_data.u_rot_next = Quaternion::look_rotation(self.orbit_normal, astro_data.u_pos_next);
        astro_data.u_pos_next = astro_data.u_pos_next * num + center;
    }

    pub fn get_velocity_at_point(&self, center: VectorLF3, u_pos: VectorLF3) -> VectorLF3 {
        let mut vector_lf = u_pos - center;
        self.orbit_rotation.rotation_lf(Quaternion::angle_axis(
            -360.0 / (self.orbital_period * 60.0),
            self.orbit_normal,
        )) - vector_lf
    }

    pub fn get_estimate_point_offset(&self, eta: f64) -> f32 {
        let num: f64 = PI * eta * 360.0 / self.orbital_period;
        self.orbit_radius * 40000.0 * 0.5 * (num.powi(2) as f32)
    }
}
