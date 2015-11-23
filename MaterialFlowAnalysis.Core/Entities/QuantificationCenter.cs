using System;
using System.Collections.ObjectModel;
using MaterialFlowAnalysis.Core.Entities.Abstract;

namespace MaterialFlowAnalysis.Core.Entities
{
    [Serializable()]
    public class QuantificationCenter : Entity
    {
        private double _x;
        private double _y;
        private string _name;
        private double _systemCost;
        private double _energyCost;
        private double _wasteProcessingCost;



        public QuantificationCenter()
        {
            Waste = new MaterialFlow(this, null);
            _name = "Точка контроля";
        }

        public readonly ObservableCollection<MaterialFlow> IncomingFlows = new ObservableCollection<MaterialFlow>();
        public readonly ObservableCollection<MaterialFlow> OutgoingFlows = new ObservableCollection<MaterialFlow>();
        public MaterialFlow Waste { get; private set; }

        public double X
        {
            get { return _x; }
            set { SetField(ref _x, value); }
        }

        public double Y
        {
            get { return _y; }
            set { SetField(ref _y, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public double SystemCost
        {
            get { return _systemCost; }
            set { SetField(ref _systemCost, value); }
        }

        public double EnergyCost
        {
            get { return _energyCost; }
            set { SetField(ref _energyCost, value); }
        }

        public double WasteProcessingCost
        {
            get { return _wasteProcessingCost; }
            set { SetField(ref _wasteProcessingCost, value); }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}