using System;

namespace Chart.Axes {
    public class DateTimeFormatter : ILabelFormatter {
        public DateTimeFormatter(string format = "HH:mm:ss") {
            this.Format = format;
        }

        public string ToString(double value) {
            var dt = new DateTime(Convert.ToInt64(value));
            return dt.ToString(this.Format);
        }

        public string Format { get; set; }
    }
}
