using SkyCastWeather.Data;
using SkyCastWeather.Helpers;
using SkyCastWeather.Models;
using SkyCastWeather.Services;

namespace SkyCastWeather.Forms;

public partial class FavoritesForm : Form
{
    private readonly DatabaseService _database;
    private readonly WeatherService _weatherService;
    private readonly bool _imperial;

    /// <summary>Raised when the user opens a favorite city on the dashboard.</summary>
    public event Action<GeoLocation>? OpenRequested;

    public FavoritesForm(DatabaseService database, WeatherService weatherService, bool imperial)
    {
        _database = database;
        _weatherService = weatherService;
        _imperial = imperial;

        InitializeComponent();
        Load += async (_, _) => await RefreshListAsync();
    }

    /// <summary>Reloads the list, then fills in each city's current temperature.</summary>
    private async Task RefreshListAsync()
    {
        var favorites = _database.GetFavorites();

        lvFavorites.BeginUpdate();
        lvFavorites.Items.Clear();
        foreach (var favorite in favorites)
        {
            var item = new ListViewItem(favorite.Name) { Tag = favorite };
            item.SubItems.Add(favorite.Country);
            item.SubItems.Add("Loading...");
            lvFavorites.Items.Add(item);
        }
        lvFavorites.EndUpdate();
        lblEmpty.Visible = favorites.Count == 0;

        foreach (var item in lvFavorites.Items.Cast<ListViewItem>().ToList())
        {
            var favorite = (FavoriteCity)item.Tag!;
            try
            {
                var snapshot = await _weatherService.GetWeatherAsync(favorite.Latitude, favorite.Longitude, _imperial);
                var current = snapshot.Current;
                var (_, description) = WeatherIconHelper.DescribeWeatherCode(current.WeatherCode, current.IsDay);
                var unit = _imperial ? "°F" : "°C";
                item.SubItems[2].Text = $"{Math.Round(current.Temperature)}{unit}  {description}";
            }
            catch (Exception)
            {
                item.SubItems[2].Text = "Weather unavailable";
            }
        }
    }

    private void BtnOpen_Click(object? sender, EventArgs e) => OpenSelected();

    private void LvFavorites_ItemActivate(object? sender, EventArgs e) => OpenSelected();

    private void OpenSelected()
    {
        if (lvFavorites.SelectedItems.Count == 0)
            return;

        var favorite = (FavoriteCity)lvFavorites.SelectedItems[0].Tag!;
        OpenRequested?.Invoke(new GeoLocation
        {
            Name = favorite.Name,
            Country = favorite.Country,
            Latitude = favorite.Latitude,
            Longitude = favorite.Longitude
        });
        Close();
    }

    private async void BtnRemove_Click(object? sender, EventArgs e)
    {
        if (lvFavorites.SelectedItems.Count == 0)
            return;

        var favorite = (FavoriteCity)lvFavorites.SelectedItems[0].Tag!;
        _database.RemoveFavorite(favorite.Id);
        await RefreshListAsync();
    }

    private void BtnClose_Click(object? sender, EventArgs e) => Close();
}
