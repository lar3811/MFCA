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
        private double _r;

        public IService Service;

        public QuantificationCenter Model;

        public ICommand CreateFlowCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public QuantificationCenterViewModel()
        {
            R = 32;

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
            var destination = (e.OriginalSource as FrameworkElement).DataContext as QuantificationCenterViewModel;
            if (destination == null) return;
            Service.CreateMaterialFlow(Model, destination.Model);
            e.Handled = true;
        }



        public double R
        {
            get { return _r; }
            set { SetField(ref _r, value); }
        }

        public double D => 2*R;

        public double X
        {
            get { return Model.X - R; }
            set
            {
                if (Model.X - R == value) return;
                Model.X = value + R;
                OnPropertyChanged();
            } 
        }

        public double Y
        {
            get { return Model.Y - R; }
            set
            {
                if (Model.Y - R == value) return;
                Model.Y = value + R;
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
        


        public void Move(Point destination)
        {
            X = destination.X - R;
            Y = destination.Y - R;
        }
    }
}
