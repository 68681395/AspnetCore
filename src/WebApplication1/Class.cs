using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace WebApplication9
{

    public static class IMvcBuilderExtension
    {
        public static IMvcBuilder AddPrecompiledRazorViews(this IMvcBuilder builder, IEnumerable<Assembly> assemblies)
        {
            if(assemblies !=null)
                foreach (var asm in assemblies)
                {
                    builder.AddApplicationPart(asm);
                    
                }

            return builder;
        }
    }
    
}
