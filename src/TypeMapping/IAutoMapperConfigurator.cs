using AutoMapper;

namespace TypeMapping
{
    public interface IAutoMapperConfigurator
    {
		void Configure(IMapperConfiguration configuration);
    }
}