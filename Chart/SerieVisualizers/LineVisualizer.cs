using Chart.Plotters;
using Chart.Series;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Chart.SerieVisualizers {
    public class LineVisualizer : ISerieVisualizer {
        public void Draw(ISerie serie, DrawingVisual visual, Size size, SeriesDataRange dataRange) {
            using (var dc = visual.RenderOpen()) {
                if (!serie.Any()) {
                    return;
                }

                int count = 0;

                foreach (var _ in serie) {
                    if (++count > 1) {
                        break;
                    }
                }

                if (count < 2) {
                    return;
                }

                Point? previous = null;
                var pen = new Pen(serie.Stroke, serie.StrokeThickness);

                foreach (var point in serie) {
                    var next = new Point(
                        this.Proportion(point.XValue, dataRange.MinX, dataRange.MaxX) * size.Width,
                        size.Height - this.Proportion(point.YValue, dataRange.MinY, dataRange.MaxY) * size.Height);

                    if (previous.HasValue) {
                        dc.DrawLine(pen, previous.Value, next);
                    }

                    previous = next;
                }
            }
        }

        private double Proportion(double value, double min, double max) => (value - min) / (max - min);
    }
}
