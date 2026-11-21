using SkyCastWeather.Helpers;

namespace SkyCastWeather.Controls;

/// <summary>Reusable tile for one hourly or daily forecast entry.</summary>
public partial class ForecastTile : UserControl
{
    public ForecastTile()
    {
        InitializeComponent();
    }

    public string HeaderText
    {
        get => lblHeader.Text;
        set => lblHeader.Text = value;
    }

    public string TemperatureText
    {
        get => lblTemperature.Text;
        set => lblTemperature.Text = value;
    }

    public string DetailText
    {
        get => lblDetail.Text;
        set => lblDetail.Text = value;
    }

    /// <summary>The tile owns this image and disposes it when replaced or removed.</summary>
    public Image? Icon
    {
        get => picIcon.Image;
        set
        {
            picIcon.Image?.Dispose();
            picIcon.Image = value;
        }
    }

    public void ApplyTheme(AppTheme theme)
    {
        BackColor = theme.Card;
        lblHeader.ForeColor = theme.Muted;
        lblTemperature.ForeColor = theme.Text;
        lblDetail.ForeColor = theme.Highlight;
    }
}
