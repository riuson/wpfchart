using Chart.Series;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Chart.Plotters;

namespace Chart.SerieVisualizers {
    public class LineVisualizer : ISerieVisualizer {
        public GeometryGroup GetGeometryGroup(ISerie serie, Size size, SeriesDataRange dataRange) {
            var result = new GeometryGroup();

            if (!serie.Any()) {
                return result;
            }

            var count = 0;

            foreach (var _ in serie) {
                if (++count > 1) {
                    break;
                }
            }

            if (count < 2) {
                return result;
            }

            var geometryPoints = serie.Select(point =>
                new Point(
                    this.Proportion(point.XValue, dataRange.MinX, dataRange.MaxX) * size.Width,
                    this.Proportion(point.YValue, dataRange.MinY, dataRange.MaxY) * size.Height));

            Point? previousPoint = null;

            foreach (var point in geometryPoints) {
                if (!previousPoint.HasValue) {
                    previousPoint = point;
                    continue;
                }

                var lineSegment = new LineGeometry(previousPoint.Value, point);
                result.Children.Add(lineSegment);

                previousPoint = point;
            }

            return result;
        }

        private double Proportion(double value, double min, double max) => (value - min) / (max - min);
    }
}
