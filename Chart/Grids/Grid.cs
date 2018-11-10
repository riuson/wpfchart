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

            this.MarksX = new double[] { };
            this.MarksY = new double[] { };
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
        public double[] MarksX { get; private set; }
        public double[] MarksY { get; private set; }

        protected override Size MeasureOverride(Size availableSize) {
            var group = new GeometryGroup();

            var count = Math.Ceiling(availableSize.Width / this.Interval);

            if (count > 2) {
                var step = availableSize.Width / count;
                this.MarksX = Enumerable.Range(0, Convert.ToInt32(count)).Select(i => i * step).ToArray();

                foreach (var x in this.MarksX) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(x, 0),
                            new Point(x, availableSize.Height)));
                }
            } else {
                this.MarksX = new double[] { };
            }

            count = Math.Ceiling(availableSize.Height / this.Interval);

            if (count > 2) {
                var step = availableSize.Height / count;
                this.MarksY = Enumerable.Range(0, Convert.ToInt32(count)).Select(i => i * step).ToArray();

                foreach (var y in this.MarksY) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(0, y),
                            new Point(availableSize.Width, y)));
                }
            } else {
                this.MarksY = new double[] { };
            }

            this.mPath.Data = group;

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
