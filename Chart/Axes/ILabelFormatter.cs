using Chart.Points;

namespace Chart.Axes {
    public interface ILabelFormatter {
        string ToString(double value);
    }
}
