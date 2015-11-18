using MaterialFlowAnalysis.GUI.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace MaterialFlowAnalysis.GUI
{
    public class CanvasItemsStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is QuantificationCenterViewModel)
            {                
                return App.Current.MainWindow.Resources["QuantificationCenterStyle"] as Style;
            }

            return base.SelectStyle(item, container);
        }
    }
}
