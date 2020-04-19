using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart.Grids {
    public class Grid : Panel, IGrid {
        private readonly Path mPath;

        public Grid() {
            this.mPath = new Path();
            this.Children.Add(this.mPath);
            this.Marks = new Marks();

            BindingOperations.SetBinding(this.mPath, Shape.StrokeProperty,
                new Binding("Stroke") { Source = this, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(this.mPath, Shape.StrokeThicknessProperty,
                new Binding("StrokeThickness") { Source = this, Mode = BindingMode.OneWay });
        }

        public Brush Stroke {
            get => this.GetValue(StrokeProperty) as Brush;
            set => this.SetValue(StrokeProperty, value);
        }

        public double StrokeThickness {
            get => Convert.ToDouble(this.GetValue(StrokeThicknessProperty));
            set => this.SetValue(StrokeThicknessProperty, value);
        }

        public double Interval {
            get => Convert.ToDouble(this.GetValue(IntervalProperty));
            set => this.SetValue(IntervalProperty, value);
        }

        public Marks Marks {
            get => this.GetValue(MarksProperty) as Marks;
            set => this.SetValue(MarksProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            var group = new GeometryGroup();

            var size = new Size(
                double.IsPositiveInfinity(availableSize.Width) ? 100 : availableSize.Width,
                double.IsPositiveInfinity(availableSize.Height) ? 100 : availableSize.Height);

            double interval = this.Interval;

            double count = Math.Ceiling(size.Width / interval);
            var marks = new Marks();
            marks.Size = size;

            if (count > 2) {
                double step = 1.0d / count;
                marks.X = Enumerable.Range(0, Convert.ToInt32(count + 1)).Select(i => i * step).ToArray();

                foreach (double x in marks.X) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(x * marks.Size.Width, 0),
                            new Point(x * marks.Size.Width, size.Height)));
                }
            }

            count = Math.Ceiling(size.Height / interval);

            if (count > 2) {
                double step = 1.0d / count;
                marks.Y = Enumerable.Range(0, Convert.ToInt32(count + 1)).Select(i => i * step).ToArray();

                foreach (double y in marks.Y) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(0, y * marks.Size.Height),
                            new Point(size.Width, y * marks.Size.Height)));
                }
            }

            this.mPath.Data = group;
            this.Marks = marks;

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement item in this.Children) {
                item.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        #region Dependency properties

        public static readonly DependencyProperty MarksProperty;
        public static readonly DependencyProperty StrokeProperty;
        public static readonly DependencyProperty StrokeThicknessProperty;
        public static readonly DependencyProperty IntervalProperty;

        static Grid() {
            MarksProperty = DependencyProperty.Register("Marks",
                typeof(Marks), typeof(Grid),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.None));

            StrokeProperty = DependencyProperty.Register("Stroke",
                typeof(Brush), typeof(Grid),
                new FrameworkPropertyMetadata(
                    Brushes.Black,
                    FrameworkPropertyMetadataOptions.AffectsRender));

            StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness",
                typeof(double), typeof(Grid),
                new FrameworkPropertyMetadata(
                    1d,
                    FrameworkPropertyMetadataOptions.AffectsRender));

            IntervalProperty = DependencyProperty.Register("Interval",
                typeof(double), typeof(Grid),
                new FrameworkPropertyMetadata(
                    50d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        #endregion
    }
}
