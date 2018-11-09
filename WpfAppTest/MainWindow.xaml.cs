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
        private readonly Serie mSerie1;
        private readonly Serie mSerie2;
        private readonly Task mTask;
        private readonly CancellationTokenSource mToken;
        private readonly IProgress<Tuple<double, double>> mProgress;

        public MainWindow() {
            this.InitializeComponent();
            this.mSerie1 = new Serie() {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Title = "Hello, World!"
            };
            this.mSerie2 = new Serie() {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Title = "Serie 2"
            };

            this.plotter1.Series = new ISerie[] { this.mSerie1, this.mSerie2 };

            this.mToken = new CancellationTokenSource();
            this.mProgress = new Progress<Tuple<double, double>>(item => {
                this.mSerie1.Add(new PointDateTime(DateTime.Now, item.Item1));
                this.mSerie2.Add(new PointDateTime(DateTime.Now, item.Item2));

                if (this.mSerie1.Count > 20) {
                    this.mSerie1.RemoveAt(0);
                }

                if (this.mSerie2.Count > 20) {
                    this.mSerie2.RemoveAt(0);
                }
            });
            this.mTask = Task.Factory.StartNew(o => this.Method((CancellationToken)o, this.mProgress), this.mToken.Token, this.mToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            this.Closing += this.MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this.mToken.Cancel();
            this.mTask.Wait();
        }

        private void Method(CancellationToken cancellationToken, IProgress<Tuple<double, double>> progress) {
            var rnd = new Random();
            this.mSerie1.Clear();

            while (!cancellationToken.IsCancellationRequested) {
                progress.Report(new Tuple<double, double>(0x800000 + rnd.NextDouble() * 0x100000, 0x800000 + rnd.NextDouble() * 0x100000));
                cancellationToken.WaitHandle.WaitOne(500);
            }
        }
    }
}
