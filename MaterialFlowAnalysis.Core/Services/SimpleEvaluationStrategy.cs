using System.Collections.Generic;
using System.Linq;
using MaterialFlowAnalysis.Core.Entities;
using MathNet.Numerics.LinearAlgebra;

namespace MaterialFlowAnalysis.Core.Services
{
    public class SimpleEvaluationStrategy
    {
        private IList<QuantificationCenter> _qcs;
        private IList<MaterialFlow> _mfs;

        public void Execute(IEnumerable<QuantificationCenter> qcs)
        {
            _qcs = qcs.ToList();
            _mfs = qcs.SelectMany(qc => qc.OutgoingFlows).ToList();
            
            // Calculate waste flows volumes
            foreach (var qc in _qcs)
            {
                if (qc.IncomingFlows.Count == 0 || qc.OutgoingFlows.Count == 0) continue;
                var incomingFlowsVolume = qc.IncomingFlows.Sum(flow => flow.Volume);
                var outgoingFlowsVolume = qc.OutgoingFlows.Sum(flow => flow.Volume);
                qc.Waste.Volume = incomingFlowsVolume - outgoingFlowsVolume;
            }

            // Calculate general flows values

            // для случая, когда известны веса опорных ребер графа
            //var unknown = _mfs.Where(mf => mf.Source.IncomingFlows.Count > 0).ToList(); 
            //var known = _mfs.Where(mf => mf.Source.IncomingFlows.Count == 0).ToList();

            // для случая, когда известны системные затраты опорных узлов
            var unknown = _mfs.ToList(); 
            var known = new List<MaterialFlow>();

            var n = unknown.Count;
            var matrix = new double[n, n];
            var vector = new double[n];
                        
            for (var i = 0; i < n; i++)
            {
                var source = unknown[i].Source;
                var baseVolume = source.OutgoingFlows.Sum(x => x.Volume) + source.Waste.Volume;
                var baseValue = known.Where(f => f.Destination == source).Sum(x => x.Value);

                var k = unknown[i].Volume / baseVolume;

                matrix[i, i] = -1;
                foreach (var entry in unknown.Where(f => f.Destination == source))
                {
                    var j = unknown.IndexOf(entry);
                    if (i == j) continue;
                    matrix[i, j] = k;
                }

                vector[i] = -k * (baseValue + source.SystemCost + source.EnergyCost + source.Waste.Volume * source.WasteProcessingCost); // -k * (Wpc * W + S + E + Mc(0))
            }

            var A = Matrix<double>.Build.DenseOfArray(matrix);
            var B = Vector<double>.Build.Dense(vector);
            var X = A.Solve(B);

            for (var i = 0; i < n; i++)
                unknown[i].Value = X[i];

            // Calculate waste flows volumes
            foreach (var qc in qcs)
            {
                if (qc.IncomingFlows.Count == 0 || qc.OutgoingFlows.Count == 0) continue;
                var incomingFlowsValue = qc.IncomingFlows.Sum(flow => flow.Value);
                var incomingFlowsVolume = qc.IncomingFlows.Sum(flow => flow.Volume);
                qc.Waste.Value = (incomingFlowsValue + qc.SystemCost + qc.EnergyCost + qc.WasteProcessingCost * qc.Waste.Volume) * (qc.Waste.Volume / incomingFlowsVolume);
            }
        }
    }
}
