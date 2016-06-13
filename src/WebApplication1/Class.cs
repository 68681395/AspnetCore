using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;

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
