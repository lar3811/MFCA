using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.Core.Services;
using MathNet.Numerics;

namespace MaterialFlowAnalysis.Core
{
    public class Core
    {
        public void Evaluate(IEnumerable<QuantificationCenter> qcs)
        {
            new SimpleEvaluationStrategy().Execute(qcs);
        }
    }
}
