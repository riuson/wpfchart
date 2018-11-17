using System.Windows.Controls;
using System.Windows.Media;

namespace Chart {
    internal interface ILabels {
        Dock Side { get; set; }
        Brush Foreground { get; set; }
        Marks Marks { get; set; }
    }
}
