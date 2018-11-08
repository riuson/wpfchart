using System;
using Chart.Points;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Chart.Series {
    public class Serie : ObservableCollection<IPoint>, ISerie {
        public string Title { get; set; }
        public Brush LineBrush { get; set; }
        public double LineWidth { get; set; }

        public Serie() {
            this.Title = String.Empty;
            this.LineBrush = Brushes.Black;
            this.LineWidth = 1;
        }
    }
}
