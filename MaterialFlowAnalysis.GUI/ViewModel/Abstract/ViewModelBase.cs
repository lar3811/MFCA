using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MaterialFlowAnalysis.Core.Annotations;

namespace MaterialFlowAnalysis.GUI.ViewModel.Abstract
{
    public abstract class ViewModelBase<TModel> : INotifyPropertyChanged where TModel: class 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public virtual void OnPropertyChanged(params string[] args)
        {
            foreach (var prop in args)
                OnPropertyChanged(prop);
        }

        public bool SetField<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
}
