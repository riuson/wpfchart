using Chart.Grids;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart.Ticks {
    public class Ticks : Panel, ITicks {
        private readonly Path mPath;

        public Ticks() {
            this.mPath = new Path();
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 1;
            this.Children.Add(this.mPath);

            this.Side = Dock.Left;
            this.Grid = null;
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 1;
            this.StrokeLength = 5;
        }

        public Dock Side { get; set; }

        public IGrid Grid { get; set; }

        public Brush Stroke {
            get => this.mPath.Stroke;
            set => this.mPath.Stroke = value;
        }

        public double StrokeThickness {
            get => this.mPath.StrokeThickness;
            set => this.mPath.StrokeThickness = value;
        }

        public double StrokeLength { get; set; }

        protected override Size MeasureOverride(Size availableSize) {
            var group = new GeometryGroup();
            var grid = this.Grid;

            if (grid != null) {
                switch (this.Side) {
                    case Dock.Left:
                    case Dock.Right: {
                            availableSize = new Size(this.StrokeLength, availableSize.Height);

                            foreach (var y in grid.MarksY) {
                                group.Children.Add(
                                    new LineGeometry(
                                        new Point(0, y),
                                        new Point(this.StrokeLength, y)));
                            }
                            break;
                        }
                    case Dock.Top:
                    case Dock.Bottom: {
                            availableSize = new Size(availableSize.Width, this.StrokeLength);

                            foreach (var x in grid.MarksX) {
                                group.Children.Add(
                                    new LineGeometry(
                                        new Point(x, 0),
                                        new Point(x, this.StrokeLength)));
                            }

                            break;
                        }
                }
            } else {
                availableSize = Size.Empty;
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
