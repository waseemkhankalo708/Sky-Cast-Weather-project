namespace SkyCastWeather.Helpers;

/// <summary>Every color the dashboard uses. Change a palette here to restyle the whole app.</summary>
public sealed record AppTheme(
    string Name,
    Color SidebarTop, Color SidebarBottom,
    Color ContentTop, Color ContentBottom,
    Color Header, Color Card, Color Input, Color Secondary,
    Color Accent, Color AccentText,
    Color Text, Color Muted, Color Highlight,
    Color NavText, Color NavActive);

public static class AppThemes
{
    private static Color C(int r, int g, int b) => Color.FromArgb(r, g, b);

    public static readonly AppTheme Colorful = new("Colorful",
        C(70, 28, 130), C(28, 12, 66),
        C(40, 16, 84), C(110, 40, 150),
        C(78, 34, 140), C(112, 58, 176), C(96, 50, 160), C(150, 70, 200),
        C(255, 110, 170), C(30, 10, 40),
        C(235, 240, 255), C(200, 180, 240), C(255, 214, 102),
        C(230, 200, 255), C(150, 70, 200));

    public static readonly AppTheme Dark = new("Dark",
        C(20, 24, 42), C(14, 17, 30),
        C(14, 17, 30), C(26, 32, 56),
        C(22, 27, 47), C(30, 36, 62), C(32, 38, 64), C(40, 48, 80),
        C(56, 189, 248), C(10, 14, 28),
        C(235, 240, 255), C(140, 150, 180), C(56, 189, 248),
        C(170, 180, 210), C(40, 48, 80));

    public static readonly AppTheme Light = new("Light",
        C(236, 240, 252), C(214, 222, 242),
        C(248, 249, 253), C(226, 232, 246),
        C(255, 255, 255), C(255, 255, 255), C(242, 245, 251), C(228, 234, 248),
        C(37, 99, 235), C(255, 255, 255),
        C(20, 27, 45), C(90, 100, 130), C(37, 99, 235),
        C(60, 70, 100), C(210, 222, 246));

    public static readonly AppTheme Black = new("Black",
        C(12, 12, 12), C(0, 0, 0),
        C(0, 0, 0), C(20, 20, 20),
        C(10, 10, 10), C(26, 26, 26), C(38, 38, 38), C(48, 48, 48),
        C(0, 229, 255), C(0, 0, 0),
        C(240, 240, 240), C(150, 150, 150), C(0, 229, 255),
        C(180, 180, 180), C(44, 44, 44));

    public static AppTheme Get(string? name) => name switch
    {
        "Black" => Black,
        "Dark" => Dark,
        "Light" => Light,
        _ => Colorful
    };
}
