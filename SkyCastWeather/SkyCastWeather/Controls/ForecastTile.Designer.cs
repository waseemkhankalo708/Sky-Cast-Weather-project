namespace SkyCastWeather.Controls;

partial class ForecastTile
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblHeader = null!;
    private PictureBox picIcon = null!;
    private Label lblTemperature = null!;
    private Label lblDetail = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            picIcon.Image?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblHeader = new Label();
        picIcon = new PictureBox();
        lblTemperature = new Label();
        lblDetail = new Label();
        ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
        SuspendLayout();

        lblHeader.Location = new Point(0, 10);
        lblHeader.Size = new Size(120, 20);
        lblHeader.TextAlign = ContentAlignment.MiddleCenter;
        lblHeader.ForeColor = Color.FromArgb(220, 200, 255);
        lblHeader.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblHeader.Name = "lblHeader";
        lblHeader.Text = "Time";

        picIcon.Location = new Point(32, 34);
        picIcon.Size = new Size(56, 56);
        picIcon.SizeMode = PictureBoxSizeMode.Zoom;
        picIcon.BackColor = Color.Transparent;
        picIcon.Name = "picIcon";
        picIcon.TabStop = false;

        lblTemperature.Location = new Point(0, 94);
        lblTemperature.Size = new Size(120, 24);
        lblTemperature.TextAlign = ContentAlignment.MiddleCenter;
        lblTemperature.ForeColor = Color.FromArgb(235, 240, 255);
        lblTemperature.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
        lblTemperature.Name = "lblTemperature";
        lblTemperature.Text = "--";

        lblDetail.Location = new Point(0, 120);
        lblDetail.Size = new Size(120, 20);
        lblDetail.TextAlign = ContentAlignment.MiddleCenter;
        lblDetail.ForeColor = Color.FromArgb(255, 214, 102);
        lblDetail.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
        lblDetail.Name = "lblDetail";
        lblDetail.Text = "";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(112, 58, 176);
        Name = "ForecastTile";
        Size = new Size(120, 150);
        Margin = new Padding(0, 0, 10, 0);
        Controls.Add(lblDetail);
        Controls.Add(lblTemperature);
        Controls.Add(picIcon);
        Controls.Add(lblHeader);
        ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
        ResumeLayout(false);
    }
}
