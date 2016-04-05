using System;

namespace TypeMapping
{
    public interface IMapper
    {
        IMapBuilder Map(object source, object sideInformation = null);
        string GetDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName);
    }

    public static class IMapperExtensions
    {
        public static TDestination MapTo<TDestination>(this IMapper mapper, object source, object sideInformation = null)
        {
            return mapper.Map(source, sideInformation).To<TDestination>();
        }
    }

    public interface IMapBuilder
    {
        IMapBuilder AndMap(object source, object sideInformation = null);
        TDestination To<TDestination>();
        void To<TDestination>(TDestination destination);
    }
}