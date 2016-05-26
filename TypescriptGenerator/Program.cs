using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using TypescriptGenerator.Utils;
using TypescriptGenerator.Writers;

namespace TypescriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            var serializer = new DataContractJsonSerializer(typeof (Options));

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
                var moduleDir = StringUtils.GetFolder(options.OutFolder, assemblyModule.Module);
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


            var loadeds = new List<Tuple<AssemblyModule, Assembly>>();
            foreach (var assemblyConfig in options.Assemblies)
            {
                var assemblyPath = Path.Combine(options.BasePath, assemblyConfig.Path);
                var assembly = Assembly.LoadFrom(assemblyPath);
                loadeds.Add(new Tuple<AssemblyModule, Assembly>(assemblyConfig, assembly));
            }
            var knownTypesModule = new Dictionary<string, string>();
            foreach (var loaded in loadeds)
            {
                foreach (var knownType in AssemblyUtils.GetKnownTypes(loaded.Item2))
                {
                    knownTypesModule[knownType.GetFullName()] = loaded.Item1.Module;
                }
            }
            foreach (var loaded in loadeds)
            {
                foreach (JsFramework jsFramework in Enum.GetValues(typeof (JsFramework)))
                {
                    var outFolder = options.OutFolder + "\\" + jsFramework;
                    AssemblyWriter.WriteAssembly(loaded.Item2, loaded.Item1, knownTypesModule, jsFramework, outFolder);
                }
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

        private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            var location = Path.Combine(Path.GetDirectoryName(args.RequestingAssembly.Location) ?? "", assemblyName.Name + ".dll");
            return Assembly.LoadFrom(location);
        }

    }
}
