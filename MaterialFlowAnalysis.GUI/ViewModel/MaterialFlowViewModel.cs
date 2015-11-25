using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.GUI.Repository.Abstract;
using MaterialFlowAnalysis.GUI.ViewModel.Abstract;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MaterialFlowAnalysis.GUI.ViewModel
{
    public class MaterialFlowViewModel : ViewModelBase<MaterialFlow>
    {
        public IService Service;

        public MaterialFlow Model { get; set; }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public MaterialFlowViewModel(QuantificationCenterViewModel source, QuantificationCenterViewModel destination)
        {
            Source = source;
            Destination = destination;
            Source.PropertyChanged += Source_PropertyChanged;
            Destination.PropertyChanged += Destination_PropertyChanged;

            EditCommand = new Command(obj =>
            {
                var window = new MFSettingsWindow(this) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
                window.ShowDialog();
            });
            DeleteCommand = new Command(obj => Service.DeleteMaterialFlow(Model));
        }

        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                OnPropertyChanged("SourcePosition");
        }

        private void Destination_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                OnPropertyChanged("DestinationPosition");
        }



        public QuantificationCenterViewModel Source { get; set; }
        public QuantificationCenterViewModel Destination { get; set; }

        public Point SourcePosition { get { return new Point(Source.Model.X, Source.Model.Y); } }
        public Point DestinationPosition { get { return new Point(Destination.Model.X, Destination.Model.Y); } }

        public int Id
        {
            get { return Model.Id; }
            set
            {
                if (Model.Id == value) return;
                Model.Id = value;
                OnPropertyChanged();
            }
        }

        public double Value
        {
            get { return Model.Value; }
            set
            {
                if (Model.Value == value) return;
                Model.Value = value;
                OnPropertyChanged();
                OnPropertyChanged("Description");
            }
        }

        public double Volume
        {
            get { return Model.Volume; }
            set
            {
                if (Model.Volume == value) return;
                Model.Volume = value;
                OnPropertyChanged();
                OnPropertyChanged("Description");
            }
        }

        public string MeasureUnit
        {
            get { return Model.MeasureUnit; }
            set
            {
                if (Model.MeasureUnit == value) return;
                Model.MeasureUnit = value;
                OnPropertyChanged();
                OnPropertyChanged("Description");
            }
        }

        public string Description
        {
            get { return $"{Model.Volume} {Model.MeasureUnit}\n{Model.Value:C}"; }
        }
    }
}
