using Chart.Points;
using Chart.Series;
using System.Windows;
using System.Windows.Media;

namespace WpfAppTest {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Serie _serie1;

        public MainWindow() {
            this.InitializeComponent();
            this._serie1 = new Serie() {
                LineBrush = Brushes.Blue,
                LineWidth = 2,
                Title = "Hello, World!"
            };
            this._serie1.Add(new PointDouble(0, 0));
            this._serie1.Add(new PointDouble(0.3, 0.7));
            this._serie1.Add(new PointDouble(1, 1));

            this.plotter1.Series = new ISerie[] { this._serie1 };
        }
    }
}
