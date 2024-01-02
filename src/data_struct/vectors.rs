use std::ops::{Add, AddAssign, Div, Mul, MulAssign, Neg, Sub, SubAssign};

use nalgebra::{Vector2, Vector3, Vector4};

#[derive(Clone, Copy)]
pub struct VectorLF2 {
    pub vec: Vector2<f64>,
}

#[derive(Clone, Copy)]
pub struct VectorLF3 {
    pub vec: Vector3<f64>,
}

#[derive(Clone, Copy)]
pub struct VectorLF4 {
    pub vec: Vector4<f64>,
}

#[derive(Clone, Copy)]
pub struct VectorF2 {
    pub vec: Vector2<f32>,
}

#[derive(Clone, Copy)]
pub struct VectorF3 {
    pub vec: Vector3<f32>,
}

#[derive(Clone, Copy)]
pub struct VectorF4 {
    pub vec: Vector4<f32>,
}

macro_rules! short_impl {
    ($vec_type: ty, $f_type: ty) => {
        pub fn zero() -> Self {
            Self {
                vec: <$vec_type>::zeros(),
            }
        }
        pub fn magnitude(&self) -> $f_type {
            self.vec.norm()
        }
        pub fn sqr_magnitude(&self) -> $f_type {
            self.vec.norm_squared()
        }
        pub fn normalized(&self) -> Self {
            if self.sqr_magnitude() < 1e-34 {
                return Self::zero();
            }
            Self {
                vec: self.vec.normalize(),
            }
        }
        pub fn move_towards(current: Self, target: Self, max_distance_delta: $f_type) -> Self {
            let delta = target - current;
            let magnitude = delta.magnitude();
            if magnitude <= max_distance_delta || magnitude < 1e-6 {
                return target.to_owned();
            }
            Self::zero() + delta / magnitude * max_distance_delta
        }
    };
}

macro_rules! derive_impl {
    ($type: ty, $f_type: ty) => {
        impl Add for $type {
            type Output = Self;
            fn add(self, rhs: Self) -> Self {
                Self {
                    vec: self.vec + rhs.vec,
                }
            }
        }
        impl Sub for $type {
            type Output = Self;
            fn sub(self, rhs: Self) -> Self {
                Self {
                    vec: self.vec - rhs.vec,
                }
            }
        }
        impl Div<$f_type> for $type {
            type Output = Self;
            fn div(self, rhs: $f_type) -> Self {
                Self {
                    vec: self.vec / rhs,
                }
            }
        }
        impl Mul<$f_type> for $type {
            type Output = Self;
            fn mul(self, rhs: $f_type) -> Self {
                Self {
                    vec: self.vec * rhs,
                }
            }
        }
        impl Neg for $type {
            type Output = Self;
            fn neg(self) -> Self {
                Self { vec: -self.vec }
            }
        }
        impl MulAssign<$f_type> for $type {
            fn mul_assign(&mut self, rhs: $f_type) {
                self.vec *= rhs;
            }
        }
        impl AddAssign for $type {
            fn add_assign(&mut self, rhs: Self) {
                self.vec += rhs.vec;
            }
        }
        impl SubAssign for $type {
            fn sub_assign(&mut self, rhs: Self) {
                self.vec -= rhs.vec;
            }
        }
    };
}

derive_impl!(VectorLF2, f64);
derive_impl!(VectorLF3, f64);
derive_impl!(VectorLF4, f64);
derive_impl!(VectorF2, f32);
derive_impl!(VectorF3, f32);
derive_impl!(VectorF4, f32);

impl VectorLF2 {
    short_impl!(Vector2<f64>, f64);
    pub fn new(x: f64, y: f64) -> Self {
        Self {
            vec: Vector2::new(x, y),
        }
    }
}

impl VectorLF3 {
    short_impl!(Vector3<f64>, f64);
    pub fn new(x: f64, y: f64, z: f64) -> Self {
        Self {
            vec: Vector3::new(x, y, z),
        }
    }
}

impl VectorLF4 {
    short_impl!(Vector4<f64>, f64);
    pub fn new(x: f64, y: f64, z: f64, w: f64) -> Self {
        Self {
            vec: Vector4::new(x, y, z, w),
        }
    }
}

impl VectorF2 {
    short_impl!(Vector2<f32>, f32);
    pub fn new(x: f32, y: f32) -> Self {
        Self {
            vec: Vector2::new(x, y),
        }
    }
}

impl VectorF3 {
    short_impl!(Vector3<f32>, f32);
    pub fn new(x: f32, y: f32, z: f32) -> Self {
        Self {
            vec: Vector3::new(x, y, z),
        }
    }
}

impl VectorF4 {
    short_impl!(Vector4<f32>, f32);
    pub fn new(x: f32, y: f32, z: f32, w: f32) -> Self {
        Self {
            vec: Vector4::new(x, y, z, w),
        }
    }
}
