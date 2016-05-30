using System.Collections.Generic;
using System.Reflection;
using TypescriptModel.Common;

namespace TypescriptModel.Poco
{
    public class ModelProperty
    {
        public ModelProperty(PropertyInfo propertyInfo, Dictionary<string, string> knownTypesModule)
        {
            PropertyName = propertyInfo.Name;
            ClassName = TypeUtils.GetTsTypeName(propertyInfo.PropertyType, knownTypesModule, false);
        }

        public string PropertyName { get; }
        public string ClassName { get;  }
    }
}