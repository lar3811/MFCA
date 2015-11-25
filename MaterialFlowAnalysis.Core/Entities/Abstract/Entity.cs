using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MaterialFlowAnalysis.Core.Annotations;

namespace MaterialFlowAnalysis.Core.Entities.Abstract
{
    [Serializable]
    public abstract class Entity
    {
        public int Id { get; set; }



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
    }
}