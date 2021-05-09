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
using EstimationApplication.API.Models;
using Microsoft.Extensions.Logging;

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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(EstimationApplicationAPIExceptionFilter));
            });

            AddAuthenticationServices(services);
            AddEstimationApplicationServices(services);

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EstimationApplication.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EstimationApplication.API v1"));
            }
            else
            {
                app.UseExceptionHandler("/api/Error/error");
            }
            app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");

            loggerFactory.AddFile("Logs/EstimationApplicationLog-{Date}.txt");

            app.UseRouting();

            app.UseCorsMiddleware();
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddEstimationApplicationServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(UserManager<ApplicationUser>), typeof(ApplicationUserManager<ApplicationUser>), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IUserBusiness), typeof(UserBusiness), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IEstimateBusiness), typeof(EstimateBusiness), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IUserData), typeof(UserData), ServiceLifetime.Transient));

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
        }

        private void AddAuthenticationServices(IServiceCollection services)
        {
            // For Entity Framework
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(Configuration.GetConnectionString(EstimationApplicationConstant.XMLConnectionString)));

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
        }
    }
}
