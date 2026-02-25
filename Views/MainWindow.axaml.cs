using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using VSProjectManager_GUI.ViewModels;
using Res = VSProjectManager_GUI.Resources.Resources; // 多语言资源文件统一命名空间

namespace VSProjectManager_GUI.Views
{

    // 主窗口类
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> FileItems
        = new ObservableCollection<string>();
        internal static object TagsListBox;

        public MainWindow()
        {
            InitializeComponent();

            FileListBox.ItemsSource = FileItems;

            // 默认禁用保存路径
            SaveFilePathText.IsEnabled = false;
            SaveFilePathButton.IsEnabled = false;
            // 保存路径相关控件事件
            SaveFilePathCheckBox.Checked += SaveFilePathCheckBox_Changed;
            SaveFilePathCheckBox.Unchecked += SaveFilePathCheckBox_Changed;

            // 文件拖放事件
            OpenFilePathText.AddHandler(DragDrop.DropEvent, OpenFile_Drop);
            FileListBox.AddHandler(DragDrop.DropEvent, FileList_Drop);
            DragDrop.SetAllowDrop(OpenFilePathText, true);
            DragDrop.SetAllowDrop(FileListBox, true);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中

            LoadConfig(); // 加载配置文件
        }

        // 读取ini配置文件
        private void LoadConfig()
        {
            var configPath = IniHelper.GetValue("ConfigPath");
            var lang = IniHelper.GetValue("Lang");

            if (!string.IsNullOrWhiteSpace(configPath) &&
                File.Exists(configPath))
            {
                OpenFilePathText.Text = configPath;
            }

            if (!string.IsNullOrWhiteSpace(lang))
            {
                var ci = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }

        // 选择配置文件按钮
        public async void OpenFileButtonClik(object sender, RoutedEventArgs args)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = Res.Title_Selectprojectsfile, // 选择 projects.json 文件
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("JSON File")
                    {
                        Patterns = new[] { "*.json" }
                    }
                }
            });

            if (files.Count > 0)
            {
                string path = files[0].Path.LocalPath;
                OpenFilePathText.Text = path;

                IniHelper.SetValue("ConfigPath", path); // 保存配置文件路径
            }
        }

        // 添加文件按钮
        public async void AddFileButtonCilck(object sender, RoutedEventArgs args)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = Res.Title_Selectfile, // 选择文件
                AllowMultiple = true
            });

            foreach (var file in files)
            {
                string path = file.Path.LocalPath;

                if (!FileItems.Contains(path))
                    FileItems.Add(path);
            }
        }

        // 标签列表选择
        public async void TagsListClick(object sender, RoutedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(OpenFilePathText.Text) ||
                !File.Exists(OpenFilePathText.Text))
            {
                await ShowMessage(Res.Msg_SelectvalidJSONfile); // 请先选择有效的 JSON 文件。
                return;
            }

            List<ProjectItem> projects;

            try
            {
                string json = File.ReadAllText(OpenFilePathText.Text);
                projects = JsonSerializer.Deserialize<List<ProjectItem>>(json)
                           ?? new List<ProjectItem>();
            }
            catch
            {
                await ShowMessage(Res.Msg_JSONfileparsingfailed); // JSON 文件解析失败。
                return;
            }

            // 收集所有 tags
            var allTags = projects
                .Where(p => p.tags != null)
                .SelectMany(p => p.tags)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            if (allTags.Count == 0)
            {
                await ShowMessage(Res.Msg_Therenotags); // JSON 中没有找到任何标签。
                return;
            }

            var tagWindow = new TagsSelectWindow(allTags);

            var result = await tagWindow.ShowDialog<bool>(this);

            if (result)
            {
                TagsInputText.Text = string.Join(",", tagWindow.SelectedTags);
            }
        }


        // 打开手动输入路径窗口
        private void InputFileButtonCilck(object sender, RoutedEventArgs e)
        {
            var window = new InputFileListBox();
            window.Show();
        }


        // 删除文件按钮
        public void DelFileButtonCilck(object sender, RoutedEventArgs args)
        {
            var selected = FileListBox.SelectedItems.Cast<string>().ToList();

            foreach (var item in selected)
            {
                FileItems.Remove(item);
            }
        }


        // 清空文件
        public void CleanAllfileButton(object sender, RoutedEventArgs args)
        {
            FileItems.Clear();
        }


        // 保存路径选择
        private void SaveFilePathCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            bool enabled = SaveFilePathCheckBox.IsChecked == true;
            SaveFilePathText.IsEnabled = enabled; // 开启保存路径文本框
            SaveFilePathButton.IsEnabled = enabled; // 开启保存路径按钮
            OpenFilePathText.IsEnabled = !enabled; // 禁用打开文件路径文本框
            OpenFileButton.IsEnabled = !enabled; // 禁用打开文件按钮
        }

        // 选择保存路径按钮
        public async void SaveFilePathButtonClick(object sender, RoutedEventArgs args)
        {
            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = Res.Title_ChooseSaveJSONfile, // 选择保存 JSON 文件
                SuggestedFileName = "projects.json",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("JSON File")
                    {
                        Patterns = new[] { "*.json" }
                    }
                }
            });

            if (file != null)
            {
                SaveFilePathText.Text = file.Path.LocalPath;
            }
        }


        // 生成 JSON 文件按钮
        public async void OutputJSONClick(object sender, RoutedEventArgs args)
        {
            var fileList = FileListBox.Items.Cast<string>().ToList();

            // 检查是否有文件
            if (fileList.Count == 0)
            {
                await ShowMessage(Res.Msg_listempty); // 列表为空，请添加文件。
                return;
            }

            string outputPath; // 默认输出路径

            // 如果启用了保存路径选项，使用指定的路径
            if (SaveFilePathCheckBox.IsChecked == true &&
                !string.IsNullOrWhiteSpace(SaveFilePathText.Text))
            {
                outputPath = SaveFilePathText.Text;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(OpenFilePathText.Text))
                {
                    await ShowMessage(Res.Msg_Noconfigfileselected); // 没有选择配置文件路径
                    return;
                }

                outputPath = OpenFilePathText.Text;
            }

            List<ProjectItem> projects = new List<ProjectItem>();

            // 如果文件存在，读取现有内容
            if (File.Exists(outputPath))
            {
                try
                {
                    string json = File.ReadAllText(outputPath);
                    projects = JsonSerializer.Deserialize<List<ProjectItem>>(json)
                               ?? new List<ProjectItem>();
                }
                catch
                {
                    projects = new List<ProjectItem>();
                }
            }

            int oldCount = projects.Count;

            // 处理 tags
            // 多标签例："tags": ["Note1","Note2","Note3"]
            string tagText = TagsInputText.Text ?? "";
            var tagList = tagText
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();

            // json格式：
            // [
            //    {
            //        "name": "名称",
            //        "rootPath": "C:\\NOTE1.txt",
            //        "paths": [],
            //        "tags": ["note1"],
            //        "enabled": true
            //    },
            // ]

            int addedCount = 0;
            int skippedCount = 0;

            // 取出已有 rootPath（注意反序列化后已经是单斜杠形式）
            var existingPaths = projects
                .Select(p => p.rootPath)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var path in fileList)
            {
                if (existingPaths.Contains(path))
                {
                    skippedCount++;
                    continue;
                }

                string fileName = System.IO.Path.GetFileName(path);

                var item = new ProjectItem
                {
                    name = fileName, // 文件名
                    //rootPath = path.Replace("\\", "\\\\"), // json自动转义
                    rootPath = path, // 路径
                    paths = new List<string>(),
                    tags = tagList, // 使用输入的标签
                    enabled = true // 默认启用
                };

                projects.Add(item);
                existingPaths.Add(path);
                addedCount++;
            }

            // JSON 序列化选项
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // 格式化输出
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // 支持所有 Unicode 字符
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, // 强制序列化所有属性，包括null值和默认值
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // 确保属性名使用小驼峰命名（与您的期望格式一致）
            };

            File.WriteAllText(outputPath,
                JsonSerializer.Serialize(projects, options),
                new System.Text.UTF8Encoding(false));

            int newCount = projects.Count - oldCount;

            //await ShowMessage(
            // $"当前共 {projects.Count} 个文件\n" +
            // $"新增 {addedCount} 个文件\n" +
            // $"跳过 {skippedCount} 个重复文件。");

            await ShowMessage(
                string.Format(
                    Res.Msg_ProjectResult,
                    projects.Count,addedCount,skippedCount));
        }

        // 消息框
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
                LineHeight = 22, // 行高
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

        // 文件拖放
        private async void OpenFile_Drop(object? sender, DragEventArgs e)
        {
            if (e.DataTransfer is not IAsyncDataTransfer asyncData)
                return;

            var files = await asyncData.TryGetFilesAsync();

            if (files == null)
                return;

            foreach (var file in files)
            {
                var path = file.Path?.LocalPath;

                if (string.IsNullOrEmpty(path))
                    continue;

                if (System.IO.Path.GetExtension(path)
                    .Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    OpenFilePathText.Text = path;
                    IniHelper.SetValue("ConfigPath", path); // 保存配置文件路径
                    break;
                }
            }
        }


        // 文件列表框拖放文件
        private async void FileList_Drop(object? sender, DragEventArgs e)
        {
            if (e.DataTransfer is not IAsyncDataTransfer asyncData)
                return;

            var files = await asyncData.TryGetFilesAsync();

            if (files == null)
                return;

            foreach (var file in files)
            {
                var path = file.Path?.LocalPath;

                if (!string.IsNullOrEmpty(path) && !FileItems.Contains(path))
                    FileItems.Add(path);
            }
        }

        // 打开关于窗口
        private void AboutMenuClick(object sender, RoutedEventArgs e)
        {
            var window = new About();
            window.Show();
        }

        // 打开扩展链接
        private void ExpansionLinkClick(object? sender, RoutedEventArgs e)
        {
            var url = "https://marketplace.visualstudio.com/items?itemName=alefragnani.project-manager";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        // 动态切换语言
        private void SetLanguage(string culture)
        {
            IniHelper.SetValue("Lang", culture); // 保存语言设置
            var ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            // 强制刷新窗口
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        // 英语
        private void English_Click(object? sender, RoutedEventArgs e)
        {
            SetLanguage("en");
        }

        // 简体中文
        private void Simplified_Click(object? sender, RoutedEventArgs e)
        {
            SetLanguage("zh-Hans");
        }

        // 繁体中文
        private void Traditional_Click(object? sender, RoutedEventArgs e)
        {
            SetLanguage("zh-Hant");
        }

        // 切换主题
        private void SetTheme(string theme)
        {
            if (Application.Current == null)
                return;

            switch (theme)
            {
                case "Light":
                    Application.Current.RequestedThemeVariant = ThemeVariant.Light;
                    break;

                case "Dark":
                    Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
                    break;

                default:
                    Application.Current.RequestedThemeVariant = ThemeVariant.Default;
                    break;
            }

            IniHelper.SetValue("Theme", theme);
        }

        // 主题切换事件-日间模式
        private void ThemeLightClick(object? sender, RoutedEventArgs e)
        {
            SetTheme("Light");
        }

        // 主题切换事件-夜间模式
        private void ThemeDarkClick(object? sender, RoutedEventArgs e)
        {
            SetTheme("Dark");
        }

        // 导出-导出TXT列表
        private async void MENUExportTXTlIST(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OpenFilePathText.Text) || !File.Exists(OpenFilePathText.Text))
            {
                await ShowMessage(Res.Msg_SelectvalidJSONfile); // 请先选择有效的 JSON 文件。
                return;
            }

            try
            {
                // 读取 JSON 文件内容
                string json = File.ReadAllText(OpenFilePathText.Text);
                var projects = JsonSerializer.Deserialize<List<ProjectItem>>(json) ?? new List<ProjectItem>();

                // 收集所有标签和对应的路径
                Dictionary<string, List<string>> tagFiles = new Dictionary<string, List<string>>();

                foreach (var project in projects)
                {
                    if (project.tags != null && project.tags.Count > 0 && !string.IsNullOrWhiteSpace(project.rootPath))
                    {
                        foreach (var tag in project.tags)
                        {
                            if (!string.IsNullOrWhiteSpace(tag))
                            {
                                if (!tagFiles.ContainsKey(tag))
                                {
                                    tagFiles[tag] = new List<string>();
                                }
                                if (!tagFiles[tag].Contains(project.rootPath))
                                {
                                    tagFiles[tag].Add(project.rootPath);
                                }
                            }
                        }
                    }
                }

                // 生成 TXT 内容
                StringBuilder sb = new StringBuilder();
                foreach (var tag in tagFiles.Keys)
                {
                    sb.AppendLine($"tags: {tag}");
                    foreach (var file in tagFiles[tag])
                    {
                        sb.AppendLine(file);
                    }
                    sb.AppendLine();
                }

                // 弹出保存对话框
                var saveFile = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = Res.Title_ExportTXTlIST_Filesave, // 导出 TXT 列表
                    SuggestedFileName = "projects_list.txt",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("Text File")
                        {
                            Patterns = new[] { "*.txt" }
                        }
                    }
                });

                if (saveFile != null)
                {
                    // 保存文件
                    File.WriteAllText(saveFile.Path.LocalPath, sb.ToString(), Encoding.UTF8);
                    await ShowMessage($"{Res.Msg_ExportSuccess} {saveFile.Path.LocalPath}"); // 导出成功！文件导出在：
                }
            }
            catch (Exception ex)
            {
                await ShowMessage($"{Res.Msg_ExportFailed}: {ex.Message}"); // 导出失败: {ex.Message}
            }
        }

        // 导出-备份
        private async void MENUBackup(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OpenFilePathText.Text) || !File.Exists(OpenFilePathText.Text))
            {
                await ShowMessage(Res.Msg_SelectvalidJSONfile); // 请先选择有效的 JSON 文件。
                return;
            }

            try
            {
                // 创建 Backup 文件夹
                string backupDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                // 生成备份文件名
                string dateStr = DateTime.Now.ToString("yyyyMMdd");
                int index = 1;
                string backupFileName;
                do
                {
                    backupFileName = $"projects_{dateStr}_{index:000}.json";
                    index++;
                } while (File.Exists(System.IO.Path.Combine(backupDir, backupFileName)));

                // 复制文件
                string backupPath = System.IO.Path.Combine(backupDir, backupFileName);
                File.Copy(OpenFilePathText.Text, backupPath, true);

                // 显示成功消息
                string fullBackupPath = System.IO.Path.Combine(backupDir, backupFileName);
                await ShowMessage($"{Res.Msg_BackupSuccess} {fullBackupPath}"); // 备份成功！文件已保存到：
            }
            catch (Exception ex)
            {
                await ShowMessage($"{Res.Msg_BackupFailed}: {ex.Message}"); // 备份失败: {ex.Message}
            }
        }

        // ========== END =============
    }

    // JSON值
    public class ProjectItem
    {
        public string name { get; set; } = string.Empty;
        public string rootPath { get; set; } = string.Empty;
        public List<string> paths { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
        public bool enabled { get; set; } = true;
    }
}
