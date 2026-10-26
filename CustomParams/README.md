# CustomParams 自定义参数元数据

把汉化/定制后的 `.apm.pdef.xml` 文件放在本文件夹，编译后会随程序复制到
`MissionPlanner.exe` 同目录的 `CustomParams\` 下，程序启动时**优先**从这里加载
参数的中文名称/说明/选项，找不到才回退到系统数据目录缓存。

## 文件命名约定

按机型文件名放：

- `ArduCopter.apm.pdef.xml`
- `ArduPlane.apm.pdef.xml`
- `Rover.apm.pdef.xml`
- `ArduSub.apm.pdef.xml`
- `AntennaTracker.apm.pdef.xml`
- `Heli.apm.pdef.xml`
- `Blimp.apm.pdef.xml`
- `AP_Periph.apm.pdef.xml`
- `SITL.apm.pdef.xml`

## 在 VS 中设置复制属性

选中本文件夹内所有 `.pdef.xml` 文件 → 属性窗口：

- 生成操作：`内容`（Content）
- 复制到输出目录：`如果较新则复制`

## 行为说明

- 服务器下载（autotest.ardupilot.org）已禁用，完全离线工作。
- 加载优先级：`exe目录/CustomParams/<机型>.apm.pdef.xml` → `数据目录/<机型>.apm.pdef.xml`。
- 自定义文件 XML 解析失败时会被删除并回退到缓存文件。
