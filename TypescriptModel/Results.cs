using System.Collections.Generic;
using TypescriptModel.Controller;
using TypescriptModel.Poco;

namespace TypescriptModel
{
    public class Results
    {
        public List<ClassModel> ClassModels { get; } = new List<ClassModel>();
        public List<ControllerModel> ControllerModels { get; } = new List<ControllerModel>();
    }
}
