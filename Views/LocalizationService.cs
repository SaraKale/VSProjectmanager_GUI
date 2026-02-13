using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Globalization;
using Res = VSProjectManager_GUI.Resources.Resources; // 多语言资源文件统一命名空间

namespace VSProjectManager_GUI;

public class LocalizationService : INotifyPropertyChanged
{
    private static LocalizationService? _instance;
    public static LocalizationService Instance =>
        _instance ??= new LocalizationService();

    public event PropertyChangedEventHandler? PropertyChanged;

    public string this[string key]
        => Res.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;

    public void ChangeCulture(string culture)
    {
        CultureInfo.CurrentUICulture = new CultureInfo(culture);

        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(null));
    }
}
