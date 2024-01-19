use nalgebra::{Unit, UnitQuaternion, Vector2, Vector3, Vector4};

pub type VectorF2 = Vector2<f32>;
pub type VectorF3 = Vector3<f32>;
pub type VectorF4 = Vector4<f32>;

pub type VectorLF2 = Vector2<f64>;
pub type VectorLF3 = Vector3<f64>;
pub type VectorLF4 = Vector4<f64>;

pub type Quaternion = VectorF4;

pub trait LocalVectors<T> {
    // fn zeros() -> Self;
    fn sqr_magnitude(&self) -> T;
}

pub trait LocalQuaternion {
    fn look_rotation(forward: &VectorLF3, up: &VectorLF3) -> Quaternion;
    fn angle_axis(angle: f32, axis: &VectorF3) -> Quaternion;
    fn rotation_lf(&self, v: &VectorLF3) -> VectorLF3;
}

impl LocalQuaternion for Quaternion {
    fn look_rotation(forward: &VectorLF3, up: &VectorLF3) -> Quaternion {
        let angle_quaternion = UnitQuaternion::look_at_lh(forward, up);
        angle_quaternion.as_vector().cast::<f32>()
    }

    fn angle_axis(angle: f32, axis: &VectorF3) -> Quaternion {
        let angle_quaternion = UnitQuaternion::from_axis_angle(&Unit::new_normalize(*axis), angle);
        angle_quaternion.as_vector().cast::<f32>()
    }

    fn rotation_lf(&self, v: &VectorLF3) -> VectorLF3 {
        let mut v = v * 2.0;
        let num = (self.w as f64).powi(2) - 0.5;
        let num2: f64 = self.x as f64 * v.x + self.y as f64 * v.y + self.z as f64 * v.z;
        v.x = v.x * num + (self.y as f64 * v.z - self.z as f64 * v.y) * self.w as f64 + self.x as f64 * num2;
        v.y = v.y * num + (self.z as f64 * v.x - self.x as f64 * v.z) * self.w as f64 + self.y as f64 * num2;
        v.z = v.z * num + (self.x as f64 * v.y - self.y as f64 * v.x) * self.w as f64 + self.z as f64 * num2;
        v
    }
}

impl LocalVectors<f32> for VectorF2 {
    fn sqr_magnitude(&self) -> f32 {
        self.x * self.x + self.y * self.y
    }
}

impl LocalVectors<f32> for VectorF3 {
    fn sqr_magnitude(&self) -> f32 {
        self.x * self.x + self.y * self.y + self.z * self.z
    }
}

impl LocalVectors<f32> for VectorF4 {
    fn sqr_magnitude(&self) -> f32 {
        self.x * self.x + self.y * self.y + self.z * self.z + self.w * self.w
    }
}

impl LocalVectors<f64> for VectorLF2 {
    fn sqr_magnitude(&self) -> f64 {
        self.x * self.x + self.y * self.y
    }
}

impl LocalVectors<f64> for VectorLF3 {
    fn sqr_magnitude(&self) -> f64 {
        self.x * self.x + self.y * self.y + self.z * self.z
    }
}

impl LocalVectors<f64> for VectorLF4 {
    fn sqr_magnitude(&self) -> f64 {
        self.x * self.x + self.y * self.y + self.z * self.z + self.w * self.w
    }
}
