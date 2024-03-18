using SystemAggregator.Site.Features;

namespace SystemAggregator.Site.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddApiServices(
            this WebApplicationBuilder builder,
            bool addApiVersioning = false)
        {
            if (addApiVersioning)
            {
                builder.Host.ConfigureServices(ApiVersioning.Configure);
            }

            builder.AddSwagger(addApiVersioning);
        }
    }
}
