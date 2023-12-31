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
    M,
    K,
    G,
    F,
    A,
    B,
    O,
    X,
}
