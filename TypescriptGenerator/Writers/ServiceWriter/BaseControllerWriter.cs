using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypescriptGenerator.Model;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Writers.ServiceWriter
{
    internal abstract class BaseControllerWriter
    {
        public abstract void WriteControllerModel(ControllerModel controllerModel, string module,
            Dictionary<string, string> knownTypesModule, string outputDir);
        protected abstract string GetServiceName(string name);

        protected static string BuildCallParams(MethodModel methodModel)
        {
            var sb = new StringBuilder();
            foreach (var parameterInfo in methodModel.Parameters)
            {
                sb.Append($"{parameterInfo.Name}, ");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }


        protected static void WriteUrlBuilder(CodeWriter writer, string module, List<MethodModel> methods, Dictionary<string, string> knownTypesModule)
        {
            writer.AddLine("urlBuilder =");
            using (writer.CreateBlock())
            {
                foreach (var methodModel in methods)
                {
                    WriteUrlMethodBuilder(writer, methodModel, knownTypesModule);
                }
                writer.AddLine($"_domain: Global.host + Global.{module},");
                WriteUrlClassFooter(writer);
            }
        }

        protected static string GetMethodName(MethodModel methodModel)
        {
            var name = methodModel.Name.ToLower();
            if (name == "get")
                return "getBy" + StringUtils.GetPascalCase(methodModel.Parameters.FirstOrDefault()?.Name);
            if (name == "post")
                return "post" + StringUtils.GetPascalCase(methodModel.Parameters.FirstOrDefault()?.Name);
            if (name == "put")
            {
                var parameter = methodModel.Parameters.FirstOrDefault();
                if (methodModel.Parameters.Count > 1 && parameter?.Name.ToLower() == "id")
                    parameter = methodModel.Parameters[1];
                return "put" + StringUtils.GetPascalCase(parameter?.Name);
            }
            return StringUtils.GetCamelCase(methodModel.Name);
        }

        protected static string BuildParams(MethodModel methodModel, Dictionary<string, string> knownTypesModule)
        {
            var sb = new StringBuilder();
            foreach (var parameterInfo in methodModel.Parameters)
            {
                sb.Append($"{parameterInfo.Name}: {TypeUtils.GetTsTypeName(parameterInfo.Type, knownTypesModule, false)}, ");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        private static void WriteUrlMethodBuilder(CodeWriter writer, MethodModel methodModel, Dictionary<string, string> knownTypesModule)
        {
            writer.AddLine($"{GetMethodName(methodModel)}({BuildParams(methodModel, knownTypesModule)})");
            using (writer.CreateBlock(","))
            {
                writer.AddLine($"var url = {FixRouteParameters(methodModel.Route)};");
                writer.AddLine("var params = '';");
                AddParameters(writer, methodModel);
                writer.AddLine("return this._getUrl(url, params);");
            }
        }

        private static void AddParameters(CodeWriter writer, MethodModel methodModel)
        {
            var parameterInfos =
                methodModel.Parameters.Where(p => p.ParameterType == ParameterType.QueryString).ToList();
            if (parameterInfos.Count == 0) return;

            foreach (var parameterInfo in parameterInfos)
            {
                AddParameter(writer, parameterInfo);
            }
        }


        private static void AddParameter(CodeWriter writer, ParameterModel parameterInfo)
        {
            var paramName = parameterInfo.Name;
            writer.AddLine(parameterInfo.IsComplex
                ? $"params = params + this._addParam({paramName});"
                : $"params = params + '{paramName}=' + this._encode({paramName}) + '&';");
        }

        private static string FixRouteParameters(string route)
        {
            var result = "'" + route.Replace("{", "' + ").Replace("}", " + '");
            if (result.EndsWith(" + '"))
                result = result.Substring(0, result.Length - 3);
            else
                result = result + "'";
            return result;
        }

        private static void WriteUrlClassFooter(CodeWriter writer)
        {
            writer.AddLine("_addParam(p: any)");
            using (writer.CreateBlock(","))
            {
                writer.AddLine("var result = '';");
                writer.AddLine("if (p)");
                using (writer.CreateBlock())
                {
                    writer.AddLine("for (var propertyName in p)");
                    using (writer.CreateBlock())
                    {
                        writer.AddLine("if (p[propertyName] && !(p[propertyName] instanceof Function) && ((p[propertyName] != '')))");
                        using (writer.CreateBlock())
                        {
                            writer.AddLine("result = result + propertyName + '=' + this._encode(p[propertyName]) + '&';");
                        }
                    }
                }
                writer.AddLine("return result;");
            }
            writer.AddLine("_encode(p: any)");
            using (writer.CreateBlock(","))
            {
                writer.AddLine("if (p instanceof Date)");
                writer.AddLine("    return p.getYears() + '-' + p.getMonths() + '-' + p.getDays();");
                writer.AddLine("else");
                writer.AddLine("    return encodeURIComponent(p);");
            }
            writer.AddLine("_getUrl(baseUrl: string, params: string)");
            using (writer.CreateBlock())
            {
                writer.AddLine("var result = this._domain + '/' + baseUrl");
                writer.AddLine("if (params.length > 0)");
                using (writer.CreateBlock())
                {
                    writer.AddLine("result = result + '?' + params.slice(0, -1);");
                }
                writer.AddLine("return result;");
            }
        }
    }
}
