using System;
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

        protected override Size MeasureOverride(Size availableSize) {
            var group = new GeometryGroup();

            var count = Math.Ceiling(availableSize.Width / this.Interval);

            if (count > 2) {
                var step = availableSize.Width / count;

                for (var i = 0; i < Convert.ToInt32(count); i++) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(i * step, 0),
                            new Point(i * step, availableSize.Height)));
                }
            }

            count = Math.Ceiling(availableSize.Height / this.Interval);

            if (count > 2) {
                var step = availableSize.Height / count;

                for (var i = 0; i < Convert.ToInt32(count); i++) {
                    group.Children.Add(
                        new LineGeometry(
                            new Point(0, i * step),
                            new Point(availableSize.Width, i * step)));
                }
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
