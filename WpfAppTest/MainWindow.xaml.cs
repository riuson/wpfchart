using Chart.Points;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppTest {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly ChartData mData;
        private readonly IProgress<Tuple<double, double>> mProgress;
        private readonly Task mTask;
        private readonly CancellationTokenSource mToken;

        public MainWindow() {
            this.InitializeComponent();
            this.mData = new ChartData();
            this.DataContext = this.mData;

            this.mToken = new CancellationTokenSource();
            this.mProgress = new Progress<Tuple<double, double>>(item => {
                this.mData.Serie1.Add(new PointDateTime(DateTime.Now, item.Item1));
                this.mData.Serie2.Add(new PointDateTime(DateTime.Now, item.Item2));

                if (this.mData.Serie1.Count > 500) {
                    this.mData.Serie1.RemoveAt(0);
                }

                if (this.mData.Serie2.Count > 500) {
                    this.mData.Serie2.RemoveAt(0);
                }
            });
            this.mTask = Task.Factory.StartNew(o => this.Method((CancellationToken)o, this.mProgress),
                this.mToken.Token, this.mToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            this.Closing += this.MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) {
            this.mToken.Cancel();
            this.mTask.Wait();
        }

        private void Method(CancellationToken cancellationToken, IProgress<Tuple<double, double>> progress) {
            var rnd = new Random();
            this.mData.Serie1.Clear();
            this.mData.Serie2.Clear();

            while (!cancellationToken.IsCancellationRequested) {
                double sin = Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 1000);
                double cos = Math.Cos(DateTime.Now.TimeOfDay.TotalMilliseconds / 500);

                progress.Report(new Tuple<double, double>(sin, cos));
                cancellationToken.WaitHandle.WaitOne(60);
            }
        }
    }
}
