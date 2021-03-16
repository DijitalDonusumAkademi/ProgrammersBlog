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

            //session
            services.AddSession();

            //derlenme esnas�nda automapper'in burada olan s�n�flar� taramas�n� sa�l�yoruz.
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile));

            //servis katman� ile mvc katman� aras�naki ba�lant�
            services.LoadMyService();

            //cookie
            services.ConfigureApplicationCookie(options=> {

                //login yap�l�rken hangi sayfaya y�nlendirilece�im?
                //admin area user controller login action
                options.LoginPath = new PathString("/Admin/User/Login");

                options.LogoutPath = new PathString("/Admin/User/Logout");

                options.Cookie = new CookieBuilder {
                    Name = "ProgrammersBlog",
                    HttpOnly = true,   //g�venlik i�in true veriyoruz. cookie i�lemlerini sadece http �zerinden g�ndermesini sa�l�yoruz ve b�ylece javascript taraf�nda(�n y�z) kimse cookie bilgilerimize eri�emmei� oluyor.
                    SameSite = SameSiteMode.Strict,   //CSRF'yi �nlemek i�in, cookie ilgileri sadece benim sitemden gelirse kullan dmei� olduk.
                    SecurePolicy= CookieSecurePolicy.SameAsRequest, //http �zeirnden gelirse htttp, https �zerinden gelirse https �zerinden bilgileri aktar demek ama ger�ek uygulamalrda always olarak kullan�l�r yani ne olursa olsun https �zerinden bilgileri aktar demi� oluruz. 
                };

                options.SlidingExpiration = true; //kullan�c� giri� yapt�ktan sonra ne kadar s�re tan�nacak ve ne zaman tekrar giri� yapmas� gerekecek?
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);  //7 g�n boyunca bir daha giri� yapmas� gerekmesin
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied"); //sistem i�erisinde zaten giri� yapm�� bir kullan�c� yetkisinin olmad��� yere girerse buraya y�nlendir.
            });
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
