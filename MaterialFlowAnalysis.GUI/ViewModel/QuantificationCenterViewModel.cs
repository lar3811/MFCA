using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.GUI.ViewModel.Abstract;
using System.Windows.Input;
using System;
using System.Windows;
using MaterialFlowAnalysis.GUI.CustomControls;
using MaterialFlowAnalysis.GUI.Repository.Abstract;

namespace MaterialFlowAnalysis.GUI.ViewModel
{
    public class QuantificationCenterViewModel : ViewModelBase<QuantificationCenter>
    {
        public IService Service;

        public QuantificationCenter Model;

        public ICommand CreateFlowCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public QuantificationCenterViewModel()
        {
            CreateFlowCommand = new Command(CreateFlow_SelectSource);
            EditCommand = new Command(obj =>
            {
                var window = new QCSettingsWindow(this) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
                window.ShowDialog();
            });
            DeleteCommand = new Command(obj => Service.DeleteQuantificationCenter(Model));
        }

        private void CreateFlow_SelectSource(object obj)
        {
            Application.Current.MainWindow.MouseDown += CreateFlow_SelectDestination;
        }

        private void CreateFlow_SelectDestination(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.MouseDown -= CreateFlow_SelectDestination;
            if (e.ChangedButton != MouseButton.Left) return;
            var element = e.OriginalSource as FrameworkElement;
            var destination = element.DataContext as QuantificationCenterViewModel;         // IF IT IS A CONTROL
            if (destination == null)
                destination = ((element.TemplatedParent as FrameworkElement)
                                       .Parent as FrameworkElement)
                                       .DataContext as QuantificationCenterViewModel;       // IF IT IS AN ELEMENT OF A CONTROL
            if (destination == null)
                return;
            Service.CreateMaterialFlow(Model, destination.Model);
            e.Handled = true;
        }

        

        public Point Position
        {
            get { return new Point(Model.X, Model.Y); }
            set
            {
                if (Model.X == value.X && Model.Y == value.Y) return;
                Model.X = value.X;
                Model.Y = value.Y;
                OnPropertyChanged();
            }
        }
        
        public int Id
        {
            get { return Model.Id; }
        }
        
        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (Model.Name == value) return;
                Model.Name = value;
                OnPropertyChanged();
            }
        }

        public double SystemCost
        {
            get { return Model.SystemCost; }
            set
            {
                if (Model.SystemCost == value) return;
                Model.SystemCost = value;
                OnPropertyChanged();
                OnPropertyChanged("SystemCostDescription");
            }
        }

        public double EnergyCost
        {
            get { return Model.EnergyCost; }
            set
            {
                if (Model.EnergyCost == value) return;
                Model.EnergyCost = value;
                OnPropertyChanged();
            }
        }

        public double WasteProcessingCost
        {
            get { return Model.WasteProcessingCost; }
            set 
            { 
                if (Model.WasteProcessingCost == value) return;
                Model.WasteProcessingCost = value;
                OnPropertyChanged();
            }
        }

        public double WasteVolume { get { return Model.Waste.Volume; } }

        public double WasteValue { get { return Model.Waste.Value; } }



        public string SystemCostDescription { get { return $"{SystemCost:C}"; } }

        public string WasteDescription { get { return $"{WasteVolume} {Model.Waste.MeasureUnit}\n{WasteValue:C}"; } }

        public string IdDescription { get { return $"ТК.{Id:00}"; } }
    }
}
