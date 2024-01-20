use bitflags::bitflags;

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub enum EPlanetType {
    None,
    Vocano,
    Ocean,
    Desert,
    Ice,
    Gas,
}

bitflags! {
    #[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
    pub struct EPlanetSingularity: u32 {
        const NONE = 0;
        const TIDAL_LOCKED = 1;
        const TIDAL_LOCKED2 = 2;
        const TIDAL_LOCKED4 = 4;
        const LAY_SIDE = 8;
        const CLOCKWISE_ROTATE = 16;
        const MULTIPLE_SATELLITES = 32;
    }
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, PartialOrd, Ord)]
pub enum EStarType {
    MainSeqStar = 0,
    GiantStar = 1,
    WhiteDwarf = 2,
    NeutronStar = 3,
    BlackHole = 4,
}

#[derive(Debug, Clone, Copy)]
pub enum ESpectrType {
    M = -4,
    K = -3,
    G = -2,
    F = -1,
    A = 0,
    B = 1,
    O = 2,
    X = 3,
}

impl ESpectrType {
    pub fn new(star_type: i32) -> ESpectrType {
        match star_type {
            -4 => ESpectrType::M,
            -3 => ESpectrType::K,
            -2 => ESpectrType::G,
            -1 => ESpectrType::F,
            0 => ESpectrType::A,
            1 => ESpectrType::B,
            2 => ESpectrType::O,
            _ => ESpectrType::X,
        }
    }
}

#[derive(Debug, Clone, Copy)]
pub enum EVeinType {
    None,
    Iron,
    Copper,
    Silicium,
    Titanium,
    Stone,
    Coal,
    Oil,
    Fireice,
    Diamond,
    Fractal,
    Crysrub,
    Grat,
    Bamboo,
    Mag,
    Max,
}

#[derive(Debug, Clone, Copy)]
pub enum EAstroType {
    None,
    Star,
    Planet,
    Asteroid,
    Fortress = 10,
    Platform = 15,
    EnemyHive = 20,
    Anchor = 30,
}
