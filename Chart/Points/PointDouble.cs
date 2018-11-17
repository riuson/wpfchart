﻿namespace Chart {
    public class PointDouble : IPoint {
        double IPoint.XValue => this.X;

        double IPoint.YValue => this.Y;

        public PointDouble() {
            this.X = 0;
            this.Y = 0;
        }

        public PointDouble(double x, double y) {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
