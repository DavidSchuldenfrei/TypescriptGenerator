using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace TypescriptGenerator.Model
{
    public class ParameterModel
    {
        public string Name { get; }
        public Type Type { get; }
        public ParameterType ParameterType { get; }
        public bool IsComplex { get; }
        public ParameterModel(MethodModel methodModel, ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;
            Type = parameterInfo.ParameterType;
            IsComplex = Type.IsClass && !(Type == typeof (string));
            ParameterType = GetParameterType(methodModel, parameterInfo);
        }

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

    public enum ParameterType
    {
        Route, QueryString, Body
    }
}