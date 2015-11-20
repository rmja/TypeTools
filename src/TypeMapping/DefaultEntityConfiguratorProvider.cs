using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace TypeMapping
{
    public class DefaultAutoMapperConfiguratorProvider : IAutoMapperConfiguratorProvider
    {
        private static string _libraryName = "TypeMapping";
        private readonly ILibraryManager _libraryManager;

        public DefaultAutoMapperConfiguratorProvider(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        public IEnumerable<TypeInfo> AutoMapperConfiguratorTypes
        {
            get
            {
                var assemblies = GetCandidateLibraries()
                    .SelectMany(x => x.Assemblies)
                    .Select(Load);

                var types = assemblies.SelectMany(x => x.DefinedTypes);

                return types.Where(IsEntityConfigurator);
            }
        }

        private IEnumerable<Library> GetCandidateLibraries()
        {
            return _libraryManager.GetReferencingLibraries(_libraryName)
                .Distinct()
                .Where(IsCandidateLibrary);
        }

        private static Assembly Load(AssemblyName assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        private bool IsCandidateLibrary(Library library)
        {
            return library.Name != _libraryName;
        }

        private bool IsEntityConfigurator(TypeInfo typeInfo)
        {
            return typeof(IAutoMapperConfigurator).GetTypeInfo().IsAssignableFrom(typeInfo) && typeInfo.IsClass;
        }
    }
}
