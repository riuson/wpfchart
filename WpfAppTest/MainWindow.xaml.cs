using Chart.Points;
using Chart.Series;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfAppTest {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly Task mTask;
        private readonly CancellationTokenSource mToken;
        private readonly IProgress<Tuple<double, double>> mProgress;
        private readonly ChartData mData;

        public MainWindow() {
            this.InitializeComponent();
            this.mData = new ChartData();
            this.DataContext = this.mData;
            this.grid1.Stroke = Brushes.DarkGreen;
            this.grid1.StrokeThickness = 0.5;
            this.grid1.Interval = 50;

            this.mToken = new CancellationTokenSource();
            this.mProgress = new Progress<Tuple<double, double>>(item => {
                this.mData.Serie1.Add(new PointDateTime(DateTime.Now, item.Item1));
                this.mData.Serie2.Add(new PointDateTime(DateTime.Now, item.Item2));

                if (this.mData.Serie1.Count > 20) {
                    this.mData.Serie1.RemoveAt(0);
                }

                if (this.mData.Serie2.Count > 20) {
                    this.mData.Serie2.RemoveAt(0);
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
            this.mData.Serie1.Clear();
            this.mData.Serie2.Clear();

            while (!cancellationToken.IsCancellationRequested) {
                progress.Report(new Tuple<double, double>(0x800000 + rnd.NextDouble() * 0x100000, 0x850000 + rnd.NextDouble() * 0x100000));
                cancellationToken.WaitHandle.WaitOne(500);
            }
        }
    }
}
