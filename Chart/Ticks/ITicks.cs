using System.Windows.Controls;
using System.Windows.Media;

namespace Chart.Ticks {
    public interface ITicks {
        Dock Side { get; set; }
        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }
        double StrokeLength { get; set; }
    }
}
