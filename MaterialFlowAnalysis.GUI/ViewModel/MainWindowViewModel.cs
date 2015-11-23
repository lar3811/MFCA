using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.Core.Entities.Abstract;
using System.Linq;
using System.Windows;

namespace MaterialFlowAnalysis.GUI.ViewModel
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

        event EventHandler OnModelUpdated;
    }


    public class TheService : IService
    {
        public event EventHandler OnModelUpdated;

        private List<QuantificationCenter> QCs = new List<QuantificationCenter>();
        private List<MaterialFlow> MFs = new List<MaterialFlow>();

        private int index = 0;
        


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
    }

    public class MainWindowViewModel
    {
        public IService Service;

        public ObservableCollection<QuantificationCenterViewModel> QCVMs { get; set; }
        public ObservableCollection<MaterialFlowViewModel> MFVMs { get; set; }

        public MainWindowViewModel()
        {
            QCVMs = new ObservableCollection<QuantificationCenterViewModel>();
            MFVMs = new ObservableCollection<MaterialFlowViewModel>();
            Service = new TheService();
            Service.OnQuantificationCenterCreated += OnQuantificationCenterCreated;
            Service.OnQuantificationCenterDeleted += OnQuantificationCenterDeleted;
            Service.OnMaterialFlowCreated += OnMaterialFlowCreated;
            Service.OnMaterialFlowDeleted += OnMaterialFlowDeleted;
        }

        private void OnQuantificationCenterCreated(object sender, QuantificationCenter qc)
        {
            var qcvm = new QuantificationCenterViewModel
            {
                Model = qc,
                Service = Service
            };
            QCVMs.Add(qcvm);
        }

        private void OnQuantificationCenterDeleted(object sender, QuantificationCenter qc)
        {
            var qcvm = QCVMs.FirstOrDefault(_qcvm => _qcvm.Model == qc);
            QCVMs.Remove(qcvm);
        }

        private void OnMaterialFlowCreated(object sender, MaterialFlow mf)
        {
            var source = QCVMs.FirstOrDefault(_qcvm => _qcvm.Model == mf.Source);
            var destination = QCVMs.FirstOrDefault(_qcvm => _qcvm.Model == mf.Destination);
            var mfvm = new MaterialFlowViewModel (source, destination)
            {
                Model = mf,
                Service = Service
            };
            MFVMs.Add(mfvm);
        }

        private void OnMaterialFlowDeleted(object sender, MaterialFlow mf)
        {
            var mfvm = MFVMs.FirstOrDefault(_mfvm => _mfvm.Model == mf);
            MFVMs.Remove(mfvm);
        }
        


        public void EvaluateModel()
        {
        }
        


        private string _path = @"E:\model.dat";
        public void SaveModel()
        {
            var stream = File.Create(_path);
            var serializer = new BinaryFormatter();
            serializer.Serialize(stream, QCVMs);
            stream.Close();
        }

        public void LoadModel()
        {
            var stream = File.OpenRead(_path);
            var serializer = new BinaryFormatter();
            var loaded = (IEnumerable<QuantificationCenter>)serializer.Deserialize(stream);
            //foreach (var qc in loaded) QCs.Add(qc);
            stream.Close();
        }
    }
}
