using System.Text.Json.Serialization;

namespace SkyCastWeather.Models;

// ---------- Raw Open-Meteo JSON (matches the API response shape) ----------

public sealed class ForecastApiResponse
{
    public string Timezone { get; set; } = "";
    public CurrentBlock Current { get; set; } = new();
    public HourlyBlock Hourly { get; set; } = new();
    public DailyBlock Daily { get; set; } = new();
}

public sealed class CurrentBlock
{
    public string Time { get; set; } = "";

    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public double Humidity { get; set; }

    [JsonPropertyName("apparent_temperature")]
    public double ApparentTemperature { get; set; }

    [JsonPropertyName("is_day")]
    public int IsDay { get; set; }

    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; set; }

    [JsonPropertyName("cloud_cover")]
    public double CloudCover { get; set; }

    [JsonPropertyName("pressure_msl")]
    public double PressureMsl { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("wind_direction_10m")]
    public double WindDirection { get; set; }
}

public sealed class HourlyBlock
{
    public List<string> Time { get; set; } = new();

    [JsonPropertyName("temperature_2m")]
    public List<double?> Temperature { get; set; } = new();

    [JsonPropertyName("precipitation_probability")]
    public List<double?> PrecipitationProbability { get; set; } = new();

    [JsonPropertyName("weather_code")]
    public List<int?> WeatherCode { get; set; } = new();

    public List<double?> Visibility { get; set; } = new();

    [JsonPropertyName("uv_index")]
    public List<double?> UvIndex { get; set; } = new();
}

public sealed class DailyBlock
{
    public List<string> Time { get; set; } = new();

    [JsonPropertyName("weather_code")]
    public List<int?> WeatherCode { get; set; } = new();

    [JsonPropertyName("temperature_2m_max")]
    public List<double?> TemperatureMax { get; set; } = new();

    [JsonPropertyName("temperature_2m_min")]
    public List<double?> TemperatureMin { get; set; } = new();

    public List<string> Sunrise { get; set; } = new();
    public List<string> Sunset { get; set; } = new();

    [JsonPropertyName("uv_index_max")]
    public List<double?> UvIndexMax { get; set; } = new();
}

// ---------- Geocoding ----------

public sealed class GeocodingResponse
{
    [JsonPropertyName("results")]
    public List<GeoLocation>? Results { get; set; }
}

public sealed class GeoLocation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = "";

    [JsonPropertyName("admin1")]
    public string? Admin1 { get; set; }
}

// ---------- Approximate location (IP based, used by "My location") ----------

public sealed class IpLocationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; } = true;

    [JsonPropertyName("city")]
    public string City { get; set; } = "";

    [JsonPropertyName("country")]
    public string CountryName { get; set; } = "";

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

// ---------- App models (what the dashboard displays) ----------

public sealed record CurrentConditions(
    double Temperature,
    double FeelsLike,
    double Humidity,
    double WindSpeed,
    double WindDirection,
    double PressureHpa,
    double CloudCover,
    int WeatherCode,
    bool IsDay,
    double? VisibilityMeters,
    double? UvIndex,
    double MinTemperature,
    double MaxTemperature,
    DateTime Sunrise,
    DateTime Sunset);

public sealed record HourlyPoint(DateTime Time, double Temperature, double RainChancePercent, int WeatherCode);

public sealed record DailyPoint(DateOnly Date, double MaxTemperature, double MinTemperature, int WeatherCode, double? UvIndexMax);

public sealed record WeatherSnapshot(
    CurrentConditions Current,
    IReadOnlyList<HourlyPoint> Hourly,
    IReadOnlyList<DailyPoint> Daily,
    string Units,
    DateTimeOffset FetchedAt);

// ---------- Reverse geocoding (Nominatim) ----------

public sealed class NominatimResponse
{
    [JsonPropertyName("address")]
    public NominatimAddress? Address { get; set; }
}

public sealed class NominatimAddress
{
    public string? City { get; set; }
    public string? Town { get; set; }
    public string? Village { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
}
