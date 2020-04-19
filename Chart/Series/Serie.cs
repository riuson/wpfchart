using Chart.Points;
using Chart.SerieVisualizers;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Chart.Series {
    public class Serie : ObservableCollection<IPoint>, ISerie {
        public Serie() {
            this.Title = string.Empty;
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 1;
            this.Visualizer = new LineVisualizer();
        }

        public string Title { get; set; }
        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }
        public ISerieVisualizer Visualizer { get; set; }
    }
}
