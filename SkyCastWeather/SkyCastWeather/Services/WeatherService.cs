using System.Globalization;
using System.Net;
using System.Text.Json;
using SkyCastWeather.Models;

namespace SkyCastWeather.Services;

/// <summary>A friendly, user-facing error (network problem, unknown city, bad response...).</summary>
public sealed class WeatherServiceException : Exception
{
    public WeatherServiceException(string message) : base(message) { }
}

/// <summary>
/// Talks to Open-Meteo (free, no API key) and maps its JSON to the app's models.
/// The UI never builds URLs or parses raw JSON itself.
/// </summary>
public sealed class WeatherService
{
    private static readonly HttpClient Http = CreateHttpClient();
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(10);

    private readonly Dictionary<string, (DateTime StoredUtc, WeatherSnapshot Snapshot)> _cache = new();

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("SkyCastWeather/1.0");
        return client;
    }

    /// <summary>Finds cities by name (Open-Meteo Geocoding API, free).</summary>
    public async Task<List<GeoLocation>> SearchLocationAsync(string query, CancellationToken ct = default)
    {
        var url = "https://geocoding-api.open-meteo.com/v1/search" +
                  $"?name={Uri.EscapeDataString(query)}&count=5&language=en&format=json";
        var response = await GetJsonAsync<GeocodingResponse>(url, ct);
        return response?.Results ?? new List<GeoLocation>();
    }

    /// <summary>Current weather, 24 hourly points and 7 daily points. Cached for 10 minutes.</summary>
    public async Task<WeatherSnapshot> GetWeatherAsync(
        double latitude, double longitude, bool imperial, bool forceRefresh = false, CancellationToken ct = default)
    {
        var units = imperial ? "imperial" : "metric";
        var cacheKey = $"{Coord(latitude)},{Coord(longitude)},{units}";
        if (!forceRefresh && TryGetCached(cacheKey, out var cached))
            return cached;

        var url = "https://api.open-meteo.com/v1/forecast" +
                  $"?latitude={Coord(latitude)}&longitude={Coord(longitude)}" +
                  "&current=temperature_2m,relative_humidity_2m,apparent_temperature,is_day,weather_code," +
                  "cloud_cover,pressure_msl,wind_speed_10m,wind_direction_10m" +
                  "&hourly=temperature_2m,precipitation_probability,weather_code,visibility,uv_index" +
                  "&daily=weather_code,temperature_2m_max,temperature_2m_min,sunrise,sunset,uv_index_max" +
                  "&timezone=auto&forecast_days=7" +
                  $"&temperature_unit={(imperial ? "fahrenheit" : "celsius")}" +
                  $"&wind_speed_unit={(imperial ? "mph" : "ms")}";

        var api = await GetJsonAsync<ForecastApiResponse>(url, ct)
            ?? throw new WeatherServiceException("The weather service returned no data.");

        var snapshot = MapSnapshot(api, units);
        _cache[cacheKey] = (DateTime.UtcNow, snapshot);
        return snapshot;
    }

    /// <summary>Approximate location from the internet connection (IP-based, not GPS).</summary>
    public async Task<IpLocationResponse> GetApproximateLocationAsync(CancellationToken ct = default)
    {
        var result = await GetJsonAsync<IpLocationResponse>("https://ipwho.is/", ct);
        if (result is null || !result.Success || (result.Latitude == 0 && result.Longitude == 0))
            throw new WeatherServiceException("Could not determine your approximate location. Use the search box instead.");
        return result;
    }

    /// <summary>Turns coordinates into a city name (OpenStreetMap Nominatim, free with attribution).</summary>
    public async Task<GeoLocation> ReverseGeocodeAsync(double latitude, double longitude, CancellationToken ct = default)
    {
        NominatimAddress? address = null;
        try
        {
            var url = "https://nominatim.openstreetmap.org/reverse?format=json&zoom=10&accept-language=en" +
                      $"&lat={Coord(latitude)}&lon={Coord(longitude)}";
            address = (await GetJsonAsync<NominatimResponse>(url, ct))?.Address;
        }
        catch (HttpRequestException)
        {
            // Name lookup is optional. Coordinates still work without it.
        }
        catch (WeatherServiceException)
        {
        }

        return new GeoLocation
        {
            Name = address?.City ?? address?.Town ?? address?.Village ?? address?.County ?? "My location",
            Country = address?.Country ?? "",
            Admin1 = address?.State,
            Latitude = latitude,
            Longitude = longitude
        };
    }

    // ---------- Mapping ----------

    private static WeatherSnapshot MapSnapshot(ForecastApiResponse api, string units)
    {
        var cur = api.Current;
        var now = ParseLocal(cur.Time);
        var hourIndex = FindHourIndex(api.Hourly.Time, now);

        var hourly = new List<HourlyPoint>();
        for (var i = hourIndex; i < Math.Min(hourIndex + 24, api.Hourly.Time.Count); i++)
        {
            hourly.Add(new HourlyPoint(
                ParseLocal(api.Hourly.Time[i]),
                ValueAt(api.Hourly.Temperature, i) ?? 0,
                ValueAt(api.Hourly.PrecipitationProbability, i) ?? 0,
                CodeAt(api.Hourly.WeatherCode, i)));
        }

        var daily = new List<DailyPoint>();
        for (var i = 0; i < api.Daily.Time.Count; i++)
        {
            daily.Add(new DailyPoint(
                DateOnly.FromDateTime(ParseLocal(api.Daily.Time[i])),
                ValueAt(api.Daily.TemperatureMax, i) ?? 0,
                ValueAt(api.Daily.TemperatureMin, i) ?? 0,
                CodeAt(api.Daily.WeatherCode, i),
                ValueAt(api.Daily.UvIndexMax, i)));
        }

        var today = 0;
        var current = new CurrentConditions(
            Temperature: cur.Temperature,
            FeelsLike: cur.ApparentTemperature,
            Humidity: cur.Humidity,
            WindSpeed: cur.WindSpeed,
            WindDirection: cur.WindDirection,
            PressureHpa: cur.PressureMsl,
            CloudCover: cur.CloudCover,
            WeatherCode: cur.WeatherCode,
            IsDay: cur.IsDay == 1,
            VisibilityMeters: ValueAt(api.Hourly.Visibility, hourIndex),
            UvIndex: ValueAt(api.Hourly.UvIndex, hourIndex),
            MinTemperature: ValueAt(api.Daily.TemperatureMin, today) ?? cur.Temperature,
            MaxTemperature: ValueAt(api.Daily.TemperatureMax, today) ?? cur.Temperature,
            Sunrise: api.Daily.Sunrise.Count > 0 ? ParseLocal(api.Daily.Sunrise[today]) : now,
            Sunset: api.Daily.Sunset.Count > 0 ? ParseLocal(api.Daily.Sunset[today]) : now);

        return new WeatherSnapshot(current, hourly, daily, units, DateTimeOffset.Now);
    }

    /// <summary>Index of the first hourly entry at or after the start of the current hour.</summary>
    private static int FindHourIndex(IReadOnlyList<string> hourlyTimes, DateTime now)
    {
        var hourStart = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
        for (var i = 0; i < hourlyTimes.Count; i++)
        {
            if (ParseLocal(hourlyTimes[i]) >= hourStart)
                return i;
        }
        return 0;
    }

    private static DateTime ParseLocal(string text) =>
        DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.None);

    private static double? ValueAt(List<double?> values, int index) =>
        index >= 0 && index < values.Count ? values[index] : null;

    private static int CodeAt(List<int?> codes, int index) =>
        index >= 0 && index < codes.Count ? codes[index] ?? 0 : 0;

    // ---------- Cache and HTTP ----------

    private bool TryGetCached(string key, out WeatherSnapshot snapshot)
    {
        if (_cache.TryGetValue(key, out var entry) && DateTime.UtcNow - entry.StoredUtc < CacheLifetime)
        {
            snapshot = entry.Snapshot;
            return true;
        }

        snapshot = null!;
        return false;
    }

    private static async Task<T?> GetJsonAsync<T>(string url, CancellationToken ct) where T : class
    {
        using var response = await Http.GetAsync(url, ct);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
            throw new WeatherServiceException("Too many requests. Please wait a minute and try again.");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        if (!response.IsSuccessStatusCode)
            throw new WeatherServiceException($"Weather service error ({(int)response.StatusCode}).");

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, ct);
    }

    private static string Coord(double value) => value.ToString("F4", CultureInfo.InvariantCulture);
}
