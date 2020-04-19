using Chart.Series;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Chart.Legends {
    /// <summary>
    /// Логика взаимодействия для Legend.xaml
    /// </summary>
    public partial class Legend : UserControl {
        public Legend() {
            this.InitializeComponent();
        }

        public IEnumerable<ISerie> Series {
            get => this.GetValue(SeriesProperty) as IEnumerable<ISerie>;
            set => this.SetValue(SeriesProperty, value);
        }

        #region Dependency properties

        public static readonly DependencyProperty SeriesProperty;

        static Legend() {
            SeriesProperty = DependencyProperty.Register("Series",
                typeof(IEnumerable<ISerie>), typeof(Legend),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        #endregion
    }
}
