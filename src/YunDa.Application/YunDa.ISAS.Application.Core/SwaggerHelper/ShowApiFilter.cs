using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace YunDa.ISAS.Application.Core.SwaggerHelper
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public partial class ShowApiAttribute : Attribute { }

    public class ShowApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null || context == null) return;
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.CustomAttributes().OfType<ShowApiAttribute>().Any())
                    continue;
                var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                if (swaggerDoc.Paths.ContainsKey(key))
                    swaggerDoc.Paths.Remove(key);
            }
        }
    }
}