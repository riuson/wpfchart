﻿using System.Collections.Generic;
using System.Windows.Media;

namespace Chart {
    public interface ISerie : IEnumerable<IPoint> {
        string Title { get; }
        Brush Stroke { get; }
        double StrokeThickness { get; }
        ISerieVisualizer Visualizer { get; }
    }
}
