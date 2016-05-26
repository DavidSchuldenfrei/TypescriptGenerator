using System.Collections.Generic;
using System.IO;
using System.Linq;
using TypescriptGenerator.Model;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Writers.ServiceWriter
{
    class Angular1ControllerWriter : BaseControllerWriter
    {
        public override void WriteControllerModel(ControllerModel controllerModel, string module,
            Dictionary<string, string> knownTypesModule, string outputDir)
        {
            var writer = new CodeWriter();

            var usedTypes = controllerModel.GetUsedClasses();
            UsedTypesWriter.Write(writer, usedTypes, knownTypesModule, module);

            WriteService(writer, controllerModel, module, knownTypesModule);

            writer.SaveToFile(Path.Combine(outputDir, GetServiceName(controllerModel.Name) + ".ts"));

        }

        private void WriteService(CodeWriter writer, ControllerModel controllerModel, string module,
            Dictionary<string, string> knownTypesModule)
        {
            writer.AddLine($"export class {GetServiceName(controllerModel.Name)}");

            using (writer.CreateBlock())
            {
                writer.AddLine("static $inject = ['$http', '$q'];");
                writer.AddLine("constructor(private $http: ng.IHttpService, private $q: ng.IQService)");
                using (writer.CreateBlock()) { }
                WriteUrlBuilder(writer, module, controllerModel.Methods, knownTypesModule);
                foreach (var methodModel in controllerModel.Methods)
                {
                    WriteMethod(writer, methodModel, knownTypesModule);
                }

            }
        }

        private void WriteMethod(CodeWriter writer, MethodModel methodModel, Dictionary<string, string> knownTypesModule)
        {
            var methodName = GetMethodName(methodModel);
            var line = $"{methodName}({BuildParams(methodModel, knownTypesModule)})";
            if (methodModel.ReturnType != null)
                line = line + $": ng.Promise<{TypeUtils.GetTsTypeName(methodModel.ReturnType, knownTypesModule, false)}>";
            writer.AddLine(line);
            using (writer.CreateBlock())
            {
                writer.AddLine($"var url = this.urlBuilder.{methodName}({BuildCallParams(methodModel)});");
                writer.AddLine("var defer = this.$q.defer();");
                line = $"this.$http.{methodModel.HttpMethod.ToString().ToLower()}(url";
                writer.AddLine($"{line}{AddBodySuffix(methodModel)})");
                writer.AddLine(".success(function(data)");
                using (writer.CreateBlock(");"))
                {
                    writer.AddLine("return defer.resolve();");
                }
                writer.AddLine("return defer.promise;");
            }
        }

        protected override string GetServiceName(string name)
        {
            return name.Replace("Controller", "Api");
        }

        private static string AddBodySuffix(MethodModel methodModel)
        {
            if (methodModel.NeedsBody)
            {
                var postParam = methodModel.Parameters.FirstOrDefault(p => p.ParameterType == ParameterType.Body);
                return postParam != null ? $", {postParam.Name}" : ",''";
            }
            return "";
        }
    }
}
