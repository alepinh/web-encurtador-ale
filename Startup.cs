using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace web_encurtador_ale
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
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();

            // Add LiteDB
            services.AddSingleton<ILiteDatabase, LiteDatabase>(_ => new LiteDatabase("short-links.db"));



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapPost("/shorten", HandleShortenUrl);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Shortener}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "Shorten",
                                pattern: "{controller=Shorten}/{action=Shortening}/{id?}");

                endpoints.MapFallback(HandleRedirect);
            });
        }


        static Task HandleRedirect(HttpContext context)
        {
            var db = context.RequestServices.GetService<ILiteDatabase>();
            var collection = db.GetCollection<ShortLink>();

            var path = context.Request.Path.ToUriComponent().Trim('/');

            var id = new ShortLink().GetId(path);
            var entry = collection.Find(p => p.Id == id).FirstOrDefault();

            if (entry != null)
                context.Response.Redirect(entry.Url);
            else
                context.Response.Redirect("/");

            return Task.CompletedTask;
        }

    }
}
