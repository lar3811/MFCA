using MaterialFlowAnalysis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialFlowAnalysis.GUI.Repository.Abstract
{
    public interface IService
    {
        QuantificationCenter CreateQuantificationCenter(Point position);
        IEnumerable<QuantificationCenter> RetrieveQuantificationCenter();
        bool DeleteQuantificationCenter(QuantificationCenter obj);
        event EventHandler<QuantificationCenter> OnQuantificationCenterCreated;
        event EventHandler<QuantificationCenter> OnQuantificationCenterDeleted;

        MaterialFlow CreateMaterialFlow(QuantificationCenter source, QuantificationCenter destination);
        IEnumerable<MaterialFlow> RetrieveMaterialFlow();
        bool DeleteMaterialFlow(MaterialFlow obj);
        event EventHandler<MaterialFlow> OnMaterialFlowCreated;
        event EventHandler<MaterialFlow> OnMaterialFlowDeleted;

        void SaveModel(string path);
        void LoadModel(string path);
        void EvaluateFlows();
        void Clear();

        event EventHandler OnModelUpdated;
    }
}
