using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chart {
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
        private Dictionary<ISerie, DrawingVisual> mVisuals;

        public Plotter() {
            this.mSeries = new ISerie[] { };
            this.mVisuals = new Dictionary<ISerie, DrawingVisual>();
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
                var visual = this.mVisuals[serie];
                serie.Visualizer.Draw(serie, visual, size, this.Range);
            }

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement item in this.Children) {
                item.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        protected override Visual GetVisualChild(int index) {
            return this.mVisuals.Values.ElementAt(index);
        }

        protected override int VisualChildrenCount => this.mVisuals?.Count ?? 0;

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

        private void AssignSeries(IEnumerable<ISerie> series) {
            foreach (var pair in this.mVisuals) {
                base.RemoveVisualChild(pair.Value);
                base.RemoveLogicalChild(pair.Value);

                if (pair.Key is INotifyCollectionChanged notifyable) {
                    notifyable.CollectionChanged -= this.OnSerieCollectionChanged;
                }
            }

            this.mVisuals.Clear();

            this.mSeries = series;

            if (this.mSeries != null) {
                foreach (var serie in this.mSeries) {
                    if (serie is INotifyCollectionChanged notifyable) {
                        notifyable.CollectionChanged += this.OnSerieCollectionChanged;
                    }

                    var visual = new DrawingVisual();
                    base.AddVisualChild(visual);
                    base.AddLogicalChild(visual);
                    this.mVisuals.Add(serie, visual);
                }
            }
        }
    }
}
