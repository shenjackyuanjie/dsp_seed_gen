use bitflags::bitflags;

pub enum PlanetTypeEnum {
    None,
    Vocano,
    Ocean,
    Desert,
    Ice,
    Gas,
}

bitflags! {
    pub struct PlanetSingularityEnum: u32 {
        const NONE = 0;
        const TIDAL_LOCKED = 1;
        const TIDAL_LOCKED2 = 2;
        const TIDAL_LOCKED4 = 4;
        const LAY_SIDE = 8;
        const CLOCKWISE_ROTATE = 16;
        const MULTIPLE_SATELLITES = 32;
    }
}

pub enum StarTypeEnum {
    MainSeqStar,
    GiantStar,
    WhiteDwarf,
    NeutronStar,
    BlackHole,
}
pub enum SpectrTypeEnum {
    M = -4,
    K = -3,
    G = -2,
    F = -1,
    A = 0,
    B = 1,
    O = 2,
    X = 3,
}

impl SpectrTypeEnum {
    pub fn new(star_type: i32) -> SpectrTypeEnum {
        match star_type {
            -4 => SpectrTypeEnum::M,
            -3 => SpectrTypeEnum::K,
            -2 => SpectrTypeEnum::G,
            -1 => SpectrTypeEnum::F,
            0 => SpectrTypeEnum::A,
            1 => SpectrTypeEnum::B,
            2 => SpectrTypeEnum::O,
            _ => SpectrTypeEnum::X,
        }
    }
}
