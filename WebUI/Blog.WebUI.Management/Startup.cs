using Blog.Data;
using Blog.Data.Infrastructure.Entities;
using Blog.WebUI.Infrastructure.Cache;
using Blog.WebUI.Infrastructure.Rules;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebUI.Management
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; set; }
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment { get; set; }
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();



        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }
            //config
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.AddOptions();

            //cache
            services.AddMemoryCache();
            services.AddTransient<ICache, Blog.Infrastructure.Caching.Memory.Cache>();
            services.AddTransient<CacheHelper>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o => { o.LoginPath = new PathString("/login/"); });

            //Data
            services.AddTransient<CategoryData>();
            services.AddTransient<ContentData>();
            services.AddTransient<ContentCategoryData>();
            services.AddTransient<MediaData>();
            services.AddTransient<AuthorData>();
            services.AddTransient<SettingData>();
            services.AddTransient<TagData>();
            services.AddTransient<ContentTagData>();
            services.AddTransient<RolePageData>();
            services.AddTransient<RoleData>();
            

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
            });
            services.Configure<RouteOptions>(routeOptions => routeOptions.AppendTrailingSlash = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseStatusCodePages(); }
            app.UseDeveloperExceptionPage();
            RedirectToHttpsWwwNonWwwRule rule = new RedirectToHttpsWwwNonWwwRule
            {
                status_code = 301,
                redirect_to_https = true,
                redirect_to_www = false,
                redirect_to_non_www = true,
                append_slash = true
            };
            RewriteOptions options = new RewriteOptions();
            options.Rules.Add(rule);
            app.UseRewriter(options);

            app.UseRouting();
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {                
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
