using Chart.Series;
using System.Collections.Generic;
using System.Windows.Media;

namespace WpfAppTest {
    internal class ChartData {
        public ChartData() {
            this.Serie1 = new Serie { Stroke = Brushes.Blue, StrokeThickness = 2, Title = "Hello, World!" };
            this.Serie2 = new Serie { Stroke = Brushes.Red, StrokeThickness = 3, Title = "Serie 2" };

            this.Series = new ISerie[] {
                this.Serie1,
                this.Serie2
            };
        }

        public Serie Serie1 { get; }

        public Serie Serie2 { get; }

        public IEnumerable<ISerie> Series { get; }
    }
}
