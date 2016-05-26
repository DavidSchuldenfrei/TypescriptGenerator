using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace TypescriptGenerator.Model
{
    public class WebModel
    {
        public List<ControllerModel> Controllers { get; }
        public WebModel(Assembly assembly)
        {
            Controllers = assembly
                .GetTypes()
                .Where(type => typeof (ApiController).IsAssignableFrom(type))
                .Select(type => new ControllerModel(type))
                .ToList();
        }
    }
}
