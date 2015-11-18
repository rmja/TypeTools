using ModelEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModelEditing(this IServiceCollection services)
        {
            services.AddSingleton<IEditablePropertyDetector, JObjectEditablePropertyDetector>();
            services.AddSingleton<IPropertyUpdater, PropertyUpdater>();

            return services;
        }
    }
}
