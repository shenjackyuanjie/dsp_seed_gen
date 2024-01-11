use crate::data_struct::vectors::{LocalVectors, VectorF3};
use crate::data_struct::vege_data::VegeData;
use crate::data_struct::vein_data::VeinData;

pub static mut VERTS_80: [VectorF3; 80] = [];
pub static mut VERTS_200: [VectorF3; 200] = [];

pub static mut INDEX_MAP_80: [i32; 80] = [];
pub static mut INDEX_MAP_200: [i32; 200] = [];

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
    // pub vein_capacity: i32,
    pub vege_pool: Vec<VegeData>,
    pub vege_cursor: i32,
    // pub vege_capacity: i32,
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
            // vein_capacity: 0,
            vege_pool: Vec::new(),
            vege_cursor: 0,
            // vege_capacity: 0,
        }
    }
    pub fn trans(&self, x: f32, pr: i32) -> i32 {
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
    pub fn query_index(&self, vpos: &VectorF3) -> i32 {
        let mut vpos = vpos.to_owned().normalize();
        let num = self.position_hash(&vpos, None);
        let num2 = self.index_map[num as usize];
        let num3 = 3.1415927 / (self.precision * 2) as f64 * 0.25;
        let num3 = num3 * num3;
        let stride = self.index_map_face_stride;
        let mut num4 = 10.0;
        let mut result = num2;
        for i in -1..=3 {
            for j in -1..=3 {
                let num5 = num2 + i + j * stride;
                if num5 >= 0 && num5 < self.index_map_data_length {
                    let sqr_magnitude = (self.vertices[num5 as usize] - vpos).sqr_magnitude();
                    if sqr_magnitude < num3 as f32 {
                        return num5;
                    }
                    if sqr_magnitude < num4 {
                        num4 = sqr_magnitude;
                        result = num5;
                    }
                }
            }
        }
        return result;
    }
    pub fn add_vein_data(&mut self, mut vein_data: VeinData) -> i32 {
        self.vein_cursor += 1;
        vein_data.id = self.vein_cursor;
        self.vein_pool[self.vege_cursor as usize] = vein_data;
        return self.vege_cursor;
    }
    pub fn add_vege_data(&mut self, mut vege_data: VegeData) -> i32 {
        self.vege_cursor += 1;
        vege_data.id = self.vege_cursor;
        self.vege_pool[self.vege_cursor as usize] = vege_data;
        return self.vege_cursor;
    }
}
