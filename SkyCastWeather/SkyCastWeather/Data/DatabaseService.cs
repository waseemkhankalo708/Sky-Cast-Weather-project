using Microsoft.Data.Sqlite;

namespace SkyCastWeather.Data;

public sealed record FavoriteCity(int Id, string Name, string Country, double Latitude, double Longitude);

/// <summary>Stores favorites and settings in a local SQLite file.</summary>
public sealed class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string databasePath)
    {
        var folder = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrEmpty(folder))
            Directory.CreateDirectory(folder);

        _connectionString = new SqliteConnectionStringBuilder { DataSource = databasePath }.ToString();
        CreateTables();
    }

    private SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private void CreateTables()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Favorites (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Country TEXT NOT NULL,
                Latitude REAL NOT NULL,
                Longitude REAL NOT NULL,
                UNIQUE (Name, Country));
            CREATE TABLE IF NOT EXISTS Settings (
                SettingKey TEXT PRIMARY KEY,
                SettingValue TEXT NOT NULL);
            """;
        command.ExecuteNonQuery();
    }

    public List<FavoriteCity> GetFavorites()
    {
        var favorites = new List<FavoriteCity>();
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Country, Latitude, Longitude FROM Favorites ORDER BY Name;";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            favorites.Add(new FavoriteCity(
                reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                reader.GetDouble(3), reader.GetDouble(4)));
        }
        return favorites;
    }

    public void AddFavorite(string name, string country, double latitude, double longitude)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT OR IGNORE INTO Favorites (Name, Country, Latitude, Longitude)
            VALUES ($name, $country, $lat, $lon);
            """;
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$country", country);
        command.Parameters.AddWithValue("$lat", latitude);
        command.Parameters.AddWithValue("$lon", longitude);
        command.ExecuteNonQuery();
    }

    public void RemoveFavorite(int id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Favorites WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    public string? GetSetting(string key)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT SettingValue FROM Settings WHERE SettingKey = $key;";
        command.Parameters.AddWithValue("$key", key);
        return command.ExecuteScalar() as string;
    }

    public void SetSetting(string key, string value)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Settings (SettingKey, SettingValue) VALUES ($key, $value)
            ON CONFLICT(SettingKey) DO UPDATE SET SettingValue = excluded.SettingValue;
            """;
        command.Parameters.AddWithValue("$key", key);
        command.Parameters.AddWithValue("$value", value);
        command.ExecuteNonQuery();
    }
}
