using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Repositories;
using WowDin.Frontstage.Repositories.Interface;
using WowDin.Frontstage.Services.Store;
using WowDin.Frontstage.Models.ViewModel.Member;
using WowDin.Frontstage.Services.Member;
using WowDin.Frontstage.Services.Member.Interface;
using WowDin.Frontstage.Services.Order.Interface;
using WowDin.Frontstage.Services.Order;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WowDin.Frontstage.Services;
using WowDin.Frontstage.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using WowDin.Frontstage.Common.AutoMapingProfile;
using WowDin.Frontstage.Common;
using CoreMVC_Project.Repository.Interface;
using CoreMVC_Project.Repository;

namespace WowDin.Frontstage
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Member/Login";
            });

            //facebook、google登入
            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Google:ClientSecret"];
                });
            


            //讓Service也可以存取HttpContext
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            //Repository 
            services.AddTransient<IRepository, Repository>();


            //註冊Redis
            services.AddStackExchangeRedisCache(options => options.Configuration = Configuration["RedisConfig:WoWdinMemoryCache"]);

            //Service 注入
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMemoryCacheRepository, MemoryCacheRepository>();

            //AutoMapper
            services.AddAutoMapper(typeof(ControllerProfile), typeof(ServiceProfile));
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "search",
                   pattern: "Search/{Method}/{Lat?}/{Lng?}/{Address?}/{Brand?}/{Category?}/{Evaluate?}",
                   defaults: new { controller = "Store", action = "Search" });
                endpoints.MapControllerRoute(
                   name: "shopmenu",
                   pattern: "Menu/{id?}",
                   defaults: new { controller = "Store", action = "ShopMenu"});
                endpoints.MapControllerRoute(
                    name: "shopmenu by group",
                    pattern: "Group/{groupId}",
                    defaults: new { controller = "Store", action = "ShopMenuByGroup" }); 
            });
        }
    }
}
