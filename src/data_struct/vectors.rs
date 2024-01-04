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

/// 四元数
/// 用于旋转
/// 只是一个别名而已(￣▽￣)"
pub type Quaternion = VectorF4;

macro_rules! short_impl {
    ($vec_type: ty, $f_type: ty) => {
        pub fn zero() -> Self {
            Self {
                vec: <$vec_type>::zeros(),
            }
        }
        pub fn dot(&self, rhs: Self) -> $f_type {
            self.vec.dot(&rhs.vec)
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

#[test]
fn create_vector() {
    let v1 = VectorLF2::new(1.0, 2.0);
    let v2 = VectorLF3::new(1.0, 2.0, 3.0);
    let v3 = VectorLF4::new(1.0, 2.0, 3.0, 4.0);
    let v4 = VectorF2::new(1.0, 2.0);
    let v5 = VectorF3::new(1.0, 2.0, 3.0);
    let v6 = VectorF4::new(1.0, 2.0, 3.0, 4.0);
    assert_eq!(v1.vec, Vector2::new(1.0, 2.0));
    assert_eq!(v2.vec, Vector3::new(1.0, 2.0, 3.0));
    assert_eq!(v3.vec, Vector4::new(1.0, 2.0, 3.0, 4.0));
    assert_eq!(v4.vec, Vector2::new(1.0, 2.0));
    assert_eq!(v5.vec, Vector3::new(1.0, 2.0, 3.0));
    assert_eq!(v6.vec, Vector4::new(1.0, 2.0, 3.0, 4.0));
}

#[test]
fn dot() {
    let v1 = VectorLF2::new(1.0, 2.0);
    let v2 = VectorLF3::new(1.0, 2.0, 3.0);
    let v3 = VectorLF4::new(1.0, 2.0, 3.0, 4.0);
    let v4 = VectorF2::new(1.0, 2.0);
    let v5 = VectorF3::new(1.0, 2.0, 3.0);
    let v6 = VectorF4::new(1.0, 2.0, 3.0, 4.0);
    assert_eq!(v1.dot(v1), 5.0);
    assert_eq!(v2.dot(v2), 14.0);
    assert_eq!(v3.dot(v3), 30.0);
    assert_eq!(v4.dot(v4), 5.0);
    assert_eq!(v5.dot(v5), 14.0);
    assert_eq!(v6.dot(v6), 30.0);
}

#[test]
fn zero() {
    let v1 = VectorLF2::zero();
    let v2 = VectorLF3::zero();
    let v3 = VectorLF4::zero();
    let v4 = VectorF2::zero();
    let v5 = VectorF3::zero();
    let v6 = VectorF4::zero();
    assert_eq!(v1.vec, Vector2::new(0.0, 0.0));
    assert_eq!(v2.vec, Vector3::new(0.0, 0.0, 0.0));
    assert_eq!(v3.vec, Vector4::new(0.0, 0.0, 0.0, 0.0));
    assert_eq!(v4.vec, Vector2::new(0.0, 0.0));
    assert_eq!(v5.vec, Vector3::new(0.0, 0.0, 0.0));
    assert_eq!(v6.vec, Vector4::new(0.0, 0.0, 0.0, 0.0));
}

#[test]
fn magnitude() {
    let v1 = VectorLF2::new(1.0, 2.0);
    let v2 = VectorLF3::new(1.0, 2.0, 3.0);
    let v3 = VectorLF4::new(1.0, 2.0, 3.0, 4.0);
    let v4 = VectorF2::new(1.0, 2.0);
    let v5 = VectorF3::new(1.0, 2.0, 3.0);
    let v6 = VectorF4::new(1.0, 2.0, 3.0, 4.0);
    assert_eq!(v1.magnitude(), 2.23606797749979);
    assert_eq!(v2.magnitude(), 3.7416573867739413);
    assert_eq!(v3.magnitude(), 5.477225575051661);
    assert_eq!(v4.magnitude(), 2.23606797749979);
    assert_eq!(v5.magnitude(), 3.7416573867739413);
    assert_eq!(v6.magnitude(), 5.477225575051661);
}
