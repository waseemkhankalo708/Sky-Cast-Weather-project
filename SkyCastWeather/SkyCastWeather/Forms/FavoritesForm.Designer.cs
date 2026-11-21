namespace SkyCastWeather.Forms;

partial class FavoritesForm
{
    private System.ComponentModel.IContainer? components = null;
    private ListView lvFavorites = null!;
    private ColumnHeader colCity = null!;
    private ColumnHeader colCountry = null!;
    private ColumnHeader colWeather = null!;
    private Panel panelBottom = null!;
    private Label lblEmpty = null!;
    private Button btnOpen = null!;
    private Button btnRemove = null!;
    private Button btnClose = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lvFavorites = new ListView();
        colCity = new ColumnHeader();
        colCountry = new ColumnHeader();
        colWeather = new ColumnHeader();
        panelBottom = new Panel();
        lblEmpty = new Label();
        btnOpen = new Button();
        btnRemove = new Button();
        btnClose = new Button();
        SuspendLayout();
        panelBottom.SuspendLayout();

        colCity.Text = "City";
        colCity.Width = 200;
        colCountry.Text = "Country";
        colCountry.Width = 120;
        colWeather.Text = "Current weather";
        colWeather.Width = 240;

        lvFavorites.Columns.AddRange(new ColumnHeader[] { colCity, colCountry, colWeather });
        lvFavorites.Dock = DockStyle.Fill;
        lvFavorites.View = View.Details;
        lvFavorites.FullRowSelect = true;
        lvFavorites.MultiSelect = false;
        lvFavorites.HideSelection = false;
        lvFavorites.BorderStyle = BorderStyle.FixedSingle;
        lvFavorites.BackColor = Color.FromArgb(78, 34, 140);
        lvFavorites.ForeColor = Color.FromArgb(235, 240, 255);
        lvFavorites.Name = "lvFavorites";
        lvFavorites.ItemActivate += LvFavorites_ItemActivate;

        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Height = 58;
        panelBottom.BackColor = Color.FromArgb(78, 34, 140);
        panelBottom.Name = "panelBottom";
        panelBottom.Controls.Add(lblEmpty);
        panelBottom.Controls.Add(btnOpen);
        panelBottom.Controls.Add(btnRemove);
        panelBottom.Controls.Add(btnClose);

        lblEmpty.Location = new Point(14, 18);
        lblEmpty.Size = new Size(310, 22);
        lblEmpty.ForeColor = Color.FromArgb(200, 180, 240);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Text = "No favorite cities yet. Use \"Save city\" on the dashboard.";
        lblEmpty.Visible = false;

        btnOpen.Location = new Point(340, 12);
        btnOpen.Size = new Size(90, 34);
        StyleButton(btnOpen, "Open", true);
        btnOpen.Click += BtnOpen_Click;

        btnRemove.Location = new Point(440, 12);
        btnRemove.Size = new Size(100, 34);
        StyleButton(btnRemove, "Remove", false);
        btnRemove.Click += BtnRemove_Click;

        btnClose.Location = new Point(548, 12);
        btnClose.Size = new Size(60, 34);
        StyleButton(btnClose, "Close", false);
        btnClose.Click += BtnClose_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(22, 12, 52);
        ForeColor = Color.FromArgb(235, 240, 255);
        ClientSize = new Size(620, 420);
        MinimumSize = new Size(480, 300);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Favorite Cities";
        Name = "FavoritesForm";

        Controls.Add(lvFavorites);
        Controls.Add(panelBottom);
        panelBottom.ResumeLayout(false);
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
