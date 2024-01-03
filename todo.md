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

## 生成

- [ ] [星系生成](src/gen/universe_gen.rs)
- [ ] [星球生成](src/gen/planet_gen.rs)
  - [ ] [星球生成算法](src/gen/planet_algorithm.rs)
    - [ ] [0~14](src/gen/planet_algorithm)
    - [ ] [生成参数](src/gen/planet_algorithm)
  - [ ] 
- [x] [名称生成](src/gen/name_gen.rs)
