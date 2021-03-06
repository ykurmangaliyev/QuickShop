using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using QuickShop.DependencyInjection;
using QuickShop.DependencyInjection.Logger;
using QuickShop.DependencyInjection.PaymentProvider;
using QuickShop.DependencyInjection.Repository;
using QuickShop.WebApp.Authentication;

namespace QuickShop.WebApp
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
                .AddMongoRepository()
                .AddConsoleLogger()
                .AddStripePaymentProvider();

            // API
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // Authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
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
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SymmetricKey)),
                    };

                    options.Configuration = new OpenIdConnectConfiguration();

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[JwtBearerAuthenticationOptions.JwtBearerAuthentication];
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var cookie = context.Request.Cookies[JwtBearerAuthenticationOptions.JwtBearerAuthentication];

                            if (cookie != null)
                            {
                                context.Response.Cookies.Append(
                                    JwtBearerAuthenticationOptions.JwtBearerAuthentication, 
                                    cookie,
                                    new CookieOptions
                                    {
                                        Expires = DateTimeOffset.Now.AddDays(7),
                                        HttpOnly = false,
                                    }
                                );
                            }

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

            var defaultFileOptions = new DefaultFilesOptions();
            defaultFileOptions.DefaultFileNames.Clear();
            defaultFileOptions.DefaultFileNames.Add("index.html");

            app.UseDefaultFiles(defaultFileOptions);
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(_ => { });
        }
    }
}
