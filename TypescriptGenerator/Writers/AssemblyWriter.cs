using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Http;
using TypescriptGenerator.Model;
using TypescriptGenerator.Utils;
using TypescriptGenerator.Writers.ServiceWriter;

namespace TypescriptGenerator.Writers
{
    internal static class AssemblyWriter
    {
        public static void WriteAssembly(Assembly assembly, AssemblyModule assemblyModule, Dictionary<string, string> knownTypesModule,
            JsFramework jsFramework, string output)
        {
            WriteModelAssembly(assembly, assemblyModule, knownTypesModule, output);
            WriteWebAssembly(assembly, assemblyModule, knownTypesModule, jsFramework, output);
        }

        private static void WriteModelAssembly(Assembly assembly, AssemblyModule assemblyModule, Dictionary<string, string> knownTypesModule, string output)
        {
            var folder = StringUtils.GetFolder(output, assemblyModule.Module);
            Directory.CreateDirectory(folder);
            var skipNamespaces = new HashSet<string>(assemblyModule.SkipNamespcase ?? new List<string>());
            var skipTypes = new HashSet<string>(assemblyModule.SkipTypes ?? new List<string>());
            foreach (var type in assembly.GetTypes())
            {
                if (IsModelType(type) && !assemblyModule.ShouldSkip(type))
                    TypeWriter.WriteType(type, assemblyModule.Module, knownTypesModule, folder);
            }
        }

        private static bool IsModelType(Type type)
        {
            return type.IsPublic && type.IsClass && !typeof(Attribute).IsAssignableFrom(type) && !(type.IsAbstract && type.IsSealed)
                && !typeof(ApiController).IsAssignableFrom(type);
        }

        private static void WriteWebAssembly(Assembly assembly, AssemblyModule assemblyModule, Dictionary<string, string> knownTypesModule,
            JsFramework jsFramework, string output)
        {
            var folder = StringUtils.GetFolder(output, assemblyModule.Module);
            Directory.CreateDirectory(folder);
            var webModel = new WebModel(assembly);
            foreach (var controllerModel in webModel.Controllers)
            {
                if (!assemblyModule.ShouldSkip(controllerModel.Type))
                    CreateControllerWriter(jsFramework).WriteControllerModel(controllerModel, assemblyModule.Module, 
                        knownTypesModule, folder);
            }
        }

        private static BaseControllerWriter CreateControllerWriter(JsFramework jsFramework)
        {
            switch (jsFramework)
            {
                case JsFramework.Angular1:
                    return new Angular1ControllerWriter();
                case JsFramework.Angular2:
                    return new Angular2ControllerWriter();
            }
            return new EmptyControllerWriter();
        }
    }
}
