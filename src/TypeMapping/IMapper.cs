using System;

namespace TypeMapping
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
		TDestination MapTo<TDestination>(object source);
        IMapBuilder Map(object source);
        string GetDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName);
    }

    public interface IMapBuilder
    {
        IMapBuilder AndMap(object source);
        TDestination To<TDestination>();
        void To<TDestination>(TDestination destination);
    }
}