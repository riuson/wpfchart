using Chart.Points;
using System.Collections.Generic;
using System.Drawing;

namespace Chart.Series {
    public interface ISerie : IEnumerable<IPoint> {
        string Title { get; }
        Brush LineBrush { get; }
        double LineWidth { get; }
    }
}
