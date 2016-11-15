using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Mvc;

namespace MProjectWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddCaching(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(
                //options =>
                //{
                //    options.IdleTimeout = TimeSpan.FromMinutes(30);
                //    options.CookieName = ".MyApplication";
                //    //options.CookieName = ".AdventureWorks.Session";
                //    //options.IdleTimeout = TimeSpan.FromSeconds(10);
                //}
                );

           // services.AddEntityFramework()
           //.AddSqlite();

            services.AddEntityFramework()
                .AddNpgsql();
            
            services.AddMvc();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseSession();
            app.UseDeveloperExceptionPage();
            //app.UseDirectoryBrowser();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Index}/{action=index}/{id?}"
                    );

            });
            app.UseCors("AllowSpecificOrigin");
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
