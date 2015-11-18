using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModelEditing
{
    public class PropertyUpdater : IPropertyUpdater
    {
        private readonly IEditablePropertyDetector _editablePropertyDetector;

        public PropertyUpdater(IEditablePropertyDetector editablePropertyDetector)
        {
            _editablePropertyDetector = editablePropertyDetector;
        }

        public void UpdateProperties<TFragment, TTarget>(TTarget target, object fragment)
        {
            var updatedPropertyValueMap = _editablePropertyDetector.GetPropertyValueMap<TFragment, TTarget>(fragment);

            var propertyInfos = typeof(TTarget).GetProperties();
            foreach (var pair in updatedPropertyValueMap)
            {
                propertyInfos.Single(x => x.Name == pair.Key).SetValue(target, pair.Value);
            }
        }
    }
}
