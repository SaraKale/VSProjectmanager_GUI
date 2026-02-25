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
語言：<a href="README.md">English</a> | <a href="README_SC.md">简体中文</a>
</p>

## 介紹

這是用於給VScode擴展插件的 Project Manager 批量管理與生成 projects.json 配置文件，實現自動添加文件標籤和路徑功能。   
Project Manager擴展安裝：https://marketplace.visualstudio.com/items?itemName=alefragnani.project-manager

## 主要特點

- 支持拖拽 .json 文件路徑導入，支持自定義導出 JSON 文件。
- 批量添加輸入路徑（多行文本）
- 自動去重
- 支持填寫標籤，輸入 `Note1, Note2, Note3` ，自動拆分為數組 "tags": ["Note1","Note2","Note3",],
- 日間/夜間主題切換
- 程序多語言切換（English、简体中文、繁體中文）

## 下載

請選擇下面任意節點下載。

|   節點    |                                 鏈接                                 |
| :------: | :-----------------------------------------------------------------: |
|  Github  | [releases](https://github.com/SaraKale/VSProjectmanager_GUI/releases) |
|  Gitee   | [releases](https://gitee.com/sarakale/VSProjectmanager_GUI/releases)  |
|  lanzouu   | [Download](https://wwavg.lanzouu.com/b0rayryud)   密码:bskw |

Windows: [Download](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_win-x64.zip)  
Mac: [osx-x64](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_osx-x64.tar.gz) or [osx-arm64](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_osx-arm64.tar.gz)  
Linux: [Download](https://github.com/SaraKale/VSProjectmanager_GUI/releases/download/v1.0.0/VSProjectManager_GUI_v1.0.0_linux-arm64.tar.gz)

Windows系統請選擇 `VSProjectManager_GUI_v1.0.0_win-x64.zip`   
macOS系統請選擇 `VSProjectManager_GUI_v1.0.0_osx-x64.tar.gz`  或 `VSProjectManager_GUI_v1.0.0_osx-arm64.tar.gz`  
Linux系統請選擇 `VSProjectManager_GUI_v1.0.0_linux-arm64.tar.gz` 

## 運行環境

需要安裝 .NET 6.0 運行環境，請到這裡下載：https://dotnet.microsoft.com/zh-tw/download/dotnet/6.0  
選擇 「桌面運行時」（Runtime）而非 「SDK」，對應系統版本安裝即可。

支持系統版本：  
Windows：桌面端需 Windows 10 1607及以上（不支持 Win7/8/8.1）；服務器端需 Windows Server 2016 及以上。  
macOS：macOS 10.15 (Catalina) 及以上（不支持 10.14 及更低版本）。  
Linux：主流發行版的 LTS 版本（如 Ubuntu 18.04 LTS、Debian 10、CentOS 7、RHEL 7 等）。  

## 編譯構建

我的開發環境：  
系統：Windows 10  
環境：[Visual Studio 2022](https://visualstudio.microsoft.com/)  
框架：.NET 6.0  

需要安裝Nuget包：  
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

然後直接運行 `MMD MorphNote Project.sln` 編譯即可。

或者其他方式編譯，例如**dotnet**編譯：
```
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r osx-x64
dotnet publish -c Release -r osx-arm64
dotnet publish -c Release -r linux-x64
```

或者直接點擊 `BatchBuild.bat` 批量生成多平台框架。

## 使用方法

1、雙擊運行 `VSProjectManager_GUI` 程序即可。  
2、在菜單選擇你熟悉的語言。  
3、選擇打開 `projects.json` 配置文件，通常在： `C:\Users\用戶名\AppData\Roaming\Code\User\globalStorage\alefragnani.project-manager`   
4、標籤可填寫單個標籤或多個標籤，多個標籤填寫案例： `Note1, Note2, Note3` 。要用英文逗號間隔。  
5、然後可以添加文件或手動輸入文件路徑到列表中。  
6、最後點擊「生成JSON文件」按鈕就會更新JSON文件內容了。  

## 許可證

使用 [GPL-3.0 license](LICENSE) 許可證
