using System;
using MaterialFlowAnalysis.Core.Entities.Abstract;

namespace MaterialFlowAnalysis.Core.Entities
{
    [Serializable()]
    public class MaterialFlow : Entity
    {
        public QuantificationCenter Destination { get; set; }

        public QuantificationCenter Source { get; set; }

        public double Value { get; set; }

        public double Volume { get; set; }

        public string MeasureUnit { get; set; }

        public MaterialFlow(QuantificationCenter source, QuantificationCenter destination) : this()
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