using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using RestaurantReview.DataAccess;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Extensions;
using RestaurantReview.Web.Infrastructure;
using RestaurantReview.Web.Middleware;

namespace RestaurantReview.Web
{
    /// <summary>
    /// The startup methods for ASP .NET CORE
    /// </summary>
    public class Startup
    {
        readonly string CorsPolicyName = "_myCorsPolicy";

        /// <inheritdoc />
        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Provides information about the web hosting environment an application is running in.
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.All);

            var connectionString = Configuration.GetConnectionString("RestaurantReviewContext");

            services
                .AddHealthChecks()
                .AddSqlServer(connectionString, name: "SQL - RestaurantReview");

            services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddMvc(options => {
                    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                    options.RespectBrowserAcceptHeader = true;
                    })
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Error = (object sender, ErrorEventArgs args) =>
                        {
                            var test = args.ErrorContext.Error;
                        };
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddVersioning();

            services.AddSwagger();
            services.AddSwaggerGen(options => options.AddSecurityDefinition("jwt", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            }));

            // Setup auth
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<RestaurantReviewContext>()
                .AddDefaultTokenProviders();

            // Setup JWT token
            services.Configure<JwtTokenSettings>(Configuration.GetSection("JwtToken"));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    var jwtTokenSettings = Configuration.GetSection("JwtToken").Get<JwtTokenSettings>();

                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtTokenSettings.Issuer,
                        ValidAudience = jwtTokenSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.SigningKey))
                    };
                });

            //setup cors
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddDbConfiguration(connectionString);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            // setup MVC
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Defines a class that provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            // Seed data
            app.SeedData();

            app.UseForwardedHeaders()
                .UseHealthChecks("/health", new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
                .UseMiddleware<ApiLoggingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.ConfigureExceptionHandler(NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger());

            app.UseCors(CorsPolicyName);
            app.UseAuthentication();

            var rewriteOptions = new RewriteOptions();
            rewriteOptions.AddRedirect("^$", "/swagger/index.html");
            app.UseRewriter(rewriteOptions);

            app.UseSpaStaticFiles();

            app.UseMvc()
                .UseSwagger()
                .UseSwaggerUI(
                    options =>
                    {
                        options.DisplayRequestDuration();

                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(description => description.ApiVersion))
                        {
                            options.SwaggerEndpoint($"./{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
