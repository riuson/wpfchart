using System.Windows.Controls;
using System.Windows.Media;
using Chart.Grids;
using Grid = Chart.Grids.Grid;

namespace Chart.Ticks {
    public interface ITicks {
        Dock Side { get; set; }
        IGrid Grid { get; set; }
        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }
        double StrokeLength { get; set; }
    }
}
