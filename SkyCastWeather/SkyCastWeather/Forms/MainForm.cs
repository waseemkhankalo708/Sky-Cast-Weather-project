using System.Text.Json;
using SkyCastWeather.Controls;
using SkyCastWeather.Data;
using SkyCastWeather.Helpers;
using SkyCastWeather.Models;
using SkyCastWeather.Services;

namespace SkyCastWeather.Forms;

public partial class MainForm : Form
{
    private const string FallbackCity = "London";
    private readonly System.Windows.Forms.Timer _autoRefreshTimer = new();

    private readonly WeatherService _weatherService = new();
    private DatabaseService? _database;
    private GeoLocation? _currentLocation;
    private CancellationTokenSource? _loadCts;
    private MapForm? _mapForm;
    private AppTheme _theme = AppThemes.Colorful;

    public MainForm()
    {
        InitializeComponent();
        _autoRefreshTimer.Tick += async (_, _) => await RefreshAsync(forceRefresh: true);

        panelContent.Paint += (_, e) => PaintGradient(e, panelContent,
            _theme.ContentTop, _theme.ContentBottom);
        panelSidebar.Paint += (_, e) => PaintGradient(e, panelSidebar,
            _theme.SidebarTop, _theme.SidebarBottom);
    }

    // Unit helpers --------------------------------------------------------

    private bool IsImperial => cmbUnits.SelectedIndex == 1;
    private string TempSymbol => IsImperial ? "°F" : "°C";
    private string WindUnit => IsImperial ? "mph" : "m/s";
    private string CurrentUnits => IsImperial ? "imperial" : "metric";

    private void ApplyTheme(AppTheme theme)
    {
        _theme = theme;

        panelSidebar.Invalidate();
        panelContent.Invalidate();
        lblLogo.ForeColor = theme.Highlight;

        foreach (var nav in new[] { btnNavDashboard, btnNavMap, btnNavForecast, btnNavFavorites, btnNavSettings, btnNavAbout })
        {
            nav.BackColor = Color.Transparent;
            nav.ForeColor = theme.NavText;
        }
        btnNavDashboard.BackColor = theme.NavActive;
        btnNavDashboard.ForeColor = theme.Text;

        panelHeader.BackColor = theme.Header;
        txtSearch.BackColor = theme.Input;
        txtSearch.ForeColor = theme.Text;
        cmbUnits.BackColor = theme.Input;
        cmbUnits.ForeColor = theme.Text;
        StyleHeaderButton(btnSearch, theme.Accent, theme.AccentText);
        StyleHeaderButton(btnLocation, theme.Secondary, theme.Text);
        StyleHeaderButton(btnRefresh, theme.Secondary, theme.Text);
        StyleHeaderButton(btnFavorite, theme.Secondary, theme.Text);

        lblStatus.ForeColor = theme.Muted;
        panelCurrent.BackColor = theme.Card;
        lblCity.ForeColor = theme.Text;
        lblCountry.ForeColor = theme.Muted;
        lblTemperature.ForeColor = theme.Text;
        lblCondition.ForeColor = theme.Highlight;
        lblDescription.ForeColor = theme.Text;
        lblFeelsLike.ForeColor = theme.Muted;
        lblMinMax.ForeColor = theme.Muted;
        lblUpdated.ForeColor = theme.Muted;
        lblHourlyTitle.ForeColor = theme.Text;
        lblDailyTitle.ForeColor = theme.Text;
        flowHourly.BackColor = Color.Transparent;
        flowDaily.BackColor = Color.Transparent;

        foreach (var card in new[] { mcHumidity, mcWind, mcWindDirection, mcPressure, mcVisibility, mcCloudiness, mcUvIndex, mcSunrise, mcSunset })
            card.ApplyTheme(theme.Card, theme.Muted, theme.Text);

        ApplyThemeToTiles();
    }

    private void ApplyThemeToTiles()
    {
        foreach (var tile in flowHourly.Controls.OfType<ForecastTile>().Concat(flowDaily.Controls.OfType<ForecastTile>()))
            tile.ApplyTheme(_theme);
    }

    private static void StyleHeaderButton(Button button, Color back, Color fore)
    {
        button.BackColor = back;
        button.ForeColor = fore;
    }

