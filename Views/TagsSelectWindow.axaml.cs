using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using System;

namespace VSProjectManager_GUI
{
    // 标签选择窗口类
    public partial class TagsSelectWindow : Window
    {
        public List<string> SelectedTags { get; private set; }
            = new List<string>();

        private ObservableCollection<string> TagItems;

        // 设计器用，传递假数据，否则无法在设计器中显示
        public TagsSelectWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中

            // 给设计器一点假数据
            TagItems = new ObservableCollection<string>
            {
                "Tag1",
                "Tag2",
                "Tag3"
            };
            TagsListBox.ItemsSource = TagItems;
        }

        // 运行时用
        public TagsSelectWindow(List<string> tags)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;// 窗体居中

            TagItems = new ObservableCollection<string>(tags);
            TagsListBox.ItemsSource = TagItems;
        }

        // 确定按钮点击事件
        private void ConfirmClick(object? sender, RoutedEventArgs e)
        {
            SelectedTags = TagsListBox.SelectedItems
                .Cast<string>()
                .ToList();

            Close(true);
        }

        // 取消按钮点击事件
        private void CancelClick(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
