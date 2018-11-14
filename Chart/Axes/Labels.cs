using Chart.Grids;
using Chart.Plotters;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chart.Axes {
    public class Labels : Panel, ILabels {
        #region Dependency properties
        public static readonly DependencyProperty MarksProperty;
        public static readonly DependencyProperty RangeProperty;

        static Labels() {
            MarksProperty = DependencyProperty.Register("Marks",
                typeof(Marks), typeof(Labels),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

            RangeProperty = DependencyProperty.Register("Range",
                typeof(SeriesDataRange), typeof(Labels),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        }
        #endregion

        private List<TextBlock> mTextBlocks;

        public Labels() {
            this.mTextBlocks = new List<TextBlock>();
            this.Foreground = Brushes.Black;
            this.Side = Dock.Left;
        }

        public Dock Side { get; set; }

        public Brush Foreground { get; set; }

        public Marks Marks {
            get => this.GetValue(Labels.MarksProperty) as Marks;
            set => this.SetValue(Labels.MarksProperty, value);
        }

        public SeriesDataRange Range {
            get => this.GetValue(Labels.RangeProperty) as SeriesDataRange;
            set => this.SetValue(Labels.RangeProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            var marks = this.Marks;
            var range = this.Range;

            var width = double.IsPositiveInfinity(availableSize.Width) ? 20 : availableSize.Width;
            var height = double.IsPositiveInfinity(availableSize.Height) ? 10 : availableSize.Height;
            var side = this.Side;

            TextBlock[] textBlocks = null;

            if (marks != null && range != null) {
                switch (side) {
                    case Dock.Left:
                    case Dock.Right: {
                            textBlocks = this.GetTextBlocks(marks.Y.Length);

                            for (var i = 0; i < marks.Y.Length; i++) {
                                this.SetText(textBlocks[i], side, availableSize, i, marks, range);
                                var location = this.CalculateLocation(textBlocks[i], side, i, marks, range);
                                textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                            }

                            if (textBlocks.Length > 0) {
                                width = textBlocks.Max(item => item.DesiredSize.Width);
                            } else {
                                width = 0;
                            }

                            break;
                        }
                    case Dock.Top:
                    case Dock.Bottom: {
                            textBlocks = this.GetTextBlocks(marks.X.Length);

                            for (var i = 0; i < marks.X.Length; i++) {
                                this.SetText(textBlocks[i], side, availableSize, i, marks, range);
                                var location = this.CalculateLocation(textBlocks[i], side, i, marks, range);
                                textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                            }

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

            this.Children.Clear();

            if (textBlocks != null) {
                foreach (var textBlock in textBlocks) {
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

        private TextBlock[] GetTextBlocks(int count) {
            if (this.mTextBlocks.Count < count) {
                this.mTextBlocks.AddRange(Enumerable.Range(0, count - this.mTextBlocks.Count).Select(_ => new TextBlock()));
            }

            return this.mTextBlocks.Take(count).ToArray();
        }

        private void SetText(TextBlock textBlock, Dock side, Size availableSize, int i, Marks marks, SeriesDataRange range) {

            switch (side) {
                case Dock.Left:
                case Dock.Right: {
                        var value = range.MaxY - marks.Y[i] * (range.MaxY - range.MinY);
                        textBlock.Text = value.ToString("F3");
                        break;
                    }
                case Dock.Top:
                case Dock.Bottom: {
                        var value = range.MaxX - marks.X[i] * (range.MaxX - range.MinX);
                        textBlock.Text = value.ToString("F3");
                        break;
                    }
            }

            textBlock.Measure(availableSize);
        }

        private Point CalculateLocation(TextBlock textBlock, Dock side, int i, Marks marks, SeriesDataRange range) {
            var location = new Point(0, 0);
            var size = textBlock.DesiredSize;

            switch (side) {
                case Dock.Left:
                case Dock.Right: {
                        location.Y = marks.Y[i] * marks.Size.Height - size.Height / 2;

                        if (i == 0) {
                            location.Y = 0;
                        } else if (i == marks.Y.Length - 1) {
                            location.Y = marks.Size.Height - size.Height;
                        }

                        break;
                    }
                case Dock.Top:
                case Dock.Bottom: {
                        location.X = marks.X[i] * marks.Size.Width - size.Width / 2;

                        if (i == 0) {
                            location.X = 0;
                        } else if (i == marks.X.Length - 1) {
                            location.X = marks.Size.Width - size.Width;
                        }

                        break;
                    }
            }

            return location;
        }
    }
}
