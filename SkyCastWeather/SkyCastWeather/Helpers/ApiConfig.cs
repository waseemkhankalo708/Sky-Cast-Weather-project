namespace SkyCastWeather.Helpers;

/// <summary>Local folder for the SQLite database. Open-Meteo needs no API key.</summary>
public static class ApiConfig
{
    public static string ConfigFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SkyCastWeather");
}
