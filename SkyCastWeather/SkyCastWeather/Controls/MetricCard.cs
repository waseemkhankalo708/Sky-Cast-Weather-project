using System.ComponentModel;

namespace SkyCastWeather.Controls;

/// <summary>A card showing one weather metric (title + value).</summary>
public partial class MetricCard : UserControl
{
    public MetricCard()
    {
        InitializeComponent();
    }

    [Category("SkyCast")]
    [Description("The small caption shown at the top of the card.")]
    public string Title
    {
        get => lblTitle.Text;
        set => lblTitle.Text = value;
    }

    [Category("SkyCast")]
    [Description("The main value shown in the card.")]
    public string Value
    {
        get => lblValue.Text;
        set => lblValue.Text = value;
    }

    public void ApplyTheme(Color cardColor, Color titleColor, Color valueColor)
    {
        BackColor = cardColor;
        lblTitle.ForeColor = titleColor;
        lblValue.ForeColor = valueColor;
    }
}
