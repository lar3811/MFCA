using System.Globalization;
using System.Windows;
using MaterialFlowAnalysis.Core.Entities;

namespace MaterialFlowAnalysis.GUI
{
    /// <summary>
    /// Логика взаимодействия для MFSettingsWindow.xaml
    /// </summary>
    public partial class MFSettingsWindow : Window
    {
        private readonly MaterialFlow _mf;

        public MFSettingsWindow(MaterialFlow mf)
        {
            _mf = mf;
            InitializeComponent();

            IdL.Content = _mf.Id;
            SourceL.Content = _mf.Source.ToString();
            DestinationL.Content = _mf.Destination.ToString();
            VolumeTB.Text = _mf.Volume.ToString();
            ValueTB.Text = _mf.Value.ToString("C");
        }


        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            _mf.Volume = double.Parse(VolumeTB.Text);
            _mf.Value = double.Parse(ValueTB.Text, NumberStyles.Currency);
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
