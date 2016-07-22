using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;

namespace TestConsole
{
    public class Program
    {

        //   private static IFileProvider provider;
        public static void Main(string[] args)
        {
            // provider = new PhysicalFileProvider(PlatformServices.Default.Application.ApplicationBasePath);

            //var token = provider.Watch("tfolder");

            //token.RegisterChangeCallback(
            //    x =>
            //        {
            //            Console.WriteLine(x.ToString() + token.HasChanged);
            //        }, "test state");


            IServiceCollection services = new ServiceCollection();
            services.AddScoped<ITodoRepository, TodoRepository>();
            Console.WriteLine(PlatformServices.Default.Application.ApplicationBasePath);


            var serviceProvider = services.BuildServiceProvider();

            var scoped = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();


            var sp = scoped.ServiceProvider;




            var key = Console.ReadLine();
            int i = 1;
            while (key != "quit")
            {
                var svr = sp.GetService<ITodoRepository>();
                svr.Add(new TodoItem());

                Console.WriteLine($"{i} scope Add 1,items:{svr.AllItems.Count()}");
                var topsvr = serviceProvider.GetService<ITodoRepository>();
                topsvr.Add(new TodoItem());

                Console.WriteLine($"{i} top Add 1,items:{topsvr.AllItems.Count()}");
                if (key == "d")
                {
                    scoped.Dispose();
                }
                else if (key == "c")
                {
                    i = 1;
                    scoped = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
                    sp = scoped.ServiceProvider;
                }
                //  scoped.Dispose();
                Console.WriteLine("=========");
                i = i + 1;
                key = Console.ReadLine();
            }
        }
    }
}
