using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //bir mvc katman� oldu�unu g�sterdik.
            //her �ny�z de�i�ikli�inde �ny�z� derlemek zorunda kalmamak i�in addrazorruntimecompiilation ekledik.
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //derlenme esnas�nda automapper'in burada olan s�n�flar� taramas�n� sa�l�yoruz.
            services.AddAutoMapper(typeof(Startup));

            //servis katman� ile mvc katman� aras�naki ba�lant�
            services.LoadMyService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //bulunamayan bir sayfaya gitti�inde 404 hatas� ver
                app.UseStatusCodePages();
            }

            //static dosyalar� kullan
            app.UseStaticFiles();


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //admin i�in route
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{Controller=Home}/{action=Index}/{id?}"
                    );


                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
