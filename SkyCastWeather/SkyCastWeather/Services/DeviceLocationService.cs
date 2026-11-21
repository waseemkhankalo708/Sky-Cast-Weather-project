using Windows.Devices.Geolocation;

namespace SkyCastWeather.Services;

/// <summary>Reads the device position from Windows (GPS or Wi-Fi). Location must be allowed in Windows.</summary>
public static class DeviceLocationService
{
    public static async Task<(double Latitude, double Longitude)> GetPositionAsync(CancellationToken ct = default)
    {
        GeolocationAccessStatus access;
        try
        {
            access = await Geolocator.RequestAccessAsync().AsTask(ct);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new WeatherServiceException("Windows location service is not available on this PC.");
        }

        if (access != GeolocationAccessStatus.Allowed)
            throw new WeatherServiceException(
                "Location access is off. Go to Settings > Privacy & security > Location and allow desktop apps.");

        try
        {
            var geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.High };
            var position = await geolocator
                .GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(15))
                .AsTask(ct);

            var point = position.Coordinate.Point.Position;
            return (point.Latitude, point.Longitude);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new WeatherServiceException("Could not read your device location. Try again or use the search box.");
        }
    }
}
