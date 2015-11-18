using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.GUI.ViewModel.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialFlowAnalysis.GUI.ViewModel
{
    public class MaterialFlowViewModel : INotifyPropertyChanged
    {
        public IService Service;

        public MaterialFlow Model { get; set; }

        public QuantificationCenterViewModel Source { get; set; }
        public QuantificationCenterViewModel Destination { get; set; }

        public MaterialFlowViewModel(QuantificationCenterViewModel source, QuantificationCenterViewModel destination)
        {
            Source = source;
            Destination = destination;
            Source.PropertyChanged += Source_PropertyChanged;
            Destination.PropertyChanged += Destination_PropertyChanged;
        }

        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Source"));
        }

        private void Destination_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Destination"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
