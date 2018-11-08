using Chart.Series;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public class LineVisualizer : ISerieVisualizer {
        public GeometryGroup GetGeometryGroup(ISerie serie, Size size) {
            var result = new GeometryGroup();

            if (serie.Any()) {

                double? minX = null;
                double? maxX = null;
                double? minY = null;
                double? maxY = null;

                foreach (var point in serie) {
                    minX = !minX.HasValue ? point.XValue : Math.Min(minX.Value, point.XValue);
                    maxX = !maxX.HasValue ? point.XValue : Math.Max(maxX.Value, point.XValue);
                    minY = !minY.HasValue ? point.YValue : Math.Min(minY.Value, point.YValue);
                    maxY = !maxY.HasValue ? point.YValue : Math.Max(maxY.Value, point.YValue);
                }

                var geometryPoints = serie.Select(point =>
                    new Point(
                        this.Proportion(point.XValue, minX.Value, maxX.Value) * size.Width,
                        this.Proportion(point.YValue, minY.Value, maxY.Value) * size.Height));

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
            }

            return result;
        }

        private double Proportion(double value, double min, double max) => (value - min) / (max - min);
    }
}
