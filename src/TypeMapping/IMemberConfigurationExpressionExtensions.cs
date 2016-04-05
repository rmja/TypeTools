using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class IMemberConfigurationExpressionExtensions
    {
        public static IMemberConfigurationExpression<T> UseSideInformation<T>(this IMemberConfigurationExpression<T> opts, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            opts.ResolveUsing(ctx => ctx.Context.Options.Items[key]);
            return opts;
        }

        public static IMemberConfigurationExpression<T> MapFromSideInformation<T, TSideInformation>(this IMemberConfigurationExpression<T> opts, string key, Func<TSideInformation, object> selector)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            opts.ResolveUsing(ctx => selector((TSideInformation)ctx.Context.Options.Items[key]));
            return opts;
        }

        public static IMemberConfigurationExpression<TSource> MapFromSideInformation<TSource, TSideInformation, TDestination>(this IMemberConfigurationExpression<TSource> opts, string key, Func<TSideInformation, TSource, TDestination> selector)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Argument is null or empty", "key");

            opts.ResolveUsing(ctx => selector((TSideInformation)ctx.Context.Options.Items[key], (TSource)ctx.Context.SourceValue));
            return opts;
        }
    }
}
