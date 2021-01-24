using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using QuickShop.DependencyInjection;
using QuickShop.DependencyInjection.Repository;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Repository.Mongo.Configuration;
using WebApp.Authentication;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Domain-specific
            services.ConfigureMongoRepositoryOptions(Configuration);
            services.Configure<JwtBearerAuthenticationOptions>(Configuration.GetSection(JwtBearerAuthenticationOptions.JwtBearerAuthentication));

            services.AddQuickShop()
                .AddMongoRepository();

            // API
            services.AddControllers();

            // Authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var configuration = Configuration
                        .GetSection(JwtBearerAuthenticationOptions.JwtBearerAuthentication)
                        .Get<JwtBearerAuthenticationOptions>();

                    options.Audience = configuration.Audience;
                    options.Authority = configuration.Authority;
                    
                    options.RequireHttpsMetadata = !Environment.IsDevelopment();
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SymmetricKey)),
                    };

                    options.Configuration = new OpenIdConnectConfiguration();

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSingleton<JwtTokenGenerator>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
