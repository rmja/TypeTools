using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TypeMapping
{
    public class AutoMapperConfiguratorTypeProvider
    {
        public IEnumerable<TypeInfo> AutoMapperConfiguratorTypes
        {
            get
            {
                var assemblies = LoadCandidateAssemblies();
                var types = assemblies.SelectMany(x => x.DefinedTypes);

                return types.Where(x => x.IsAutoMapperConfigurator());
            }
        }

        public IEnumerable<TypeInfo> ValueResolverTypes
        {
            get
            {
                var assemblies = LoadCandidateAssemblies();
                var types = assemblies.SelectMany(x => x.DefinedTypes);

                return types.Where(x => x.IsAutoMapperValueResolver());
            }
        }

        public IEnumerable<TypeInfo> TypeConverterTypes
        {
            get
            {
                var assemblies = LoadCandidateAssemblies();
                var types = assemblies.SelectMany(x => x.DefinedTypes);

                return types.Where(x => x.IsAutoMapperTypeConverter());
            }
        }

        //private IEnumerable<RuntimeLibrary> GetCandidateLibraries()
        //{
        //    return DependencyContext.Default.RuntimeLibraries.Distinct();
        //}

        //private IEnumerable<Assembly> LoadCandidateAssemblies()
        //{
        //    return GetCandidateLibraries()
        //        .SelectMany(x => x.Assemblies)
        //        .Select(x => Assembly.Load(x.Name));
        //}

        private IEnumerable<Assembly> LoadCandidateAssemblies()
        {
            return DnxPlatformServices.Default.LibraryManager.GetReferencingLibraries("TypeMapping")
                .SelectMany(x => x.Assemblies)
                .Select(x => Assembly.Load(x));
        }
    }
}
