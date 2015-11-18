using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TypeMapping
{
    public interface IAutoMapperConfiguratorProvider
    {
        IEnumerable<TypeInfo> AutoMapperConfiguratorTypes { get; }
    }
}
