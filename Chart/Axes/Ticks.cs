using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chart {
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
            var geometry = new PathGeometry();
            var marks = this.Marks;

            var width = double.IsPositiveInfinity(availableSize.Width) ? this.StrokeLength : availableSize.Width;
            var height = double.IsPositiveInfinity(availableSize.Height) ? this.StrokeLength : availableSize.Height;

            if (marks != null) {
                switch (this.Side) {
                    case Dock.Left:
                    case Dock.Right: {
                            width = this.StrokeLength;

                            for (var i = 0; i < marks.Y.Length; i++) {
                                var figure = new PathFigure {
                                    StartPoint = new Point(0, marks.Y[i] * marks.Size.Height)
                                };
                                figure.Segments.Add(new LineSegment(new Point(this.StrokeLength, marks.Y[i] * marks.Size.Height), true));
                                geometry.Figures.Add(figure);
                            }

                            break;
                        }
                    case Dock.Top:
                    case Dock.Bottom: {
                            height = this.StrokeLength;

                            for (var i = 0; i < marks.X.Length; i++) {
                                var figure = new PathFigure {
                                    StartPoint = new Point(marks.X[i] * marks.Size.Width, 0)
                                };
                                figure.Segments.Add(new LineSegment(new Point(marks.X[i] * marks.Size.Width, this.StrokeLength), true));
                                geometry.Figures.Add(figure);
                            }

                            break;
                        }
                }
            } else {
                width = 0;
                height = 0;
            }

            this.mPath.Data = geometry;

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
