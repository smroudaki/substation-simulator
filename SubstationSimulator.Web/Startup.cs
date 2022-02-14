using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricalEmulator.Application.Common;
using ElectricalEmulator.Domain.Entities;
using ElectricalEmulator.Infra.Data.Identity;
using ElectricalEmulator.Infra.Data.Persistence;
using ElectricalEmulator.Infra.IoC;
using ElectricalEmulator.Web.Swashbuckle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElectricalEmulator.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ElectricalEmulatorContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("Local"),
                    b => b.MigrationsAssembly(typeof(ElectricalEmulatorContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;

            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ElectricalEmulatorContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RemainigTime", policy =>
                    policy.Requirements.Add(new RemainingTimeRequirement(Constants.DecreasingRemainigTimeAmount)));
            });

            services.AddScoped<IAuthorizationHandler, RemainingTimeHandler>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();

            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();

            //    config.Filters.Add(new AuthorizeFilter(policy));
            //    config.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //});

            services.RegisterServices();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithOrigins("http://localhost:8000")
                           .WithOrigins("http://127.0.0.1:8000")
                           .WithOrigins("http://185.211.56.9")
                           .WithOrigins("http://185.211.56.9:8000")
                           .AllowCredentials();
                });
            });

            services.AddControllersWithViews();

            services.AddSwaggerDocumentation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSwaggerDocumentation();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
