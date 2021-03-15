using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
            // json objelerinin d�n��t�rlmesi i�in addjsonoptions geldi
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=> {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                //i� i�e olan objeler birbirini referans etti�inde sorun olmu�mamas� i�in ekledik.
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            services.AddSession();

            //derlenme esnas�nda automapper'in burada olan s�n�flar� taramas�n� sa�l�yoruz.
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile));

            //servis katman� ile mvc katman� aras�naki ba�lant�
            services.LoadMyService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //BULUNAMAYAN B�R SAYFAYA G�TT���NDE 404 HATASI VER
                app.UseStatusCodePages();
            }

            //session'un yeri de a�a��daki sebepten �t�r� �nemli
            app.UseSession();

            //static dosyalar� kullan
            app.UseStaticFiles();
            app.UseRouting();

//routing yap�ld�ktan(kullan�nc�n�n nereye gitmek istedi�ini ��rendikten) sonra authentication ve authorization kontrollerinin yap�lmas� gerekiyor daha �nce yapamay�z.
            app.UseAuthentication();
            app.UseAuthorization();


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
