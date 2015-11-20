using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeMapping;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string _libraryName = "TypeMapping";

		public static IServiceCollection AddTypeMapping(this IServiceCollection services, ILibraryManager libraryManager)
		{
			services.AddSingleton<IMapper, AutoMapperAdapter>();
            services.AddSingleton<IAutoMapperConfiguratorProvider, DefaultAutoMapperConfiguratorProvider>();

            var libraries = libraryManager.GetReferencingLibraries(_libraryName)
                .Distinct()
                .Where(x => x.Name != _libraryName);

            foreach (var library in libraries)
            {
                foreach (var assemblyName in library.Assemblies)
                {
                    var assembly = Assembly.Load(assemblyName);

                    foreach (var type in assembly.DefinedTypes)
                    {
                        if (typeof(IValueResolver).GetTypeInfo().IsAssignableFrom(type) && type.IsClass)
                        {
                            services.AddSingleton(type.AsType());
                        }
                    }
                }
            }
            

            return services;
		}
	}
}