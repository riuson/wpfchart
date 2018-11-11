using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart.Grids {
    public class Grid : Panel, IGrid {
        private readonly Path mPath;

        public Grid() {
            this.mPath = new Path();
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 1;
            this.Interval = 50;
            this.Children.Add(this.mPath);

            this.Marks = new Marks();
        }

        public Brush Stroke {
            get => this.mPath.Stroke;
            set => this.mPath.Stroke = value;
        }

        public double StrokeThickness {
            get => this.mPath.StrokeThickness;
            set => this.mPath.StrokeThickness = value;
        }

        public double Interval { get; set; }
        public Marks Marks { get; private set; }

        protected override Size MeasureOverride(Size availableSize) {
            var group = new GeometryGroup();

            var size = new Size(
                double.IsPositiveInfinity(availableSize.Width) ? 100 : availableSize.Width,
                double.IsPositiveInfinity(availableSize.Height) ? 100 : availableSize.Height);

            var count = Math.Ceiling(size.Width / this.Interval);
            var marks = new Marks();

            if (count > 2) {
                var step = size.Width / count;
                marks.X = Enumerable.Range(0, Convert.ToInt32(count + 1)).Select(i => i * step).ToArray();

                foreach (var x in marks.X) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(x, 0),
                            new Point(x, size.Height)));
                }
            }

            count = Math.Ceiling(size.Height / this.Interval);

            if (count > 2) {
                var step = size.Height / count;
                marks.Y = Enumerable.Range(0, Convert.ToInt32(count + 1)).Select(i => i * step).ToArray();

                foreach (var y in marks.Y) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(0, y),
                            new Point(size.Width, y)));
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
    }
}
