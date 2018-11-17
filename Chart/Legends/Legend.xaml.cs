using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Chart {
    /// <summary>
    /// Логика взаимодействия для Legend.xaml
    /// </summary>
    public partial class Legend : UserControl {
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

        public Legend() {
            this.InitializeComponent();
        }

        public IEnumerable<ISerie> Series {
            get => this.GetValue(Legend.SeriesProperty) as IEnumerable<ISerie>;
            set => this.SetValue(Legend.SeriesProperty, value);
        }
    }
}
