using System.Windows.Media;

namespace Chart.Grids {
    public interface IGrid {
        Brush Stroke { get; }
        double StrokeThickness { get; }
        double Interval { get; }
    }
}
