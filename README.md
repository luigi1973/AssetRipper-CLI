# AssetRipper CLI

[![English](https://img.shields.io/badge/README-English-blue?style=for-the-badge)](README.en.md)

从 Unity 游戏中提取素材的命令行工具 —— CG、音频、背景、立绘，一条命令搞定。

基于 [AssetRipper](https://github.com/AssetRipper/AssetRipper) 构建，专注于 CLI 导出工作流。

> **免责声明**
>
> 本工具是一个通用的 Unity 资源解析与导出工具，仅供学习研究、个人备份、游戏 MOD 开发等合法用途。用户应确保自身使用行为符合所在地区法律法规及相关作品的许可协议。本项目不鼓励、不协助、不认可任何形式的侵权行为。因使用本工具而产生的一切法律责任由使用者自行承担，与本项目及其贡献者无关。
>
> 如果您是版权所有者并认为本项目侵犯了您的权利，请参阅下方 [DMCA](#dmca) 部分。

## 环境要求

- [.NET SDK 10](https://dotnet.microsoft.com/download)（版本 >= `10.0.200`）

```powershell
# Windows 安装
winget install Microsoft.DotNet.SDK.10

# 确认版本
dotnet --version
# 输出应 >= 10.0.200
```

## 编译

```powershell
dotnet build AssetRipperCLI.slnx -c Release
```

设置别名方便后续使用：

```powershell
# PowerShell
Set-Alias arc .\artifacts\bin\AssetRipper.Tools.ExportRunner\Release\net10.0\AssetRipper.Tools.ExportRunner.exe
```

```bash
# Bash / Zsh
alias arc="dotnet ./artifacts/bin/AssetRipper.Tools.ExportRunner/Release/net10.0/AssetRipper.Tools.ExportRunner.dll"
```

以下示例统一使用 `arc` 作为别名。

## 快速上手

### 1. 检查 — 先看看游戏里有什么

```powershell
arc inspect .\GameRoot
```

输出素材数量、推荐的导出配置、内容分类概览。

### 2. 导出 — 提取你想要的素材

提取 CG（事件图、立绘、剧情插画）：

```powershell
arc export .\GameRoot --output .\output\cg --profile cg
```

提取音频（BGM、语音、音效）：

```powershell
arc export .\GameRoot --output .\output\audio --profile audio
```

全量提取：

```powershell
arc export .\GameRoot --output .\output\all --mode primary
```

### 3. 查看 — 确认导出结果

```powershell
arc report .\output\cg\export-manifest.json
```

## 预设配置

通过预设配置（Profile）可以按类别定向提取，不需要导出整个游戏。

| 配置 | 提取目标 |
|---|---|
| `cg` | 事件 CG、剧情插画、回忆画廊 |
| `audio` | BGM、语音、音效 |
| `characters` | 角色立绘、头像、半身像 |
| `backgrounds` | 场景背景 |
| `sprites` | 2D sprites |
| `ui` | UI 界面元素 |
| `player-art` | 面向玩家的美术素材 |
| `narrative` | 剧情文本相关素材 |
| `full-raw` | 全部主要内容，不做过滤 |
| `full-project` | 完整 Unity 工程导出 |

配置基于启发式匹配 —— 大多数游戏效果很好，但可能存在少量误差。建议先用针对性配置，不够再换 `full-raw`。

## 命令一览

| 命令 | 用途 |
|---|---|
| `inspect <路径>` | 快速内容概览 —— 拿到游戏先跑这个 |
| `analyze <路径> --report out.json` | 同 inspect，额外保存 JSON 报告 |
| `export <路径> --output <目录> --profile <名称>` | 按配置导出 |
| `export <路径> --output <目录> --mode primary\|dump` | 按导出模式导出 |
| `report <artifact.json>` | 将导出产物渲染为可读文本 |

### 导出选项

```
--keep-output                   保留输出目录已有内容（默认会清空）
--recursive-unpack on|off       是否递归解包嵌套 bundle（默认 on）
--shard-strategy off|direct-children|auto
                                大型资源库的分片并行策略
```

## 导出产物

每次导出会在输出目录生成结构化的记录文件：

| 文件 | 内容 |
|---|---|
| `export-plan.json` | 导出计划 |
| `export-manifest.json` | 实际导出清单 |
| `summary.txt` | 运行摘要 |
| `skipped-assets.json` | 被配置过滤跳过的集合 |
| `failed-assets.json` | 导出失败的集合 |

用 `arc report <文件>` 可以在终端查看任意产物。

## 推荐工作流

```
inspect  →  选配置  →  export  →  report 检查
```

1. 先跑 `inspect` 了解游戏内容
2. 选一个配置（`cg`、`audio`、`characters` 等）
3. 用该配置导出
4. 结果太少？换 `full-raw`；需要 Unity 工程？用 `full-project`
5. 用 `report` 检查导出清单和失败记录

## 已知限制

- **配置匹配基于启发式** —— 配置是快捷方式，不是完美分类器，建议检查导出结果
- **部分游戏会出现导入警告** —— 通常不影响导出，具体看 `failed-assets.json`
- **仅支持静态素材** —— Spine、Live2D、Cubism 等动态重建不在范围内

## 仓库结构

```
src/              CLI 源代码
vendor/           上游 AssetRipper 库（vendor 方式引入）
docs/             详细文档、架构说明、已知问题
```

## 详细文档

- [使用指南](docs/articles/CliUsageGuide.md) — 完整命令参考
- [实现状态](docs/articles/CliImplementationStatus.md) — 内部实现与执行模型
- [已知问题](docs/articles/CodeReviewFindings.md) — 当前限制与验证记录
- [架构说明](docs/articles/CliArchitectureRefactor.md) — 设计决策

## DMCA

本项目尊重知识产权。如果您是版权所有者并认为本仓库中的内容侵犯了您的权利，请通过 GitHub 提交 Issue 或按照 [GitHub DMCA 流程](https://docs.github.com/en/site-policy/content-removal-policies/dmca-takedown-policy) 发送通知，我们会及时处理。

## 致谢

本项目基于 [AssetRipper](https://github.com/AssetRipper/AssetRipper)，感谢上游项目的出色工作。

本项目由 [Linux.do](https://linux.do) 社区激励实现。学 AI，上 L 站！真诚、友善、团结、专业，共建你我引以为荣之社区。

## 许可证

本仓库整体按 [GPL-3.0](LICENSE) 分发。

本仓库包含并修改了来自 [AssetRipper](https://github.com/AssetRipper/AssetRipper) 的代码。第三方许可与归属说明请参阅 [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) 与 [vendor/assetripper/Source/Licenses](vendor/assetripper/Source/Licenses)。
