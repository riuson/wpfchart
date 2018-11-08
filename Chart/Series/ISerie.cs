using Chart.Points;
using System.Collections.Generic;
using System.Windows.Media;
using Chart.SerieVisualizers;

namespace Chart.Series {
    public interface ISerie : IEnumerable<IPoint> {
        string Title { get; }
        Brush LineBrush { get; }
        double LineWidth { get; }
        ISerieVisualizer Visualizer { get; }
    }
}
