using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MainUI.Domain;
using MainUI.Middleware;
using MainUI.Services;
using MainUI.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MainUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<CompositePages>(Pages);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();


            app.UseMvc(routes =>
            {
                routes.MapRoute("mapped", "{controller}/{action}");
            });
            
            app.UseMiddleware<CompositeMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{*url}",
                    new
                    {
                        controller = "Template",
                        action = "Index"
                    });
            });
        }

        private static CompositePages Pages = new CompositePages(new[] {
            new CompositePage
            {
                Name = "Alo",
                MatchString = "/alo",
                BaseUrl = "http://localhost:8080",
                MenuItems = new MenuItem[0]
            },
            new CompositePage
            {
                Name = "Brb",
                MatchString = "/brb",
                BaseUrl = "http://localhost:3000",
                MenuItems = new MenuItem[0]
            },
            new CompositePage
            {
                Name = "Service1",
                MatchString = "/service1",
                BaseUrl = "http://localhost:3635/",
                MenuItems = new MenuItem[]
                {
                    new MenuItem
                    {
                        Name = "Home",
                        Url = "service1/Home/Index"
                    },
                    new MenuItem
                    {
                        Name = "About",
                        Url = "service1/Home/About"
                    },
                    new MenuItem
                    {
                    Name = "Contact",
                    Url = "service1/Home/Contact"
                    }
                }
            }
        });
    }
}
