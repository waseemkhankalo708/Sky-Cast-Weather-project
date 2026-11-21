using System.Drawing.Drawing2D;

namespace SkyCastWeather.Helpers;

/// <summary>Weather code descriptions, colored vector icons, and formatting helpers.</summary>
public static class WeatherIconHelper
{
    private static readonly string[] CompassPoints = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };

    private enum WeatherKind { Sun, Moon, PartlyCloudy, Cloudy, Fog, Drizzle, Rain, Snow, Thunder }

    /// <summary>Maps a WMO weather code (used by Open-Meteo) to a short description.</summary>
    public static (string Emoji, string Description) DescribeWeatherCode(int code, bool isDay = true) => code switch
    {
        0 => isDay ? ("", "Clear sky") : ("", "Clear night"),
        1 => isDay ? ("", "Mainly clear") : ("", "Mainly clear"),
        2 => ("", "Partly cloudy"),
        3 => ("", "Overcast"),
        45 or 48 => ("", "Fog"),
        51 or 53 or 55 => ("", "Drizzle"),
        56 or 57 => ("", "Freezing drizzle"),
        61 or 63 or 65 => ("", "Rain"),
        66 or 67 => ("", "Freezing rain"),
        71 or 73 or 75 or 77 => ("", "Snow"),
        80 or 81 or 82 => ("", "Rain showers"),
        85 or 86 => ("", "Snow showers"),
        95 => ("", "Thunderstorm"),
        96 or 99 => ("", "Thunderstorm with hail"),
        _ => ("", "Unknown")
    };

    /// <summary>Draws a colored weather icon into a new bitmap that the caller owns.</summary>
    public static Bitmap CreateIcon(int weatherCode, bool isDay, int size)
    {
        var bitmap = new Bitmap(size, size);
        using var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);
        g.ScaleTransform(size / 100f, size / 100f); // design on a 100 x 100 grid

        switch (Classify(weatherCode, isDay))
        {
            case WeatherKind.Sun:
                DrawSun(g, 50, 50, 22);
                break;
            case WeatherKind.Moon:
                DrawMoon(g);
                break;
            case WeatherKind.PartlyCloudy:
                DrawSun(g, 38, 36, 15);
                DrawCloud(g, 14, 14);
                break;
            case WeatherKind.Cloudy:
                DrawCloud(g, 0, 0);
                break;
            case WeatherKind.Fog:
                DrawCloud(g, 0, -14);
                DrawFogLines(g);
                break;
            case WeatherKind.Drizzle:
                DrawCloud(g, 0, -8);
                DrawDots(g, new[] { new PointF(30, 82), new PointF(52, 86), new PointF(72, 80) }, Color.FromArgb(120, 190, 255));
                break;
            case WeatherKind.Rain:
                DrawCloud(g, 0, -8);
                DrawRainLines(g);
                break;
            case WeatherKind.Snow:
                DrawCloud(g, 0, -8);
                DrawDots(g, new[] { new PointF(28, 84), new PointF(50, 88), new PointF(72, 84) }, Color.FromArgb(235, 246, 255));
                break;
            case WeatherKind.Thunder:
                DrawCloud(g, 0, -8);
                DrawBolt(g);
                break;
        }

        return bitmap;
    }

    private static WeatherKind Classify(int code, bool isDay) => code switch
    {
        0 or 1 => isDay ? WeatherKind.Sun : WeatherKind.Moon,
        2 => WeatherKind.PartlyCloudy,
        3 => WeatherKind.Cloudy,
        45 or 48 => WeatherKind.Fog,
        >= 51 and <= 57 => WeatherKind.Drizzle,
        >= 61 and <= 67 => WeatherKind.Rain,
        >= 80 and <= 82 => WeatherKind.Rain,
        >= 71 and <= 77 => WeatherKind.Snow,
        85 or 86 => WeatherKind.Snow,
        >= 95 => WeatherKind.Thunder,
        _ => WeatherKind.Cloudy
    };

    private static void DrawSun(Graphics g, float cx, float cy, float r)
    {
        using var rayPen = new Pen(Color.FromArgb(255, 196, 60), 4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        for (var i = 0; i < 8; i++)
        {
            var angle = i * Math.PI / 4;
            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);
            g.DrawLine(rayPen, cx + cos * (r + 5), cy + sin * (r + 5), cx + cos * (r + 12), cy + sin * (r + 12));
        }

        using var brush = new SolidBrush(Color.FromArgb(255, 200, 60));
        g.FillEllipse(brush, cx - r, cy - r, r * 2, r * 2);
    }

    private static void DrawMoon(Graphics g)
    {
        using var outer = new GraphicsPath();
        outer.AddEllipse(22, 22, 56, 56);
        using var inner = new GraphicsPath();
        inner.AddEllipse(38, 16, 50, 50);
        using var crescent = new Region(outer);
        crescent.Exclude(inner);

        using var brush = new SolidBrush(Color.FromArgb(255, 220, 110));
        g.FillRegion(brush, crescent);
    }

    private static void DrawCloud(Graphics g, float dx, float dy)
    {
        var parts = new[]
        {
            new RectangleF(dx + 8, dy + 40, 36, 24),
            new RectangleF(dx + 26, dy + 26, 38, 36),
            new RectangleF(dx + 50, dy + 36, 32, 28),
            new RectangleF(dx + 12, dy + 44, 66, 22)
        };

        using var shadow = new SolidBrush(Color.FromArgb(110, 120, 145));
        foreach (var part in parts)
            g.FillEllipse(shadow, part.X, part.Y + 3, part.Width, part.Height);

        using var cloud = new SolidBrush(Color.FromArgb(236, 242, 252));
        foreach (var part in parts)
            g.FillEllipse(cloud, part);
        g.FillRectangle(cloud, dx + 20, dy + 50, 50, 14);
    }

    private static void DrawRainLines(Graphics g)
    {
        using var pen = new Pen(Color.FromArgb(80, 170, 255), 4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        g.DrawLine(pen, 28, 76, 22, 90);
        g.DrawLine(pen, 50, 76, 44, 90);
        g.DrawLine(pen, 72, 76, 66, 90);
    }

    private static void DrawFogLines(Graphics g)
    {
        using var pen = new Pen(Color.FromArgb(170, 180, 200), 5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        g.DrawLine(pen, 18, 70, 82, 70);
        g.DrawLine(pen, 24, 82, 76, 82);
        g.DrawLine(pen, 18, 94, 82, 94);
    }

    private static void DrawDots(Graphics g, PointF[] centers, Color color)
    {
        using var brush = new SolidBrush(color);
        foreach (var c in centers)
            g.FillEllipse(brush, c.X - 4, c.Y - 4, 8, 8);
    }

    private static void DrawBolt(Graphics g)
    {
        var points = new[]
        {
            new PointF(56, 58), new PointF(40, 84), new PointF(52, 84),
            new PointF(44, 100), new PointF(66, 70), new PointF(54, 70), new PointF(64, 58)
        };
        using var brush = new SolidBrush(Color.FromArgb(255, 214, 80));
        g.FillPolygon(brush, points);
    }

    public static string CompassDirection(double degrees)
    {
        var normalized = ((degrees % 360) + 360) % 360;
        var index = (int)Math.Round(normalized / 45.0) % 8;
        return CompassPoints[index];
    }

    public static string ToTitle(string text) =>
        System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
}
