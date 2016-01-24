using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MaterialFlowAnalysis.Core.Entities;
using MaterialFlowAnalysis.Core.Entities.Abstract;
using System.Linq;
using System.Windows;
using MaterialFlowAnalysis.GUI.Repository.Abstract;
using MaterialFlowAnalysis.GUI.Repository;
using System.Windows.Input;

namespace MaterialFlowAnalysis.GUI.ViewModel
{
    public class MainWindowViewModel
    {
        public IService Service;

        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand EvaluateCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        public ObservableCollection<QuantificationCenterViewModel> QCVMs { get; set; }
        public ObservableCollection<MaterialFlowViewModel> MFVMs { get; set; }

        public MainWindowViewModel()
        {
            SaveCommand = new Command(obj => SaveModel());
            LoadCommand = new Command(obj => LoadModel());
            EvaluateCommand = new Command(obj => EvaluateModel());
            ClearCommand = new Command(obj => ClearModel());
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



        public void SaveModel()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "model";
            dialog.DefaultExt = ".dat";

            var confirmed = dialog.ShowDialog();
            if (confirmed == null || confirmed == false) return;

            Service.SaveModel(dialog.FileName);
        }

        public void LoadModel()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "model";
            dialog.DefaultExt = ".dat";

            var confirmed = dialog.ShowDialog();
            if (confirmed == null || confirmed == false) return;

            QCVMs.Clear();
            MFVMs.Clear();
            Service.LoadModel(dialog.FileName);
            foreach (var qc in Service.RetrieveQuantificationCenter())
                OnQuantificationCenterCreated(null, qc);
            foreach (var mf in Service.RetrieveMaterialFlow())
                OnMaterialFlowCreated(null, mf);
        }

        public void EvaluateModel()
        {
            Service.EvaluateFlows();
            foreach (var qcvm in QCVMs) qcvm.OnPropertyChanged("WasteDescription");
            foreach (var mfvm in MFVMs) mfvm.OnPropertyChanged("Description");
        }

        public void ClearModel()
        {
            Service.Clear();
        }
    }
}
