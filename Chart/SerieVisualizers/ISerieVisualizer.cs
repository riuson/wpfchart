using Chart.Plotters;
using Chart.Series;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public interface ISerieVisualizer {
        Geometry GetGeometry(ISerie serie, Size size, SeriesDataRange dataRange);
    }
}
