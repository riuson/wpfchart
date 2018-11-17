using System.Windows;

namespace Chart {
    public class Marks {
        public Marks() {
            this.X = new double[] { };
            this.Y = new double[] { };
            this.Size = Size.Empty;
        }
        public double[] X { get; set; }
        public double[] Y { get; set; }
        public Size Size { get; set; }
    }
}
