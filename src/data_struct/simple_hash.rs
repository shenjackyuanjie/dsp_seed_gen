pub struct SimpleHash {
    bits: u32,
}

impl SimpleHash {
    pub fn set_empty(&mut self) {
        self.bits = 0;
    }

    pub fn init_hash_bits(&mut self, x: f32, y: f32, z: f32) {
        let mut x_index = ((x + 200.0) / 40.0) as i32;
        let mut y_index = ((y + 200.0) / 40.0) as i32;
        let mut z_index = ((z + 200.0) / 40.0) as i32;
        x_index = if x_index < 9 {
            if x_index < 0 {
                0
            } else {
                x_index
            }
        } else {
            9
        };
        y_index = if y_index < 9 {
            if y_index < 0 {
                0
            } else {
                y_index
            }
        } else {
            9
        };
        z_index = if z_index < 9 {
            if z_index < 0 {
                0
            } else {
                z_index
            }
        } else {
            9
        };
        self.bits = (1 << x_index | 1024 << y_index | 1048576 << z_index);
    }

    pub fn mask_pass(&self, mask: u32) -> bool {
        (mask & self.bits) == self.bits
    }

    pub fn generate_hash_mask(x: f32, y: f32, z: f32, mut area: i32) -> u32 {
        let mut x_index = ((x + 200.0) / 40.0) as i32;
        let mut y_index = ((y + 200.0) / 40.0) as i32;
        let mut z_index = ((z + 200.0) / 40.0) as i32;
        x_index = if x_index < 9 {
            if x_index < 0 {
                0
            } else {
                x_index
            }
        } else {
            9
        };
        y_index = if y_index < 9 {
            if y_index < 0 {
                0
            } else {
                y_index
            }
        } else {
            9
        };
        z_index = if z_index < 9 {
            if z_index < 0 {
                0
            } else {
                z_index
            }
        } else {
            9
        };
        let mut hash_mask = 0;
        hash_mask |= 1 << x_index;
        hash_mask |= 1024 << y_index;
        hash_mask |= 1048576 << z_index;
        if area > 9 {
            area = 9;
        }
        for i in 1..=area {
            if x_index >= i {
                hash_mask |= 1 << x_index - i;
            }
            if y_index >= i {
                hash_mask |= 1024 << y_index - i;
            }
            if z_index >= i {
                hash_mask |= 1048576 << z_index - i;
            }
            if x_index < 10 - i {
                hash_mask |= 1 << x_index + i;
            }
            if y_index < 10 - i {
                hash_mask |= 1024 << y_index + i;
            }
            if z_index < 10 - i {
                hash_mask |= 1048576 << z_index + i;
            }
        }
        hash_mask
    }
}
