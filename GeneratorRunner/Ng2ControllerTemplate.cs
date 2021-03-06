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
    
    #line 1 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class Ng2ControllerTemplate : ControllerTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("import {Injectable} from \'@angular/core\';\r\nimport {Http, Headers, RequestOptions}" +
                    " from \'@angular/http\';\r\nimport \'rxjs/add/operator/map\';\r\nimport {Observable} fro" +
                    "m \'rxjs/Observable\';\r\n\r\n");
            
            #line 9 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 ImportUsedClasses(Model.UsedClasses, Model.Module); 
            
            #line default
            #line hidden
            this.Write("declare var Global;\r\n\r\n@Injectable()\r\nexport class ");
            
            #line 13 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name.Replace("Controller", "Service")));
            
            #line default
            #line hidden
            this.Write(" {\r\n    constructor(private _http: Http) {\r\n    }\r\n");
            
            #line 16 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 WriteUrlBuilder(); 
            
            #line default
            #line hidden
            
            #line 17 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 foreach (var method in Model.Methods) {
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 18 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetMethodName(method)));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 18 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BuildParams(method)));
            
            #line default
            #line hidden
            this.Write(")");
            
            #line 18 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 if(method.ReturnType != null) Write($": Observable<{method.TsReturnType}>"); 
            
            #line default
            #line hidden
            this.Write(" {\r\n        var url = this.urlBuilder.");
            
            #line 19 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetMethodName(method)));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 19 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(BuildCallParams(method)));
            
            #line default
            #line hidden
            this.Write(");\r\n        return this._http.");
            
            #line 20 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.HttpMethod.ToString().ToLower()));
            
            #line default
            #line hidden
            this.Write("(url, ");
            
            #line 20 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 
if (method.NeedsBody)
{
    var postParam = method.Parameters.FirstOrDefault(p => p.ParameterType == ParameterType.Body);
    Write(postParam != null ? $"JSON.stringify({postParam.Name}), " : "'', ");
}
		
            
            #line default
            #line hidden
            this.Write("this._buildOptions())");
            
            #line 26 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 if (method.ReturnType != null) {
            
            #line default
            #line hidden
            this.Write("\r\n            .map(res => res.json())");
            
            #line 28 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write(";\r\n    }\r\n");
            
            #line 30 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\Ng2ControllerTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("    private _buildOptions() {\r\n        let headers = new Headers({ \'Content-Type\'" +
                    ": \'application/json; charset=utf-8\' });\r\n        return new RequestOptions({head" +
                    "ers: headers});\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
