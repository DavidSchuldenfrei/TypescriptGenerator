﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 14.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace GeneratorRunner
{
    using System.Linq;
    using TypescriptModel.Controller;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class Ng1ControllerTemplate : ControllerTextWriter
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 4 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 foreach (var usedClass in Model.UsedClasses) { 
            
            #line default
            #line hidden
            this.Write("import {");
            
            #line 5 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(usedClass.Name));
            
            #line default
            #line hidden
            this.Write("} from \"");
            
            #line 5 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(usedClass.GetRelativePath(Model.Module)));
            
            #line default
            #line hidden
            
            #line 5 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(usedClass.FileName));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 6 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            
            #line 7 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 if (Model.UsedClasses.Any()) {
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 9 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("declare var Global;\r\n\r\nexport class ");
            
            #line 12 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.Replace("Controller", "Api")));
            
            #line default
            #line hidden
            this.Write(" {\r\n    static $inject = [\'$http\', \'$q\'];\r\n    constructor(private $http: ng.IHtt" +
                    "pService, private $q: ng.IQService) {\r\n    }\r\n    urlBuilder = {\r\n");
            
            #line 17 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 foreach (var method in Model.Methods) {
            
            #line default
            #line hidden
            this.Write("        ");
            
            #line 18 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetMethodName(method)));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 18 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BuildParams(method)));
            
            #line default
            #line hidden
            this.Write(") {\r\n            var url = ");
            
            #line 19 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FixRouteParameters(method.Route)));
            
            #line default
            #line hidden
            this.Write(";\r\n            var params = \'\';\r\n");
            
            #line 21 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 foreach (var p in method.Parameters.Where(prm => prm.ParameterType == ParameterType.QueryString)) {
            
            #line default
            #line hidden
            this.Write("            params = params + ");
            
            #line 22 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetUrlAdjust(p)));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 23 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("            return this._getUrl(url, params);\r\n        },\r\n");
            
            #line 26 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n        _domain: Global.host + Global.");
            
            #line 28 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Module));
            
            #line default
            #line hidden
            this.Write(@",
        _addParam(p: any) {
            var result = '';
            if (p) {
                for (var propertyName in p) {
                    if (p[propertyName] && !(p[propertyName] instanceof Function) && ((p[propertyName] != ''))) {
                        result = result + propertyName + '=' + this._encode(p[propertyName]) + '&';
                    }
                }
            }
            return result;
        },
        _encode(p: any) {
            if (p instanceof Date)
                return p.getYears() + '-' + p.getMonths() + '-' + p.getDays();
            else
                return encodeURIComponent(p);
        },
        _getUrl(baseUrl: string, params: string) {
            var result = this._domain + '/' + baseUrl
            if (params.length > 0) {
                result = result + '?' + params.slice(0, -1);
            }
            return result;
        }
    }
");
            
            #line 54 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 foreach (var method in Model.Methods) {
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 55 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetMethodName(method)));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 55 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BuildParams(method)));
            
            #line default
            #line hidden
            this.Write(")");
            
            #line 55 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 if(method.ReturnType != null) Write($": ng.Promise<{method.TsReturnType}>"); 
            
            #line default
            #line hidden
            this.Write(" {\r\n        var url = this.urlBuilder.");
            
            #line 56 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetMethodName(method)));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 56 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BuildCallParams(method)));
            
            #line default
            #line hidden
            this.Write(");\r\n        var defer = this.$q.defer();\r\n        this.$http.");
            
            #line 58 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.HttpMethod.ToString().ToLower()));
            
            #line default
            #line hidden
            this.Write("(url");
            
            #line 58 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 
if (method.NeedsBody)
{
    var postParam = method.Parameters.FirstOrDefault(p => p.ParameterType == ParameterType.Body);
    Write(postParam != null ? $", {postParam.Name}" : ", ''");
}
        
            
            #line default
            #line hidden
            this.Write(")\r\n            .success(function(data) {\r\n                return defer.resolve();" +
                    "\r\n        });\r\n        return defer.promise;\r\n    }\t\r\n");
            
            #line 70 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng1ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
