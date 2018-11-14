using Chart.Grids;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart.Axes {
    public class Ticks : Panel, ITicks {
        #region Dependency properties
        public static readonly DependencyProperty MarksProperty;
        public static readonly DependencyProperty SideProperty;

        static Ticks() {
            MarksProperty = DependencyProperty.Register("Marks",
                typeof(Marks), typeof(Ticks),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

            SideProperty = DependencyProperty.Register("Side",
                typeof(Dock), typeof(Ticks),
                new FrameworkPropertyMetadata(
                    Dock.Bottom,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        }
        #endregion

        private readonly Path mPath;

        public Ticks() {
            this.mPath = new Path();
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 1;
            this.Children.Add(this.mPath);

            this.Side = Dock.Left;
            this.StrokeLength = 5;
        }

        public Dock Side {
            get => (Dock)this.GetValue(Ticks.SideProperty);
            set => this.SetValue(Ticks.SideProperty, value);
        }

        public Brush Stroke {
            get => this.mPath.Stroke;
            set => this.mPath.Stroke = value;
        }

        public double StrokeThickness {
            get => this.mPath.StrokeThickness;
            set => this.mPath.StrokeThickness = value;
        }

        public double StrokeLength { get; set; }

        public Marks Marks {
            get => this.GetValue(Ticks.MarksProperty) as Marks;
            set => this.SetValue(Ticks.MarksProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            var group = new GeometryGroup();
            var marks = this.Marks;

            var width = double.IsPositiveInfinity(availableSize.Width) ? this.StrokeLength : availableSize.Width;
            var height = double.IsPositiveInfinity(availableSize.Height) ? this.StrokeLength : availableSize.Height;

            if (marks != null) {
                switch (this.Side) {
                    case Dock.Left:
                    case Dock.Right: {
                            width = this.StrokeLength;

                            foreach (var y in marks.Y) {
                                group.Children.Add(
                                    new LineGeometry(
                                        new Point(0, y * marks.Size.Height),
                                        new Point(this.StrokeLength, y * marks.Size.Height)));
                            }
                            break;
                        }
                    case Dock.Top:
                    case Dock.Bottom: {
                            height = this.StrokeLength;

                            foreach (var x in marks.X) {
                                group.Children.Add(
                                    new LineGeometry(
                                        new Point(x * marks.Size.Width, 0),
                                        new Point(x * marks.Size.Width, this.StrokeLength)));
                            }

                            break;
                        }
                }
            } else {
                width = 0;
                height = 0;
            }

            this.mPath.Data = group;

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement item in this.Children) {
                item.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }
    }
}
