using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModelEditing
{
    public class PropertyUpdater : IPropertyUpdater
    {
        private IEditablePropertyDetector _editablePropertyDetector;

        public PropertyUpdater(IEditablePropertyDetector editablePropertyDetector)
        {
            _editablePropertyDetector = editablePropertyDetector;
        }

        public TTarget CreateFromProperties<TTarget>(object fragment)
            where TTarget : new()
        {
            if (!(fragment is JObject))
            {
                fragment = JObject.FromObject(fragment);
            }

            var updatedPropertyValueMap = _editablePropertyDetector.GetPropertyValueMap<TTarget, TTarget>(fragment, true);

            var target = new TTarget();
            var propertyInfos = typeof(TTarget).GetProperties();
            foreach (var pair in updatedPropertyValueMap)
            {
                var propertyInfo = propertyInfos.Single(x => x.Name == pair.Key);
                if (propertyInfo.GetSetMethod() != null)
                {
                    var targetType = propertyInfo.PropertyType;
                    var value = pair.Value;

                    if (value is string && !string.IsNullOrEmpty((string)value))
                    {
                        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            targetType = Nullable.GetUnderlyingType(targetType);
                        }

                        if (targetType.IsEnum)
                        {
                            value = Enum.Parse(targetType, (string)value, true);
                        }
                    }

                    propertyInfo.SetValue(target, value);
                }
            }

            return target;
        }

        public void UpdateProperties<TFragment, TTarget>(TTarget target, object fragment)
        {
            if (!(fragment is JObject))
            {
                fragment = JObject.FromObject(fragment);
            }

            var updatedPropertyValueMap = _editablePropertyDetector.GetPropertyValueMap<TFragment, TTarget>(fragment, false);

            var propertyInfos = typeof(TTarget).GetProperties();
            foreach (var pair in updatedPropertyValueMap)
            {
                var propertyInfo = propertyInfos.Single(x => x.Name == pair.Key);
                if (propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(target, pair.Value);
                }
            }
        }
    }
}
