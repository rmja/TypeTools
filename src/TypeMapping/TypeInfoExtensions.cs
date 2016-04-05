using AutoMapper;
using System.Linq;
using TypeMapping;

namespace System.Reflection
{
    public static class TypeInfoExtensions
    {
        public static bool IsAutoMapperConfigurator(this TypeInfo self)
        {
            return typeof(IAutoMapperConfigurator).GetTypeInfo().IsAssignableFrom(self) && self.IsClass && self.Name.EndsWith("AutoMapperConfigurator");
        }

        public static bool IsAutoMapperValueResolver(this TypeInfo self)
        {
            return typeof(IValueResolver).GetTypeInfo().IsAssignableFrom(self) && self.IsClass;
        }

        public static bool IsAutoMapperTypeConverter(this TypeInfo self)
        {
            return self.GetInterfaces().Any(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(ITypeConverter<,>));
        }
    }
}
