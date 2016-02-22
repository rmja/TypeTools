using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using TypeMapping;
using AutoMapper;
using System.Linq;

namespace System
{
    public static class ServiceProviderExtensions
    {
		public static IServiceProvider ConfigureTypeMapping(this IServiceProvider serviceProvider)
		{
            var provider = serviceProvider.GetService<IAutoMapperConfiguratorProvider>();
            var configurators = provider.AutoMapperConfiguratorTypes.Select(x => (IAutoMapperConfigurator)Activator.CreateInstance(x.AsType())).ToList();

            Mapper.Initialize(config =>
            {
                config.ConstructServicesUsing(type =>
                {
                    return serviceProvider.GetService(type);
                });

                foreach (var configurator in configurators)
                {
                    configurator.Configure(config);
                }
            });
            Mapper.Configuration.AllowNullCollections = true;

            Mapper.AssertConfigurationIsValid();

            return serviceProvider;
		}
    }
}