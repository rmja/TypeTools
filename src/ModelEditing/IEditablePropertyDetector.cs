using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelEditing
{
    public interface IEditablePropertyDetector
    {
        IEnumerable<string> GetNamesOfProperties<T>(object fragment, bool allowRequired);
        Dictionary<string, object> GetPropertyValueMap<TFragment, TTarget>(object fragment, bool allowRequired);
    }
}
