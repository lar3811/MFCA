using System.Globalization;
using System.Windows;
using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.GUI.ViewModel;

namespace MaterialFlowAnalysis.GUI
{
    /// <summary>
    /// Логика взаимодействия для MFSettingsWindow.xaml
    /// </summary>
    public partial class MFSettingsWindow : Window
    {

        public MFSettingsWindow(MaterialFlowViewModel mf)
        {
            InitializeComponent();
            DataContext = mf;
        }
        
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
