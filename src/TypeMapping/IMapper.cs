using System;

namespace TypeMapping
{
    public interface IMapper
    {
		TDestination Map<TDestination>(object source);
        string GetDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName);
    }
}