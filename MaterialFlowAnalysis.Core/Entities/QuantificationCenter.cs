using System;
using System.Collections.Generic;
using MaterialFlowAnalysis.Core.Entities.Abstract;

namespace MaterialFlowAnalysis.Core.Entities
{
    [Serializable()]
    public class QuantificationCenter : Entity
    {
        public QuantificationCenter()
        {
            Waste = new MaterialFlow(this, null);
            Name = "Точка контроля";
        }

        public readonly IList<MaterialFlow> IncomingFlows = new List<MaterialFlow>();
        public readonly IList<MaterialFlow> OutgoingFlows = new List<MaterialFlow>();
        public MaterialFlow Waste { get; private set; }

        public double X { get; set; }

        public double Y { get; set; }

        public string Name { get; set; }

        public double SystemCost { get; set; }

        public double EnergyCost { get; set; }

        public double WasteProcessingCost { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}