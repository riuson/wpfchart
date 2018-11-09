using Chart.Series;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart.Plotters {
    public class Plotter : Panel {
        private Path _visualizedSeries;

        public Plotter() {
            this.Series = null;
            this._visualizedSeries = new Path() {
                Stroke = Brushes.Blue,
                StrokeThickness = 3
            };
            this.Children.Add(this._visualizedSeries);
        }

        public IEnumerable<ISerie> Series { get; set; }

        protected override Size MeasureOverride(Size availableSize) {
            var geometryGroups = this.Series?.Select(serie => serie.Visualizer.GetGeometryGroup(serie, availableSize)).ToArray();

            var combinedGroup = new GeometryGroup();

            foreach (var group in geometryGroups) {
                combinedGroup.Children.Add(group);
            }

            this._visualizedSeries.Data = combinedGroup;

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement item in this.Children) {
                item.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }
    }
}
