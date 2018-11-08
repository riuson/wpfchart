using Chart.Series;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Chart.Plotters {
    public class Plotter : Panel {
        public Plotter() {
            this.Series = null;
        }

        public IEnumerable<ISerie> Series { get; set; }
    }
}
