using Chart.Grids;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chart.Axes {
    public class Labels : Panel, ILabels {
        #region Dependency properties
        public static readonly DependencyProperty MarksProperty;

        static Labels() {
            MarksProperty = DependencyProperty.Register("Marks",
                typeof(Marks), typeof(Labels),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        }
        #endregion

        private TextBlock[] mTextBlocks;

        public Labels() {
            this.mTextBlocks = null;
            this.Foreground = Brushes.Black;
            this.Side = Dock.Left;
        }

        public Dock Side { get; set; }

        public Brush Foreground { get; set; }

        public Marks Marks {
            get => this.GetValue(Labels.MarksProperty) as Marks;
            set => this.SetValue(Labels.MarksProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            var marks = this.Marks;

            var width = double.IsPositiveInfinity(availableSize.Width) ? 20 : availableSize.Width;
            var height = double.IsPositiveInfinity(availableSize.Height) ? 10 : availableSize.Height;

            TextBlock[] textBlocks = null;

            if (marks != null) {
                switch (this.Side) {
                    case Dock.Left:
                    case Dock.Right: {
                            textBlocks = marks.Y
                                .Select(y => {
                                    var tb = new TextBlock() {
                                        Text = y.ToString("F3")
                                    };
                                    tb.Measure(availableSize);
                                    tb.RenderTransform = new TranslateTransform(0, y * marks.Size.Height);
                                    return tb;
                                })
                                .ToArray();

                            if (textBlocks.Length > 0) {
                                width = textBlocks.Max(item => item.DesiredSize.Width);
                            } else {
                                width = 0;
                            }

                            break;
                        }
                    case Dock.Top:
                    case Dock.Bottom: {
                            textBlocks = marks.X
                                .Select(x => {
                                    var tb = new TextBlock() {
                                        Text = x.ToString("F3")
                                    };
                                    tb.Measure(availableSize);
                                    tb.RenderTransform = new TranslateTransform(x * marks.Size.Width, 0);
                                    return tb;
                                })
                                .ToArray();

                            if (textBlocks.Length > 0) {
                                height = textBlocks.Max(item => item.DesiredSize.Height);
                            } else {
                                height = 0;
                            }

                            break;
                        }
                }
            } else {
                width = 0;
                height = 0;
            }

            this.mTextBlocks = textBlocks;
            this.Children.Clear();

            if (this.mTextBlocks != null) {
                foreach (var textBlock in this.mTextBlocks) {
                    this.Children.Add(textBlock);
                }
            }

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
