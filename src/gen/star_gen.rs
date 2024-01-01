pub const E: f64 = 2.7182817;
pub const GRAVITY: f64 = 1.3538551990520382E-06;
pub const PI: f64 = 3.141592653589793;

pub struct StarGen {
    /// hiveOrbitCondition
    pub hive_orbit_condition: [bool; 18],
    pub hive_orbit_radius: [f32; 18],
    pub orbit_radius: [f32; 17],
    pub planet_2_hive_orbit_tabel: [i32; 8],
    /// public static float specifyBirthStarMass = 0f;
    pub specify_birth_star_mass: f32,
    /// public static float specifyBirthStarAge = 0f;
    pub specify_birth_star_age: f32,
    /// private static double[] pGas = new double[10];
    pub p_gas: [f64; 10],
}
