using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using TypeMapping;

namespace ModelEditing
{
    public class JObjectEditablePropertyDetector : IEditablePropertyDetector
    {
        private readonly IMapper _mapper;

        public JObjectEditablePropertyDetector(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<string> GetNamesOfProperties<T>(object fragment, bool allowRequired)
        {
            var propertyInfos = typeof(T).GetProperties();
            var modifiablePropertyInfos = propertyInfos.Where(x =>
            {
                if (allowRequired)
                {
                    if (x.GetCustomAttributes(typeof(RequiredAttribute), false).Any())
                    {
                        return true;
                    }
                }

                var editableAttribute = x.GetCustomAttributes(typeof(EditableAttribute), false).FirstOrDefault() as EditableAttribute;

                return editableAttribute != null && editableAttribute.AllowEdit;
            });

            var namesOfFragmentProperties = ((JObject)fragment).Properties().Select(x => x.Name);
            var namesOfEditableProperties = modifiablePropertyInfos.Select(x => x.Name)
                .Where(x => namesOfFragmentProperties.Contains(x, StringComparer.OrdinalIgnoreCase));

            return namesOfEditableProperties;
        }

        public Dictionary<string, object> GetPropertyValueMap<TFragment, TTarget>(object fragment, bool allowRequired)
        {
            var namesOfModifiedFragmentProperties = GetNamesOfProperties<TFragment>(fragment, allowRequired).ToList();
            var entityPropertyInfos = typeof(TTarget).GetProperties();
            var fragmentJProperties = ((JObject)fragment).Properties().ToDictionary(x => x.Name, x => x);
            var map = new Dictionary<string, object>();

            foreach (var entityPropertyInfo in entityPropertyInfos)
            {
                string fragmentPropertyName = _mapper.GetDestinationPropertyName(typeof(TTarget), typeof(TFragment), entityPropertyInfo.Name);

                if (fragmentPropertyName != null)
                {
                    if (namesOfModifiedFragmentProperties.Contains(fragmentPropertyName))
                    {
                        var fragmentJProperty = fragmentJProperties[fragmentPropertyName];
                        var value = ((JValue)fragmentJProperty.Value).Value;
                        map.Add(entityPropertyInfo.Name, value);
                    }
                }
            }
            
            return map;
        }
    }
}
