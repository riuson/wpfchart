using Chart.Series;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chart.Plotters {
    public class Plotter : Panel, IPlotter {
        private IEnumerable<ISerie> mSeries;
        private Dictionary<ISerie, Path> mPaths;

        public Plotter() {
            this.mSeries = new ISerie[] { };
            this.mPaths = new Dictionary<ISerie, Path>();
            this.Range = new SeriesDataRange();
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
                        StrokeThickness = serie.StrokeThickness,
                        Stroke = serie.Stroke
                    };
                    this.mPaths.Add(serie, path);
                    this.Children.Add(path);
                }
            }
        }

        public SeriesDataRange Range { get; private set; }

        protected override Size MeasureOverride(Size availableSize) {
            this.UpdateRange();

            foreach (var serie in this.mSeries) {
                var group = serie.Visualizer.GetGeometryGroup(serie, availableSize, this.Range);
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

        private void UpdateRange() {
            double? minX = null;
            double? maxX = null;
            double? minY = null;
            double? maxY = null;

            foreach (var serie in this.Series) {

                foreach (var point in serie) {
                    minX = !minX.HasValue ? point.XValue : Math.Min(minX.Value, point.XValue);
                    maxX = !maxX.HasValue ? point.XValue : Math.Max(maxX.Value, point.XValue);
                    minY = !minY.HasValue ? point.YValue : Math.Min(minY.Value, point.YValue);
                    maxY = !maxY.HasValue ? point.YValue : Math.Max(maxY.Value, point.YValue);
                }
            }

            this.Range.MinX = minX ?? 0;
            this.Range.MinY = minY ?? 0;
            this.Range.MaxX = maxX ?? 0;
            this.Range.MaxY = maxY ?? 0;
        }
    }
}
