using System.Collections.Generic;
using System.IO;
using System.Linq;
using TypescriptGenerator.Model;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Writers.ServiceWriter
{
    internal class Angular2ControllerWriter: BaseControllerWriter
    {
        public override void WriteControllerModel(ControllerModel controllerModel, string module,
            Dictionary<string, string> knownTypesModule, string outputDir)
        {
            var writer = new CodeWriter();

            WriteHeader(writer);
            var usedTypes = controllerModel.GetUsedClasses();
            UsedTypesWriter.Write(writer, usedTypes, knownTypesModule, module);
            WriteGlobal(writer);
            WriteService(writer, controllerModel, module, knownTypesModule);

            writer.SaveToFile(Path.Combine(outputDir, GetServiceName(controllerModel.Name) + ".ts"));
        }

        private void WriteService(CodeWriter writer, ControllerModel controllerModel, string module, Dictionary<string, string> knownTypesModule)
        {
            writer.AddLine("@Injectable()");
            writer.AddLine($"export class {GetServiceName(controllerModel.Name)}");
            using (writer.CreateBlock())
            {
                WriteClassHeader(writer);
                WriteUrlBuilder(writer, module, controllerModel.Methods, knownTypesModule);
                foreach (var methodModel in controllerModel.Methods)
                {
                    WriteMethod(writer, methodModel, knownTypesModule);
                }
                WriteOptionBuilder(writer);
            }
        }

        private void WriteOptionBuilder(CodeWriter writer)
        {
            writer.AddLine("private _buildOptions()");
            using (writer.CreateBlock())
            {
                writer.AddLine("let headers = new Headers({ 'Content-Type': 'application/json; charset=utf-8' });");
                writer.AddLine("return new RequestOptions({headers: headers});");
            }
        }

        private static void WriteMethod(CodeWriter writer, MethodModel methodModel, Dictionary<string, string> knownTypesModule)
        {
            var methodName = GetMethodName(methodModel);
            var line = $"{methodName}({BuildParams(methodModel, knownTypesModule)})";
            if (methodModel.ReturnType != null)
                line = line + $": Observable<{TypeUtils.GetTsTypeName(methodModel.ReturnType, knownTypesModule, false)}>";
            writer.AddLine(line);
            using (writer.CreateBlock())
            {
                writer.AddLine($"var url = this.urlBuilder.{methodName}({BuildCallParams(methodModel)});");
                var method = methodModel.HttpMethod.ToString().ToLower();
                line = $"return this._http.{method}(url";
                line = $"{line} {AddBodySuffix(methodModel)}, this._buildOptions())";
                if (methodModel.ReturnType != null)
                {
                    writer.AddLine(line);
                    writer.AddLine("    .map(res => res.json());");
                }
                else
                {
                    line = line + ";";
                    writer.AddLine(line);
                }
            }
        }

        private static void WriteClassHeader(CodeWriter writer)
        {
            writer.AddLine("constructor(private _http: Http) {");
            writer.AddLine("}");
        }


        private static void WriteGlobal(CodeWriter writer)
        {
            writer.AddLine("declare var Global;");
            writer.AddLine("");
        }

        private static void WriteHeader(CodeWriter writer)
        {
            writer.AddLine("import {Injectable} from '@angular/core';");
            writer.AddLine("import {Http, Headers, RequestOptions} from '@angular/http';");
            writer.AddLine("import 'rxjs/add/operator/map';");
            writer.AddLine("import {Observable} from 'rxjs/Observable';");
            writer.WriteLine();
        }

        protected override string GetServiceName(string name)
        {
            return name.Replace("Controller", "Service");
        }

        private static string AddBodySuffix(MethodModel methodModel)
        {
            if (methodModel.NeedsBody)
            {
                var postParam = methodModel.Parameters.FirstOrDefault(p => p.ParameterType == ParameterType.Body);
                return postParam != null ? $", JSON.stringify({postParam.Name})" : ",''";
            }
            return "";
        }

    }
}