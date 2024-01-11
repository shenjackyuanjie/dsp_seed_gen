# dsp world gen todo

## 数据格式

- [x] Vectors
  - [x] `VectorLF 2/3/4` (`Vector<f64>`)
  - [x] `Vector 2/3/4` (`Vector<f32>`)
  - [x] `Quaternion` (就是 `VectorF4` )

- [ ] Enums
  - [x] `EPlanetType`
  - [x] `EPlanetSingularity`
  - [x] `EStarType`
  - [x] `ESpectrType`
  - [x] `EVeinType`
  - [ ] 检查是否有别的需要的 Enum

- [ ] `combat_settings`
  - [x] [主体](src/data_struct/combat_setting.rs)
  - [ ] 计算难度系数

- [ ] `game_desc`
  - [x] [主体](src/data_struct/game_desc.rs)
  - [ ] 计算难度系数
    - [ ] 计算元数据产出倍率 ( 在等战斗模式难度系数 )

- [x] [`simple_hash`](src/data_struct/simple_hash.rs)

- [ ] [`astro_orbit_data`](src/data_struct/astro_orbit_data.rs)
  - [x] 转换为 rust 代码
  - [x] `predict_pose`
  - [x] `get_velocity_at_point`
  - [ ] `get_estimate_point_offset`

- [ ] [`astro_data`](src/data_struct/astro_data.rs)
  - [x] 框架搭好
  - [ ] 那一大堆方法复刻好
    - [x] `PositionU`
      - [x] `position_u_l`
      - [x] `position_u_f`
    - [ ] `VelocityU`
    - [ ] `VelocityL2U`
    - [ ] `VelocityU2L`
    - [ ] `IdString`

- [ ] [`planet_data`](src/data_struct/planet_data.rs)
  - [ ] 完善成员

- [ ] [`planet_raw_data`](src/data_struct/planet_raw_data.rs)
  - [ ] 完善方法
  - [x] `calc_verts`
  - [x] `trans`
  - [x] `position_hash`
  - [x] `query_index`
  - [x] `add_vein/vege_data`

- [ ] [`star_data`](src/data_struct/star_data.rs)

- [x] [`vege_data`](src/data_struct/vega_data.rs)

- [x] [`vein_data`](src/data_struct/vein_data.rs)

- [x] [`vein_group`](src/data_struct/vein_group.rs)

## 生成

- [ ] [宇宙生成](src/gen/universe_gen.rs)
  - [ ] `universe_gen::create_galaxy`
  - [x] `universe_gen::random_poses`
  - [x] `universe_gen::generate_temp_poses`

- [ ] [星系生成](src/gen/star_gen.rs)
  - [x] `star_gen::create_birth_star`
  - [ ] `star_gen::create_star`
  - [x] `star_gen::set_star_age`

- [ ] [星球生成](src/gen/planet_gen.rs)
  - [ ] [星球生成算法](src/gen/planet_algorithm.rs)
    - [ ] [0~14](src/gen/planet_algorithm)
    - [ ] [生成参数](src/gen/planet_algorithm)

- [x] [名称生成](src/gen/name_gen.rs)
