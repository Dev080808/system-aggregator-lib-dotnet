using SystemAggregator.Site.Extensions;

namespace SystemAggregator.Site
{
    public static class ApiWebApplication
    {
        public static void Run(
            Action<WebApplicationBuilder> configureBuilder,
            Action<WebApplication> configureApplication,
            bool addApiVersioning = false)
        {
            try
            {
                var builder = WebApplication.CreateBuilder();

                configureBuilder(builder);

                builder.AddApiServices(addApiVersioning);

                var app = builder.Build();

                configureApplication(app);

                app.UseApiPipeline();

                app.Run();
            }
            catch
            {
            }
            finally
            {
            }
        }
    }
}
