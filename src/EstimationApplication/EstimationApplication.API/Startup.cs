using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System;
using EstimationApplication.Data;
using EstimationApplication.API.Authentication;
using EstimationApplication.BusinessRule;
using EstimationApplication.Entities;

namespace EstimationApplication.API
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
            services.AddControllers();

            // For Entity Framework
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(Configuration.GetConnectionString("ConnStr")));

            // For Identity  
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.Add(new ServiceDescriptor(typeof(UserManager<ApplicationUser>), typeof(ApplicationUserManager<ApplicationUser>), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IUserBusiness), typeof(UserBusiness), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IEstimateBusiness), typeof(EstimateBusiness), ServiceLifetime.Transient));

            services.AddSingleton<PrintScreenBusiness>();
            services.AddSingleton<PrintFileBusiness>();
            services.AddSingleton<PrintPaperBusiness>();

            services.AddTransient<Func<string, IPrintBusiness>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case EstimationApplicationConstant.PrintToScreen:
                        return serviceProvider.GetService<PrintScreenBusiness>();
                    case EstimationApplicationConstant.PrintToFile:
                        return serviceProvider.GetService<PrintFileBusiness>();
                    case EstimationApplicationConstant.PrintToPaper:
                        return serviceProvider.GetService<PrintPaperBusiness>();
                    default:
                        return null;
                }
            });

            services.Add(new ServiceDescriptor(typeof(IUserData), typeof(UserData), ServiceLifetime.Transient));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EstimationApplication.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EstimationApplication.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
