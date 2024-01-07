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

/// RGBA颜色
/// 四个通道分别为R, G, B, A
pub type Color = VectorF4;

macro_rules! short_impl {
    ($vec_type: ty, $f_type: ty) => {
        pub fn from_vec(vec: $vec_type) -> Self {
            Self { vec }
        }
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
        pub fn is_parallel_to(&self, rhs: Self) -> bool {
            let cross_product = self.vec.cross(&rhs.vec);
            cross_product.magnitude() < 1e-6
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

impl Into<VectorF2> for VectorLF2 {
    fn into(self) -> VectorF2 {
        VectorF2 {
            vec: self.vec.cast(),
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

impl Into<VectorF3> for VectorLF3 {
    fn into(self) -> VectorF3 {
        VectorF3 {
            vec: self.vec.cast(),
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

impl Into<VectorF4> for VectorLF4 {
    fn into(self) -> VectorF4 {
        VectorF4 {
            vec: self.vec.cast(),
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

impl Into<VectorLF2> for VectorF2 {
    fn into(self) -> VectorLF2 {
        VectorLF2 {
            vec: self.vec.cast(),
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

impl Into<VectorLF3> for VectorF3 {
    fn into(self) -> VectorLF3 {
        VectorLF3 {
            vec: self.vec.cast(),
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

impl Into<VectorLF4> for VectorF4 {
    fn into(self) -> VectorLF4 {
        VectorLF4 {
            vec: self.vec.cast(),
        }
    }
}

impl Quaternion {
    pub fn q_rotate_lf(&self, v: VectorLF3) -> VectorLF3 {
        let v = (v * 2.0).vec;
        let adjust_factor = (self.vec.w * self.vec.w - 0.5) as f64;
        let f_vec: VectorLF4 = self.clone().into();
        let f_vec = f_vec.vec;
        let dot = f_vec.x * v.x + f_vec.y * v.y + f_vec.z * v.z;
        VectorLF3::new(
            v.x * adjust_factor + (f_vec.y * v.z - f_vec.z * v.y) * f_vec.w + f_vec.x * dot,
            v.y * adjust_factor + (f_vec.z * v.x - f_vec.x * v.z) * f_vec.w + f_vec.y * dot,
            v.z * adjust_factor + (f_vec.x * v.y - f_vec.y * v.x) * f_vec.w + f_vec.z * dot,
        )
    }
    pub fn look_rotation(forward: VectorLF3, up: VectorLF3) -> Quaternion {
        if forward.magnitude() == 0.0 || up.magnitude() == 0.0 || forward.is_parallel_to(up) {
            return Quaternion::new( 0.0, 0.0, 0.0, 1.0 );
        }

        let forward = forward.vec.normalize();
        let right = up.vec.cross(&forward).normalize();
        let up = forward.cross(&right);

        let m00 = right.x;
        let m01 = right.y;
        let m02 = right.z;
        let m10 = up.x;
        let m11 = up.y;
        let m12 = up.z;
        let m20 = forward.x;
        let m21 = forward.y;
        let m22 = forward.z;

        let num8 = (m00 + m11) + m22;
        let mut quaternion = Quaternion::zero();

        if num8 > 0.0 {
            let num = (num8 + 1.0).sqrt();
            quaternion.vec.w = (num * 0.5) as f32;
            let num2 = 0.5 / num;
            quaternion.vec.x = ((m12 - m21) * num2) as f32;
            quaternion.vec.y = ((m20 - m02) * num2) as f32;
            quaternion.vec.z = ((m01 - m10) * num2) as f32;
            return quaternion;
        }
        if (m00 >= m11) && (m00 >= m22) {
            let num7 = (((1.0 + m00) - m11) - m22).sqrt();
            let num4 = 0.5 / num7;
            quaternion.vec.x = (0.5 * num7) as f32;
            quaternion.vec.y = ((m01 + m10) * num4) as f32;
            quaternion.vec.z = ((m02 + m20) * num4) as f32;
            quaternion.vec.w = ((m12 - m21) * num4) as f32;
            return quaternion;
        }
        if m11 > m22 {
            let num6 = (((1.0 + m11) - m00) - m22).sqrt();
            let num3 = 0.5 / num6;
            quaternion.vec.x = ((m01 + m10) * num3) as f32;
            quaternion.vec.y = (0.5 * num6) as f32;
            quaternion.vec.z = ((m12 + m21) * num3) as f32;
            quaternion.vec.w = ((m02 - m20) * num3) as f32;
            return quaternion; 
        }
        let num5 = (((1.0 + m22) - m00) - m11).sqrt();
        let num2 = 0.5 / num5;
        quaternion.vec.x = ((m02 - m20) * num2) as f32;
        quaternion.vec.y = ((m12 - m21) * num2) as f32;
        quaternion.vec.z = (0.5 * num5) as f32;
        quaternion.vec.w = ((m01 + m10) * num2) as f32;
        quaternion
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
