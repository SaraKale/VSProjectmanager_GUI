using System.Globalization;
using System.Resources;

// 语言类
public static class Lang
{
    private static ResourceManager rm =
        new ResourceManager("VSProjectManager_GUI.Resources.Strings",
        typeof(Lang).Assembly);

    public static string Get(string key)
    {
        return rm.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }

    public static void SetCulture(string culture)
    {
        CultureInfo.CurrentUICulture = new CultureInfo(culture);
    }
}
