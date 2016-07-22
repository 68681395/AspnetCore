using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.Runtime.Loader;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {

           // AssemblyLoadContext.Default.Resolving += Default_Resolving;

           // using (TSharp.Core.Osgi.OsgiEngine.InitWebEngine())
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    //.UseEnvironment()
                    .UseStartup<Startup>()
                  // .ConfigureServices(s=> s.Add(  ServiceDescriptor.Scoped<IApplicationLifetime>()))
                  .CaptureStartupErrors(true)
                  .ConfigureServices(
                      s =>
                          {

                          })

                    .Build();
               
                host.Run();

            }
        }

        private static System.Reflection.Assembly Default_Resolving(AssemblyLoadContext arg1, System.Reflection.AssemblyName arg2)
        {
            return arg1.LoadFromAssemblyName(arg2);

        }
    }
}
