using Chart.Plotters;
using Chart.Series;
using System.Windows;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public interface ISerieVisualizer {
        void Draw(ISerie serie, DrawingVisual visual, Size size, SeriesDataRange dataRange);
    }
}
