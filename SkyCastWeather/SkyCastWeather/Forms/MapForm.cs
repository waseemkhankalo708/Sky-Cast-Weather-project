using System.Text.Json;
using Microsoft.Web.WebView2.Core;
using SkyCastWeather.Data;

namespace SkyCastWeather.Forms;

public sealed class MapLocationEventArgs : EventArgs
{
    public MapLocationEventArgs(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }
}

/// <summary>
/// Hosts the Leaflet map (Map/index.html) in WebView2. C# and JavaScript talk through
/// ExecuteScriptAsync (C# to JS) and WebMessageReceived (JS to C#).
/// </summary>
public partial class MapForm : Form
{
    private const string VirtualHost = "skycast.map";

    private bool _webReady;
    private readonly List<string> _pendingScripts = new();

    public event EventHandler<MapLocationEventArgs>? LocationPicked;

    public MapForm()
    {
        InitializeComponent();
        KeyPreview = true;
        KeyDown += MapForm_KeyDown;
        Load += MapForm_Load;
    }

    private async void MapForm_Load(object? sender, EventArgs e)
    {
        try
        {
            await webView.EnsureCoreWebView2Async();
            var core = webView.CoreWebView2;

            // Serve the Map folder under a fake https host so Leaflet and fetch() work normally.
            var mapFolder = Path.Combine(AppContext.BaseDirectory, "Map");
            core.SetVirtualHostNameToFolderMapping(VirtualHost, mapFolder, CoreWebView2HostResourceAccessKind.Allow);

            core.WebMessageReceived += CoreWebView2_WebMessageReceived;
            core.NavigationCompleted += CoreWebView2_NavigationCompleted;
            core.Navigate($"https://{VirtualHost}/index.html");
        }
        catch (Exception ex)
        {
            lblHint.Text = $"Map could not start. Install the Microsoft Edge WebView2 Runtime. ({ex.Message})";
        }
    }

    private async void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
        {
            lblHint.Text = "The map page failed to load. Check your internet connection.";
            return;
        }

        _webReady = true;
        foreach (var script in _pendingScripts)
            await webView.CoreWebView2.ExecuteScriptAsync(script);
        _pendingScripts.Clear();
    }

    private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        using var document = JsonDocument.Parse(e.WebMessageAsJson);
        var root = document.RootElement;

        if (!root.TryGetProperty("type", out var type) || type.GetString() != "pick")
            return;
        if (!root.TryGetProperty("lat", out var lat) || !root.TryGetProperty("lng", out var lng))
            return;

        LocationPicked?.Invoke(this, new MapLocationEventArgs(lat.GetDouble(), lng.GetDouble()));
    }

    // Public API used by MainForm ------------------------------------------

    public Task ShowSelectionAsync(double latitude, double longitude) =>
        RunScriptAsync("skycast.setSelection", new { lat = latitude, lng = longitude });

    public Task ShowFavoritesAsync(IEnumerable<FavoriteCity> favorites) =>
        RunScriptAsync("skycast.setMarkers", favorites
            .Select(f => new { lat = f.Latitude, lng = f.Longitude, name = f.Name })
            .ToList());

    private async Task RunScriptAsync(string functionName, object argument)
    {
        var script = $"{functionName}({JsonSerializer.Serialize(argument)});";

        // Calls made before the page finishes loading are queued and sent later.
        if (!_webReady)
        {
            _pendingScripts.Add(script);
            return;
        }

        await webView.CoreWebView2.ExecuteScriptAsync(script);
    }

    // Local handlers (connected in the Designer) ---------------------------

    private void CmbLayer_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _ = RunScriptAsync("skycast.setLayer", cmbLayer.SelectedIndex == 1 ? "radar" : "standard");
    }

    private void BtnFullscreen_Click(object? sender, EventArgs e) => ToggleFullscreen();

    private void MapForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape && FormBorderStyle == FormBorderStyle.None)
            ToggleFullscreen();
    }

    private void ToggleFullscreen()
    {
        if (FormBorderStyle == FormBorderStyle.None)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;
            btnFullscreen.Text = "Fullscreen";
        }
        else
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            btnFullscreen.Text = "Exit fullscreen";
        }
    }
}
