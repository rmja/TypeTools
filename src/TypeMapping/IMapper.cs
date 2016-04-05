using System;

namespace TypeMapping
{
    public interface IMapper
    {
		TDestination MapTo<TDestination>(object source, object sideInformation = null);
        IMapBuilder Map(object source, object sideInformation = null);
        string GetDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName);
    }

    public interface IMapBuilder
    {
        IMapBuilder AndMap(object source, object sideInformation = null);
        TDestination To<TDestination>();
        void To<TDestination>(TDestination destination);
    }
}