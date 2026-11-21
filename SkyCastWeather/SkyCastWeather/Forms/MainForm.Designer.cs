using SkyCastWeather.Controls;

namespace SkyCastWeather.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    // Sidebar
    private Panel panelSidebar = null!;
    private Label lblLogo = null!;
    private Button btnNavDashboard = null!;
    private Button btnNavMap = null!;
    private Button btnNavForecast = null!;
    private Button btnNavFavorites = null!;
    private Button btnNavSettings = null!;
    private Button btnNavAbout = null!;

    // Header
    private Panel panelHeader = null!;
    private TextBox txtSearch = null!;
    private Button btnSearch = null!;
    private Button btnLocation = null!;
    private Button btnRefresh = null!;
    private ComboBox cmbUnits = null!;
    private ProgressBar progressLoading = null!;
    private Label lblStatus = null!;
    private Button btnFavorite = null!;

    // Content
    private Panel panelContent = null!;
    private Panel panelCurrent = null!;
    private Label lblCity = null!;
    private Label lblCountry = null!;
    private PictureBox picIcon = null!;
    private Label lblTemperature = null!;
    private Label lblCondition = null!;
    private Label lblDescription = null!;
    private Label lblFeelsLike = null!;
    private Label lblMinMax = null!;
    private Label lblUpdated = null!;
    private MetricCard mcHumidity = null!;
    private MetricCard mcWind = null!;
    private MetricCard mcWindDirection = null!;
    private MetricCard mcPressure = null!;
    private MetricCard mcVisibility = null!;
    private MetricCard mcCloudiness = null!;
    private MetricCard mcUvIndex = null!;
    private MetricCard mcSunrise = null!;
    private MetricCard mcSunset = null!;
    private Label lblHourlyTitle = null!;
    private FlowLayoutPanel flowHourly = null!;
    private Label lblDailyTitle = null!;
    private FlowLayoutPanel flowDaily = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        panelSidebar = new Panel();
        lblLogo = new Label();
        btnNavDashboard = new Button();
        btnNavMap = new Button();
        btnNavForecast = new Button();
        btnNavFavorites = new Button();
        btnNavSettings = new Button();
        btnNavAbout = new Button();
        panelHeader = new Panel();
        txtSearch = new TextBox();
        btnSearch = new Button();
        btnLocation = new Button();
        btnRefresh = new Button();
        cmbUnits = new ComboBox();
        progressLoading = new ProgressBar();
        lblStatus = new Label();
        btnFavorite = new Button();
        panelContent = new Panel();
        panelCurrent = new Panel();
        lblCity = new Label();
        lblCountry = new Label();
        picIcon = new PictureBox();
        lblTemperature = new Label();
        lblCondition = new Label();
        lblDescription = new Label();
        lblFeelsLike = new Label();
        lblMinMax = new Label();
        lblUpdated = new Label();
        mcHumidity = new MetricCard();
        mcWind = new MetricCard();
        mcWindDirection = new MetricCard();
        mcPressure = new MetricCard();
        mcVisibility = new MetricCard();
        mcCloudiness = new MetricCard();
        mcUvIndex = new MetricCard();
        mcSunrise = new MetricCard();
        mcSunset = new MetricCard();
        lblHourlyTitle = new Label();
        flowHourly = new FlowLayoutPanel();
        lblDailyTitle = new Label();
        flowDaily = new FlowLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
        SuspendLayout();

        // ---------- Sidebar ----------
        panelSidebar.BackColor = Color.FromArgb(58, 24, 110);
        panelSidebar.Dock = DockStyle.Left;
        panelSidebar.Size = new Size(210, 800);
        panelSidebar.Name = "panelSidebar";
        panelSidebar.Controls.Add(lblLogo);
        panelSidebar.Controls.Add(btnNavDashboard);
        panelSidebar.Controls.Add(btnNavMap);
        panelSidebar.Controls.Add(btnNavForecast);
        panelSidebar.Controls.Add(btnNavFavorites);
        panelSidebar.Controls.Add(btnNavSettings);
        panelSidebar.Controls.Add(btnNavAbout);

        lblLogo.Location = new Point(20, 22);
        lblLogo.Size = new Size(180, 40);
        lblLogo.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
        lblLogo.ForeColor = Color.FromArgb(255, 214, 102);
        lblLogo.Name = "lblLogo";
        lblLogo.Text = "SkyCast Weather";

        StyleNavButton(btnNavDashboard, "Dashboard", 90, true);
        StyleNavButton(btnNavMap, "Weather Map", 138, false);
        StyleNavButton(btnNavForecast, "Forecast", 186, false);
        StyleNavButton(btnNavFavorites, "Favorite Cities", 234, false);
        StyleNavButton(btnNavSettings, "Settings", 282, false);
        StyleNavButton(btnNavAbout, "About", 330, false);

        // ---------- Header ----------
        panelHeader.BackColor = Color.FromArgb(78, 34, 140);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Height = 80;
        panelHeader.Name = "panelHeader";
        panelHeader.Controls.Add(txtSearch);
        panelHeader.Controls.Add(btnSearch);
        panelHeader.Controls.Add(btnLocation);
        panelHeader.Controls.Add(btnRefresh);
        panelHeader.Controls.Add(cmbUnits);
        panelHeader.Controls.Add(progressLoading);
        panelHeader.Controls.Add(lblStatus);
        panelHeader.Controls.Add(btnFavorite);

        txtSearch.Location = new Point(20, 16);
        txtSearch.Size = new Size(300, 30);
        txtSearch.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        txtSearch.BackColor = Color.FromArgb(96, 50, 160);
        txtSearch.ForeColor = Color.FromArgb(235, 240, 255);
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search city, e.g. Karachi, PK";
        txtSearch.KeyDown += TxtSearch_KeyDown;

        btnSearch.Location = new Point(330, 14);
        btnSearch.Size = new Size(90, 34);
        StyleHeaderButton(btnSearch, "Search", true);
        btnSearch.Click += BtnSearch_Click;

        btnLocation.Location = new Point(428, 14);
        btnLocation.Size = new Size(120, 34);
        StyleHeaderButton(btnLocation, "My location", false);
        btnLocation.Click += BtnLocation_Click;

        btnRefresh.Location = new Point(556, 14);
        btnRefresh.Size = new Size(100, 34);
        StyleHeaderButton(btnRefresh, "Refresh", false);
        btnRefresh.Click += BtnRefresh_Click;

        cmbUnits.Location = new Point(664, 14);
        cmbUnits.Size = new Size(150, 34);
        cmbUnits.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnits.BackColor = Color.FromArgb(96, 50, 160);
        cmbUnits.ForeColor = Color.FromArgb(235, 240, 255);
        cmbUnits.Items.AddRange(new object[] { "Celsius (°C)", "Fahrenheit (°F)" });
        cmbUnits.SelectedIndex = 0;
        cmbUnits.Name = "cmbUnits";
        cmbUnits.SelectedIndexChanged += CmbUnits_SelectedIndexChanged;

        progressLoading.Location = new Point(830, 24);
        progressLoading.Size = new Size(160, 14);
        progressLoading.Style = ProgressBarStyle.Marquee;
        progressLoading.MarqueeAnimationSpeed = 30;
        progressLoading.Visible = false;
        progressLoading.Name = "progressLoading";

        btnFavorite.Location = new Point(1000, 14);
        btnFavorite.Size = new Size(110, 34);
        btnFavorite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        StyleHeaderButton(btnFavorite, "Save city", false);
        btnFavorite.Click += BtnFavorite_Click;

        lblStatus.Location = new Point(20, 54);
        lblStatus.Size = new Size(980, 20);
        lblStatus.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
        lblStatus.ForeColor = Color.FromArgb(200, 180, 240);
        lblStatus.Name = "lblStatus";
        lblStatus.Text = "Loading...";

        // ---------- Content ----------
        panelContent.AutoScroll = true;
        panelContent.BackColor = Color.FromArgb(22, 12, 52);
        panelContent.Dock = DockStyle.Fill;
        panelContent.Name = "panelContent";
        panelContent.Controls.Add(panelCurrent);
        panelContent.Controls.Add(mcHumidity);
        panelContent.Controls.Add(mcWind);
        panelContent.Controls.Add(mcWindDirection);
        panelContent.Controls.Add(mcPressure);
        panelContent.Controls.Add(mcVisibility);
        panelContent.Controls.Add(mcCloudiness);
        panelContent.Controls.Add(mcUvIndex);
        panelContent.Controls.Add(mcSunrise);
        panelContent.Controls.Add(mcSunset);
        panelContent.Controls.Add(lblHourlyTitle);
        panelContent.Controls.Add(flowHourly);
        panelContent.Controls.Add(lblDailyTitle);
        panelContent.Controls.Add(flowDaily);

        // Main weather card
        panelCurrent.BackColor = Color.FromArgb(112, 58, 176);
        panelCurrent.Location = new Point(20, 20);
        panelCurrent.Size = new Size(460, 300);
        panelCurrent.Name = "panelCurrent";
        panelCurrent.Controls.Add(lblCity);
        panelCurrent.Controls.Add(lblCountry);
        panelCurrent.Controls.Add(picIcon);
        panelCurrent.Controls.Add(lblTemperature);
        panelCurrent.Controls.Add(lblCondition);
        panelCurrent.Controls.Add(lblDescription);
        panelCurrent.Controls.Add(lblFeelsLike);
        panelCurrent.Controls.Add(lblMinMax);
        panelCurrent.Controls.Add(lblUpdated);

        lblCity.Location = new Point(20, 16);
        lblCity.Size = new Size(420, 38);
        lblCity.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point);
        lblCity.ForeColor = Color.FromArgb(235, 240, 255);
        lblCity.Name = "lblCity";
        lblCity.Text = "City";

        lblCountry.Location = new Point(20, 54);
        lblCountry.Size = new Size(420, 20);
        lblCountry.ForeColor = Color.FromArgb(200, 180, 240);
        lblCountry.Name = "lblCountry";
        lblCountry.Text = "Country";

        picIcon.Location = new Point(20, 86);
        picIcon.Size = new Size(100, 100);
        picIcon.SizeMode = PictureBoxSizeMode.Zoom;
        picIcon.BackColor = Color.Transparent;
        picIcon.Name = "picIcon";
        picIcon.TabStop = false;

        lblTemperature.Location = new Point(130, 92);
        lblTemperature.Size = new Size(300, 90);
        lblTemperature.Font = new Font("Segoe UI Light", 52F, FontStyle.Regular, GraphicsUnit.Point);
        lblTemperature.ForeColor = Color.White;
        lblTemperature.Name = "lblTemperature";
        lblTemperature.Text = "--";

        lblCondition.Location = new Point(20, 194);
        lblCondition.Size = new Size(420, 26);
        lblCondition.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold, GraphicsUnit.Point);
        lblCondition.ForeColor = Color.FromArgb(255, 214, 102);
        lblCondition.Name = "lblCondition";
        lblCondition.Text = "Condition";

        lblDescription.Location = new Point(20, 220);
        lblDescription.Size = new Size(420, 22);
        lblDescription.ForeColor = Color.FromArgb(240, 230, 255);
        lblDescription.Name = "lblDescription";
        lblDescription.Text = "Description";

        lblFeelsLike.Location = new Point(20, 250);
        lblFeelsLike.Size = new Size(200, 20);
        lblFeelsLike.ForeColor = Color.FromArgb(220, 200, 255);
        lblFeelsLike.Name = "lblFeelsLike";
        lblFeelsLike.Text = "Feels like --";

        lblMinMax.Location = new Point(220, 250);
        lblMinMax.Size = new Size(230, 20);
        lblMinMax.ForeColor = Color.FromArgb(220, 200, 255);
        lblMinMax.Name = "lblMinMax";
        lblMinMax.Text = "Min -- Max --";

        lblUpdated.Location = new Point(20, 272);
        lblUpdated.Size = new Size(420, 18);
        lblUpdated.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
        lblUpdated.ForeColor = Color.FromArgb(180, 160, 220);
        lblUpdated.Name = "lblUpdated";
        lblUpdated.Text = "Last updated --";

        // Detail metric cards (3 columns x 3 rows)
        ConfigureMetric(mcHumidity, "Humidity", 500, 20);
        ConfigureMetric(mcWind, "Wind speed", 672, 20);
        ConfigureMetric(mcWindDirection, "Wind direction", 844, 20);
        ConfigureMetric(mcPressure, "Pressure", 500, 110);
        ConfigureMetric(mcVisibility, "Visibility", 672, 110);
        ConfigureMetric(mcCloudiness, "Cloudiness", 844, 110);
        ConfigureMetric(mcUvIndex, "UV index", 500, 200);
        ConfigureMetric(mcSunrise, "Sunrise", 672, 200);
        ConfigureMetric(mcSunset, "Sunset", 844, 200);

        // Hourly forecast
        lblHourlyTitle.Location = new Point(20, 340);
        lblHourlyTitle.Size = new Size(400, 26);
        lblHourlyTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
        lblHourlyTitle.ForeColor = Color.FromArgb(235, 240, 255);
        lblHourlyTitle.Name = "lblHourlyTitle";
        lblHourlyTitle.Text = "Next 24 hours";

        flowHourly.Location = new Point(20, 370);
        flowHourly.Size = new Size(1000, 150);
        flowHourly.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        flowHourly.AutoScroll = true;
        flowHourly.FlowDirection = FlowDirection.LeftToRight;
        flowHourly.WrapContents = false;
        flowHourly.Name = "flowHourly";

        // Daily forecast
        lblDailyTitle.Location = new Point(20, 540);
        lblDailyTitle.Size = new Size(400, 26);
        lblDailyTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
        lblDailyTitle.ForeColor = Color.FromArgb(235, 240, 255);
        lblDailyTitle.Name = "lblDailyTitle";
        lblDailyTitle.Text = "5-day forecast";

        flowDaily.Location = new Point(20, 570);
        flowDaily.Size = new Size(1000, 150);
        flowDaily.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        flowDaily.AutoScroll = true;
        flowDaily.FlowDirection = FlowDirection.LeftToRight;
        flowDaily.WrapContents = false;
        flowDaily.Name = "flowDaily";

        // ---------- Form ----------
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(22, 12, 52);
        ForeColor = Color.FromArgb(235, 240, 255);
        ClientSize = new Size(1280, 800);
        MinimumSize = new Size(1000, 680);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        Text = "SkyCast Weather";
        Name = "MainForm";

        // Docking order: the control added last docks first.
        // Content (Fill) is added first, then header (Top), then sidebar (Left).
        Controls.Add(panelContent);
        Controls.Add(panelHeader);
        Controls.Add(panelSidebar);

        Load += MainForm_Load;
        FormClosing += MainForm_FormClosing;

        ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
        panelSidebar.ResumeLayout(false);
        panelHeader.ResumeLayout(false);
        panelContent.ResumeLayout(false);
        ResumeLayout(false);
    }

    private void StyleNavButton(Button button, string text, int top, bool isActive)
    {
        button.Location = new Point(12, top);
        button.Size = new Size(186, 42);
        button.Text = text;
        button.TextAlign = ContentAlignment.MiddleLeft;
        button.Padding = new Padding(14, 0, 0, 0);
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(150, 70, 200);
        button.BackColor = isActive ? Color.FromArgb(150, 70, 200) : Color.Transparent;
        button.ForeColor = isActive ? Color.White : Color.FromArgb(230, 200, 255);
        button.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        button.Cursor = Cursors.Hand;
        button.Name = "btnNav" + text.Replace(" ", "");
        button.Click += NavButton_Click;
    }

    private static void StyleHeaderButton(Button button, string text, bool isPrimary)
    {
        button.Text = text;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = isPrimary ? Color.FromArgb(255, 110, 170) : Color.FromArgb(150, 70, 200);
        button.ForeColor = isPrimary ? Color.FromArgb(30, 10, 40) : Color.FromArgb(235, 240, 255);
        button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
        button.Cursor = Cursors.Hand;
        button.Name = "btn" + text.Replace(" ", "");
    }

    private void ConfigureMetric(MetricCard card, string title, int left, int top)
    {
        card.Location = new Point(left, top);
        card.Size = new Size(162, 80);
        card.Title = title;
        card.Value = "--";
        card.Name = "mc" + title.Replace(" ", "");
    }
}
