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
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class ModelTemplate : ModelTextWriter
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 6 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
 foreach (var usedClass in Model.UsedClasses) { 
            
            #line default
            #line hidden
            this.Write("import {");
            
            #line 7 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(usedClass.Name));
            
            #line default
            #line hidden
            this.Write("} from \"");
            
            #line 7 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(usedClass.GetRelativePath(Model.Module)));
            
            #line default
            #line hidden
            
            #line 7 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(usedClass.FileName));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 8 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
 } 
            
            #line default
            #line hidden
            
            #line 9 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
 if (Model.UsedClasses.Any()) {
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 11 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("export class ");
            
            #line 12 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.TsName));
            
            #line default
            #line hidden
            this.Write(" {\r\n");
            
            #line 13 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
 foreach (var property in Model.Properties) { 
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 14 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyName));
            
            #line default
            #line hidden
            this.Write(": ");
            
            #line 14 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.ClassName));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 15 "C:\src\Credfi\Deploy\Tools\TypescriptGenerator\GeneratorRunner\ModelTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}