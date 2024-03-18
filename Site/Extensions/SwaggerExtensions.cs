using System.Reflection;

using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

using SystemAggregator.Site.Cfg;

namespace SystemAggregator.Site
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this WebApplicationBuilder builder, bool addApiVersioning = false)
        {
            builder.Services.Configure<SwaggerSettings>(
                builder.Configuration.GetSection(SwaggerSettings.Section));

            var swaggerGenOptions = builder.Services.AddSwaggerGen().AddOptions<SwaggerGenOptions>();

            var swaggerUiOptions = builder.Services.AddOptions<SwaggerUIOptions>();

            if (addApiVersioning)
            {
                swaggerGenOptions.Configure<IOptions<SwaggerSettings>, IApiVersionDescriptionProvider, IWebHostEnvironment>(ConfigureSwaggerGenVersioned);
                swaggerUiOptions.Configure<IOptions<SwaggerSettings>, IApiVersionDescriptionProvider>(ConfigureSwaggerUiVersioned);
            }
            else
            {
                swaggerGenOptions.Configure<IOptions<SwaggerSettings>, IWebHostEnvironment>(ConfigureSwaggerGenNonversioned);
                swaggerUiOptions.Configure<IOptions<SwaggerSettings>>(ConfigureSwaggerUiNonversioned);
            }
        }

        private static void ConfigureSwaggerGen(
            SwaggerGenOptions options,
            IOptions<SwaggerSettings> settings,
            IWebHostEnvironment env)
        {
            options.IncludeXmlDoc(settings, env);

            options.AddEnumsWithValuesFixFilters();

            options.EnableAnnotations();
        }

        private static void ConfigureSwaggerGenNonversioned(
            SwaggerGenOptions options,
            IOptions<SwaggerSettings> settings,
            IWebHostEnvironment env)
        {
            ConfigureSwaggerGen(options, settings, env);

            options.SwaggerDoc(settings.Value.DocName, new OpenApiInfo
            {
                Title = settings.Value.DocTitle,
                Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(),
                Description = settings.Value.DocDescription
            });
        }

        private static void ConfigureSwaggerGenVersioned(
            SwaggerGenOptions options,
            IOptions<SwaggerSettings> settings,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IWebHostEnvironment env)
        {
            ConfigureSwaggerGen(options, settings, env);

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = settings.Value.DocTitle,
                    Version = description.ApiVersion.ToString(),
                    Description = settings.Value.DocDescription
                });
            }
        }

        private static void ConfigureSwaggerUiNonversioned(
            SwaggerUIOptions options,
            IOptions<SwaggerSettings> settings)
        {
            options.SwaggerEndpoint(
                $"{settings.Value.DocName}/swagger.json",
                settings.Value.DocTitle);
        }

        private static void ConfigureSwaggerUiVersioned(
            SwaggerUIOptions options,
            IOptions<SwaggerSettings> settings,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"{description.GroupName}/swagger.json",
                    description.GroupName);
            }
        }

        private static void IncludeXmlDoc(
            this SwaggerGenOptions options,
            IOptions<SwaggerSettings> settings,
            IWebHostEnvironment env)
        {
            if (settings.Value.XmlDocPath == null ||
                settings.Value.XmlDocFilePattern == null)
            {
                return;
            }

            var path = Path.Combine(env.ContentRootPath, settings.Value.XmlDocPath);

            if (!Directory.Exists(path))
            {
                return;
            }

            foreach (var filePath in Directory.EnumerateFiles(path, settings.Value.XmlDocFilePattern))
            {
                options.IncludeXmlComments(filePath);
            }
        }
    }
}
