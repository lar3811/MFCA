using System;
using System.Globalization;
using System.Windows;
using MaterialFlowAnalysis.Core;
using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.GUI.ViewModel;

namespace MaterialFlowAnalysis.GUI
{
    /// <summary>
    /// Логика взаимодействия для QCSettingsWindow.xaml
    /// </summary>
    public partial class QCSettingsWindow : Window
    {
        public QCSettingsWindow(QuantificationCenterViewModel qc)
        {
            InitializeComponent();
            DataContext = qc;
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
