using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestaurantReview.DataAccess;
using RestaurantReview.DataAccess.Repositories;
using RestaurantReview.DataAccess.Repositories.Interfaces;
using RestaurantReview.Web.Infrastructure;
using RestaurantReview.Web.Infrastructure.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestaurantReview.Web.Extensions
{
    /// <summary>
    /// Extension of <see cref="IServiceCollection"/> for well structured service registration.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add and configure DB context.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <param name="connectionString">The connection string of the database to connect to.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RestaurantReviewContext>(
                options =>
                {
                    options.UseSqlServer(
                        connectionString,
                        sqlOptions =>
                        {
                            // See https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);

                            // sqlOptions.MigrationsHistoryTable("EFMigrationsHistory");
                        });
                });

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReplyRepository, ReplyRepository>();
            services.AddScoped<JwtSecurityTokenHandler>();

            return services;
        }

        /// <summary>
        /// Add and configure API versioning.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    // reporting API versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                    options.Conventions.Add(new VersionByNamespaceConvention());
                });

            return services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by URL segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        /// <summary>
        /// Add and configure swagger generator.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <returns>Collection of service descriptors.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            return services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate XML comments
                    foreach (var xmlCommentsFilePath in GetXmlCommentsFilePaths())
                    {
                        options.IncludeXmlComments(xmlCommentsFilePath);
                    }
                });
        }

        private static IEnumerable<string> GetXmlCommentsFilePaths()
        {
            var docFiles = new List<string>();

            var basePath = AppContext.BaseDirectory;
            var files = Directory.EnumerateFiles(basePath, "*.xml");
            foreach (var file in files)
            {
                try
                {
                    var doc = XDocument.Load(file);
                    var rootName = doc.Root?.Name.LocalName;
                    if ("doc".Equals(rootName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        docFiles.Add(file);
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    // ignored
                }
            }

            return docFiles;
        }
    }
}
