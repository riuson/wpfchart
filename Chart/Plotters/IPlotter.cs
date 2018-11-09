using Chart.Series;
using System.Collections.Generic;
using System.Windows;

namespace Chart.Plotters {
    public interface IPlotter {
        IEnumerable<ISerie> Series { get; set; }
        SeriesDataRange Range { get; }
    }
}
