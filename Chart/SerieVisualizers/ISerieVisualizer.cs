using System.Windows;
using Chart.Series;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public interface ISerieVisualizer {
        GeometryGroup GetGeometryGroup(ISerie serie, Size size);
    }
}
