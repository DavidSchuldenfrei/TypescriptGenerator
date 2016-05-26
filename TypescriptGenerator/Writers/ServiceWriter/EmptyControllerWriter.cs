using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypescriptGenerator.Model;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Writers.ServiceWriter
{
    internal class EmptyControllerWriter : BaseControllerWriter
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

        protected override string GetServiceName(string name)
        {
            return name;
        }

        private void WriteService(CodeWriter writer, ControllerModel controllerModel, string module,
            Dictionary<string, string> knownTypesModule)
        {
            writer.AddLine($"export class {GetServiceName(controllerModel.Name)}");
            using (writer.CreateBlock())
            {
            }
        }
    }
}
