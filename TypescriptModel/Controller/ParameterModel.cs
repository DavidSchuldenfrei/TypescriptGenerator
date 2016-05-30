using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using TypescriptModel.Common;

namespace TypescriptModel.Controller
{
    public class ParameterModel
    {
        public ParameterModel(MethodModel methodModel, ParameterInfo parameterInfo, Dictionary<string, string> knownTypesModule)
        {
            Name = parameterInfo.Name;
            Type = parameterInfo.ParameterType;
            IsComplex = Type.IsClass && !(Type == typeof(string));
            ParameterType = GetParameterType(methodModel, parameterInfo);
            TsType = TypeUtils.GetTsTypeName(Type, knownTypesModule, false);
        }

        public Type Type { get; }
        public string TsType { get; }
        public string Name { get; }
        public ParameterType ParameterType { get; }
        public bool IsComplex { get; }

        private ParameterType GetParameterType(MethodModel methodModel, ParameterInfo parameterInfo)
        {
            var route = methodModel.Route;
            if (route.Contains("{" + parameterInfo.Name + "}"))
                return ParameterType.Route;
            if (parameterInfo.GetCustomAttributes<FromBodyAttribute>().FirstOrDefault() != null)
                return ParameterType.Body;
            if (parameterInfo.GetCustomAttributes<FromUriAttribute>().FirstOrDefault() != null)
                return ParameterType.QueryString;
            return IsComplex ? ParameterType.Body : ParameterType.QueryString;
        }

    }
}