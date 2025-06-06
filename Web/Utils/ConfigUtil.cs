

using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Utils;

public static class ConfigUtil
{
    public static string GetConfigValue(string key)
    {
        return Environment.GetEnvironmentVariable(key) ?? throw new ArgumentNullException($"Environment variable '{key}' not found.");
    }

    public static string GetConnectionString(string name)
    {
        return GetConfigValue($"ConnectionStrings:{name}");
    }

    public static string GetAppSetting(string key)
    {
        return GetConfigValue($"AppSettings:{key}");
    }

    public static T? GetSection<T>(IConfiguration config, string sectionName) where T : class, new()
    {
        var section = config.GetSection(sectionName);
        if (section.Exists())
        {
            return section.Get<T>() ?? new T();
        }
        throw new ArgumentException($"Configuration section '{sectionName}' not found.");
    }

    public static List<TaiwanCity> GetTaiwanCitisSection(IConfiguration config)
    {
        var Cities = GetSection<List<TaiwanCity>>(config, "Cities");
        if (Cities == null || !Cities.Any())
        {
            throw new ArgumentException($"Configuration section 'Cities' not found or is empty.");
        }
        if (Cities.Any(x => x.CityName == "釣魚臺"))
            Cities.Remove(Cities.Find(x => x.CityName == "釣魚臺"));

        return Cities;

    }
}