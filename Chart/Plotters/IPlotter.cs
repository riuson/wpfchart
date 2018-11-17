using System.Collections.Generic;

namespace Chart {
    public interface IPlotter {
        IEnumerable<ISerie> Series { get; set; }
        SeriesDataRange Range { get; }
    }
}
