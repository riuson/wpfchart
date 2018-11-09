using Chart.Points;
using Chart.Series;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfAppTest {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Serie _serie1;
        private Task _task;
        private CancellationTokenSource _token;
        private IProgress<Tuple<double>> _progress;

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

            this._token = new CancellationTokenSource();
            this._progress = new Progress<Tuple<double>>(item => this._serie1.Add(new PointDateTime(DateTime.Now, item.Item1)));
            this._task = Task.Factory.StartNew(o => this.Method((CancellationToken)o, this._progress), this._token.Token, this._token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            this.Closing += this.MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this._token.Cancel();
            this._task.Wait();
        }

        private void Method(CancellationToken cancellationToken, IProgress<Tuple<double>> progress) {
            var rnd = new Random();
            this._serie1.Clear();

            while (!cancellationToken.IsCancellationRequested) {
                progress.Report(new Tuple<double>(0x800000 + rnd.NextDouble() * 0x100000));
                cancellationToken.WaitHandle.WaitOne(500);
            }
        }
    }
}
