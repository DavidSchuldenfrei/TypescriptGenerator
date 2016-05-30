using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TypescriptModel;

namespace GeneratorRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "typescript.generator.json";
            if (args.Length == 1)
            {
                fileName = args[0];
            }
            else if (args.Length > 1)
            {
                Console.WriteLine("Usage: TypescriptGenerator jsonFile");
                return;
            }
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Couldn't find file {fileName}.");
                return;
            }

            var options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(fileName));
            foreach (var assemblyModule in options.Assemblies)
            {
                var moduleDir = GetFolder(options.OutFolder, assemblyModule.Module);
                if (Directory.Exists(moduleDir))
                {
                    try
                    {
                        Directory.Delete(moduleDir, true);
                        RemoveEmptyParents(moduleDir);
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }
            }

            options.Assemblies.ForEach(am => am.Path = Path.Combine(options.BasePath, am.Path));
            var result = new Builder().Build(options.Assemblies);

            foreach (var classModel in result.ClassModels)
            {
                var template = new ModelTemplate { Model = classModel };
                var outTemplate = template.TransformText();
                var folder = GetFolder(options.OutFolder + "/Angular1", classModel.Module);
                Directory.CreateDirectory(folder);
                var outFile = Path.Combine(folder, classModel.FileName + ".ts");
                File.WriteAllText(outFile, outTemplate);
                folder = GetFolder(options.OutFolder + "/Angular2", classModel.Module);
                Directory.CreateDirectory(folder);
                outFile = Path.Combine(folder, classModel.FileName + ".ts");
                File.WriteAllText(outFile, outTemplate);
            }
            foreach (var controllerModel in result.ControllerModels)
            {
                var folder = GetFolder(options.OutFolder + "/Angular1", controllerModel.Module);
                Directory.CreateDirectory(folder);
                var template = new Ng1ControllerTemplate{ Model = controllerModel };
                var outFile = Path.Combine(folder, controllerModel.Name.Replace("Controller", "Api") + ".ts");
                File.WriteAllText(outFile, template.TransformText());
            }
            foreach (var controllerModel in result.ControllerModels)
            {
                var folder = GetFolder(options.OutFolder + "/Angular2", controllerModel.Module);
                Directory.CreateDirectory(folder);
                var template = new Ng2ControllerTemplate { Model = controllerModel };
                var outFile = Path.Combine(folder, controllerModel.Name.Replace("Controller", "Service") + ".ts");
                File.WriteAllText(outFile, template.TransformText());
            }
        }

        private static void RemoveEmptyParents(string directory)
        {
            var parentDirectory = Path.GetDirectoryName(directory) ?? "";
            if (Directory.Exists(parentDirectory) &&
                !Directory.EnumerateDirectories(parentDirectory).Any() &&
                !Directory.EnumerateFiles(parentDirectory).Any())
            {
                try
                {
                    Directory.Delete(parentDirectory, true);
                    RemoveEmptyParents(parentDirectory);
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        static string GetFolder(string outputFolder, string module)
        {
            var parts = module.Split('.');
            foreach (var part in parts)
            {
                outputFolder = Path.Combine(outputFolder, part);
            }
            return outputFolder;
        }
    }
}
