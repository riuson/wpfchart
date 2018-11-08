using Chart.Series;
using System.Windows;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public class LineVisualizer : ISerieVisualizer {
        public GeometryGroup GetGeometryGroup(ISerie serie, Size size) {
            var result = new GeometryGroup();
            return result;
        }
    }
}
