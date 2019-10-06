using System;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestaurantReview.Web
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private const string serviceTitle = "Template API";

        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var framework = assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, informationalVersion, framework));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, string informationalVersion, string framework)
        {
            var info = new OpenApiInfo
            {
                Title = serviceTitle,
                Version = description.ApiVersion.ToString(),
                Description = $"{serviceTitle} ({framework})  " + Environment.NewLine // two spaces at end are by intention, see https://www.markdownguide.org/basic-syntax#line-breaks
                            + $"Assembly Version {informationalVersion}" + Environment.NewLine
                            + "#### Dates" + Environment.NewLine
                            + "All dates must be in ISO8601 format (e.g. 2019-04-10T00:00:00.000Z)" + Environment.NewLine
                            + "#### GUIDs respectively UUIDs" + Environment.NewLine
                            + "All GUIDs can be supplied with or without hyphens (e.g. 6ac6fac7-2070-4226-833f-3a3049d11c6e / 6ac6fac720704226833f3a3049d11c6e)",
                Contact = new OpenApiContact { Name = "Toptal", Email = "support@toptal.com" }
            };

            if (description.IsDeprecated)
            {
                info.Description += Environment.NewLine + "## This API version has been deprecated.";
            }

            return info;
        }
    }
}
