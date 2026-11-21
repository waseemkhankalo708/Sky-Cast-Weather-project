using System.Globalization;
using SkyCastWeather.Data;

namespace SkyCastWeather.Forms;

public partial class SettingsForm : Form
{
    private readonly DatabaseService _database;

    /// <summary>Raised after settings are saved, so the dashboard can apply them.</summary>
    public event EventHandler? SettingsSaved;

    public SettingsForm(DatabaseService database)
    {
        _database = database;
        InitializeComponent();
        LoadValues();
    }

    private void LoadValues()
    {
        txtDefaultCity.Text = _database.GetSetting("DefaultCity") ?? "London";
        cmbSettingsUnits.SelectedIndex = _database.GetSetting("Units") == "imperial" ? 1 : 0;
        chkAutoRefresh.Checked = _database.GetSetting("AutoRefreshEnabled") != "false";
        cmbTheme.SelectedIndex = Math.Max(0, cmbTheme.Items.IndexOf(_database.GetSetting("Theme") ?? "Colorful"));

        var minutes = int.TryParse(_database.GetSetting("AutoRefreshMinutes"), out var stored) ? stored : 10;
        nudRefreshMinutes.Value = Math.Clamp(minutes, 5, 180);
        nudRefreshMinutes.Enabled = chkAutoRefresh.Checked;
    }

    private void ChkAutoRefresh_CheckedChanged(object? sender, EventArgs e)
    {
        nudRefreshMinutes.Enabled = chkAutoRefresh.Checked;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var city = txtDefaultCity.Text.Trim();
        if (city.Length == 0)
        {
            MessageBox.Show(this, "Please enter a default city.", "SkyCast Weather",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _database.SetSetting("DefaultCity", city);
        _database.SetSetting("Units", cmbSettingsUnits.SelectedIndex == 1 ? "imperial" : "metric");
        _database.SetSetting("Theme", cmbTheme.SelectedItem?.ToString() ?? "Colorful");
        _database.SetSetting("AutoRefreshEnabled", chkAutoRefresh.Checked ? "true" : "false");
        _database.SetSetting("AutoRefreshMinutes", ((int)nudRefreshMinutes.Value).ToString(CultureInfo.InvariantCulture));

        SettingsSaved?.Invoke(this, EventArgs.Empty);
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e) => Close();
}
