using Chart.Series;
using System.Collections.Generic;

namespace Chart.Plotters {
    public interface IPlotter {
        IEnumerable<ISerie> Series { get; set; }
        SeriesDataRange Range { get; }
    }
}
