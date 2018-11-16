using Chart.Plotters;
using Chart.Series;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public class LineVisualizer : ISerieVisualizer {
        public Geometry GetGeometry(ISerie serie, Size size, SeriesDataRange dataRange) {
            var result = new PathGeometry();

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

            var figure = new PathFigure();
            result.Figures.Add(figure);
            var started = false;

            foreach (var point in serie) {
                var x = this.Proportion(point.XValue, dataRange.MinX, dataRange.MaxX) * size.Width;
                var y = this.Proportion(point.YValue, dataRange.MinY, dataRange.MaxY) * size.Height;

                if (!started) {
                    started = true;
                    figure.StartPoint = new Point(x, y);
                    continue;
                }

                var lineSegment = new LineSegment(new Point(x, y), true);
                figure.Segments.Add(lineSegment);
            }

            return result;
        }

        private double Proportion(double value, double min, double max) => (value - min) / (max - min);
    }
}
