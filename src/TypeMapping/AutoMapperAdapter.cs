using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeMapping
{
	public class AutoMapperAdapter : IMapper
	{
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return MapTo<TDestination>(source);
        }

        public TDestination MapTo<TDestination>(object source)
        {
            return Map(source).To<TDestination>();
        }

        public IMapBuilder Map(object source)
        {
            return new MapBuilder(source);
        }

        public string GetDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName)
        {
            if (sourceType != destinationType)
            {
                var typeMap = Mapper.GetAllTypeMaps().Single(x => x.SourceType == sourceType && x.DestinationType == destinationType);
                var propertyMaps = typeMap.GetPropertyMaps();
                var propertyMap = propertyMaps.SingleOrDefault(x => x.SourceMember != null && x.SourceMember.Name == sourcePropertyName);

                return propertyMap?.DestinationProperty.Name;
            }
            else
            {
                return sourcePropertyName;
            }
        }
    }

    public class MapBuilder : IMapBuilder
    {
        private readonly List<object> _sources = new List<object>();

        public MapBuilder(object source)
        {
            _sources.Add(source);
        }

        public IMapBuilder AndMap(object source)
        {
            _sources.Add(source);
            return this;
        }

        public TDestination To<TDestination>()
        {
            return _sources.Aggregate(default(TDestination), (dest, src) => Map(src, dest));
        }

        public void To<TDestination>(TDestination destination)
        {
            _sources.ForEach(src => Map(src, destination));
        }

        private TDestination Map<TDestination>(object source, TDestination destination)
        {
            return destination != null
                ? (TDestination)Mapper.Map(source, destination, source.GetType(), typeof(TDestination))
                : Mapper.Map<TDestination>(source);
        }
    }
}