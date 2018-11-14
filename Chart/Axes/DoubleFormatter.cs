namespace Chart.Axes {
    public class DoubleFormatter : ILabelFormatter {
        public DoubleFormatter() {
            this.Format = "F3";
        }

        public DoubleFormatter(string format) {
            this.Format = format;
        }

        public string ToString(double value) => value.ToString(this.Format);

        public string Format { get; set; }
    }
}
