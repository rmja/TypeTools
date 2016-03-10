using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelEditing
{
    public interface IPropertyUpdater
    {
        TTarget CreateFromProperties<TTarget>(object fragment) where TTarget : new();
        void UpdateProperties<TFragment, TTarget>(TTarget target, object fragment);
    }
}
