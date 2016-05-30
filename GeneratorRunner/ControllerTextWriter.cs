using System.Linq;
using System.Text;
using TypescriptModel.Controller;

namespace GeneratorRunner
{
    public class ControllerTextWriter: BaseTextTemplate
    {
        public ControllerModel Model { get; set; }

        protected string GetMethodName(MethodModel methodModel)
        {
            var name = methodModel.Name.ToLower();
            if (name == "get")
                return "getBy" + GetPascalCase(methodModel.Parameters.FirstOrDefault()?.Name);
            if (name == "post")
                return "post" + GetPascalCase(methodModel.Parameters.FirstOrDefault()?.Name);
            if (name == "put")
            {
                var parameter = methodModel.Parameters.FirstOrDefault();
                if (methodModel.Parameters.Count > 1 && parameter?.Name.ToLower() == "id")
                    parameter = methodModel.Parameters[1];
                return "put" + GetPascalCase(parameter?.Name);
            }
            return GetCamelCase(methodModel.Name);
        }

        protected string BuildParams(MethodModel methodModel)
        {
            var sb = new StringBuilder();
            foreach (var parameterInfo in methodModel.Parameters)
            {
                sb.Append($"{parameterInfo.Name}: {parameterInfo.TsType}, ");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
        protected string BuildCallParams(MethodModel methodModel)
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

        protected string FixRouteParameters(string route)
        {
            var result = "'" + route.Replace("{", "' + ").Replace("}", " + '");
            if (result.EndsWith(" + '"))
                result = result.Substring(0, result.Length - 3);
            else
                result = result + "'";
            return result;
        }

        protected string GetUrlAdjust(ParameterModel p)
        {
            return p.IsComplex
                ? $"this._addParam({p.Name});"
                : $"'{p.Name}=' + this._encode({p.Name}) + '&';";
        }
    }
}
