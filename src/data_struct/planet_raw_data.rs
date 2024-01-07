use crate::data_struct::vectors::VectorF3;
use crate::data_struct::vege_data::VegeData;
use crate::data_struct::vein_data::VeinData;

pub struct PlanetRawData {
    pub precision: i32,
    pub height_data: Vec<u16>,
    pub mod_data: Vec<u8>,
    pub vege_ids: Vec<u16>,
    pub biomo_data: Vec<u8>,
    pub tempr_data: Vec<i16>,
    pub vertices: Vec<VectorF3>,
    pub normals: Vec<VectorF3>,
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

impl PlanetRawData {
    pub fn new() -> Self {
        PlanetRawData {
            precision: 0,
            height_data: Vec::new(),
            mod_data: Vec::new(),
            vege_ids: Vec::new(),
            biomo_data: Vec::new(),
            tempr_data: Vec::new(),
            vertices: Vec::new(),
            normals: Vec::new(),
            index_map: Vec::new(),
            index_map_precision: 0,
            index_map_data_length: 0,
            index_map_face_stride: 0,
            index_map_corner_stride: 0,
            vein_pool: Vec::new(),
            vein_cursor: 0,
            vein_capacity: 0,
            vege_pool: Vec::new(),
            vege_cursor: 0,
            vege_capacity: 0,
        }
    }

    fn trans(&self, x: f32, pr: i32) -> i32 {
        let mut num = ((x + 0.23).sqrt() - 0.4795832) / 0.6294705 * pr as f32;
        if num >= pr as f32 {
            num = pr as f32 - 1.0;
        }
        num as i32
    }

    pub fn position_hash(&self, v: &VectorF3, corner: Option<i32>) -> i32 {
        let mut v = v.to_owned();
        let corner = match corner {
            Some(c) => c,
            None => {
                let mut c = 0;
                if v.x > 0.0 {
                    c += 1;
                }
                if v.y > 0.0 {
                    c += 2;
                }
                if v.z > 0.0 {
                    c += 4;
                }
                c
            }
        };
        v = v.abs();
        if v.x.abs() < 1e-06 && v.y.abs() < 1e-06 && v.z.abs() < 1e-06 {
            return 0;
        }
        let (num, num2, num3);
        if v.x >= v.y && v.x >= v.z {
            num = 0;
            num2 = self.trans(v.z / v.x, self.index_map_precision);
            num3 = self.trans(v.y / v.x, self.index_map_precision);
        } else if v.y >= v.x && v.y >= v.z {
            num = 1;
            num2 = self.trans(v.x / v.y, self.index_map_precision);
            num3 = self.trans(v.z / v.y, self.index_map_precision);
        } else {
            num = 2;
            num2 = self.trans(v.x / v.z, self.index_map_precision);
            num3 = self.trans(v.y / v.z, self.index_map_precision);
        }
        return num2
            + num3 * self.index_map_precision
            + num * self.index_map_face_stride
            + corner * self.index_map_corner_stride;
    }
}
