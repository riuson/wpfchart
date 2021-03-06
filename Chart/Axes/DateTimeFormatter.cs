﻿using System;

namespace Chart.Axes {
    public class DateTimeFormatter : ILabelFormatter {
        public DateTimeFormatter() {
            this.Format = "HH:mm:ss";
        }

        public DateTimeFormatter(string format) {
            this.Format = format;
        }

        public string Format { get; set; }

        public string ToString(double value) {
            var dt = new DateTime(Convert.ToInt64(value));
            return dt.ToString(this.Format);
        }
    }
}
