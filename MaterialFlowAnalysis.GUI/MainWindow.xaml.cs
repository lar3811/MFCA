using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialFlowAnalysis.GUI.ViewModel;

namespace MaterialFlowAnalysis.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
        }

        private Point _position;

        private void Evaluate_OnClick(object sender, RoutedEventArgs e)
        {
            //new SimpleEvaluationStrategy().Execute(_QCVM.Values);
        }

        private void Canvas_OpenContextMenu(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            if (canvas == null || canvas.ContextMenu == null) return;

            //if (_isSelectingMaterialFlowDestination == true)
            //{
            //    _isSelectingMaterialFlowDestination = false;
            //    return;
            //}

            canvas.ContextMenu.IsOpen = true;
            _position = Mouse.GetPosition(canvas);
            e.Handled = true;
        }

        private void QC_Add_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Service.CreateQuantificationCenter(_position);
        }

        private void Canvas_OnDrop(object sender, DragEventArgs e)
        {
            var qcvm = e.Data.GetData(typeof(QuantificationCenterViewModel)) as QuantificationCenterViewModel;
            var dragEnd = e.GetPosition(sender as Canvas);
            qcvm.Move(dragEnd);
        }
    }
}
