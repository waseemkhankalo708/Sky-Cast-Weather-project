# SkyCast Weather 2.0

A Windows Forms weather dashboard built with C# and .NET 8.

## Requirements

- Visual Studio 2022 with the ".NET desktop development" workload
- .NET 8 SDK
- Microsoft Edge WebView2 Runtime (already installed on most Windows 10/11 PCs; otherwise download it from Microsoft)
- Internet connection. No API key is needed.

## Open and run

1. Open `SkyCastWeather.sln` in Visual Studio 2022.
2. Restore NuGet packages if prompted (this downloads WebView2 and Microsoft.Data.Sqlite).
3. Press **F5**.

## Features

- **Dashboard:** current weather, 9 metric cards, 24-hour and 7-day forecasts.
- **Search and My location:** city search by name; approximate IP-based location.
- **Weather Map:** OpenStreetMap base map with a precipitation radar layer (RainViewer). Click anywhere to load that point's weather. Favorite cities appear as markers. Fullscreen button (Esc exits).
- **Favorite Cities:** click "Save city" on the dashboard to save the current location. The Favorites window shows each city's current weather and can open or remove them.
- **Settings:** default city, °C/°F, auto-refresh on/off, and the refresh interval (5 to 180 minutes).

## Edit the design

Double-click the `.Designer.cs` file for any form, or open the Designer from its `.cs` file. Each
form has its own designer. The map's HTML, CSS, and JavaScript are in `Map/` and can be edited directly.

## Data sources

| Feature | Service | Key needed |
|---|---|---|
| Weather, forecasts, UV, visibility | Open-Meteo (`api.open-meteo.com`) | No |
| City search | Open-Meteo Geocoding | No |
| "My location" | ipapi.co (approximate) | No |
| Map tiles | OpenStreetMap | No |
| Radar layer | RainViewer public API | No |

Open-Meteo's free tier is for non-commercial use. A commercial app needs their paid plan.
RainViewer and OpenStreetMap also have attribution and usage rules; the map shows the required attribution.

## Files

- Database: `%APPDATA%\SkyCastWeather\skycast.db` (favorites and settings)
- Map page: `Map/index.html`, `Map/map.js`, `Map/map.css` (copied to the output folder on build)

## Build an EXE

```
dotnet publish SkyCastWeather\SkyCastWeather.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The EXE appears in `SkyCastWeather\bin\Release\net8.0-windows\win-x64\publish\`.
Keep the `Map` folder next to the EXE, or the map will not load. With single-file publishing, the
`Map` folder is still copied to the publish output.

## Not included yet

- Temperature, wind, and cloud map layers. Their tile services (for example OpenWeatherMap) need an API key.
- Theme switching and wind-unit settings.
- Weather alerts and rain/storm animations.
