using AutoMapper;
using System;
using System.Linq;

namespace TypeMapping
{
	public class AutoMapperAdapter : IMapper
	{
		public TDestination Map<TDestination>(object source)
		{
			return Mapper.Map<TDestination>(source);
		}

        public string GetDestinationPropertyName(Type sourceType, Type destinationType, string sourcePropertyName)
        {
            var typeMap = Mapper.GetAllTypeMaps().Single(x => x.SourceType == sourceType && x.DestinationType == destinationType);
            var propertyMaps = typeMap.GetPropertyMaps();
            var propertyMap = propertyMaps.SingleOrDefault(x => x.SourceMember != null && x.SourceMember.Name == sourcePropertyName);

            return propertyMap?.DestinationProperty.Name;
        }
    }
}