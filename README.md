# MR museum

HZT开发的 Unity VR 博物馆漫游项目，面向 Meta Quest 头显，支持 360° 全景展厅浏览与展区切换。

## 概述

本项目基于 Unity URP 模板构建，使用 Meta XR SDK 实现 VR 交互。核心玩法是：用户站在场景原点，通过旋转、平移包裹相机的全景球体来模拟在博物馆中移动，并使用控制器射线指向标记点切换不同展厅的全景贴图。

当前工程包含两套内容：

- **博物馆漫游逻辑**（`Playerinput.cs`、`MyTeleportation.cs`）— 自定义脚本，需手动配置场景后生效
- **Meta Interaction SDK 示例**（`SampleScene`）— 手势/射线抓取、UI 面板等演示内容

## 技术栈

| 组件 | 版本 |
|------|------|
| Unity | 2021.3.45f2c1 |
| URP | 12.1.15 |
| Meta XR SDK Core | 62.0.0 |
| Meta XR Interaction (OVR) | 62.0.0 |
| Oculus XR Plugin | 3.4.1 |
| XR Management | 4.5.2 |

## 环境要求

- Unity 2021.3 LTS（推荐 2021.3.45f2 或更高）
- Android Build Support（含 Android SDK & NDK）
- Meta Quest 2 / Quest Pro 头显（或对应模拟器）
- Meta Quest Developer Hub（用于设备调试与发布）

## 项目结构

```
Assets/
├── Scenes/
│   └── SampleScene.unity          # 主场景
├── MyTeleportation.cs             # 展区切换（射线传送）
├── Playerinput.cs                 # 全景球体控制
├── MouseLook.cs                   # 编辑器鼠标视角调试
├── TeleportPoint.cs               # Meta 官方传送柱示例
├── TeleportPoint.prefab           # 传送柱预制体
├── Materials/
│   ├── ColumnGlow.shader          # 传送柱发光着色器
│   └── ColumnGlow.mat
├── Resources/                     # 运行时加载资源（需补充全景材质）
├── Plugins/Android/
│   └── AndroidManifest.xml        # Quest VR 配置
├── Oculus/
│   └── OculusProjectConfig.asset  # Meta XR 项目设置
└── XR/
    ├── Loaders/OculusLoader.asset
    └── Settings/OculusSettings.asset
```

## 核心脚本

### Playerinput.cs

挂载在全景球体（`Sphere`）上，通过移动球体模拟用户漫游。

| 输入 | 功能 |
|------|------|
| A 键 | 切换到 `location_61` 全景材质 |
| B 键 | 恢复上一个材质 |
| 右握把 | 球体归位到 `(0, 0, 0)` |
| 右摇杆 ← / → | 每次拨动绕 Y 轴旋转 10° |
| 右摇杆 ↑ / ↓ | 沿头显朝向前/后平移 2 单位 |

### MyTeleportation.cs

挂载在右控制器上，通过射线选择并切换展厅。

1. **按住右扳机（> 0.6）** — 显示激光线与落点指针
2. **射线命中 `nextmarker` 标签物体** — 激光变绿，标记可传送
3. **松开扳机** — 切换 `Sphere` 材质（`location_61` ↔ `location_01`）

### MouseLook.cs

鼠标左键拖动旋转视角，仅用于 PC 编辑器内调试，不参与 VR 运行时逻辑。

### TeleportPoint.cs

来自 Meta Sample Framework 的传送柱组件，使用 `ColumnGlow` 着色器实现注视高亮效果。与 `MyTeleportation` 的 `nextmarker` 机制相互独立。

## 场景配置指南

自定义博物馆逻辑需要以下步骤才能在场景中生效：

### 1. 创建全景球体

1. 在场景中创建 `Sphere`，置于世界原点
2. 将 Sphere 法线翻转（Scale X 设为 `-1`，或使用双面材质）
3. 使用全景 Shader（如 `panoramic`）并赋予初始展厅贴图
4. 挂载 `Playerinput.cs`

### 2. 准备全景材质

将展厅全景材质放入 `Assets/Resources/Materials/`：

```
Assets/Resources/Materials/
├── location_01.mat    # 展厅 1
└── location_61.mat    # 展厅 2
```

材质需使用全景 Shader，并绑定对应的 360° 贴图。

### 3. 配置传送标记

1. 在展厅内放置传送点物体（可使用 Cube 或自定义模型）
2. 设置 Tag 为 `nextmarker`
3. 确保物体带有 Collider，以便射线检测

### 4. 挂载传送脚本

1. 在 `OVRCameraRig` 的右控制器下找到射线发射点
2. 挂载 `MyTeleportation.cs`
3. 在 Inspector 中关联以下引用：
   - `laser` — LineRenderer 组件
   - `pointer` — 落点指示物体
   - `player` — 玩家根物体
   - `sourceMat` — 激光线基础材质

## 构建与部署

### 构建设置

1. **File → Build Settings**
2. 平台选择 **Android**
3. 确认场景列表包含 `Assets/Scenes/SampleScene.unity`
4. **Player Settings** 中确认：
   - Company Name: `HZT`
   - Product Name: `MR museum`
   - Package Name: `com.HZT.MRmuseum`
   - Minimum API Level: Android 10 (API 29)
   - XR Plug-in Management → Oculus 已启用

### 部署到 Quest

1. 通过 USB 连接 Quest 头显并开启开发者模式
2. 在 Build Settings 中点击 **Build And Run**
3. 或使用 Meta Quest Developer Hub 侧载 APK

### 支持的设备

根据 `AndroidManifest.xml` 配置，支持以下设备：

- Quest 2
- Quest Pro
- Quest 3（eureka）

## 交互架构

```
用户站在世界原点
    │
    ▼
头显跟踪真实头部转动（OVRCameraRig）
    │
    ├── Playerinput：旋转/平移 Sphere → 模拟展厅内移动
    │
    └── MyTeleportation：右扳机射线瞄准 nextmarker → 切换 Sphere 贴图 → 进入新展厅
```

## 已知问题

| 问题 | 说明 |
|------|------|
| 自定义脚本未挂载 | `MyTeleportation`、`Playerinput` 当前未关联到场景对象 |
| 缺少全景材质 | `Resources/Materials/location_01` 与 `location_61` 尚未添加 |
| 缺少 Sphere 对象 | 场景中不存在名为 `Sphere` 的全景球体 |
| 缺少 nextmarker 标签 | 无传送标记物体，`MyTeleportation` 无法触发切换 |
| 材质内存泄漏 | `MyTeleportation.Update` 每帧 `new Material()`，建议改为缓存复用 |
| 硬编码查找 | 多处使用 `GameObject.Find("Sphere")`，建议改为 Inspector 引用 |

## 开发建议

- 将 `GameObject.Find` 替换为 `[SerializeField]` 引用，提升性能与可维护性
- 缓存激光线材质，避免每帧创建新实例
- 扩展 `location_XX` 材质列表，支持更多展厅而非仅两个
- 视项目需求决定保留或移除 Meta Interaction SDK 演示内容

## 许可证

- `TeleportPoint.cs`、`ColumnGlow.shader` 来自 Meta Sample Framework，遵循其许可条款
- 其余自定义脚本版权归 HZT 所有

## 相关链接

- [Meta XR SDK 文档](https://developer.oculus.com/documentation/unity/)
- [Unity URP 文档](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.1/manual/index.html)
- [Oculus Interaction SDK](https://developer.oculus.com/documentation/unity/unity-isdk-overview/)
