using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialFlowAnalysis.GUI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для QuantificationCenterControl.xaml
    /// </summary>
    public partial class QuantificationCenterControl : UserControl
    {
        public QuantificationCenterControl()
        {
            InitializeComponent();
        }

        private void QC_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu.DataContext = DataContext;
            ContextMenu.IsOpen = true;
            e.Handled = true;
        }
        
        private void QC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, DataContext, DragDropEffects.Move);
        }        
    }
}
