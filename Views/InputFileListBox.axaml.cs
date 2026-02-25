using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Res = VSProjectManager_GUI.Resources.Resources; // 多语言资源文件统一命名空间

namespace VSProjectManager_GUI
{
    // 手动输入文件列表框类
    public partial class InputFileListBox : Window
    {
        public InputFileListBox()
        {
            InitializeComponent();
            DragDrop.SetAllowDrop(BatchFileListBox, true);
            BatchFileListBox.AddHandler(DragDrop.DropEvent, Batch_Drop);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中
        }

        // 文件缩放
        private async void Batch_Drop(object? sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.Files))
            {
                var files = e.Data.GetFiles();

                foreach (var file in files)
                {
                    BatchFileListBox.Text += file.Path.LocalPath + Environment.NewLine;
                }
            }
        }

        // 手动输入路径
        public async void BatchFileClick(object sender, RoutedEventArgs args)
        {
            var main = (Application.Current.ApplicationLifetime
                as IClassicDesktopStyleApplicationLifetime)?
                .Windows
                .OfType<VSProjectManager_GUI.Views.MainWindow>()
                .FirstOrDefault();

            if (main == null) return;

            // 检查输入内容是否为空
            if (string.IsNullOrWhiteSpace(BatchFileListBox.Text))
            {
                await ShowMessage(Res.Msg_NoInputContent); // 提示没有输入内容
                return;
            }

            var lines = BatchFileListBox.Text
                .Split(new[] { "\r\n", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            var current = main.FileListBox.Items.Cast<string>().ToList();

            foreach (var line in lines)
            {
                string path = line.Trim();

                if (!main.FileItems.Contains(path))
                    main.FileItems.Add(path);
            }

            this.Close();
        }

        // 消息框布局
        private async System.Threading.Tasks.Task ShowMessage(string msg)
        {
            var dialog = new Window
            {
                Width = 350, // 宽度
                Height = 150, // 高度
                WindowStartupLocation = WindowStartupLocation.CenterOwner, // 窗口启动位置
                CanResize = false, // 禁止调整大小
                // Title = "提示"
            };

            dialog.Content = new TextBlock
            {
                Text = msg,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(20, 10, 10, 20), // 外边距：左右20，上下10
                //Padding = new Avalonia.Thickness(10), // 内边距
                LineHeight = 20, // 行高
                TextAlignment = Avalonia.Media.TextAlignment.Center // 文字居中对齐
            };

            await dialog.ShowDialog(this);
        }

        // 封装消息框
        private async Task ShowLocalizedMessage(string key, params object[] args)
        {
            var text = string.Format(
                Res.ResourceManager.GetString(key)!,
                args);

            await ShowMessage(text);
        }

        // ============ END ================

    }
}
