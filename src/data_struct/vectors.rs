pub use nalgebra::{Unit, UnitQuaternion, Vector2, Vector3, Vector4};

pub type VectorF2 = Vector2<f32>;
pub type VectorF3 = Vector3<f32>;
pub type VectorF4 = Vector4<f32>;

pub type VectorLF2 = Vector2<f64>;
pub type VectorLF3 = Vector3<f64>;
pub type VectorLF4 = Vector4<f64>;

// pub type Quaternion = UnitQuaternion<f32>;
pub type Quaternion = VectorF4;

pub trait LocalVectors<T> {
    // fn zeros() -> Self;
    fn sqr_magnitude(&self) -> T;
}

// impl LocalVectors for Quaternion {
// fn zeros() -> Self {
//     Quaternion::new(VectorF3::zeros())
// }
// }

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
