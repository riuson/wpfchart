namespace Chart.Axes {
    public class DoubleFormatter : ILabelFormatter {
        public DoubleFormatter(string format = "F3") {
            this.Format = format;
        }

        public string ToString(double value) => value.ToString(this.Format);

        public string Format { get; set; }
    }
}
