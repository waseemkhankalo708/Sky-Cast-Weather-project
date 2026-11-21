namespace SkyCastWeather.Forms;

partial class SettingsForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblDefaultCity = null!;
    private TextBox txtDefaultCity = null!;
    private Label lblUnits = null!;
    private ComboBox cmbSettingsUnits = null!;
    private CheckBox chkAutoRefresh = null!;
    private Label lblInterval = null!;
    private NumericUpDown nudRefreshMinutes = null!;
    private Label lblNote = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;
    private Label lblTheme = null!;
    private ComboBox cmbTheme = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblDefaultCity = new Label();
        txtDefaultCity = new TextBox();
        lblUnits = new Label();
        cmbSettingsUnits = new ComboBox();
        chkAutoRefresh = new CheckBox();
        lblInterval = new Label();
        nudRefreshMinutes = new NumericUpDown();
        lblNote = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        lblTheme = new Label();
        cmbTheme = new ComboBox();
        ((System.ComponentModel.ISupportInitialize)nudRefreshMinutes).BeginInit();
        SuspendLayout();

        lblDefaultCity.Location = new Point(24, 26);
        lblDefaultCity.Size = new Size(150, 20);
        lblDefaultCity.Name = "lblDefaultCity";
        lblDefaultCity.Text = "Default city";

        txtDefaultCity.Location = new Point(200, 22);
        txtDefaultCity.Size = new Size(230, 27);
        txtDefaultCity.BackColor = Color.FromArgb(96, 50, 160);
        txtDefaultCity.ForeColor = Color.FromArgb(235, 240, 255);
        txtDefaultCity.Name = "txtDefaultCity";

        lblUnits.Location = new Point(24, 66);
        lblUnits.Size = new Size(150, 20);
        lblUnits.Name = "lblUnits";
        lblUnits.Text = "Temperature units";

        cmbSettingsUnits.Location = new Point(200, 62);
        cmbSettingsUnits.Size = new Size(230, 28);
        cmbSettingsUnits.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSettingsUnits.BackColor = Color.FromArgb(96, 50, 160);
        cmbSettingsUnits.ForeColor = Color.FromArgb(235, 240, 255);
        cmbSettingsUnits.Items.AddRange(new object[] { "Celsius (°C)", "Fahrenheit (°F)" });
        cmbSettingsUnits.Name = "cmbSettingsUnits";

        chkAutoRefresh.Location = new Point(24, 108);
        chkAutoRefresh.Size = new Size(300, 24);
        chkAutoRefresh.ForeColor = Color.FromArgb(235, 240, 255);
        chkAutoRefresh.Text = "Auto-refresh weather";
        chkAutoRefresh.Name = "chkAutoRefresh";
        chkAutoRefresh.CheckedChanged += ChkAutoRefresh_CheckedChanged;

        lblInterval.Location = new Point(24, 148);
        lblInterval.Size = new Size(170, 20);
        lblInterval.Name = "lblInterval";
        lblInterval.Text = "Refresh every (minutes)";

        nudRefreshMinutes.Location = new Point(200, 144);
        nudRefreshMinutes.Size = new Size(110, 27);
        nudRefreshMinutes.Minimum = 5;
        nudRefreshMinutes.Maximum = 180;
        nudRefreshMinutes.Value = 10;
        nudRefreshMinutes.BackColor = Color.FromArgb(96, 50, 160);
        nudRefreshMinutes.ForeColor = Color.FromArgb(235, 240, 255);
        nudRefreshMinutes.Name = "nudRefreshMinutes";

        lblNote.Location = new Point(24, 192);
        lblNote.Size = new Size(410, 30);
        lblNote.ForeColor = Color.FromArgb(200, 180, 240);
        lblNote.Name = "lblNote";
        lblNote.Text = "Weather data comes from Open-Meteo. No API key is needed.";

        lblTheme.Location = new Point(24, 230);
        lblTheme.Size = new Size(150, 20);
        lblTheme.Name = "lblTheme";
        lblTheme.Text = "Theme";

        cmbTheme.Location = new Point(200, 226);
        cmbTheme.Size = new Size(230, 28);
        cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTheme.BackColor = Color.FromArgb(32, 38, 64);
        cmbTheme.ForeColor = Color.FromArgb(235, 240, 255);
        cmbTheme.Items.AddRange(new object[] { "Colorful", "Dark", "Light", "Black" });
        cmbTheme.Name = "cmbTheme";

        btnSave.Location = new Point(250, 270);
        btnSave.Size = new Size(90, 34);
        StyleButton(btnSave, "Save", true);
        btnSave.Click += BtnSave_Click;

        btnCancel.Location = new Point(350, 270);
        btnCancel.Size = new Size(90, 34);
        StyleButton(btnCancel, "Cancel", false);
        btnCancel.Click += BtnCancel_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AcceptButton = btnSave;
        CancelButton = btnCancel;
        BackColor = Color.FromArgb(78, 34, 140);
        ForeColor = Color.FromArgb(235, 240, 255);
        ClientSize = new Size(466, 326);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Settings";
        Name = "SettingsForm";

        Controls.Add(lblDefaultCity);
        Controls.Add(txtDefaultCity);
        Controls.Add(lblUnits);
        Controls.Add(cmbSettingsUnits);
        Controls.Add(chkAutoRefresh);
        Controls.Add(lblInterval);
        Controls.Add(nudRefreshMinutes);
        Controls.Add(lblNote);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
        Controls.Add(lblTheme);
        Controls.Add(cmbTheme);

        ((System.ComponentModel.ISupportInitialize)nudRefreshMinutes).EndInit();
        ResumeLayout(false);
    }

    private static void StyleButton(Button button, string text, bool isPrimary)
    {
        button.Text = text;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = isPrimary ? Color.FromArgb(255, 110, 170) : Color.FromArgb(150, 70, 200);
        button.ForeColor = isPrimary ? Color.FromArgb(30, 10, 40) : Color.FromArgb(235, 240, 255);
        button.Cursor = Cursors.Hand;
        button.Name = "btn" + text;
    }
}
