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
        #region Dependency properties
        public static readonly DependencyProperty RangeProperty;
        public static readonly DependencyProperty SeriesProperty;

        static Plotter() {
            RangeProperty = DependencyProperty.Register("Range",
                typeof(SeriesDataRange), typeof(Plotter),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.None));

            SeriesProperty = DependencyProperty.Register("Series",
                typeof(IEnumerable<ISerie>), typeof(Plotter),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnSeriesChanged)));
        }

        private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var plotter = d as Plotter;
            plotter?.AssignSeries(e.NewValue as ISerie[]);
        }

        #endregion

        private IEnumerable<ISerie> mSeries;
        private Dictionary<ISerie, Path> mPaths;

        public Plotter() {
            this.mSeries = new ISerie[] { };
            this.mPaths = new Dictionary<ISerie, Path>();
        }

        public IEnumerable<ISerie> Series {
            get => this.GetValue(Plotter.SeriesProperty) as IEnumerable<ISerie>;
            set => this.SetValue(Plotter.SeriesProperty, value);
        }


        public SeriesDataRange Range {
            get => this.GetValue(Plotter.RangeProperty) as SeriesDataRange;
            set => this.SetValue(Plotter.RangeProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            this.UpdateRange();

            var size = new Size(
                double.IsPositiveInfinity(availableSize.Width) ? 100 : availableSize.Width,
                double.IsPositiveInfinity(availableSize.Height) ? 100 : availableSize.Height);

            foreach (var serie in this.mSeries) {
                var group = serie.Visualizer.GetGeometryGroup(serie, size, this.Range);
                this.mPaths[serie].Data = group;
            }

            return size;
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

            if (this.mSeries != null) {
                foreach (var serie in this.mSeries) {
                    foreach (var point in serie) {
                        minX = !minX.HasValue ? point.XValue : Math.Min(minX.Value, point.XValue);
                        maxX = !maxX.HasValue ? point.XValue : Math.Max(maxX.Value, point.XValue);
                        minY = !minY.HasValue ? point.YValue : Math.Min(minY.Value, point.YValue);
                        maxY = !maxY.HasValue ? point.YValue : Math.Max(maxY.Value, point.YValue);
                    }
                }
            }

            this.Range = new SeriesDataRange() {
                MinX = minX ?? 0,
                MinY = minY ?? 0,
                MaxX = maxX ?? 0,
                MaxY = maxY ?? 0
            };
        }

        private void AssignSeries(ISerie[] series) {
            foreach (var pair in this.mPaths) {
                this.Children.Remove(pair.Value);
            }

            if (this.mSeries != null) {
                foreach (var serie in this.mSeries.OfType<INotifyCollectionChanged>()) {
                    serie.CollectionChanged -= this.OnSerieCollectionChanged;
                }
            }

            this.mSeries = series;

            if (this.mSeries != null) {
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
    }
}
