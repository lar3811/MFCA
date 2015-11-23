using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MaterialFlowAnalysis.Core.Annotations;

namespace MaterialFlowAnalysis.Core.Entities.Abstract
{
    [Serializable]
    public abstract class Entity : INotifyPropertyChanged
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { SetField(ref _id, value); }
        }


        public override bool Equals(object obj)
        {
            var casted = obj as Entity;
            return casted != null && casted.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return GetType().Name + ":" + Id;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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