using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace VSProjectManager_GUI;

public partial class About : Window
{
    // 关于窗口
    public About()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
    }

    // 打开github链接
    private void Github_Click(object? sender, PointerPressedEventArgs e)
    {
        var url = "https://gitee.com/sarakale/VSProjectManager_GUI";

        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }


}