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
        private IEnumerable<ISerie> mSeries;
        private Dictionary<ISerie, Path> mPaths;

        public Plotter() {
            this.mSeries = new ISerie[] { };
            this.mPaths = new Dictionary<ISerie, Path>();
        }

        public IEnumerable<ISerie> Series {
            get => this.mSeries;
            set {
                foreach (var pair in this.mPaths) {
                    this.Children.Remove(pair.Value);
                }

                foreach (var serie in this.mSeries.OfType<INotifyCollectionChanged>()) {
                    serie.CollectionChanged -= this.OnSerieCollectionChanged;
                }

                this.mSeries = value;

                foreach (var serie in this.mSeries.OfType<INotifyCollectionChanged>()) {
                    serie.CollectionChanged += this.OnSerieCollectionChanged;
                }

                foreach (var serie in this.mSeries) {
                    var path = new Path() {
                        StrokeThickness = serie.LineWidth,
                        Stroke = serie.LineBrush
                    };
                    this.mPaths.Add(serie, path);
                    this.Children.Add(path);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize) {
            foreach (var serie in this.mSeries) {
                var group = serie.Visualizer.GetGeometryGroup(serie, availableSize);
                this.mPaths[serie].Data = group;
            }

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
