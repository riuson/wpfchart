using System.Windows.Media;

namespace Chart {
    public interface IGrid {
        Brush Stroke { get; }
        double StrokeThickness { get; }
        double Interval { get; }
        Marks Marks { get; }
    }
}
