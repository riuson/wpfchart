using Chart.Series;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart.Plotters {
    public class Plotter : Panel {
        private Path mVisualizedSeries;
        private IEnumerable<ISerie> mSeries;

        public Plotter() {
            this.mSeries = new ISerie[] { };
            this.mVisualizedSeries = new Path() {
                Stroke = Brushes.Blue,
                StrokeThickness = 3
            };
            this.Children.Add(this.mVisualizedSeries);
        }

        public IEnumerable<ISerie> Series {
            get => this.mSeries;
            set {
                foreach (var serie in this.mSeries.OfType<INotifyCollectionChanged>()) {
                    serie.CollectionChanged -= this.OnSerieCollectionChanged;
                }

                this.mSeries = value;

                foreach (var serie in this.mSeries.OfType<INotifyCollectionChanged>()) {
                    serie.CollectionChanged += this.OnSerieCollectionChanged;
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize) {
            var geometryGroups = this.Series?.Select(serie => serie.Visualizer.GetGeometryGroup(serie, availableSize)).ToArray();

            var combinedGroup = new GeometryGroup();

            foreach (var group in geometryGroups) {
                combinedGroup.Children.Add(group);
            }

            this.mVisualizedSeries.Data = combinedGroup;

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement item in this.Children) {
                item.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        private void OnSerieCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            this.InvalidateMeasure();
        }
    }
}
