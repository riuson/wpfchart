using Chart.Points;
using System.Collections.Generic;
using System.Windows.Media;
using Chart.SerieVisualizers;

namespace Chart.Series {
    public interface ISerie : IEnumerable<IPoint> {
        string Title { get; }
        Brush Stroke { get; }
        double StrokeThickness { get; }
        ISerieVisualizer Visualizer { get; }
    }
}
