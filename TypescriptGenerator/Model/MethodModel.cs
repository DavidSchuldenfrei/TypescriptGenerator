using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Model
{
    public class MethodModel
    {
        public string Name { get; }
        public Type ReturnType { get; }
        public string Route { get; }
        public HttpMethod HttpMethod { get; }
        public List<ParameterModel> Parameters { get; }

        public bool NeedsBody
            => HttpMethod == HttpMethod.Patch || HttpMethod == HttpMethod.Post || HttpMethod == HttpMethod.Put;
        public MethodModel(MethodInfo methodInfo, string controllerRoutePrefix)
        {
            Name = methodInfo.Name;
            ReturnType = GetReturnType(methodInfo);
            Route = GetRoute(methodInfo, controllerRoutePrefix);
            HttpMethod = GetMethod(methodInfo);
            Parameters = methodInfo.GetParameters()
                .Select(p => new ParameterModel(this, p))
                .ToList();
        }

        private Type GetReturnType(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType == typeof (void))
                return null;
            if (typeof (IHttpActionResult).IsAssignableFrom(methodInfo.ReturnType) ||
                typeof (HttpResponseMessage).IsAssignableFrom(methodInfo.ReturnType))
                return null;
            return methodInfo.ReturnType;
        }

        private string GetRoute(MethodInfo methodInfo, string controllerRoutePrefix)
        {
            var methodAttrib = methodInfo.GetCustomAttributes<RouteAttribute>().FirstOrDefault();
            if (methodAttrib != null)
            {
                var methodRoute = methodAttrib.Template;
                if (methodRoute.StartsWith("~/"))
                    return methodRoute.Substring(2);
                return controllerRoutePrefix + "/" + methodRoute;
            }
            if (methodInfo.GetParameters().Length == 1 && methodInfo.GetParameters()[0].Name == "id")
                return controllerRoutePrefix + "/{id}";
            return controllerRoutePrefix;
        }

        private static HttpMethod GetMethod(MethodInfo methodInfo)

        {
            if (methodInfo.GetCustomAttributes<HttpGetAttribute>().FirstOrDefault() != null)
                return HttpMethod.Get;
            if (methodInfo.GetCustomAttributes<HttpPostAttribute>().FirstOrDefault() != null)
                return HttpMethod.Post;
            if (methodInfo.GetCustomAttributes<HttpPutAttribute>().FirstOrDefault() != null)
                return HttpMethod.Put;
            if (methodInfo.GetCustomAttributes<HttpPatchAttribute>().FirstOrDefault() != null)
                return HttpMethod.Patch;
            if (methodInfo.GetCustomAttributes<HttpDeleteAttribute>().FirstOrDefault() != null)
                return HttpMethod.Delete;
            var name = methodInfo.Name.ToLower();
            if (name.StartsWith("get"))
                return HttpMethod.Get;
            if (name.StartsWith("post"))
                return HttpMethod.Post;
            if (name.StartsWith("put"))
                return HttpMethod.Put;
            if (name.StartsWith("patch"))
                return HttpMethod.Patch;
            if (name.StartsWith("delete"))
                return HttpMethod.Delete;
            return HttpMethod.Get;
        }

        public IEnumerable<Type> GetUsedClasses()
        {
            var result = Parameters.SelectMany(p => TypeUtils.GetUsedTypes(p.Type)).ToList();
            if (ReturnType != null)
                result.AddRange(TypeUtils.GetUsedTypes(ReturnType));
            return result;
        }
    }
}