using System.Windows;
using System.Windows.Media;

namespace Chart {
    public interface ISerieVisualizer {
        void Draw(ISerie serie, DrawingVisual visual, Size size, SeriesDataRange dataRange);
    }
}
