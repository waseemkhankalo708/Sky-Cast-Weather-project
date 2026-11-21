namespace SkyCastWeather.Forms;

partial class MapForm
{
    private System.ComponentModel.IContainer? components = null;
    private Microsoft.Web.WebView2.WinForms.WebView2 webView = null!;
    private Panel panelTop = null!;
    private Label lblLayer = null!;
    private ComboBox cmbLayer = null!;
    private Button btnFullscreen = null!;
    private Label lblHint = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        webView = new Microsoft.Web.WebView2.WinForms.WebView2();
        panelTop = new Panel();
        lblLayer = new Label();
        cmbLayer = new ComboBox();
        btnFullscreen = new Button();
        lblHint = new Label();
        ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
        SuspendLayout();
        panelTop.SuspendLayout();

        // webView (Fill)
        webView.Dock = DockStyle.Fill;
        webView.Name = "webView";

        // panelTop
        panelTop.BackColor = Color.FromArgb(78, 34, 140);
        panelTop.Dock = DockStyle.Top;
        panelTop.Height = 48;
        panelTop.Name = "panelTop";
        panelTop.Controls.Add(lblLayer);
        panelTop.Controls.Add(cmbLayer);
        panelTop.Controls.Add(btnFullscreen);
        panelTop.Controls.Add(lblHint);

        lblLayer.Location = new Point(14, 14);
        lblLayer.Size = new Size(48, 20);
        lblLayer.ForeColor = Color.FromArgb(235, 240, 255);
        lblLayer.Name = "lblLayer";
        lblLayer.Text = "Layer:";

        cmbLayer.Location = new Point(64, 11);
        cmbLayer.Size = new Size(230, 28);
        cmbLayer.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLayer.BackColor = Color.FromArgb(96, 50, 160);
        cmbLayer.ForeColor = Color.FromArgb(235, 240, 255);
        cmbLayer.Items.AddRange(new object[] { "Standard map", "Precipitation radar" });
        cmbLayer.SelectedIndex = 0;
        cmbLayer.Name = "cmbLayer";
        cmbLayer.SelectedIndexChanged += CmbLayer_SelectedIndexChanged;

        btnFullscreen.Location = new Point(306, 10);
        btnFullscreen.Size = new Size(120, 30);
        btnFullscreen.FlatStyle = FlatStyle.Flat;
        btnFullscreen.FlatAppearance.BorderSize = 0;
        btnFullscreen.BackColor = Color.FromArgb(150, 70, 200);
        btnFullscreen.ForeColor = Color.FromArgb(235, 240, 255);
        btnFullscreen.Text = "Fullscreen";
        btnFullscreen.Cursor = Cursors.Hand;
        btnFullscreen.Name = "btnFullscreen";
        btnFullscreen.Click += BtnFullscreen_Click;

        lblHint.Location = new Point(440, 14);
        lblHint.Size = new Size(540, 20);
        lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblHint.ForeColor = Color.FromArgb(200, 180, 240);
        lblHint.Name = "lblHint";
        lblHint.Text = "Click the map to select a location. Esc exits fullscreen.";

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(22, 12, 52);
        ClientSize = new Size(1000, 680);
        MinimumSize = new Size(640, 420);
        StartPosition = FormStartPosition.CenterParent;
        Text = "SkyCast Weather Map";
        Name = "MapForm";

        // Docking order: Fill first, then Top.
        Controls.Add(webView);
        Controls.Add(panelTop);

        ((System.ComponentModel.ISupportInitialize)webView).EndInit();
        panelTop.ResumeLayout(false);
        ResumeLayout(false);
    }
}
