using TypeMapping;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
		public static IServiceCollection AddTypeMapping(this IServiceCollection services)
		{
            services.AddSingleton<IMapper, AutoMapperAdapter>();

            var typeProvider = new AutoMapperConfiguratorTypeProvider();
            foreach (var typeInfo in typeProvider.AutoMapperConfiguratorTypes)
            {
                services.AddSingleton(typeof(IAutoMapperConfigurator), typeInfo.AsType());
            }
            foreach (var typeInfo in typeProvider.ValueResolverTypes)
            {
                services.AddSingleton(typeInfo.AsType());
            }
            foreach (var typeInfo in typeProvider.TypeConverterTypes)
            {
                services.AddSingleton(typeInfo.AsType());
            }

            return services;
		}
	}
}