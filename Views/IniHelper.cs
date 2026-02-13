using System;
using System.Collections.Generic;
using System.IO;

// ini 读取/写入类
public static class IniHelper
{
    private static string IniPath =
        Path.Combine(AppContext.BaseDirectory, "ProjectManagerConfig.ini");

    public static Dictionary<string, string> ReadIni()
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (!File.Exists(IniPath))
            return dict;

        foreach (var line in File.ReadAllLines(IniPath))
        {
            if (string.IsNullOrWhiteSpace(line) ||
                line.StartsWith("[") ||
                !line.Contains("="))
                continue;

            var parts = line.Split('=', 2);
            dict[parts[0].Trim()] = parts[1].Trim();
        }

        return dict;
    }

    public static void WriteIni(Dictionary<string, string> dict)
    {
        using var sw = new StreamWriter(IniPath, false);

        sw.WriteLine("[General]");

        foreach (var kv in dict)
        {
            sw.WriteLine($"{kv.Key} = {kv.Value}");
        }
    }

    public static void SetValue(string key, string value)
    {
        var dict = ReadIni();
        dict[key] = value;
        WriteIni(dict);
    }

    public static string? GetValue(string key)
    {
        var dict = ReadIni();
        return dict.ContainsKey(key) ? dict[key] : null;
    }
}
