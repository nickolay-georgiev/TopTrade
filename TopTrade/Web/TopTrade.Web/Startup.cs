namespace TopTrade.Web
{
    using System;
    using System.Reflection;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TopTrade.Common;
    using TopTrade.Data;
    using TopTrade.Data.Common;
    using TopTrade.Data.Common.Repositories;
    using TopTrade.Data.Models;
    using TopTrade.Data.Repositories;
    using TopTrade.Data.Seeding;
    using TopTrade.Services;
    using TopTrade.Services.CronJobs;
    using TopTrade.Services.Data;
    using TopTrade.Services.Data.AccountManager;
    using TopTrade.Services.Data.Administrator;
    using TopTrade.Services.Data.User;
    using TopTrade.Services.Mapping;
    using TopTrade.Services.Messaging;
    using TopTrade.Services.Stock;
    using TopTrade.Web.Areas.User.Hubs;
    using TopTrade.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddSignalR();

            services.AddHangfire(
                      config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                          .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings()
                          .UseSqlServerStorage(
                              this.configuration.GetConnectionString("DefaultConnection"),
                              new SqlServerStorageOptions
                              {
                                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                  QueuePollInterval = TimeSpan.Zero,
                                  UseRecommendedIsolationLevel = true,
                                  UsePageLocksOnDequeue = true,
                                  DisableGlobalLocks = true,
                              }));

            //services.AddHangfireServer();

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
              {
                  options.HeaderName = "X-CSRF-TOKEN";
              });

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IAlphaVantageApiClientService, AlphaVantageApiClientService>();
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IUploadDocumentsService, UploadDocumentsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITradeService, TradeService>();
            services.AddTransient<IAccountManagementService, AccountManagementService>();
            services.AddTransient<IAdministrationService, AdministrationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();

                this.SeedHangfireJobs(recurringJobManager);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //if (env.IsProduction())
            //{
            //    app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            //    app.UseHangfireDashboard(
            //        "/Administration/Hangfire",
            //        new DashboardOptions
            //        {
            //            Authorization = new[] { new HangfireAuthorizationFilter() },
            //            AppPath = "/Administration/Dashboard",
            //        });
            //}

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            app.UseHangfireDashboard(
                "/Administration/Hangfire",
                new DashboardOptions
                {
                    Authorization = new[] { new HangfireAuthorizationFilter() },
                    AppPath = "/Administration/Dashboard",
                });

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<TradeHub>("/user/index");
                        endpoints.MapHub<TradeHub>("/user/portfolio/index");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate<TakeAllSwapFees>("TakeAllSwapFees", x => x.Work(), Cron.Daily);
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
