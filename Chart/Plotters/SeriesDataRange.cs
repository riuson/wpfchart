namespace Chart {
    public class SeriesDataRange {
        public SeriesDataRange() {
            this.MinX = 0;
            this.MinY = 0;
            this.MaxX = 0;
            this.MaxY = 0;
        }
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
    }
}
