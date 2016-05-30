using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using TypescriptModel.Common;
using TypescriptModel.Controller;
using TypescriptModel.Poco;

namespace TypescriptModel
{
    public class Builder
    {
        public Results Build(List<AssemblyModule> assemblyModules)
        {
            var knownTypesModule = new Dictionary<string, string>();
            var loadeds = new List<Tuple<AssemblyModule, Assembly>>();
            foreach (var assemblyConfig in assemblyModules)
            {
                var assembly = Assembly.LoadFrom(assemblyConfig.Path);
                loadeds.Add(new Tuple<AssemblyModule, Assembly>(assemblyConfig, assembly));
            }
            foreach (var loaded in loadeds)
            {
                foreach (var knownType in loaded.Item2.GetTypes())
                {
                    knownTypesModule[knownType.GetFullName()] = loaded.Item1.Module;
                }
            }

            var results = new Results();
            foreach (var loaded in loadeds)
            {
                foreach (var type in loaded.Item2.GetTypes())
                {
                    if (IsModelType(type) && !loaded.Item1.ShouldSkip(type))
                    {
                        results.ClassModels.Add(new ClassModel(type, loaded.Item1.Module, knownTypesModule));
                    }
                    if (typeof (ApiController).IsAssignableFrom(type))
                    {
                        results.ControllerModels.Add(new ControllerModel(type, loaded.Item1.Module, knownTypesModule));
                    }
                }
            }
            return results;
        }

        private static bool IsModelType(Type type)
        {
            return type.IsPublic && type.IsClass && !typeof(Attribute).IsAssignableFrom(type) && !(type.IsAbstract && type.IsSealed)
                && !typeof(ApiController).IsAssignableFrom(type);
        }

    }
}
