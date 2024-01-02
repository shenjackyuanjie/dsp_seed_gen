use crate::data_struct::vectors::VectorLF3;
use crate::data_struct::vege_data::VegeData;
use crate::data_struct::vein_data::VeinData;

pub struct PlanetRawData {
    pub precision: i32,
    pub height_data: Vec<u16>,
    pub mod_data: Vec<u8>,
    pub vege_ids: Vec<u16>,
    pub biomo_data: Vec<u8>,
    pub tempr_data: Vec<i16>,
    pub vertices: Vec<VectorLF3>,
    pub normals: Vec<VectorLF3>,
    pub index_map: Vec<i32>,
    pub index_map_precision: i32,
    pub index_map_data_length: i32,
    pub index_map_face_stride: i32,
    pub index_map_corner_stride: i32,
    pub vein_pool: Vec<VeinData>,
    pub vein_cursor: i32,
    pub vein_capacity: i32,
    pub vege_pool: Vec<VegeData>,
    pub vege_cursor: i32,
    pub vege_capacity: i32,
}

// pub static POLES: [Vector3; 6] = [
//     Vector3 { x: 1.0, y: 0.0, z: 0.0 },
//     Vector3 { x: -1.0, y: 0.0, z: 0.0 },
//     Vector3 { x: 0.0, y: 1.0, z: 0.0 },
//     Vector3 { x: 0.0, y: -1.0, z: 0.0 },
//     Vector3 { x: 0.0, y: 0.0, z: 1.0 },
//     Vector3 { x: 0.0, y: 0.0, z: -1.0 },
// ];
