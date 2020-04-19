using System;

namespace Chart.Points {
    public class PointDateTime : IPoint {
        public PointDateTime() {
            this.X = DateTime.Now;
            this.Y = 0;
        }

        public PointDateTime(DateTime x, double y) {
            this.X = x;
            this.Y = y;
        }

        public DateTime X { get; set; }
        public double Y { get; set; }
        double IPoint.XValue => this.X.Ticks;

        double IPoint.YValue => this.Y;
    }
}
