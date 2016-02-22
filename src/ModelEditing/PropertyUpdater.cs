using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModelEditing
{
    public class PropertyUpdater : IPropertyUpdater
    {
        private IEnumerable<IEditablePropertyDetector> _editablePropertyDetectors;

        public PropertyUpdater(IEnumerable<IEditablePropertyDetector> editablePropertyDetectors)
        {
            _editablePropertyDetectors = editablePropertyDetectors;
        }

        public void UpdateProperties<TFragment, TTarget>(TTarget target, object fragment)
        {
            var editablePropertyDetector = GetDetector(fragment.GetType());

            var updatedPropertyValueMap = editablePropertyDetector.GetPropertyValueMap<TFragment, TTarget>(fragment);

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

        public IEditablePropertyDetector GetDetector(Type fragmentType)
        {
            return _editablePropertyDetectors.First(x => x.CanHandleFragmentType(fragmentType));
        }
    }
}
