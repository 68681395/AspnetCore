using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using WebApplication9;
using Microsoft.AspNetCore.Http;
using ClassLibrary1.Extension;

namespace WebApplication1
{
    public class Startup
    {
        private string applicationBasePath;
        


        public Startup(IHostingEnvironment env)
        {
            this.applicationBasePath = env.WebRootPath;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IEnumerable<Assembly> assemblies = Enumerable.Empty<Assembly>();
          //  AssemblyManager.LoadAssemblies(this.applicationBasePath.Substring(0, this.applicationBasePath.LastIndexOf("src")) + "artifacts\\bin\\");
           
            ExtensionManager.SetAssemblies(assemblies);
            foreach (IExtension extension in ExtensionManager.Extensions)
                extension.ConfigureServices(services);

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddPrecompiledRazorViews(ExtensionManager.Assemblies.ToArray())
                .AddRazorOptions(
                    razorOptions =>
                        {
                            razorOptions.FileProviders.Add(GetFileProvider(assemblies, this.applicationBasePath));
                        });


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                foreach (IExtension extension in ExtensionManager.Extensions)
                    extension.RegisterRoutes(routes);
            });



        }
        public IFileProvider GetFileProvider(IEnumerable<Assembly> assemblies, string path)
        {
            IEnumerable<IFileProvider> fileProviders = new IFileProvider[] { new PhysicalFileProvider(path) };

            return new Microsoft.Extensions.FileProviders.CompositeFileProvider(
              fileProviders.Concat(
                assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
              )
            );
        }
    }
}
