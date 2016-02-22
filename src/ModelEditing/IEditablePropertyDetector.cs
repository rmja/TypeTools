using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelEditing
{
    public interface IEditablePropertyDetector
    {
        bool CanHandleFragmentType(Type type);
        IEnumerable<string> GetNamesOfProperties<T>(object fragment);
        Dictionary<string, object> GetPropertyValueMap<TFragment, TTarget>(object fragment);
    }
}
