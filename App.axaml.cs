using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using VSProjectManager_GUI.ViewModels;
using VSProjectManager_GUI.Views;

namespace VSProjectManager_GUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // 系统语言
            var culture = CultureInfo.CurrentUICulture.Name; 
            if (!new[] { "en", "zh-CN", "zh-TW" }.Contains(culture))
                culture = "en";
            LocalizationService.Instance.ChangeCulture(culture); // 设置当前语言 调用LocalizationService.cs

            // 主题切换
            var theme = IniHelper.GetValue("Theme");
            if (!string.IsNullOrWhiteSpace(theme))
            {
                switch (theme)
                {
                    case "Light":
                        RequestedThemeVariant = ThemeVariant.Light;
                        break;
                    case "Dark":
                        RequestedThemeVariant = ThemeVariant.Dark;
                        break;
                }
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindow(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }


        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}