# dsp world gen todo

## 数据格式

- [x] Vectors
  - [x] `VectorLF 2/3/4` (`Vector<f64>`)
  - [x] `Vector 2/3/4` (`Vector<f32>`)

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