    private static void PaintGradient(PaintEventArgs e, Control control, Color top, Color bottom)
    {
        if (control.Width <= 0 || control.Height <= 0)
            return;

        using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(control.ClientRectangle, top, bottom, 90f);
        e.Graphics.FillRectangle(brush, control.ClientRectangle);
    }

    // Startup and shutdown ------------------------------------------------

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _database = new DatabaseService(Path.Combine(ApiConfig.ConfigFolder, "skycast.db"));
        }
        catch (Exception ex)
        {
            SetStatus($"Could not open the local database: {ex.Message}", isError: true);
        }

        cmbUnits.SelectedIndex = _database?.GetSetting("Units") == "imperial" ? 1 : 0;
        lblDailyTitle.Text = "7-day forecast";
        ApplyTheme(AppThemes.Get(_database?.GetSetting("Theme")));

        ApplyAutoRefreshFromDatabase();

        var startCity = _database?.GetSetting("DefaultCity") ?? FallbackCity;
        await SearchAndLoadAsync(startCity);
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _autoRefreshTimer.Stop();
        _autoRefreshTimer.Dispose();
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        picIcon.Image?.Dispose();
        picIcon.Image = null;
    }

    // Event handlers (connected in MainForm.Designer.cs) ------------------

    private async void BtnSearch_Click(object? sender, EventArgs e) =>
        await SearchAndLoadAsync(txtSearch.Text.Trim());

    private async void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        await SearchAndLoadAsync(txtSearch.Text.Trim());
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e) =>
        await RefreshAsync(forceRefresh: true);

    private async void CmbUnits_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _database?.SetSetting("Units", CurrentUnits);
        await RefreshAsync(forceRefresh: false);
    }

    private async void BtnLocation_Click(object? sender, EventArgs e)
    {
        var usedApproximate = false;

        await RunWithLoadingAsync("Getting your location...", async ct =>
        {
            GeoLocation location;
            try
            {
                var (latitude, longitude) = await DeviceLocationService.GetPositionAsync(ct);
                location = await _weatherService.ReverseGeocodeAsync(latitude, longitude, ct);
            }
            catch (WeatherServiceException)
            {
                // Windows location is off or unavailable: fall back to an IP-based estimate.
                usedApproximate = true;
                var ip = await _weatherService.GetApproximateLocationAsync(ct);
                location = new GeoLocation
                {
                    Name = ip.City,
                    Country = ip.CountryName,
                    Latitude = ip.Latitude,
                    Longitude = ip.Longitude
                };
            }

            _currentLocation = location;
            await LoadWeatherAsync(location, forceRefresh: false, ct);
        });

        if (usedApproximate)
            SetStatus("Windows location is unavailable, so an approximate location was used. Turn on Location in Windows settings for accuracy.");
    }

    private void BtnFavorite_Click(object? sender, EventArgs e)
    {
        var location = _currentLocation;
        if (location is null || _database is null)
            return;

        var favorites = _database.GetFavorites();
        if (favorites.Any(f => f.Name == location.Name && f.Country == location.Country))
        {
            SetStatus($"{location.Name} is already in your favorites.");
            return;
        }

        _database.AddFavorite(location.Name, location.Country, location.Latitude, location.Longitude);
        SetStatus($"Saved {location.Name} to favorites.");

        if (_mapForm is not null && !_mapForm.IsDisposed)
            _ = _mapForm.ShowFavoritesAsync(_database.GetFavorites());
    }

    private void NavButton_Click(object? sender, EventArgs e)
    {
        if (sender == btnNavDashboard)
            return;

        if (sender == btnNavMap)
            OpenMap();
        else if (sender == btnNavFavorites)
            OpenFavorites();
        else if (sender == btnNavSettings)
            OpenSettings();
        else if (sender == btnNavForecast)
            panelContent.ScrollControlIntoView(lblDailyTitle);
        else if (sender == btnNavAbout)
            MessageBox.Show(this,
                "SkyCast Weather 2.0\n\nWeather data by Open-Meteo.\nMap tiles © OpenStreetMap contributors.\nRadar © RainViewer.",
                "About SkyCast Weather", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Secondary windows ---------------------------------------------------

    private void OpenMap()
    {
        if (_mapForm is null || _mapForm.IsDisposed)
        {
            _mapForm = new MapForm();
            _mapForm.LocationPicked += MapForm_LocationPicked;
            _mapForm.Show(this);

            if (_database is not null)
                _ = _mapForm.ShowFavoritesAsync(_database.GetFavorites());
        }
        else
        {
            _mapForm.Activate();
        }

        if (_currentLocation is not null)
            _ = _mapForm.ShowSelectionAsync(_currentLocation.Latitude, _currentLocation.Longitude);
    }

    private async void MapForm_LocationPicked(object? sender, MapLocationEventArgs e)
    {
        var location = new GeoLocation
        {
            Name = $"Point {e.Latitude:0.00}, {e.Longitude:0.00}",
            Country = "",
            Latitude = e.Latitude,
            Longitude = e.Longitude
        };
        _currentLocation = location;

        await RunWithLoadingAsync("Loading weather for the selected point...",
            ct => LoadWeatherAsync(location, forceRefresh: false, ct));
    }

    private void OpenFavorites()
    {
        if (_database is null)
            return;

        var form = new FavoritesForm(_database, _weatherService, IsImperial);
        form.OpenRequested += location =>
        {
            _currentLocation = location;
            _ = RunWithLoadingAsync("Loading weather...",
                ct => LoadWeatherAsync(location, forceRefresh: false, ct));
        };
        form.Show(this);
    }

    private void OpenSettings()
    {
        if (_database is null)
            return;

        var form = new SettingsForm(_database);
        form.SettingsSaved += (_, _) => ApplySettingsFromDatabase();
        form.Show(this);
    }

    private void ApplySettingsFromDatabase()
    {
        // Changing the unit selector also refreshes the weather.
        cmbUnits.SelectedIndex = _database?.GetSetting("Units") == "imperial" ? 1 : 0;
        ApplyTheme(AppThemes.Get(_database?.GetSetting("Theme")));
        ApplyAutoRefreshFromDatabase();
        SetStatus("Settings saved.");
    }

    private void ApplyAutoRefreshFromDatabase()
    {
        var enabled = _database?.GetSetting("AutoRefreshEnabled") != "false";
        var minutes = int.TryParse(_database?.GetSetting("AutoRefreshMinutes"), out var parsed)
            ? Math.Clamp(parsed, 5, 180)
            : 10;

        _autoRefreshTimer.Enabled = false;
        _autoRefreshTimer.Interval = (int)TimeSpan.FromMinutes(minutes).TotalMilliseconds;
        _autoRefreshTimer.Enabled = enabled;
    }

    // Loading logic -------------------------------------------------------

    private async Task SearchAndLoadAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;

        await RunWithLoadingAsync("Searching for city...", async ct =>
        {
            var matches = await _weatherService.SearchLocationAsync(query, ct);
            var location = matches.FirstOrDefault()
                ?? throw new WeatherServiceException($"No city found for \"{query}\". Try adding the country, e.g. \"Lahore, Pakistan\".");

            _currentLocation = location;
            await LoadWeatherAsync(location, forceRefresh: false, ct);
        });
    }

    private async Task RefreshAsync(bool forceRefresh)
    {
        var location = _currentLocation;
        if (location is null)
            return;

        await RunWithLoadingAsync("Refreshing weather...", ct =>
            LoadWeatherAsync(location, forceRefresh, ct));
    }

    private async Task LoadWeatherAsync(GeoLocation location, bool forceRefresh, CancellationToken ct)
    {
        var snapshot = await _weatherService.GetWeatherAsync(
            location.Latitude, location.Longitude, IsImperial, forceRefresh, ct);
        ShowSnapshot(snapshot, location);

        if (_mapForm is not null && !_mapForm.IsDisposed)
            _ = _mapForm.ShowSelectionAsync(location.Latitude, location.Longitude);
    }

    /// <summary>
    /// Runs one weather request while showing the loading indicator. A newer request cancels
    /// an older one, and every failure becomes a readable status message instead of a crash.
    /// </summary>
    private async Task RunWithLoadingAsync(string busyMessage, Func<CancellationToken, Task> work)
    {
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();
        var token = _loadCts.Token;

        SetBusy(true, busyMessage);
        try
        {
            await work(token);
            if (!token.IsCancellationRequested)
                SetStatus($"Updated at {DateTime.Now:HH:mm}");
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            // Superseded by a newer request. Nothing to show.
        }
        catch (OperationCanceledException)
        {
            SetStatus("The weather service took too long to respond. Please try again.", isError: true);
        }
        catch (WeatherServiceException ex)
        {
            SetStatus(ex.Message, isError: true);
        }
        catch (HttpRequestException)
        {
            SetStatus("Could not reach the weather service. Check your internet connection.", isError: true);
        }
        catch (JsonException)
        {
            SetStatus("The weather service sent an unexpected response.", isError: true);
        }
        finally
        {
            if (!token.IsCancellationRequested)
                SetBusy(false);
        }
    }

    // Display -------------------------------------------------------------

    private void ShowSnapshot(WeatherSnapshot snapshot, GeoLocation location)
    {
        var c = snapshot.Current;
        var (_, description) = WeatherIconHelper.DescribeWeatherCode(c.WeatherCode, c.IsDay);

        lblCity.Text = location.Name;
        lblCountry.Text = string.IsNullOrWhiteSpace(location.Admin1)
            ? location.Country
            : $"{location.Admin1}, {location.Country}";
        lblTemperature.Text = $"{Math.Round(c.Temperature)}{TempSymbol}";
        lblCondition.Text = description;
        lblDescription.Text = $"Cloud cover {c.CloudCover:0}%";
        lblFeelsLike.Text = $"Feels like {Math.Round(c.FeelsLike)}{TempSymbol}";
        lblMinMax.Text = $"Min {Math.Round(c.MinTemperature)}{TempSymbol}   Max {Math.Round(c.MaxTemperature)}{TempSymbol}";
        lblUpdated.Text = $"Last updated {snapshot.FetchedAt:HH:mm}";

        mcHumidity.Value = $"{c.Humidity:0}%";
        mcWind.Value = $"{c.WindSpeed:0.0} {WindUnit}";
        mcWindDirection.Value = $"{WeatherIconHelper.CompassDirection(c.WindDirection)} ({c.WindDirection:0}°)";
        mcPressure.Value = $"{c.PressureHpa:0} hPa";
        mcVisibility.Value = c.VisibilityMeters is double meters ? $"{meters / 1000:0.0} km" : "N/A";
        mcCloudiness.Value = $"{c.CloudCover:0}%";
        mcUvIndex.Value = c.UvIndex is double uv ? $"{uv:0.0}" : "N/A";
        mcSunrise.Value = c.Sunrise.ToString("HH:mm");
        mcSunset.Value = c.Sunset.ToString("HH:mm");

        picIcon.Image?.Dispose();
        picIcon.Image = WeatherIconHelper.CreateIcon(c.WeatherCode, c.IsDay, 96);

        ShowHourly(snapshot.Hourly);
        ShowDaily(snapshot.Daily);
        ApplyThemeToTiles();
    }

    private void ShowHourly(IReadOnlyList<HourlyPoint> hourly)
    {
        ClearTiles(flowHourly);

        foreach (var point in hourly)
        {
            flowHourly.Controls.Add(new ForecastTile
            {
                HeaderText = point.Time.ToString("HH:mm"),
                TemperatureText = $"{Math.Round(point.Temperature)}{TempSymbol}",
                DetailText = $"Rain {point.RainChancePercent:0}%",
                Icon = WeatherIconHelper.CreateIcon(point.WeatherCode, true, 56)
            });
        }
    }

    private void ShowDaily(IReadOnlyList<DailyPoint> daily)
    {
        ClearTiles(flowDaily);

        foreach (var day in daily)
        {
            var (_, description) = WeatherIconHelper.DescribeWeatherCode(day.WeatherCode);
            flowDaily.Controls.Add(new ForecastTile
            {
                HeaderText = day.Date.ToString("ddd dd MMM"),
                TemperatureText = $"{Math.Round(day.MaxTemperature)}° / {Math.Round(day.MinTemperature)}°",
                DetailText = description,
                Icon = WeatherIconHelper.CreateIcon(day.WeatherCode, true, 56)
            });
        }
    }

    private static void ClearTiles(FlowLayoutPanel panel)
    {
        foreach (var tile in panel.Controls.OfType<ForecastTile>().ToList())
        {
            panel.Controls.Remove(tile);
            tile.Dispose();
        }
    }

    private void SetStatus(string message, bool isError = false)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = isError ? Color.FromArgb(248, 113, 113) : _theme.Muted;
    }

    private void SetBusy(bool busy, string? message = null)
    {
        progressLoading.Visible = busy;
        if (message is not null)
            SetStatus(message);
    }
}
