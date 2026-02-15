<h1 align="center">VSProjectManager_GUI</h1>

<p align="center">
  <img src="Assets/screen_en.jpg" align="middle" width = "700"/>
    <br /><br />
    <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-dfd.svg"></a>
    <a href="https://github.com/SaraKale/VSProjectmanager_GUI/releases"><img src="https://img.shields.io/github/v/release/SaraKale/VSProjectmanager_GUI?color=ffa"></a>
    <a href=""><img src="https://img.shields.io/badge/NET 8.0-blue"></a>
    <a href=""><img src="https://img.shields.io/badge/os-win%20osx%20linux-yellow"></a>
</p>

<p align="center">
语言：<a href="README.md">English</a> | <a href="README_TC.md">繁體中文</a> 
</p>

## 介绍

这是用于给VScode扩展插件的 Project Manager 批量管理与生成 projects.json 配置文件，实现自动添加文件标签和路径功能。   
Project Manager扩展安装：https://marketplace.visualstudio.com/items?itemName=alefragnani.project-manager

## 主要特点

- 支持拖拽 .json 文件路径导入，支持自定义导出 JSON 文件。
- 批量添加输入路径（多行文本）
- 自动去重
- 支持填写标签，输入 `Note1, Note2, Note3` ，自动拆分为数组 "tags": ["Note1","Note2","Note3",],
- 日间/夜间主题切换
- 程序多语言切换（English、简体中文、繁体中文）

## 下载

请选择下面任意节点下载。

|   节点    |                                 链接                                 |
| :------: | :-----------------------------------------------------------------: |
|  Github  | [releases](https://github.com/SaraKale/VSProjectmanager_GUI/releases) |
|  Gitee   | [releases](https://gitee.com/sarakale/VSProjectmanager_GUI/releases)  |

Windows: [Download](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_win-x64.zip)  
Mac: [osx-x64](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_osx-x64.tar.gz) or [osx-arm64](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_osx-arm64.tar.gz)  
Linux: [Download](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_linux-arm64.tar.gz)

Windows系统请选择 `VSProjectManager_GUI_v1.0.0_win-x64.zip`   
macOS系统请选择 `VSProjectManager_GUI_v1.0.0_osx-x64.tar.gz`  或 `VSProjectManager_GUI_v1.0.0_osx-arm64.tar.gz`  
Linux系统请选择 `VSProjectManager_GUI_v1.0.0_linux-arm64.tar.gz` 

## 运行环境

需要安装 .NET 6.0 运行环境，请到这里下载：https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0  
选择 “桌面运行时”（Runtime）而非 “SDK”，对应系统版本安装即可。

支持系统版本：  
Windows：桌面端需 Windows 10 1607及以上（不支持 Win7/8/8.1）；服务器端需 Windows Server 2016 及以上。  
macOS：macOS 10.15 (Catalina) 及以上（不支持 10.14 及更低版本）。  
Linux：主流发行版的 LTS 版本（如 Ubuntu 18.04 LTS、Debian 10、CentOS 7、RHEL 7 等）。  

## 编译构建

我的开发环境：  
系统：Windows 10  
环境：[Visual Studio 2022](https://visualstudio.microsoft.com/)  
框架：.NET 6.0  

需要安装Nuget包：  
```
dotnet add package Avalonia
dotnet add package Avalonia.Desktop
dotnet add package Avalonia.Diagnostics
dotnet add package Avalonia.Fonts.Inter
dotnet add package Avalonia.Themes.Fluent
dotnet add package CommunityToolkit.Mvvm
dotnet add package Microsoft.NET.ILLink.Tasks
dotnet add package System.Text.Json
```

然后直接运行 `MMD MorphNote Project.sln` 编译即可。

或者其他方式编译，例如**dotnet**编译：
```
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r osx-x64
dotnet publish -c Release -r osx-arm64
dotnet publish -c Release -r linux-x64
```

或者直接点击 `BatchBuild.bat` 批量生成多平台框架。

## 使用方法

1、双击运行 `VSProjectManager_GUI` 程序即可。  
2、在菜单选择你熟悉的语言。  
3、选择打开 `projects.json` 配置文件，通常在： `C:\Users\用户名\AppData\Roaming\Code\User\globalStorage\alefragnani.project-manager`   
4、标签可填写单个标签或多个标签，多个标签填写案例： `Note1, Note2, Note3` 。要用英文逗号间隔。  
5、然后可以添加文件或手动输入文件路径到列表中。  
6、最后点击“生成JSON文件”按钮就会更新JSON文件内容了。  

## 许可证

使用 [GPL-3.0 license](LICENSE) 许可证
