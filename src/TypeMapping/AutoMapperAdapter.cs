using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeMapping
{
	public class AutoMapperAdapter : IMapper
	{
        public TDestination MapTo<TDestination>(object source, object sideInformation = null)
        {
            return Map(source, sideInformation).To<TDestination>();
        }

        public IMapBuilder Map(object source, object sideInformation = null)
        {
            return new MapBuilder(source, sideInformation);
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
        private readonly List<Tuple<object, object>> _sources = new List<Tuple<object, object>>();

        public MapBuilder(object source, object sideInformation)
        {
            _sources.Add(Tuple.Create(source, sideInformation));
        }

        public IMapBuilder AndMap(object source, object sideInformation = null)
        {
            _sources.Add(Tuple.Create(source, sideInformation));
            return this;
        }

        public TDestination To<TDestination>()
        {
            return _sources.Aggregate(default(TDestination), (dest, src) => Map(src.Item1, src.Item2, dest));
        }

        public void To<TDestination>(TDestination destination)
        {
            _sources.ForEach(src => Map(src.Item1, src.Item2, destination));
        }

        private TDestination Map<TDestination>(object source, object sideInformation, TDestination destination)
        {
            if (sideInformation != null)
            {
                var type = sideInformation.GetType();
                var properties = type.GetProperties();
                var pairs = properties.ToDictionary(x => x.Name, x => x.GetValue(sideInformation, null));

                Action<IMappingOperationOptions> setOptions = (IMappingOperationOptions opts) =>
                {
                    foreach (var pair in pairs)
                    {
                        opts.Items[pair.Key] = pair.Value;
                    }
                };

                return destination != null
                ? (TDestination)Mapper.Map(source, destination, source.GetType(), typeof(TDestination), setOptions)
                : Mapper.Map<TDestination>(source, setOptions);
            }
            else
            {
                return destination != null
                    ? (TDestination)Mapper.Map(source, destination, source.GetType(), typeof(TDestination))
                    : Mapper.Map<TDestination>(source);
            }
        }
    }
}