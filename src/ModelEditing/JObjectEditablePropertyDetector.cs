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

        public bool CanHandleFragmentType(Type type)
        {
            return type == typeof(JObject);
        }

        public IEnumerable<string> GetNamesOfProperties<T>(object fragment)
        {
            var propertyInfos = typeof(T).GetProperties();
            var modifiablePropertyInfos = propertyInfos.Where(x =>
            {
                var editableAttribute = x.GetCustomAttributes(typeof(EditableAttribute), false).FirstOrDefault() as EditableAttribute;

                return editableAttribute != null && editableAttribute.AllowEdit;
            });

            var namesOfFragmentProperties = ((JObject)fragment).Properties().Select(x => x.Name);
            var namesOfEditableProperties = modifiablePropertyInfos.Select(x => x.Name)
                .Where(x => namesOfFragmentProperties.Contains(x, StringComparer.OrdinalIgnoreCase));

            return namesOfEditableProperties;
        }

        public Dictionary<string, object> GetPropertyValueMap<TFragment, TTarget>(object dtoFragment)
        {
            var namesOfModifiedDtoProperties = GetNamesOfProperties<TFragment>(dtoFragment).ToList();
            TTarget entity = _mapper.MapTo<TTarget>(((JObject)dtoFragment).ToObject<TFragment>());
            var entityPropertyInfos = typeof(TTarget).GetProperties();
            var map = new Dictionary<string, object>();

            foreach (string dtoPropertyName in namesOfModifiedDtoProperties)
            {
                string entityPropertyName = _mapper.GetDestinationPropertyName(typeof(TFragment), typeof(TTarget), dtoPropertyName);

                var value = entityPropertyInfos.Single(x => x.Name == entityPropertyName).GetValue(entity);
                map.Add(entityPropertyName, value);
            }

            return map;
        }
    }
}
