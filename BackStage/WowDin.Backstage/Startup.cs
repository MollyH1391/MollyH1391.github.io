using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Repositories;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using WowDin.Backstage.Common.AutoMapingProfile;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace WowDin.Backstage
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

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            

            services.AddDbContext<WowdinDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NidinShopsConnection")));

            //設定驗證方式(cookie base)
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            //讓Service也可以存取HttpContext
            services.AddHttpContextAccessor();
            services.AddHttpClient();

            //Repository 
            services.AddTransient<IRepository, Repository>();

            //service註冊
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IInformationService, InformationService>();
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ICouponService, CouponService>();
            services.AddTransient<IAdvertiseService, AdvertiseService>();

            services.AddTransient<IOrderService, OrderService>();

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            // auto mapper
            services.AddAutoMapper(typeof(ControllerProfile), typeof(ServiceProfile));
            services.AddHttpClient();

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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "menumanage",
                    defaults: new { controller = "Menu", action = "MenuManage" },
                    pattern: "Menu/{id?}");

                endpoints.MapControllerRoute(
                    name: "coupon",
                    defaults: new { controller = "Coupon", action = "Coupon" },
                    pattern: "Coupon");

                endpoints.MapControllerRoute(
                    name: "advertise",
                    defaults: new { controller = "Coupon", action = "Advertise" },
                    pattern: "Advertise");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}
