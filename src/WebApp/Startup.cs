using System;
using System.Text;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using QuickShop.DependencyInjection;
using QuickShop.DependencyInjection.Repository;
using QuickShop.WebApp.Authentication;
using QuickShop.WebApp.GraphQL;
using QuickShop.WebApp.GraphQL.Ping;

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
                .AddMongoRepository();

            // GraphQL
            GraphQLSchema.RegisterAllServices(services);

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

            app.UseGraphiQl("/graphiql", "/graphql");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(_ => { });
        }
    }
}
