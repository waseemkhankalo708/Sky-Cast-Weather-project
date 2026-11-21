namespace SkyCastWeather.Controls;

partial class MetricCard
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblTitle = null!;
    private Label lblValue = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblTitle = new Label();
        lblValue = new Label();
        SuspendLayout();

        // lblValue is added first so the Top-docked title sits above it.
        lblValue.Dock = DockStyle.Fill;
        lblValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
        lblValue.ForeColor = Color.FromArgb(235, 240, 255);
        lblValue.Padding = new Padding(12, 0, 0, 8);
        lblValue.TextAlign = ContentAlignment.MiddleLeft;
        lblValue.Name = "lblValue";
        lblValue.Text = "--";

        lblTitle.Dock = DockStyle.Top;
        lblTitle.Height = 28;
        lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblTitle.ForeColor = Color.FromArgb(230, 210, 255);
        lblTitle.Padding = new Padding(12, 8, 0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "Title";

        Controls.Add(lblValue);
        Controls.Add(lblTitle);
        BackColor = Color.FromArgb(112, 58, 176);
        Name = "MetricCard";
        Size = new Size(162, 80);
        ResumeLayout(false);
    }
}
