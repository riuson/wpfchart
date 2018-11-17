using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chart {
    public class Labels : Panel, ILabels {
        #region Dependency properties
        public static readonly DependencyProperty MarksProperty;
        public static readonly DependencyProperty RangeProperty;
        public static readonly DependencyProperty SpacingProperty;
        public static readonly DependencyProperty FormatterProperty;
        public static readonly DependencyProperty SideProperty;

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

            SpacingProperty = DependencyProperty.Register("Spacing",
                typeof(double), typeof(Labels),
                new FrameworkPropertyMetadata(
                    5d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

            FormatterProperty = DependencyProperty.Register("Formatter",
                typeof(ILabelFormatter), typeof(Labels),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

            SideProperty = DependencyProperty.Register("Side",
                typeof(Dock), typeof(Labels),
                new FrameworkPropertyMetadata(
                    Dock.Bottom,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        }
        #endregion

        private List<TextBlock> mTextBlocks;

        public Labels() {
            this.mTextBlocks = new List<TextBlock>();
            this.Foreground = Brushes.Black;
            this.Side = Dock.Left;
            this.Formatter = new DoubleFormatter();
        }

        public Dock Side {
            get => (Dock)this.GetValue(Labels.SideProperty);
            set => this.SetValue(Labels.SideProperty, value);
        }

        public Brush Foreground { get; set; }

        public Marks Marks {
            get => this.GetValue(Labels.MarksProperty) as Marks;
            set => this.SetValue(Labels.MarksProperty, value);
        }

        public SeriesDataRange Range {
            get => this.GetValue(Labels.RangeProperty) as SeriesDataRange;
            set => this.SetValue(Labels.RangeProperty, value);
        }

        public double Spacing {
            get => Convert.ToDouble(this.GetValue(Labels.SpacingProperty));
            set => this.SetValue(Labels.SpacingProperty, value);
        }

        public ILabelFormatter Formatter {
            get => this.GetValue(Labels.FormatterProperty) as ILabelFormatter;
            set => this.SetValue(Labels.FormatterProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize) {
            var marks = this.Marks;
            var range = this.Range;

            var width = double.IsPositiveInfinity(availableSize.Width) ? 20 : availableSize.Width;
            var height = double.IsPositiveInfinity(availableSize.Height) ? 10 : availableSize.Height;
            var side = this.Side;
            var spacing = this.Spacing;
            var formatter = this.Formatter;

            this.Children.Clear();

            TextBlock[] textBlocks = null;

            if (marks != null && range != null) {
                switch (side) {
                    case Dock.Left:
                    case Dock.Right: {
                            if (marks.Y.Length > 2) {
                                textBlocks = this.GetTextBlocks(marks.Y.Length);

                                var i = 0;
                                this.SetText(textBlocks[i], side, availableSize, formatter, i, marks, range);
                                var location = this.CalculateLocation(textBlocks[i], side, i, marks, range);
                                textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                                textBlocks[i].Visibility = Visibility.Visible;
                                var limit1 = location.Y + textBlocks[i].DesiredSize.Height + spacing;

                                i = this.Marks.Y.Length - 1;
                                this.SetText(textBlocks[i], side, availableSize, formatter, i, marks, range);
                                location = this.CalculateLocation(textBlocks[i], side, i, marks, range);
                                textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                                textBlocks[i].Visibility = Visibility.Visible;
                                var limit2 = location.Y - spacing;

                                for (i = 1; i < marks.Y.Length - 1; i++) {
                                    this.SetText(textBlocks[i], side, availableSize, formatter, i, marks, range);
                                    location = this.CalculateLocation(textBlocks[i], side, i, marks, range);

                                    if (location.Y > limit1 && location.Y + textBlocks[i].DesiredSize.Height < limit2) {
                                        textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                                        textBlocks[i].Visibility = Visibility.Visible;
                                        limit1 = location.Y + textBlocks[i].DesiredSize.Height + spacing;
                                    } else {
                                        textBlocks[i].Visibility = Visibility.Hidden;
                                    }
                                }

                                if (textBlocks.Length > 0) {
                                    width = textBlocks.Where(item => item.Visibility == Visibility.Visible).Max(item => item.DesiredSize.Width);
                                } else {
                                    width = 0;
                                }
                            }

                            break;
                        }
                    case Dock.Top:
                    case Dock.Bottom: {
                            if (marks.X.Length > 2) {
                                textBlocks = this.GetTextBlocks(marks.X.Length);

                                var i = 0;
                                this.SetText(textBlocks[i], side, availableSize, formatter, i, marks, range);
                                var location = this.CalculateLocation(textBlocks[i], side, i, marks, range);
                                textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                                textBlocks[i].Visibility = Visibility.Visible;
                                var limit1 = location.X + textBlocks[i].DesiredSize.Width + spacing;

                                i = this.Marks.X.Length - 1;
                                this.SetText(textBlocks[i], side, availableSize, formatter, i, marks, range);
                                location = this.CalculateLocation(textBlocks[i], side, i, marks, range);
                                textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                                textBlocks[i].Visibility = Visibility.Visible;
                                var limit2 = location.X - spacing;

                                for (i = 1; i < marks.X.Length - 1; i++) {
                                    this.SetText(textBlocks[i], side, availableSize, formatter, i, marks, range);
                                    location = this.CalculateLocation(textBlocks[i], side, i, marks, range);

                                    if (location.X > limit1 && location.X + textBlocks[i].DesiredSize.Width < limit2) {
                                        textBlocks[i].RenderTransform = new TranslateTransform(location.X, location.Y);
                                        textBlocks[i].Visibility = Visibility.Visible;
                                        limit1 = location.X + textBlocks[i].DesiredSize.Width + spacing;
                                    } else {
                                        textBlocks[i].Visibility = Visibility.Hidden;
                                    }
                                }

                                if (textBlocks.Length > 0) {
                                    height = textBlocks.Where(item => item.Visibility == Visibility.Visible).Max(item => item.DesiredSize.Height);
                                } else {
                                    height = 0;
                                }
                            }

                            break;
                        }
                }
            } else {
                width = 0;
                height = 0;
            }

            if (textBlocks != null) {
                foreach (var textBlock in textBlocks.Where(item => item.Visibility == Visibility.Visible)) {
                    this.Children.Add(textBlock);
                }
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement item in this.Children) {
                var size = new Size(
                    Math.Min(finalSize.Width, item.DesiredSize.Width),
                    Math.Min(finalSize.Height, item.DesiredSize.Height));
                item.Arrange(new Rect(size));
            }

            return finalSize;
        }

        private TextBlock[] GetTextBlocks(int count) {
            if (this.mTextBlocks.Count < count) {
                this.mTextBlocks.AddRange(Enumerable.Range(0, count - this.mTextBlocks.Count).Select(_ => new TextBlock()));
            }

            return this.mTextBlocks.Take(count).ToArray();
        }

        private void SetText(TextBlock textBlock, Dock side, Size availableSize, ILabelFormatter formatter, int i, Marks marks, SeriesDataRange range) {

            switch (side) {
                case Dock.Left:
                case Dock.Right: {
                        var value = range.MaxY - marks.Y[i] * (range.MaxY - range.MinY);
                        var text = formatter?.ToString(value) ?? value.ToString();

                        if (textBlock.Text != text) {
                            textBlock.Text = text;
                            textBlock.Measure(availableSize);
                        }

                        break;
                    }
                case Dock.Top:
                case Dock.Bottom: {
                        var value = range.MaxX - marks.X[i] * (range.MaxX - range.MinX);
                        var text = formatter?.ToString(value) ?? value.ToString();

                        if (textBlock.Text != text) {
                            textBlock.Text = text;
                            textBlock.Measure(availableSize);
                        }

                        break;
                    }
            }
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
