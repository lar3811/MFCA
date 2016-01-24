using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.Core.Services;
using MaterialFlowAnalysis.GUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialFlowAnalysis.GUI.Repository
{
    public class TheService : IService
    {
        public event EventHandler OnModelUpdated;

        private List<QuantificationCenter> QCs = new List<QuantificationCenter>();
        private List<MaterialFlow> MFs = new List<MaterialFlow>();

        private int index = 1;



        public QuantificationCenter CreateQuantificationCenter(Point position)
        {
            var newObj = new QuantificationCenter
            {
                Id = index++,
                X = position.X,
                Y = position.Y
            };
            QCs.Add(newObj);
            if (OnQuantificationCenterCreated != null)
                OnQuantificationCenterCreated(this, newObj);
            return newObj;
        }

        public IEnumerable<QuantificationCenter> RetrieveQuantificationCenter()
        {
            return QCs;
        }

        public bool DeleteQuantificationCenter(QuantificationCenter obj)
        {
            var removed = QCs.Remove(obj);
            if (removed == false) return false;
            if (OnQuantificationCenterDeleted != null)
                OnQuantificationCenterDeleted(this, obj);
            var linkedFlows = obj.IncomingFlows.Concat(obj.OutgoingFlows).ToArray();
            foreach (var mf in linkedFlows)
                DeleteMaterialFlow(mf);
            return true;
        }

        public event EventHandler<QuantificationCenter> OnQuantificationCenterCreated;
        public event EventHandler<QuantificationCenter> OnQuantificationCenterDeleted;



        public MaterialFlow CreateMaterialFlow(QuantificationCenter source, QuantificationCenter destination)
        {
            var newObj = new MaterialFlow
            {
                Id = index++,
                Source = source,
                Destination = destination
            };
            source.OutgoingFlows.Add(newObj);
            destination.IncomingFlows.Add(newObj);
            MFs.Add(newObj);
            if (OnMaterialFlowCreated != null)
                OnMaterialFlowCreated(this, newObj);
            return newObj;
        }

        public IEnumerable<MaterialFlow> RetrieveMaterialFlow()
        {
            return MFs;
        }

        public bool DeleteMaterialFlow(MaterialFlow obj)
        {
            var removed = MFs.Remove(obj);
            if (removed == false) return false;
            obj.Source.OutgoingFlows.Remove(obj);
            obj.Destination.IncomingFlows.Remove(obj);
            if (OnMaterialFlowDeleted != null)
                OnMaterialFlowDeleted(this, obj);
            return true;
        }

        public event EventHandler<MaterialFlow> OnMaterialFlowCreated;
        public event EventHandler<MaterialFlow> OnMaterialFlowDeleted;


        public void SaveModel(string path)
        {
            var stream = File.Create(path);
            var serializer = new BinaryFormatter();
            serializer.Serialize(stream, QCs);
            stream.Close();
        }

        public void LoadModel(string path)
        {
            var stream = File.OpenRead(path);
            var serializer = new BinaryFormatter();
            var loaded = (IEnumerable<QuantificationCenter>)serializer.Deserialize(stream);
            QCs.Clear();
            MFs.Clear();
            foreach (var qc in loaded)
            {
                QCs.Add(qc);
                MFs.AddRange(qc.OutgoingFlows);
            }
            if (OnModelUpdated != null)
                OnModelUpdated(this, null);
            stream.Close();
            index = Math.Max(
                QCs.Count == 0 ? 0 : QCs.Max(qc => qc.Id), 
                MFs.Count == 0 ? 0 : MFs.Max(mf => mf.Id));
        }

        public void EvaluateFlows()
        {
            var strategy = new SimpleEvaluationStrategy();
            strategy.Execute(QCs);
        }

        public void Clear()
        {         
            foreach (var qc in QCs.ToArray())
                DeleteQuantificationCenter(qc);
            index = 0;
        }
    }
}
