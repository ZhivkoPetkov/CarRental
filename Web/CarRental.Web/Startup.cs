using CarRental.Web.Hubs;

namespace CarRental.Web
{
    using AutoMapper;
    using AutoMapper.EquivalencyExpression;
    using CarRental.Common;
    using CarRental.Data;
    using CarRental.Models;
    using CarRental.Services;
    using CarRental.Services.Contracts;
    using CarRental.Services.Messaging;
    using CarRental.Web.MappingConfiguration;
    using CarRental.Web.Middlewares;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
            // Framework services
            // TODO: Add pooling when this bug is fixed: https://github.com/aspnet/EntityFrameworkCore/issues/9741
            services.AddDbContext<CarRentalDbContext>(
                options => 
                options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<CarRentalDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders()
                .AddDefaultUI(UIFramework.Bootstrap4);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = ".AspNetCore.ConsentCookie";
                });

            //Config Cloud Storage
            var cloudinaryAccount = new CloudinaryDotNet.Account(GlobalConstants.CloudifyName, GlobalConstants.CloudifyAPI, GlobalConstants.CloudifyKey);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);

            //Config AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CarRentalConfiguration>();
                cfg.AddCollectionMappers();
            });

            services.AddSingleton(this.configuration);

            // Identity stores
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            services.AddTransient<ILocationsService, LocationsService>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IVouchersService, VouchersService>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISmsSender, NullMessageSender>();


            //Anti-forgery key
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

          
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMiddleware<SeedAdminRolesMiddleware>();
            app.UseSignalR(x => x.MapHub<NotifyHub>("/notify"));
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
