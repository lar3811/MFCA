using System;
using MaterialFlowAnalysis.Core.Entities.Abstract;

namespace MaterialFlowAnalysis.Core.Entities
{
    [Serializable()]
    public class MaterialFlow : Entity
    {
        private QuantificationCenter _destination;
        private QuantificationCenter _source;
        private double _value;
        private double _volume;

        public QuantificationCenter Destination
        {
            get { return _destination; }
            set { SetField(ref _destination, value); }
        }

        public QuantificationCenter Source
        {
            get { return _source; }
            set { SetField(ref _source, value); }
        }

        public double Value
        {
            get { return _value; }
            set { SetField(ref _value, value); }
        }

        public double Volume
        {
            get { return _volume; }
            set { SetField(ref _volume, value); }
        }

        public string MeasureUnit { get; set; }

        public MaterialFlow(QuantificationCenter source, QuantificationCenter destination)
        {
            Source = source;
            Destination = destination;
        }

        public MaterialFlow()
        {
            MeasureUnit = "т.";
        }

        public override string ToString()
        {
            var destination = Destination?.ToString() ?? string.Empty;
            var source = Source?.ToString() ?? string.Empty;
            return $"{source} -[{Volume} {MeasureUnit}/${Value}]-> {destination}";
        }
    }
}