using Chart.Series;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chart.Plotters {
    public class Plotter : Panel {
        private IEnumerable<GeometryGroup> _geometryGroups;

        public Plotter() {
            this.Series = null;
            this._geometryGroups = null;
        }

        public IEnumerable<ISerie> Series { get; set; }

        protected override Size MeasureOverride(Size availableSize) {
            var geometryGroups = this.Series?.Select(serie => serie.Visualizer.GetGeometryGroup(serie, availableSize)).ToArray();

            return availableSize;
        }
    }
}
